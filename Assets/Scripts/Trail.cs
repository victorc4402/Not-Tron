
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TrailRenderer))]
public class Trail : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> colliderPosition = new List<Vector2>();
    private GameObject trailColliderObject;

    [Header("Trail Settings")]
    [SerializeField] private int maxTrailPoints = 50;

    [Header("Death Settings")]
    [SerializeField] private string deathSceneName = "DeathScreen";
    private static bool playerHasDied = false;

    private static string firstPlayerToDie;

    public static string FirstPlayerToDie => firstPlayerToDie;

    public int PointCount => trailRenderer != null
        ? trailRenderer.positionCount
        : 0;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        // Keep the trail visible until its points are trimmed.
        trailRenderer.time = Mathf.Infinity;

        // Create a separate object for the trail collider.
        trailColliderObject = new GameObject(
            gameObject.name + "_TrailCollider"
        );

        trailColliderObject.transform.position = Vector3.zero;

        edgeCollider = trailColliderObject.AddComponent<EdgeCollider2D>();
        edgeCollider.isTrigger = true;

        // Give the detector the configured death scene name.
        TrailCollisionDetector detector =
            trailColliderObject.AddComponent<TrailCollisionDetector>();

        detector.SetDeathSceneName(deathSceneName);
    }

    private void Update()
    {
        TrimTrailLength();
        UpdateColliderPosition();
    }

    private void TrimTrailLength()
    {
        int currentCount = trailRenderer.positionCount;

        if (currentCount > maxTrailPoints)
        {
            Vector3[] allPositions = new Vector3[currentCount];
            trailRenderer.GetPositions(allPositions);

            Vector3[] trimmedPositions = new Vector3[maxTrailPoints];

            System.Array.Copy(
                allPositions,
                currentCount - maxTrailPoints,
                trimmedPositions,
                0,
                maxTrailPoints
            );

            trailRenderer.Clear();
            trailRenderer.AddPositions(trimmedPositions);
        }
    }

    private void UpdateColliderPosition()
    {
        int pointCount = trailRenderer.positionCount;
        int headBuffer = 5;

        // Prevent the player from immediately hitting its own trail.
        if (pointCount < headBuffer + 2)
        {
            return;
        }

        Vector3[] worldPositions = new Vector3[pointCount];
        trailRenderer.GetPositions(worldPositions);

        colliderPosition.Clear();

        for (int i = 0; i < pointCount - headBuffer; i++)
        {
            colliderPosition.Add(worldPositions[i]);
        }

        edgeCollider.SetPoints(colliderPosition);
    }

    private void OnDestroy()
    {
        if (trailColliderObject != null)
        {
            Destroy(trailColliderObject);
        }

    }
    public static void KillPlayer(GameObject player, string sceneName)
    {
        Debug.Log("KillPlayer called for: " + player.name);

        if (playerHasDied)
        {
            Debug.LogWarning("Death blocked: playerHasDied is already true.");
            return;
        }

        if (!player.CompareTag("Player"))
        {
            Debug.LogWarning(player.name + " does not have the Player tag.");
            return;
        }

        firstPlayerToDie = player.name;

        Debug.Log("First player to die: " + firstPlayerToDie);
        Debug.Log("Loading scene: " + sceneName);

        playerHasDied = true;

        SceneManager.LoadScene(sceneName);
    }


    private class TrailCollisionDetector : MonoBehaviour
    {
        private string deathSceneName;

        public void SetDeathSceneName(string sceneName)
        {
            deathSceneName = sceneName;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Only detect collisions with players.
            if (collision.CompareTag("Player"))
            {
                Trail.KillPlayer(collision.gameObject, deathSceneName);
            }

            // Prevent repeated death triggers.
            if (playerHasDied)
            {
                return;
            }
            firstPlayerToDie = collision.gameObject.name;

            playerHasDied = true;

            Debug.Log(collision.gameObject.name + " died");

            // Switch to the death scene.
            SceneManager.LoadScene(deathSceneName);
        }
    }
}
