using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject scoringGameObject;

    [Space(10)]
    [SerializeField]
    private UnityEvent playerLost;

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

        gameManager.PlayerLost += PlayerLost;
        gameOverScreen.SetActive(false);
        scoringGameObject.SetActive(false);
    }

    private void OnDisable()
    {
        gameManager.PlayerLost -= PlayerLost;
    }

    private void PlayerLost()
    {
        playerLost.Invoke();
        scoringGameObject.SetActive(true);
        gameOverScreen.SetActive(true);
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
