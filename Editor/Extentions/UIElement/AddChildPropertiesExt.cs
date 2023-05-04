using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions {
  public static class AddChildPropertiesExt {
    public static void AddChildProperties(this VisualElement root, SerializedProperty property) {
      SerializedProperty rootProperty = property.Copy();
      SerializedProperty curProp      = property.Copy();

      if (!curProp.NextVisible(true))
        return;

      do {
        curProp = curProp.Copy();

        if (!curProp.ChildOf(rootProperty))
          break;

        root.Add(new PropertyField(curProp));
      }
      while (curProp.NextVisible(false));
    }

    private static bool ChildOf(this SerializedProperty curProp, SerializedProperty rootProperty) => curProp.propertyPath.Contains(rootProperty.propertyPath);
  }
}