using Scene = UnityEngine.SceneManagement.Scene;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManaging : MonoBehaviour
{
    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public Scene GetActiveScene()
    {
        return SceneManager.GetActiveScene();
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(++activeScene);
    }

    public void LoadPreviousScene()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(--activeScene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("ExitingGame");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
