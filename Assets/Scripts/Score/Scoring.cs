using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class Scoring : MonoBehaviour
{

    [SerializeField]
    private float timeToWait = 1;
    private float lastTimeUpdated;

    [Space(10)]

    [SerializeField]
    ScoreManager scoreManager;

    [SerializeField]
    private UIDocument m_Document;

    private Label m_Score;
    private GroupBox m_GroupBox;

    private void OnEnable()
    {
        m_Document = gameObject.GetComponent<UIDocument>();

        m_Score = m_Document.rootVisualElement.Q<Label>("TotalScore");
        m_GroupBox = m_Document.rootVisualElement.Q<GroupBox>();
    }

    private bool calculatedScore = false;
    private List<Label> m_Labels = new();
    private int index = 0;
    public void CalculateScore()
    {
        if (!calculatedScore)
        {
            Debug.Log($"TotalScore: {scoreManager.GetTotalScore()}");

            Label scoreLabel = new("Scores:");
            m_Labels.Add(scoreLabel);
            foreach (var score in scoreManager.scores)
            {
                Label newLabel = new($"\t{score.scoreName}: {score.scoreValue}");
                m_Labels.Add(newLabel);
            }
            Label modifiersLabel = new("Score modifiers:");
            m_Labels.Add(modifiersLabel);
            foreach (var modifiers in scoreManager.scoreModifiers)
            {
                Label newLabel = new($"\t{modifiers.scoreName}: {modifiers.scoreValue}");
                m_Labels.Add(newLabel);
            }

            m_GroupBox.Add(m_Labels[index]);
            index++;
            calculatedScore = true;
            lastTimeUpdated = Time.realtimeSinceStartup; 
        }
    }

    private void DisplayScore()
    {
        if (index < m_Labels.Count)
        {
            m_GroupBox.Add(m_Labels[index]);
            index++; 
        }
        else if (index == m_Labels.Count)
        {
            m_Score.text = scoreManager.GetTotalScore().ToString();
        }
    }

    void Update()
    {
        if (calculatedScore && Time.realtimeSinceStartup > lastTimeUpdated + timeToWait)
        {
            DisplayScore();
            lastTimeUpdated = Time.realtimeSinceStartup;
        }
    }
}
