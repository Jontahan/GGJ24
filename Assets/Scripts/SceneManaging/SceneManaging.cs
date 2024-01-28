using Scene = UnityEngine.SceneManagement.Scene;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManaging : MonoBehaviour
{
    public void LoadSceneByIndex(int index)
    {
        UnPauseGame();
        SceneManager.LoadScene(index);
    }

    public void LoadFirstScene()
    {
        UnPauseGame();
        SceneManager.LoadScene(0);
    }

    public void LoadSceneByName(string name)
    {
        UnPauseGame();
        SceneManager.LoadScene(name);
    }

    public Scene GetActiveScene()
    {
        UnPauseGame();
        return SceneManager.GetActiveScene();
    }

    public void RestartCurrentScene()
    {
        UnPauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene()
    {
        UnPauseGame();
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(++activeScene);
    }

    public void LoadPreviousScene()
    {
        UnPauseGame();
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(--activeScene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("ExitingGame");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        UnPauseGame();
        Application.Quit();
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }
}
