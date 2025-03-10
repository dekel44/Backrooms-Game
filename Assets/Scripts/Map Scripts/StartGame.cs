using UnityEngine;
using UnityEngine.SceneManagement; // Required to load scenes

public class StartGame : MonoBehaviour
{
    // This method is called by the Start button onClick event
    public void OnStartButtonPressed()
    {
        // Make sure "GameScene" is the exact name of your playable scene
        SceneManager.LoadScene("SampleScene");
    }

    void Start()
    {
         Cursor.lockState = CursorLockMode.None;  // Unlock the mouse
         Cursor.visible = true;                   // Show the cursor

    }
}
