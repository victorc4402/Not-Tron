
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    //serializefield just means that the variable is editable in Unity's Inspector

    [SerializeField] private bool useWASD = true;

    [SerializeField] private bool startDirection = true;

    private Vector2 direction;
    //player initially moves right

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (startDirection == true) 
        {
            direction = Vector2.up;
        }

        else
        {
            direction = Vector2.right;
        }

    }


    void Update()
    {
        HandleInput();
        MovePlayer();
    }

    // Checks which direction the player wants to move
    private void HandleInput()
    {
        if (useWASD == false) 
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            ChangeDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeDirection(Vector2.right);
        }
        }

        else 
        {
            if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S)) 
        {
            ChangeDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeDirection(Vector2.right);
        }
        }
        
    }

    // Changes direction without allowing 180-degree turns
    private void ChangeDirection(Vector2 newDirection)
    {
        if (newDirection != -direction)
        {
            direction = newDirection;
        }
        //newDirection != -direction ensures no 180-degree turns
    }

    // Continuously moves the player
    private void MovePlayer()
    {
        rb.linearVelocity = direction * moveSpeed;
    }

    // Returns the player's current direction
    public Vector2 GetDirection()
    {
        return direction;
    }

    // Allows other scripts to change movement speed
    public void SetSpeed(float newSpeed)
    {
        if (newSpeed > 0)
        {
            moveSpeed = newSpeed;
        }
    }
}

