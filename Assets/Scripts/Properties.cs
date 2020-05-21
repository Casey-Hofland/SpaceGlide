using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

// 1. DOES NOT DISABLE ARRAY SIZE EDITING!!!
// 2. DOES NOT SUPPORT CHAR VALUES!!!
// 3. DOES NOT SUPPORT COMPONENTS!!!
#region ReadOnly

/// <summary>
/// Disables the editing of the variable in the inspector.
/// <para>NOTE: unfinished version still allows array size changes and does not support chars.</para>
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR 
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Calculate display value
        string valueStr = "";

        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                valueStr = property.longValue.ToString();
                break;

            case SerializedPropertyType.Boolean:
                valueStr = property.boolValue.ToString();
                break;

            case SerializedPropertyType.Float:
                valueStr = property.doubleValue.ToString();
                break;

            case SerializedPropertyType.String:
                valueStr = property.stringValue;
                break;

            default:
                valueStr = "(not supported)";
                break;
        }

        // Draw display value in the Inspector
        EditorGUI.LabelField(position, label.text, valueStr);
    }
}
#endif

#endregion

// 1. CANNOT BE ASSIGNED A VALUE AT INSTANTIATION!!!
// 2. NEEDS SUMMARY!!!
#region decimal Inspector

[System.Serializable]
public class Decimal : ISerializationCallbackReceiver
{
    public decimal value;
    private int[] data;

    public void OnBeforeSerialize()
    {
        data = decimal.GetBits(value);
    }

    public void OnAfterDeserialize()
    {
        if (data != null && data.Length == 4)
        {
            value = new decimal(data);
        }
    }
}

#if UNITY_EDITOR 
[CustomPropertyDrawer(typeof(Decimal))]
public class SerializableDecimalDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var obj = property.serializedObject.targetObject;
        var inst = (Decimal)this.fieldInfo.GetValue(obj);
        var fieldRect = EditorGUI.PrefixLabel(position, label);
        string text = EditorGUI.DelayedTextField(fieldRect, inst.value.ToString());

        if (GUI.changed)
        {
            decimal val;
            if (decimal.TryParse(text, out val) || string.IsNullOrEmpty(text))
            {
                inst.value = val;
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif

#endregion

