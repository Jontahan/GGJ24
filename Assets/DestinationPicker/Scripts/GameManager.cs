using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action PlayerWon;
    public event Action PlayerLost;
    public event Action<float> PlayerDamaged;
    
    public void OnPlayerReachedDestination()
    {
        PlayerWon?.Invoke();
        Debug.Log("Player won");
    }
    
    public void OnPlayerDamaged(float damage)
    {
        PlayerDamaged?.Invoke(damage);
    }
    
    public void OnPlayerLost()
    {
        PlayerLost?.Invoke();
        Debug.LogError("Player lost");
    }
}
