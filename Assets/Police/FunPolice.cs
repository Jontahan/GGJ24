using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class FunPolice : MonoBehaviour
{
    [SerializeField] private float  reactionTime;
    
    private NavMeshAgent agent;
    private CarBehavior playerCar;
    private GameManager gameManager;
    
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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        gameManager.OnPlayerLost();
        
    }
}
