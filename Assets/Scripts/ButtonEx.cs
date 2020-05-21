using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonEx : MonoBehaviour 
{
    private Button button;

	private void Start()
	{
        button = GetComponent<Button>();
        button.onClick.AddListener(StopMusic);
	}

    private void StopMusic()
    {
        GameObject.Find("Music Player").GetComponent<AudioSource>().Stop();
    }
}
