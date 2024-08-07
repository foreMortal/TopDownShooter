using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewRecordObject", menuName = "ScriptableObjects/RecordScriptableObject", order = 0)]
public class RecordScriptableObject : ScriptableObject
{
    [NonSerialized] public Text pointsText;
    [NonSerialized] public float Record, currentPoints;
    [NonSerialized] public bool RecordRecieved;

    public void ChangeCurrentPoints(float points)
    {
        currentPoints += points;
        pointsText.text = "Points: " + currentPoints.ToString();
    }
    public void SetCurrentPoints(float points)
    {
        currentPoints = points;
        pointsText.text = "Points: " + currentPoints.ToString();
    }
}
