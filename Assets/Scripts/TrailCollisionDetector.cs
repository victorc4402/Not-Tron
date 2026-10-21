using UnityEngine;

public class TrailCollisionDetector : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) //if collider has player tag
            {
                Debug.Log(collision.gameObject.name + " died");
                Destroy(collision.gameObject);

                Trail playerTrail = collision.GetComponent<Trail>();

                 if (playerTrail != null)
                 {
                    //setting isalive!!!
                    playerTrail.SetAlive(false); 
                    Debug.Log(collision.gameObject.name + " is alive: " + playerTrail.GetAlive());
                }

            }
        }

    }
