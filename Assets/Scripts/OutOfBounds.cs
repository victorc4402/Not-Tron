
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private Camera mainCamera;
    private bool deathTriggered = false;
    private float nextLogTime = 0f;

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No MainCamera found!");
        }
    }

    private void Update()
    {
        if (deathTriggered || mainCamera == null)
            return;

        Vector3 screenPosition =
            mainCamera.WorldToViewportPoint(transform.position);

        // Log each player's position once per second.
        if (Time.time >= nextLogTime)
        {
            Debug.Log(
                gameObject.name +
                " viewport position: " + screenPosition
            );

            nextLogTime = Time.time + 1f;
        }

        // Check whether this player is outside the screen.
        if (screenPosition.x < 0f ||
            screenPosition.x > 1f ||
            screenPosition.y < 0f ||
            screenPosition.y > 1f)
        {
            Debug.Log(gameObject.name + " went out of bounds!");

            deathTriggered = true;

            Trail.KillPlayer(gameObject, "DeathScreen");
        }
    }
}
