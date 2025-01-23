using UnityEngine;

namespace ShadowWorker.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private bool isPaused;
        
        public bool IsPaused
        {
            get => isPaused;
            set
            {
                isPaused = value;
                Time.timeScale = isPaused ? 0f : 1f;
                OnGamePaused?.Invoke(isPaused);
            }
        }

        public delegate void GamePausedHandler(bool isPaused);
        public event GamePausedHandler OnGamePaused;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeGame()
        {
            Application.targetFrameRate = 60;
            IsPaused = false;
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
} 