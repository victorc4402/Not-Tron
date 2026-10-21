using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenControls : MonoBehaviour
{
    private void Awake()
    {
        Trail.ResetFirstDeath();
        PlayerMovement.ResetOutOfBoundsRecord();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SceneManager.LoadScene("GameScene");
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("GameScene");
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }
}