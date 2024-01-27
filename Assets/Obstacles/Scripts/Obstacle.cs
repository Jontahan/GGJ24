using UnityEngine;
using UnityEngine.Assertions;

public class Obstacle : MonoBehaviour
{

    [SerializeField] private float damage;
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Assert.IsNotNull(gameManager, "Obstacle script must be in the same scene as a GameManager");
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        
        // Damage increased the faster the player is going into the obstacle
        gameManager.OnPlayerDamaged(damage + other.relativeVelocity.magnitude);
    }
}
