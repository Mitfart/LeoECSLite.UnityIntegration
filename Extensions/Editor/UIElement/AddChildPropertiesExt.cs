using Extensions.Editor.Property;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Git.Extensions.Editor {
  public static class AddChildPropertiesExt {
    public static VisualElement AddChildPropertiesOf(this VisualElement root, SerializedProperty property) {
      foreach (SerializedProperty childProperty in property.GetChildren())
        root.Add(new PropertyField(childProperty));
      return root;
    }
  }
}