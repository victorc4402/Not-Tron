using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Trail : MonoBehaviour
{
    //encapsulation!!!
    private TrailRenderer _trailRenderer;
    private EdgeCollider2D _edgeCollider;
    private List<Vector2> _colliderPosition = new List<Vector2>();
    private GameObject _trailColliderObject;

    private bool _isAlive = true;


    private float _trailDuration = 2.5f; //controls the exact length of the trail, unity timer

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
        //visual time is trailduration -> `1f
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

    public void PlayerDies()
    {
        if (_trailColliderObject != null)
        {
            Destroy(_trailColliderObject);

            //clears when players die
        }
    }


    //encapsulation setter!!!
    public void SetAlive(bool isAlive)
    {
        this._isAlive = isAlive;

        if(isAlive == false)
        {
            PlayerDies();
        }
    }

    //encapsulation getter!!!
    public bool GetAlive()
    {
        return this._isAlive;
    }

}