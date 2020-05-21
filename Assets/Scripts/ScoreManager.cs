using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreManager : MonoBehaviour 
{
	private static float score = 0;
    private static float min;
    private static float max;

    public float minScore = 0;
    public float maxScore = float.PositiveInfinity;
    public bool resetOnLoad = false;

	private Text text;

    private void Awake()
	{
		text = GetComponent<Text>();
        min = minScore;
        max = maxScore;
        if (resetOnLoad) Reset();
	}

    private void Update()
	{
        text.text = "Score: " + score;
	}

    public static void Plus(float value)
	{
		if (score < max)
        {
            float difference = max - score;
            score += (value <= difference) ? value : difference;

            if (score > PlayerPrefsManager.GetHighScore())
            {
                PlayerPrefsManager.SetHighScore(score);
            }
        }
	}

    public static void Minus(float value)
    {
        if (score > min)
        {
            float difference = score - min;
            score -= (value <= difference) ? value : difference;
        }
    }

	public void Reset()
	{
		score = 0;
	}
}
