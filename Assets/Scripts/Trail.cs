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


    [SerializeField] private int maxTrailPoints = 50; //controls the exact length of the trail
    //serializefield just means that the variable is editable in Unity's Inspector

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
        
        //turns off unitys automatic trail time
        _trailRenderer.time = Mathf.Infinity;

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
        TrimTrailLength();
        UpdateColliderPosition();
    }

    private void TrimTrailLength()
    {
        int currentCount = _trailRenderer.positionCount;
        //tracks number of points on trial
        
        if (currentCount > maxTrailPoints)
        {
            Vector3[] allPositions = new Vector3[currentCount];
            _trailRenderer.GetPositions(allPositions);
            //all points into allPosition list

            Vector3[] trimmedPositions = new Vector3[maxTrailPoints];
            System.Array.Copy(allPositions, currentCount - maxTrailPoints, trimmedPositions, 0, maxTrailPoints);
            //keeps only the closet 50 points

            _trailRenderer.Clear();
            _trailRenderer.AddPositions(trimmedPositions);
        }
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