using System;
using UnityEngine;
using UnityEngine.Assertions;

public class Destination : MonoBehaviour
{
    [SerializeField] private GameObject visualTransform;
    [SerializeField] private LineRenderer laserbeam;
    [SerializeField] private float laserbeamHeight;
    
    private Rigidbody rb;
    private GameManager gameManager;
    private bool isPicked = false;
    private bool isReached = false;
    
    public void SetDestination()
    {
        gameManager = FindObjectOfType<GameManager>();
        Assert.IsNotNull(gameManager, "GameManager is null");

        laserbeam.SetPosition(0, transform.position);
        laserbeam.SetPosition(1, new Vector3(transform.position.x, transform.position.y + laserbeamHeight, 
            transform.position.z));
        
        isPicked = true;
        visualTransform.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))return;
        
        // We don't care if its not the destination the RandomDestinationPicker chose
        if(!isPicked)return;    
    
        isReached = true;
        
        // If the player enters the destination, we let the gameManager knows
        gameManager.OnPlayerReachedDestination();
    }
}
