using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class Obstacle : MonoBehaviour
{

    [SerializeField] private float damage;
    private GameManager gameManager;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] crashSounds;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(gameManager, "Obstacle script must be in the same scene as a GameManager");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        // Damage increased the faster the player is going into the obstacle
        gameManager.OnPlayerDamaged(damage + other.relativeVelocity.magnitude * 0.25f);

        // Play a random crash sound from its list
        audioSource.PlayOneShot(crashSounds[Random.Range(0, crashSounds.Length)]);
    }
}
