using UnityEngine;
using UnityEngine.InputSystem;
using ShadowWorker.Core;

[RequireComponent(typeof(Rigidbody2D), typeof(PersonalityProfile))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float focusedMoveSpeedMultiplier = 0.5f;
    
    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask interactableLayers;

    private Rigidbody2D rb;
    private PersonalityProfile personalityProfile;
    private Vector2 moveInput;
    private bool isFocused;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        personalityProfile = GetComponent<PersonalityProfile>();
        rb.gravityScale = 0f; // Top-down movement
    }

    private void FixedUpdate()
    {
        // Apply movement
        float currentSpeed = moveSpeed * (isFocused ? focusedMoveSpeedMultiplier : 1f);
        rb.velocity = moveInput * currentSpeed;
    }

    // Input System callbacks
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            // Check for nearby interactables
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayers);
            foreach (var collider in colliders)
            {
                // Try to interact with any object that has an IInteractable interface
                var interactable = collider.GetComponent<IInteractable>();
                interactable?.Interact(gameObject);
            }
        }
    }

    public void OnFocus(InputValue value)
    {
        isFocused = value.isPressed;
        
        // Optional: Modify personality traits when focused
        if (isFocused)
        {
            personalityProfile.ModifyIntegrationLevel(0.1f);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize interaction radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}

// Interface for interactable objects
public interface IInteractable
{
    void Interact(GameObject interactor);
} 