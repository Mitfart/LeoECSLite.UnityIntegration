using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Git.Extensions.Editor {
  public static class AddScriptFieldExt {
    private const string UNITY_SCRIPT = "m_Script";

    public static VisualElement AddScriptField(this VisualElement root, SerializedObject serializedObject) {
      SerializedProperty scriptProperty = serializedObject.FindProperty(UNITY_SCRIPT);

      if (scriptProperty == null)
        throw new Exception("Cant find Script property");

      var propertyField = new PropertyField(scriptProperty);
      propertyField.SetEnabled(false);

      root.Add(propertyField);
      return root;
    }
  }
}