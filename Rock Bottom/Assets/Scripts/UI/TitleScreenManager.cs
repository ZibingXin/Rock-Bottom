using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelGeneratorTest");
    }

    public void ResetLevels()
    {
        PlayerPrefs.DeleteAll();
    }

    // Called when Quit button is pressed
    public void QuitGame()
    {
        Application.Quit();
    }
}