using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunnyJuice : MonoBehaviour
{
    [SerializeField] private GameObject hoveredEffect;
    
    public void Highlight()
    {
       if(!hoveredEffect.activeSelf)
           hoveredEffect.SetActive(true);
    }
    
    public void Unhighlight()
    {
        if(hoveredEffect.activeSelf)
            hoveredEffect.SetActive(false);
    }

    public void Drink()
    {
        Debug.Log("Drinking funny juice");
    }
}
