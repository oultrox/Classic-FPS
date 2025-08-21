using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DumbInjector
{
    /// <summary>
    /// Scene injector: handles scene-local objects for faster lookups.
    /// </summary>
    [DefaultExecutionOrder(int.MinValue + 1000)]
    public class SceneContextInjector : MonoBehaviour
    {
        readonly HashSet<Type> _injectableTypes = new();
        readonly Dictionary<Type, object> _sceneRegistry = new();
        bool _typesCached;

        private void Awake()
        {
            CacheInjectableTypes();
            InjectSceneObjects();
        }

        void CacheInjectableTypes()
        {
            if (_typesCached) return;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t));

            foreach (var t in types)
            {
                bool injectable = t.GetFields(flags).Any(f => Attribute.IsDefined(f, typeof(InjectAttribute))) ||
                                  t.GetProperties(flags).Any(p => Attribute.IsDefined(p, typeof(InjectAttribute))) ||
                                  t.GetMethods(flags).Any(m => Attribute.IsDefined(m, typeof(InjectAttribute))) ||
                                  typeof(IDependencyProvider).IsAssignableFrom(t);
                if (injectable) _injectableTypes.Add(t);
            }

            _typesCached = true;
        }
        
        void InjectSceneObjects()
        {
            var roots = gameObject.scene.GetRootGameObjects();
            foreach (var root in roots)
            {
                var all = root.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var mb in all)
                {
                    if (mb == null) continue;

                    // Register provider outputs
                    if (mb is IDependencyProvider provider)
                        RegisterProvider(provider);

                    // Register the object itself
                    var type = mb.GetType();
                    _sceneRegistry.TryAdd(type, mb);

                    // Register all interfaces it implements
                    foreach (var iface in type.GetInterfaces())
                        _sceneRegistry.TryAdd(iface, mb);

                    if (IsInjectable(mb))
                    {
                        InjectInstance(mb);
                    }
                    
                }
            }
        }
        
        void RegisterProvider(IDependencyProvider provider)
        {
            var methods = provider.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;
                var returnType = method.ReturnType;
                var providedInstance = method.Invoke(provider, null);
                if (providedInstance != null)
                {
                    _sceneRegistry.TryAdd(returnType, providedInstance);
                }
            }
        }
        
        void InjectInstance(object instance)
        {
            var type = instance.GetType();
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            // Inject fields
            foreach (var field in type.GetFields(flags).Where(f => Attribute.IsDefined(f, typeof(InjectAttribute))))
            {
                var resolved = Resolve(field.FieldType);
                if (resolved != null)
                {
                    field.SetValue(instance, resolved);
                }
            }

            // Inject properties
            foreach (var prop in type.GetProperties(flags).Where(p => Attribute.IsDefined(p, typeof(InjectAttribute))))
            {
                if (!prop.CanWrite) continue;
                var resolved = Resolve(prop.PropertyType);
                if (resolved != null) // works for regular C# objects
                {
                    prop.SetValue(instance, resolved);
                }
            }

            // Inject methods
            foreach (var method in type.GetMethods(flags).Where(m => Attribute.IsDefined(m, typeof(InjectAttribute))))
            {
                var parameters = method.GetParameters()
                    .Select(p => Resolve(p.ParameterType))
                    .ToArray();
                method.Invoke(instance, parameters);
            }
        }
        
        object Resolve(Type t)
        {
            // Check scene-local container first
            _sceneRegistry.TryGetValue(t, out var instance);

            // Check Global optionally.
            if (instance == null) instance = Injector.Instance.Resolve(t);

            return instance;
        }
        
        bool IsInjectable(MonoBehaviour obj)
        {
            return  _injectableTypes.Contains(obj.GetType());
        }
    }
}
