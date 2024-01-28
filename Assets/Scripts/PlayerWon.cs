using UnityEngine.Events;
using UnityEngine;

public class PlayerWon : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryScreen;
    [SerializeField]
    private GameObject scoringGameObject;

    [Space(10)]
    [SerializeField]
    private UnityEvent playerWon;

    private GameManager gameManager;
    private Scoring scoring;

    private void OnEnable()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        if (scoring == null && scoringGameObject != null) scoring = scoringGameObject.GetComponent<Scoring>();

#if UNITY_EDITOR
        else Debug.LogError("scoringGameObject is null");
        if (scoring == null) Debug.LogError($"Scoring is null: {scoring}");
#endif

        gameManager.PlayerWon += Victory;
        victoryScreen.SetActive(false);
        scoringGameObject.SetActive(false);
    }

    private void OnDisable()
    {
        gameManager.PlayerWon -= Victory;
    }

    public void TestVictory()
    {
        Victory();
    }

    private void Victory()
    {
        playerWon.Invoke();
        scoringGameObject.SetActive(true);
        victoryScreen.SetActive(true);
        scoringGameObject.SetActive(true);

        PauseGame();
        scoring.CalculateScore();
    }

    private void UnPauseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
