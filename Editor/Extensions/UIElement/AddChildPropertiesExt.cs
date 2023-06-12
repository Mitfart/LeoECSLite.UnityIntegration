using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class AddChildPropertiesExt {
    public static VisualElement AddChildPropertiesOf(this VisualElement root, SerializedProperty property) {
      foreach (SerializedProperty childProperty in property.GetChildren())
        root.Add(new PropertyField(childProperty));
      return root;
    }
  }
}