using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighScore : MonoBehaviour 
{
    private Text text;

	private void Start()
	{
        text = GetComponent<Text>();
	}

	private void Update()
	{
        text.text = "High Score: " + PlayerPrefsManager.GetHighScore();
	}
}
