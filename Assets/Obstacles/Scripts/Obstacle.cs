using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class Obstacle : MonoBehaviour
{

    [SerializeField] private float damage;
    private GameManager gameManager;
    private AudioSource audioSource;
    private AudioSource playerAudioSource;

    [SerializeField] private AudioClip[] crashSounds;

    // Need to add it in this script to ensure they don't both use the same audio source
    [SerializeField] private AudioClip[] playerScreamSounds;



    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        var _audioSources = GetComponents<AudioSource>();
        audioSource = _audioSources[0];
        playerAudioSource = _audioSources[1];
        Assert.IsNotNull(gameManager, "Obstacle script must be in the same scene as a GameManager");
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            // Damage increased the faster the player is going into the obstacle
            gameManager.OnPlayerDamaged(damage + other.relativeVelocity.magnitude * 0.25f);
        }

        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Police"))
        {
            // Play a random crash sound from its list
            audioSource.PlayOneShot(crashSounds[Random.Range(0, crashSounds.Length)]);

            // If human (due to time constraint if it has scream sound effects)
            if (playerScreamSounds.Length > 0)
            {
                playerAudioSource.PlayOneShot(playerScreamSounds[Random.Range(0, playerScreamSounds.Length)]);
            }
        }

    }
}
