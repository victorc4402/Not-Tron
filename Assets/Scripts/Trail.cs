using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TrailRenderer))]
public class Trail : MonoBehaviour
{
    //encapsulation!
    private TrailRenderer trailRenderer;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> colliderPosition = new List<Vector2>();
    private GameObject trailColliderObject;

    public static string FirstPlayerToDieName { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ResetFirstDeath()
    {
        FirstPlayerToDieName = null;
    }

    [Header("Trail Settings")]
    [SerializeField] private int maxTrailPoints = 50; //controls the exact length of the trail

    public int PointCount => trailRenderer != null ? trailRenderer.positionCount : 0;
    //checks if the trail is active, returns the number of points trails using

    private void Awake()
    {
        //getting encapsulated!
        trailRenderer = GetComponent<TrailRenderer>();
        
        //turns off unitys automatic trail time
        trailRenderer.time = Mathf.Infinity;

        //new object so collider does not move with object
        trailColliderObject = new GameObject(gameObject.name + "_TrailCollider");
        trailColliderObject.transform.position = Vector3.zero;
        
        edgeCollider = trailColliderObject.AddComponent<EdgeCollider2D>();
        edgeCollider.isTrigger = true;

        TrailCollisionDetector detector = trailColliderObject.AddComponent<TrailCollisionDetector>();
        //tells collision detector which players trail it is 
    }

    private void Update()
    {
        TrimTrailLength();
        UpdateColliderPosition();
    }

    private void TrimTrailLength()
    {
        int currentCount = trailRenderer.positionCount;
        //tracks number of points on trial
        
        if (currentCount > maxTrailPoints)
        {
            Vector3[] allPositions = new Vector3[currentCount];
            trailRenderer.GetPositions(allPositions);
            //all points into allPosition list

            Vector3[] trimmedPositions = new Vector3[maxTrailPoints];
            System.Array.Copy(allPositions, currentCount - maxTrailPoints, trimmedPositions, 0, maxTrailPoints);
            //keeps only the closet 50 points

            trailRenderer.Clear();
            trailRenderer.AddPositions(trimmedPositions);
        }
    }

    private void UpdateColliderPosition()
    {
        int pointCount = trailRenderer.positionCount;
        int headBuffer = 5; //prevents instant spawn death
        if (pointCount < headBuffer + 2) 
        {
            return;
        }

        Vector3[] worldPositions = new Vector3[pointCount];
        trailRenderer.GetPositions(worldPositions);
        //upate new position

        colliderPosition.Clear();

        for (int i = 0; i < pointCount - headBuffer; i++)
        {
            colliderPosition.Add(worldPositions[i]);
        }

        edgeCollider.SetPoints(colliderPosition);
        //setting encapsulated!
    }

    private void OnDestroy()
    {
        if (FirstPlayerToDieName == null && CompareTag("Player"))
        {
            FirstPlayerToDieName = gameObject.name;

            if (gameObject.name == "player1")
            {
                SceneManager.LoadScene("player2 win");
            }
            else if (gameObject.name == "player2")
            {
                SceneManager.LoadScene("player1 win");
            }
        }

        if (trailColliderObject != null)
        {
            Destroy(trailColliderObject);
            //clear colliders when players die
        }
    }


    private class TrailCollisionDetector : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) //if collider has player tag
            {
                Debug.Log(collision.gameObject.name + " died");
                Destroy(collision.gameObject);
            }
        }
    }
}