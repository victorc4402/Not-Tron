using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TrailRenderer))]
public class Trail : MonoBehaviour
{
    //encapsulation!!!
    private TrailRenderer _trailRenderer;
    private EdgeCollider2D _edgeCollider;
    private List<Vector2> _colliderPosition = new List<Vector2>();
    private GameObject _trailColliderObject;


    private bool _isAlive = true;

    private float _trailDuration = 3f; //controls the exact length of the trail, unity timer


    //checks if the trail is active, returns the number of points trails using



    //----euans added code-----
    public static string FirstPlayerToDieName { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ResetFirstDeath()
    {
        FirstPlayerToDieName = null;
    } ///----end---


    //checks if the trail is active, returns the number of points trails using
    public int PointCount()
    {
        if (_trailRenderer != null)
        {
            return this._trailRenderer.positionCount;
        }
        else
        {
            return 0; //else crash, nothing
        }
    }


    private void Awake()
    {
        //goes to unity to find trail renderer component
        _trailRenderer = GetComponent<TrailRenderer>();

        //new object so collider does not move with object, on the pts
        _trailColliderObject = new GameObject(gameObject.name + "_TrailCollider");
        _trailColliderObject.transform.position = Vector3.zero; //to player
       
        _edgeCollider = _trailColliderObject.AddComponent<EdgeCollider2D>();
        _edgeCollider.isTrigger = true; //not solid


        TrailCollisionDetector detector = _trailColliderObject.AddComponent<TrailCollisionDetector>();
        //puts unity physics onto poitns
    }


    private void Update()
    {
        //visual time is trailduration -> 2.5f
        _trailRenderer.time = _trailDuration;

        UpdateColliderPosition();
    }


    private void UpdateColliderPosition()
    {
        int pointCount = _trailRenderer.positionCount; //points from unity
        int headBuffer = 5; //prevents instant spawn death
        if (pointCount < headBuffer)
        {
            return;
        }

        Vector3[] worldPositions = new Vector3[pointCount];
        _trailRenderer.GetPositions(worldPositions);
        //upate new poitns to list, from unity

        _colliderPosition.Clear();

        for (int i = 0; i < pointCount - headBuffer; i++)
        {
            _colliderPosition.Add(worldPositions[i]);
            //wpos points to cpos wo buffer and places colliders on the poitns
        }

        _edgeCollider.SetPoints(_colliderPosition);
    }


    private void OnDestroy()
    {
        //------euans code-----
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

        if (_trailColliderObject != null)
        {
            Destroy(_trailColliderObject);
            //clear colliders when players die
        }
    }

    //encapsulation setter!!!
    public void SetAlive(bool isAlive)
    {
        this._isAlive = isAlive;

        if(isAlive == false)
        {
            OnDestroy();
        }
    }

    //encapsulation getter!!!
    public bool GetAlive()
    {
        return this._isAlive;
    }

}