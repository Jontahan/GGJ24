using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Slider healthBar;
    private GameManager gameManager;
    
    private const float MaxHealth = 100f;
    
    
    void Start()
    {
        healthBar = GetComponent<Slider>();
        Assert.IsNotNull(healthBar, "PlayerHealth script must be attached to a GameObject with a Slider component");
        
        gameManager = FindObjectOfType<GameManager>();
        Assert.IsNotNull(gameManager, "PlayerHealth script must be in the same scene as a GameManager");
        
        gameManager.PlayerDamaged += TakeDamage;
    }

    private void TakeDamage(float damage)
    {
        healthBar.value -= (damage / MaxHealth);
    }
}
