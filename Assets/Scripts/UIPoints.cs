using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

public class UIPoints : MonoBehaviour
{

    int displayedPoints;
    public TextMeshProUGUI pointsLabel, endPoints;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnPointsUpdated.AddListener(UpdatedPoints);
    }

    private void UpdatedPoints()
    {
        StartCoroutine(UpdatedPointsCoroutine());
    }

    IEnumerator UpdatedPointsCoroutine()
    {
        while(displayedPoints < GameManager.Instance.Points)
        {
            displayedPoints++;
            pointsLabel.text = displayedPoints.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;

    }

    public void EndGame()
    {
        endPoints.text = pointsLabel.text + " points";
    }
}
