using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RandomDestinationPicker : MonoBehaviour
{
    [SerializeField] private List<Destination> destinations;
    
    void Start()
    {
        if(destinations.Count == 0 || destinations[0] == null)
        {
            Debug.LogWarning("No destinations found");
            return;
        }
        
        // Pick a random destination
        var randomIndex = Random.Range(0, destinations.Count);
        var destination = destinations[randomIndex];
        destination.SetDestination();   
    }
}
