using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeBar : MonoBehaviour
{

    public RectTransform fillRect;
    public Image fillColor;
    public Gradient gradient;
    bool able;

    private void Start()
    {
        StopTime();
    }

    void Update()
    {
        if (able)
        {
            float factor = GameManager.Instance.currentTimeToMatch / GameManager.Instance.timeToMatch;
            factor = Mathf.Clamp(factor, 0f, 1f);
            factor = 1 - factor;
            fillRect.localScale = new Vector3(factor, 1, 1);
            fillColor.color = gradient.Evaluate(factor);
        }        
    }

    public void StartTime()
    {
        able = true;
    }

    public void StopTime()
    {
        able = false;
    }
}
