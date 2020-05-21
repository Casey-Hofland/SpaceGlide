using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Fader))]
public class SceneChanger : MonoBehaviour 
{
    public bool autoLoad;
    public string autoLoadName;
    public float autoLoadTime;

    private Fader fader;
    private float delay;

	private void OnValidate()
	{
        autoLoadTime = Mathf.Max(0, autoLoadTime);
	}

	private void Awake()
	{
        fader = FindObjectOfType<Fader>();
        delay = 0;
	}

	private void Start()
	{
        if (autoLoad) 
        {
            delay = autoLoadTime;
            if (string.IsNullOrEmpty(autoLoadName))
            {
                LoadNextScene();
            }
            else
            {
                LoadScene(autoLoadName); 
            }
        }
	}

	private void OnEnable()
	{
        SceneManager.sceneLoaded += SceneLoaded;
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    private void OnDisable()
	{
        SceneManager.sceneLoaded -= SceneLoaded;
	}

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
        string previousScene = PlayerPrefsManager.GetPreviousScene();
        if (fader.fade.In && !fader.sceneExcludes.fadeIn.Contains(previousScene))
        {
            fader.Fade(FadeDirection.In);
        }
	}

    public void LoadScene(string name)
    {
        StartCoroutine(ChangeScene(name));
    }

    public void LoadScene(int builtIndex)
    {
        string nextSceneName = SceneUtilityEx.GetSceneNameByBuildIndex(builtIndex);
        StartCoroutine(ChangeScene(nextSceneName));
    }

    public void LoadNextScene()
    {
        string nextSceneName = SceneUtilityEx.GetNextSceneName();
        StartCoroutine(ChangeScene(nextSceneName));
    }

    IEnumerator ChangeScene(string name)
    {
        yield return new WaitForSeconds(delay);
        if (fader.fade.Out && !fader.sceneExcludes.fadeOut.Contains(name))
        {
            yield return new WaitForSeconds(fader.Fade(FadeDirection.Out));
        }
        PlayerPrefsManager.SetPreviousScene();
        SceneManager.LoadScene(name);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

public static class SceneUtilityEx
{
    public static string GetNextSceneName()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            return GetSceneNameByBuildIndex(nextSceneIndex);
        }

        return string.Empty;
    }

    public static string GetSceneNameByBuildIndex(int buildIndex)
    {
        return GetSceneNameFromScenePath(SceneUtility.GetScenePathByBuildIndex(buildIndex));
    }

    private static string GetSceneNameFromScenePath(string scenePath)
    {
        // Unity's asset paths always use '/' as a path separator
        var sceneNameStart = scenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
        var sceneNameEnd = scenePath.LastIndexOf(".", StringComparison.Ordinal);
        var sceneNameLength = sceneNameEnd - sceneNameStart;
        return scenePath.Substring(sceneNameStart, sceneNameLength);
    }
}
