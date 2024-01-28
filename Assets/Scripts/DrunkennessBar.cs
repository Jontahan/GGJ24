using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class DrunkennessBar : MonoBehaviour
{
    [SerializeField]
    private Image image;

    private void Awake()
    {
        image = gameObject.GetComponentsInChildren<Image>().Last();
    }

    private float targetProgressBarValue;
    private float currentValue;
    public void ChangeDrunkenness(float value)
    {
        if (value < 0)
        {
            value = 0;
        }
        else if (value > 100)
        {
            value = 100;
        }

        targetProgressBarValue = (int)Math.Round(value);
        currentValue = (int)Math.Round(image.fillAmount * 100);
        Debug.Log($"currentValue: {currentValue}, name: {image.name}");
        Debug.Log($"targetProgressBarValue: {targetProgressBarValue}");
    }

    public void Update()
    {
        if (targetProgressBarValue != currentValue)
        {
            if (targetProgressBarValue > image.fillAmount)
            {
                currentValue++;
            }
            else
            {
                currentValue--;
            }

            image.fillAmount = currentValue/100;
            Debug.Log($"Value: {currentValue/100}");
        }
    }
}
