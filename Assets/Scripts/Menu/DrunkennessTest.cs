using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine;
using System;

public class DrunkennessTest : MonoBehaviour
{
    [SerializeField]
    private UIDocument m_UIDocument;
    
    private ProgressBar m_ProgressBar;

    //Custom ProgressBar
    private VisualElement m_LoadingProgressBar;
    private StyleLength barProgress;

    private void OnEnable()
    {
        if (m_UIDocument == null)
        {
            m_UIDocument = gameObject.GetComponent<UIDocument>();
        }
        VisualElement rootElement = m_UIDocument.rootVisualElement;
        m_ProgressBar = rootElement.Q<ProgressBar>();
        m_LoadingProgressBar = rootElement.Q<VisualElement>("bar_Progress");
    }
    
    private int targetProgressBarValue;
    private int currentValue;
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
        currentValue = (int)m_ProgressBar.value;
        Debug.Log($"targetProgressBarValue: {targetProgressBarValue}");
    }

    public void Update()
    {
        if (targetProgressBarValue != currentValue)
        {
            if (targetProgressBarValue > m_ProgressBar.value)
            {
                currentValue++;
            }
            else
            {
                currentValue--;
            }

            m_ProgressBar.value = currentValue;

            //m_LoadingProgressBar.style.width = (barProgress / 100) * currentValue;
            Debug.Log($"Value: {currentValue}");
        }
    }
}
