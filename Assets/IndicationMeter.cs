using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class IndicationMeter : MonoBehaviour
{
    [SerializeField]
    private bool useMax = true;
    [SerializeField]
    private float timeToWait;
    [SerializeField]
    private string title;

    public float max = 0;
    public float min = 0;

    private float _value;
    private static readonly int digits = 2;

    /// <summary>
    /// Value from 0 to inclusive 1
    /// </summary>
    public float Value { get { return _value; } set { ChangeProgressBar(value); } }

    [SerializeField]
    private GameObject m_title;
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject m_value;
    private TextMeshProUGUI valueText;
    [SerializeField]
    private GameObject m_frame;
    [SerializeField]
    private GameObject m_meter;
    private Image meterImage;
    [SerializeField]
    private GameObject m_predictionMeter;
    private RectTransform m_predictionMeterRect;

    private float predictionAnchorPosY;
    private float progressBarWidth;
    private float currentValue = 0;

    public IndicationMeter(bool useMax, float timeToWait, float max, float min, float startValue = 0, string title = "Indication bar")
    {
        this.useMax = useMax;
        this.timeToWait = timeToWait;
        this.max = max;
        this.min = min;
        this._value = startValue;
        this.title = title;
    }

    private void OnEnable()
    {
        SetupStructure();

        progressBarWidth = m_meter.GetComponent<RectTransform>().rect.width;
        m_predictionMeterRect = m_predictionMeter.GetComponent<RectTransform>();

        float positionOfPrediction;
        if (progressBarWidth * targetProgressBarValue > m_predictionMeterRect.rect.width)
        {
            positionOfPrediction = (progressBarWidth * targetProgressBarValue - m_predictionMeterRect.rect.width / 2);
        }
        else
        {
            positionOfPrediction = (m_predictionMeterRect.rect.width / 2);
        }
        
        m_predictionMeterRect.anchoredPosition = new(positionOfPrediction, predictionAnchorPosY);
        valueText.text = (Mathf.RoundToInt(currentValue * 100)).ToString();
        meterImage.fillAmount = currentValue;
    }

    private void SetupStructure()
    {
        if (m_title == null) m_title = GameObject.Find("Title");
        if (m_title == null) Debug.LogError("No GameObject named 'Title' found");
        else titleText = m_title.GetComponent<TextMeshProUGUI>();
        if (titleText != null) titleText.text = title;
        if (m_value == null) m_value = GameObject.Find("Value");
        if (m_value == null) Debug.LogError("No GameObject named 'Value' found");
        else valueText = m_value.GetComponent<TextMeshProUGUI>();
        if (m_frame == null) m_frame = GameObject.Find("Frame");
        if (m_frame == null) Debug.LogError("No GameObject named 'Frame' found");
        if (m_meter == null) m_meter = GameObject.Find("Meter");
        if (m_meter == null) Debug.LogError("No GameObject named 'Meter' found");
        else meterImage = m_meter.GetComponent<Image>();
        if (m_predictionMeter == null) m_predictionMeter = GameObject.Find("PredictionMeter");
        if (m_predictionMeter == null) Debug.LogError("No GameObject named 'PredictionMeter' found");
    }

    private float targetProgressBarValue;
    private float lastTimeUpdated;

    private void ChangeProgressBar(float value)
    {
        if (useMax) value = (float)Math.Round(Mathf.Clamp01(value), digits);
        else if (value < 0) value = 0;
        targetProgressBarValue = (float)Math.Round(value, digits);
        currentValue = (float)Math.Round(meterImage.fillAmount, digits);

        float positionOfPrediction;
        if (progressBarWidth * targetProgressBarValue > m_predictionMeterRect.rect.width/* / 2*/)
        {
            positionOfPrediction = (progressBarWidth * targetProgressBarValue - m_predictionMeterRect.rect.width / 2);
        }
        else
        {
            positionOfPrediction = (m_predictionMeterRect.rect.width / 2);
        }
        m_predictionMeterRect.anchoredPosition = new(positionOfPrediction, predictionAnchorPosY);
        valueText.text = (Mathf.RoundToInt(currentValue * 100)).ToString();

        lastTimeUpdated = Time.realtimeSinceStartup;
    }

    public void FixedUpdate()
    {
        if (Time.realtimeSinceStartup > lastTimeUpdated + timeToWait)
        {
            UpdateMeter();
            lastTimeUpdated = Time.realtimeSinceStartup;
        }
    }

    private void UpdateMeter()
    {
        if (targetProgressBarValue != (float)Math.Round(currentValue, digits))
        {
            if (targetProgressBarValue > meterImage.fillAmount)
            {
                currentValue += .01f;
            }
            else
            {
                currentValue -= .01f;
            }

            currentValue = (float)Math.Round(currentValue, digits);
            valueText.text = (Mathf.RoundToInt(currentValue * 100)).ToString();
            meterImage.fillAmount = currentValue;
        }
        else
        {
            _value = targetProgressBarValue;
        }
    }
}
