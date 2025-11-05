using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core
{
    public class Bootloader : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void CreateBootLoader()
        {
            GameObject bootLoader = new GameObject("BootLoader");
            Bootloader loader = bootLoader.AddComponent<Bootloader>();
            loader.InitializeSystems();
            DontDestroyOnLoad(bootLoader);
        }

        private void InitializeSystems()
        {
            Assembly.GetAssembly(typeof(Bootloader))
                .GetTypes()
                .Where(t => t.GetCustomAttribute<InitializeSystemAttribute>() != null)
                .ToList()
                .ForEach(t =>
                {
                    GameObject t_gameObject = new GameObject(t.Name);
                    t_gameObject.transform.parent = transform;
                    t_gameObject.AddComponent(t);
                });
        }
    }
}