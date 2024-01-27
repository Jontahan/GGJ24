using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action PlayerWon;
    
    public void OnPlayerReachedDestination()
    {
        PlayerWon?.Invoke();
        Debug.Log("Player won");
    }
}
