using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FPS.Scripts.Enemies.Standard
{
    /// <summary>
    /// Caches and provides behaviors attached to this GameObject.
    /// </summary>
    public class BehaviorRegistry
    {
        private readonly Dictionary<Type, object> _behaviors = new();

        public BehaviorRegistry(GameObject host, IEnumerable<Type> behaviorTypes)
        {
            foreach (var type in behaviorTypes)
            {
                var behavior = host.GetComponent(type);
                if (behavior != null)
                {
                    _behaviors[type] = behavior;
                }
            }
        }
        
        
        public IEnumerable<T> GetAllOfType<T>() where T : class
        {
            return _behaviors.Values.OfType<T>();
        }

        public T Get<T>() where T : class
        {
            _behaviors.TryGetValue(typeof(T), out var behavior);
            return behavior as T;
        }
    }
}