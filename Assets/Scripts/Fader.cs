using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR 
using UnityEditor;
#endif
using UnityEngine;

public class Fader : MonoBehaviour
{
    private static float alpha;

    public Fade fade;
    public Excludes sceneExcludes;
    public Texture2D texture;

    private int fadeDir;

	private void Awake()
	{
        alpha = (fade.In) ? 1 : 0;
	}

	private void OnGUI()
    {
        alpha += fadeDir * (Time.deltaTime / fade.Time);
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = -1000;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
    }

    public float Fade(FadeDirection fadeDirection)
    {
        fadeDir = (fadeDirection == FadeDirection.In) ? -1 : 1;

        return fade.Time;
    }
}

#region Fade Properties
[Serializable]
public class Fade
{
    public bool In = true;
    public bool Out = true;
    public float Time = 1;
}

#if UNITY_EDITOR 
[CustomPropertyDrawer(typeof(Fade))]
public class FadeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Ensure prefab override logic and disable indented child fields
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var InRect = new Rect(position.x, position.y, 30, position.height);
        var OutRect = new Rect(position.x + 35, position.y, 50, position.height);
        var TimeRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields
        EditorGUIUtility.labelWidth = 15f;
        EditorGUI.PropertyField(InRect, property.FindPropertyRelative("In"));

        EditorGUIUtility.labelWidth = 26f;
        EditorGUI.PropertyField(OutRect, property.FindPropertyRelative("Out"));

        EditorGUIUtility.labelWidth = 36f;
        EditorGUI.PropertyField(TimeRect, property.FindPropertyRelative("Time"));

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif
#endregion

[Serializable]
public class Excludes
{
    public List<string> fadeIn;
    public List<string> fadeOut;
}

public enum FadeDirection
{
    In, Out
}