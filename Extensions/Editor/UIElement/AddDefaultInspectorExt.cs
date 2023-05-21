using UnityEditor;
using UnityEngine.UIElements;

namespace Git.Extensions.Editor {
  public static class AddDefaultInspectorExt {
    public static VisualElement AddDefaultInspector(
      this VisualElement root,
      SerializedObject   serializedObject,
      bool               withoutScript = false
    ) {
      if (!withoutScript)
        root.AddScriptField(serializedObject);

      root.AddChildPropertiesOf(serializedObject.GetIterator());
      return root;
    }
  }
}