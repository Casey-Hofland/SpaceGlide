using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour 
{
    private const string PREVIOUS_SCENE_KEY = "Previous Scene";
    private const string HIGH_SCORE_KEY = "High Score";

    public static void SetPreviousScene()
    {
        PlayerPrefs.SetString(PREVIOUS_SCENE_KEY, SceneManager.GetActiveScene().name);
    }

    public static string GetPreviousScene()
    {
        return PlayerPrefs.GetString(PREVIOUS_SCENE_KEY);
    }

    public static void SetHighScore(float score)
    {
        PlayerPrefs.SetFloat(HIGH_SCORE_KEY, score);
    }

    public static float GetHighScore()
    {
        return PlayerPrefs.GetFloat(HIGH_SCORE_KEY);
    }
}
