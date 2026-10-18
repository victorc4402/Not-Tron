
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private Camera mainCamera;
    private bool deathTriggered = false;

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No MainCamera found.");
        }
    }

    private void Update()
    {
        if (deathTriggered || mainCamera == null ||
            deathDetector == null)
        {
            return;
        }

        Vector3 screenPosition =
            mainCamera.WorldToViewportPoint(transform.position);

        // Kill the player when outside any screen boundary.
        if (screenPosition.x < 0f ||
            screenPosition.x > 1f ||
            screenPosition.y < 0f ||
            screenPosition.y > 1f)
        {
            deathTriggered = true;
            deathDetector.KillPlayer(gameObject);
        }
    }
}
