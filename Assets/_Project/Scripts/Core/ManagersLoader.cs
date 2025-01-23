using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadowWorker.Core
{
    public class ManagersLoader : MonoBehaviour
    {
        [SerializeField] private GameObject managersPrefab;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadManagers()
        {
            var managers = GameObject.FindObjectOfType<ManagersLoader>();
            if (managers == null)
            {
                var instance = Resources.Load<GameObject>("Managers");
                if (instance != null)
                {
                    Instantiate(instance);
                }
                else
                {
                    Debug.LogError("Managers prefab not found in Resources folder!");
                }
            }
        }
    }
} 