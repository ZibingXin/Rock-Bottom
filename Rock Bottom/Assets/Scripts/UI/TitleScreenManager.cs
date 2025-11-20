using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject guide;
    public void StartGame()
    {
        SceneManager.LoadScene("LevelGeneratorTest");
    }

    public void ResetLevels()
    {
        PlayerPrefs.DeleteAll();
    }

    public void ShowGuide()
    {
        guide.SetActive(true);
    }

    public void HideGuide()
    {
        guide.SetActive(false);
    }

    // Called when Quit button is pressed
    public void QuitGame()
    {
        Application.Quit();
    }
}