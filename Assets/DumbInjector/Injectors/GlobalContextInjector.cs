using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DumbInjector.Injectors
{
    /// <summary>
    /// Global injector: handles global services/singletons, global scoped scenes.
    /// </summary>
    [DefaultExecutionOrder(int.MinValue + 999)]
    public class GlobalContextInjector : MonoBehaviour
    {
        readonly HashSet<Type> _injectableTypes = new();
        bool _typesCached;
        const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        
        void Awake()
        {
            CacheInjectableTypes();
            InjectSceneObjects();
        }

        void CacheInjectableTypes()
        {
            if (_typesCached) return;
            
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t));

            foreach (var t in types)
            {
                bool injectable = t.GetFields(BINDING_FLAGS).Any(f => Attribute.IsDefined(f, typeof(InjectAttribute))) ||
                                  t.GetProperties(BINDING_FLAGS).Any(p => Attribute.IsDefined(p, typeof(InjectAttribute))) ||
                                  t.GetMethods(BINDING_FLAGS).Any(m => Attribute.IsDefined(m, typeof(InjectAttribute))) ||
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
                    {
                        Injector.Instance.RegisterProvider(provider);
                    }

                    if (IsInjectable(mb))
                    {
                        Injector.Instance.Inject(mb);
                    }
                    
                }
            }
        }
        
        bool IsInjectable(MonoBehaviour obj)
        {
            return  _injectableTypes.Contains(obj.GetType());
        }
    }
}