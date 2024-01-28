using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField]
    private float buttonWaitTime = .5f;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameControls gameControls;

    private bool isPaused = false;
    private float lastTimeUpdated;

    private void OnEnable()
    {
        gameControls = new();

        pauseScreen.SetActive(false);

        gameControls.Other.Enable();
    }
    
    private void OnDisable()
    {
        gameControls.Other.Disable();
    }

    private void Update()
    {
        if (gameControls.Other.Pause.IsPressed() && Time.unscaledTime > lastTimeUpdated + buttonWaitTime)
        {
            if (isPaused)
            {
                UnPauseTheGame();
            }
            else
            {
                PauseTheGame();
            }
            lastTimeUpdated = Time.unscaledTime;
        }
    }

    public void UnPauseTheGame()
    {
        pauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isPaused = false;
    }

    public void PauseTheGame()
    {
        pauseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isPaused = true;
    }
}
