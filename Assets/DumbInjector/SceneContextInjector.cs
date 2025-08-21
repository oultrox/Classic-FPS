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

                    // Inject dependencies into this instance
                    InjectInstance(mb);
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
                if (providedInstance != null && !_sceneRegistry.ContainsKey(returnType))
                {
                    _sceneRegistry[returnType] = providedInstance;
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
                if (!Resolve(field.FieldType).Equals(null))
                {
                    field.SetValue(instance, Resolve(field.FieldType));
                }
            }

            // Inject properties
            foreach (var prop in type.GetProperties(flags).Where(p => Attribute.IsDefined(p, typeof(InjectAttribute))))
            {
                if (!prop.CanWrite) continue;
                if (!Resolve(prop.PropertyType).Equals(null))
                {
                    prop.SetValue(instance, Resolve(prop.PropertyType));
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

            // Could optionally fallback to global container:
            // if (instance == null) instance = GlobalInjector.Resolve(t);

            return instance;
        }
    }
}
