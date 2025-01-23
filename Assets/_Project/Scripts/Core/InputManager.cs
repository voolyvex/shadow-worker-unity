using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowWorker.Core
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private PlayerInput playerInput;
        private InputActionMap gameplayActions;

        public Vector2 MovementInput { get; private set; }
        public bool IsInteracting { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeInput();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeInput()
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null)
            {
                playerInput = gameObject.AddComponent<PlayerInput>();
            }

            gameplayActions = playerInput.actions.FindActionMap("Gameplay");
            
            // Subscribe to input events
            var moveAction = gameplayActions.FindAction("Move");
            var interactAction = gameplayActions.FindAction("Interact");

            moveAction.performed += OnMove;
            moveAction.canceled += OnMove;
            interactAction.performed += OnInteract;
            interactAction.canceled += OnInteract;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            IsInteracting = context.ReadValueAsButton();
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (gameplayActions != null)
            {
                var moveAction = gameplayActions.FindAction("Move");
                var interactAction = gameplayActions.FindAction("Interact");

                if (moveAction != null) moveAction.performed -= OnMove;
                if (moveAction != null) moveAction.canceled -= OnMove;
                if (interactAction != null) interactAction.performed -= OnInteract;
                if (interactAction != null) interactAction.canceled -= OnInteract;
            }
        }
    }
} 