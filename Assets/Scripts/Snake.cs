using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    public const float Speed = 5f;
    public Vector2 CurrentDirection { get; private set; } = Vector2.up;

    public SnakeHead head;
    public SnakeBody body;

    [SerializeField] private ParticleSystem firework;

    private InputManager inputManager;

    /// <summary>
    /// Unity Event function.
    /// Initialize input manager on object enabled.
    /// </summary>
    private void OnEnable()
    {
        inputManager = new InputManager();
        inputManager.Enable();

        // Handle direction input
        inputManager.Snake.Direction.started += OnDirectionChanged;
    }

    #region Input Handling

    /// <summary>
    /// When a directional button is pressed.
    /// </summary>
    /// <param name="context">Input context</param>
    private void OnDirectionChanged(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>();
        ChangeDirection(inputDirection);
    }

    #endregion

    /// <summary>
    /// Unity Event function.
    /// Stop reading input on object disabled.
    /// </summary>
    private void OnDisable()
    {
        inputManager.Disable();
    }

    /// <summary>
    /// Unity Event function.
    /// Update once per frame.
    /// </summary>
    private void Update()
    {
        // If snake head move by 1 unit then snake body move by 1 unit.
        if ((Vector2)head.transform.position != head.PreviousPosition)
        {
            body.Move(head.PreviousPosition);
            head.PreviousPosition = head.transform.position;
        }
    }

    /// <summary>
    /// Change snake direction.
    /// </summary>
    /// <param name="newDirection">New direction</param>
    public void ChangeDirection(Vector2 newDirection)
    {
        if (newDirection == Vector2.left || newDirection == Vector2.right)
        {
            if (CurrentDirection == Vector2.up || CurrentDirection == Vector2.down)
            {
                CurrentDirection = newDirection;
            }
        }
        else
        {
            if (CurrentDirection == Vector2.left || CurrentDirection == Vector2.right)
            {
                CurrentDirection = newDirection;
            }
        }
    }

    /// <summary>
    /// Grow snake's length by 1 unit.
    /// </summary>
    public void Grow()
    {
        // Spawn a new body unit and and add that unit to snake body list.
        body.Units.Insert(0, Instantiate(body.Units[0], head.transform.position, head.transform.rotation));
        body.Positions.Insert(0, body.Units[0].position);
        body.Units[0].parent = body.transform;

        body.Rescale();
    }

    /// <summary>
    /// Snake eats a food object.
    /// </summary>
    /// <param name="food">Food object to eat</param>
    public void CollectFood(Food food)
    {
        Grow();

        Instantiate(firework, head.transform.position, head.transform.rotation);
        food.RandomizePosition();

        CameraShaker.Instance.Shake();
    }
}