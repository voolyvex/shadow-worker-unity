using UnityEngine;

namespace ShadowWorker.Core
{
    public class SceneBootstrapper : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        
        private void Start()
        {
            if (playerPrefab != null && GameObject.FindGameObjectWithTag("Player") == null)
            {
                Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            }
            
            // Ensure managers are initialized
            if (GameManager.Instance == null || AudioManager.Instance == null || InputManager.Instance == null)
            {
                Debug.LogError("Core managers not found! Please ensure the Managers prefab is in the Resources folder.");
            }
        }
    }
} 