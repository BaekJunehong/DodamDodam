using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    public void Change()
    {
        switch (gameObject.name)
        {
            case "login_button":
                SceneManager.LoadScene("Camera");
                break;
            case "game_start_button":
                SceneManager.LoadScene("straight");
                break;

            case "title_button":
                SceneManager.LoadScene("Title");
                break;

            case "retry_button":
                SceneManager.LoadScene("Game");
                break;

            default:
                Debug.LogWarning("No matching scene found for object name: " + gameObject.name);
                break;
        }
    }
}
