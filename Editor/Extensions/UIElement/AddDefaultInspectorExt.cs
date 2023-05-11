using UnityEditor;
using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class AddDefaultInspectorExt {
    public static VisualElement AddDefaultInspector(
      this VisualElement root,
      SerializedObject   serializedObject,
      bool               withoutScript = false
    ) {
      if (!withoutScript)
        root.AddScriptField(serializedObject);

      root.AddChildProperties(serializedObject.GetIterator());
      return root;
    }
  }
}