using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DumbInjector.Internal;
using UnityEngine;

namespace DumbInjector
{

    /// <summary>
    /// Global injector container.
    /// </summary>
    [DefaultExecutionOrder(int.MinValue + 100)]
    public class Injector: Singleton<Injector>
    {
        const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        readonly Dictionary<Type, object> registry =  new();
        
        public void Inject(object instance)
        {
            var type = instance.GetType();
            InjectFields(instance, type);
            InjectMethods(instance, type);
            InjectProperties(instance, type);
        }
        
        public object Resolve(Type type)
        {
            registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }
        
        public void RegisterProvider(IDependencyProvider provider)
        {
            var methods = provider.GetType().GetMethods(BINDING_FLAGS);

            foreach (var method in methods)
            {
                bool isAttributeNotDefined = !Attribute.IsDefined(method, typeof(ProvideAttribute));
                if (isAttributeNotDefined) continue; 
                
                var returnType = method.ReturnType;
                if (registry.ContainsKey(returnType))
                {
                    Debug.LogWarning($"Provider for {returnType.Name} already registered. Ignoring duplicate.");
                    continue;
                }
                
                var providedInstance = method.Invoke(provider, null);
                if (providedInstance != null)
                {
                    registry.Add(returnType, providedInstance);
                    Debug.Log($"Injected {returnType.Name} from {provider.GetType().Name}");
                }
                else
                {
                    throw new Exception($"Provider {provider.GetType().Name} returned null for { returnType.Name}"); 
                }
            }
        }
        
        void InjectFields(object instance, Type type)
        {
            var injectableFields = type.GetFields(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectable in injectableFields)
            {
                var fieldType = injectable.FieldType;
                var resolvedInstance =  Resolve(fieldType);
                if (resolvedInstance == null)
                {
                    throw new Exception($"Failed to inject {fieldType.Name} for {type.Name}");
                }
                
                injectable.SetValue(instance, resolvedInstance);
                Debug.Log($"Field Injected {fieldType.Name} for {type.Name}");
            }
        }

        void InjectMethods(object instance, Type type)
        {
            var injectableMethods = type.GetMethods(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var method in injectableMethods)
            {
                var requiredParameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var resolvedInstances =  requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                {
                    throw new Exception($"Failed to inject {type.Name} for {method.Name}");
                }
                
                method.Invoke(instance, resolvedInstances);
                Debug.Log($"Method Injected {type.Name}.{method.Name}");
            }
        }

        void InjectProperties(object instance, Type type)
        {
            var injectableProperties = type.GetProperties(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var propertyInfo in injectableProperties)
            {
                var propertyType = propertyInfo.PropertyType;
                var resolvedInstance =  Resolve(propertyType);
                if (resolvedInstance == null)
                {
                    throw new Exception($"Failed to inject {type.Name} for {propertyInfo.Name}");
                }
                if (!propertyInfo.CanWrite)
                {
                    throw new Exception($"Property {type.Name}.{propertyInfo.Name} is not writable.");
                }
                
                propertyInfo.SetValue(instance, resolvedInstance);
                Debug.Log($"Property Injected {type.Name}.{propertyInfo.Name}");
            }
        }
    }

}
