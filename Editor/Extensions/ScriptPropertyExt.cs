using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class ScriptPropertyExt {
    private const string SCRIPT_PROPERTY_NAME = "m_Script";
    
    
    public static void DrawScriptProperty(this SerializedObject serializedObject) {
      var enabled =  GUI.enabled;

      GUI.enabled = false;
      var prop = serializedObject.FindProperty(SCRIPT_PROPERTY_NAME);
      EditorGUILayout.PropertyField(prop, true);
      GUI.enabled = enabled;
    }
    
    public static VisualElement AddScriptProperty(this VisualElement root, SerializedObject serializedObject) {
      root.Add(
        new IMGUIContainer(
          () => {
            if (serializedObject == null) return;
            GUI.enabled = false;

            var prop = serializedObject.FindProperty(SCRIPT_PROPERTY_NAME);
            if (prop == null) return;

            EditorGUILayout.PropertyField(prop, true);
          }));

      return root;
    }
  }
}
