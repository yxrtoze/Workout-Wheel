using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Function to load the game scene (index 1)
    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene(1);  // Load the scene at index 1 (Main Game)
    }
    public void OnExitMainButtonClick()
    {
        SceneManager.LoadScene(0);  // Load the scene at index 1 (Main Game)
    }
    // Function to exit the application
    public void OnExitButtonClick()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}
