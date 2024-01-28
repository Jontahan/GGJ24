using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class FunPolice : MonoBehaviour
{
    [SerializeField] private float reactionTime;
    [SerializeField] private float damage;

    private NavMeshAgent agent;
    private CarBehavior playerCar;
    private GameManager gameManager;

    private bool isColliding = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Assert.IsNotNull(gameManager, "FunPolice script must be in the same scene as a GameManager");

        agent = GetComponent<NavMeshAgent>();
        Assert.IsNotNull(agent, "FunPolice script must be attached to a GameObject with a NavMeshAgent component");

        playerCar = FindObjectOfType<CarBehavior>();

        InvokeRepeating(nameof(GoToPlayer), 0f, reactionTime);
    }

    private void GoToPlayer()
    {
        agent.SetDestination(playerCar.transform.position);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        Debug.Log("Police collided with player");


        // Damage increased the faster the player is going into the police
        if (!isColliding)
        {
            isColliding = true;
            Debug.Log("Doing damage");
            gameManager.OnPlayerDamaged(damage);
            // Wait a a second before allowing the police to damage the player again
            StartCoroutine(ResetCollision());

        }

    }

    private IEnumerator ResetCollision()
    {
        yield return new WaitForSeconds(1f);
        isColliding = false;
    }
}
