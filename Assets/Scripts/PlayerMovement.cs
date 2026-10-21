
using System;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static string FirstPlayerOutOfBoundsName { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ResetOutOfBoundsRecord()
    { 
        FirstPlayerOutOfBoundsName = null;
    }
    [SerializeField] private float moveSpeed = 4f;
    //serializefield just means that the variable is editable in Unity's Inspector

    [SerializeField] private bool useWASD = true;

    [SerializeField] private bool startDirection = true;

    private Vector2 direction;
    //player initially moves right

    private Rigidbody2D rb;

    private Vector2 _min;
    private Vector2 _max;

    //----variables for speed increases over time -> log base 5-----
    [SerializeField] private float _currentSpeed = 5f;
    private float _timer;


    void Start()
    {
        _min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        _max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        rb = GetComponent<Rigidbody2D>();

        if (startDirection == true) 
        {
            direction = Vector2.left;
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
        GetCoordinates();
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

    private void GetCoordinates()    
    {
        if (transform.position.x < _min.x || transform.position.x > _max.x||transform.position.y < _min.y || transform.position.y > _max.y) 
        {
            if (FirstPlayerOutOfBoundsName == null)
            {
                FirstPlayerOutOfBoundsName = gameObject.name;
            }

            Debug.Log("Player out of bounds");
            Destroy(gameObject);
        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Powerup"))
        {
            PowerUp powerUp = collision.GetComponent<PowerUp>();
            if (powerUp.GetPowerUpType() == "spd_up")
            {
                this.SetSpeed(this.moveSpeed += 2f);
                Destroy(collision.gameObject);
            }
            if (powerUp.GetPowerUpType() == "spd_down")
            {
                this.SetSpeed(this.moveSpeed -= 2f);
                Destroy(collision.gameObject);
            }
        }

    }

    //---------method for speed increases-------
    private void SpeedIncreases()
    {
        _timer += Time.deltaTime;

        float newSpeed = _currentSpeed + Mathf.Log(_timer + 1f, 5f);
        //timer starts at zero, asymptote at 0

        //using the setter for speed!!!
        SetSpeed(newSpeed);
    }
}

