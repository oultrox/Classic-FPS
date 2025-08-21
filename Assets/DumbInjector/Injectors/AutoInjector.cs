using UnityEngine;

namespace DumbInjector
{
    /// <summary>
    /// Component that will help to auto-inject newly instantiated game objects from the injector.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(int.MinValue +1000)]
    public class AutoInjector : MonoBehaviour
    {
        public enum InjectionMode
        {
            Single,       // inject only the first MonoBehaviour
            Object,       // inject all MonoBehaviours on this GameObject
            Recursive     // inject all MonoBehaviours on this GameObject and children
        }

        public InjectionMode mode = InjectionMode.Single;
        

        private void Awake()
        {
            var injector = Injector.Instance;
            if (injector == null) return;

            switch (mode)
            {
                case InjectionMode.Single:
                    injector.Inject(GetComponent<MonoBehaviour>());
                    break;
                case InjectionMode.Object:
                    foreach (var mb in GetComponents<MonoBehaviour>())
                        injector.Inject(mb);
                    break;
                case InjectionMode.Recursive:
                    foreach (var mb in GetComponentsInChildren<MonoBehaviour>())
                        injector.Inject(mb);
                    break;
            }
        }
    }
}