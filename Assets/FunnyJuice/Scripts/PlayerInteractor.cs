using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionRay : MonoBehaviour
{
    [SerializeField] private LayerMask interactionMask;

    private GameObject previousObjectHit;
    
    void Update()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, interactionMask))
        {
            if(previousObjectHit != null && previousObjectHit.TryGetComponent(out FunnyJuice previousFunnyJuice))
                previousFunnyJuice.Unhighlight();
            
            return;
        }

        if (hit.collider.TryGetComponent(out FunnyJuice funnyJuice))
        {
            previousObjectHit = funnyJuice.gameObject;
            funnyJuice.Highlight();  
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                funnyJuice.Drink();
            }
        }

        // Stop highlighting the previous object if we hit a new one
        if (hit.collider.transform.gameObject != previousObjectHit)
        {
            if(previousObjectHit.TryGetComponent(out FunnyJuice previousFunnyJuice))
                previousFunnyJuice.Unhighlight();
        }
    }
}
