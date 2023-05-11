using LeoECSLite.UnityAdapter.Editor.Extensions.Property;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class AddChildPropertiesExt {
    public static VisualElement AddChildProperties(this VisualElement root, SerializedProperty property) {
      foreach (SerializedProperty childProperty in property.GetChildren())
        root.Add(new PropertyField(childProperty));
      return root;
    }
  }
}