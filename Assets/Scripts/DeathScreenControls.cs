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
        string activeScene = SceneManager.GetActiveScene().name;

        if (activeScene == "TitleScreen")
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SceneManager.LoadScene("GameScene");
            }
            else if (Input.GetKeyDown(KeyCode.RightShift))
            {
                SceneManager.LoadScene("Player1SkinChanger");
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SceneManager.LoadScene("Player2SkinChanger");
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

        if (activeScene == "Player1SkinChanger")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                PlayerMovement.SelectSkin1(1);
                SceneManager.LoadScene("TitleScreen");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                PlayerMovement.SelectSkin2(1);
                SceneManager.LoadScene("TitleScreen");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                PlayerMovement.SelectDefaultSkin(1);
                SceneManager.LoadScene("TitleScreen");
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SceneManager.LoadScene("TitleScreen");
            }

            return;
        }

        if (activeScene == "Player2SkinChanger")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                PlayerMovement.SelectSkin1(2);
                SceneManager.LoadScene("TitleScreen");
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                PlayerMovement.SelectSkin2(2);
                SceneManager.LoadScene("TitleScreen");
            }

            else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                PlayerMovement.SelectDefaultSkin(2);
                SceneManager.LoadScene("TitleScreen");
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SceneManager.LoadScene("TitleScreen");
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