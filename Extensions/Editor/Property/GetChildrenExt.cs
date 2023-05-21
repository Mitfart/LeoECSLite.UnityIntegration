using System;
using System.Collections.Generic;
using UnityEditor;

namespace Extensions.Editor.Property {
  public static class GetChildrenExt {
    public static IList<SerializedProperty> GetChildren(this SerializedProperty property, int maxAmount = int.MaxValue) {
      SerializedProperty rootProperty = property.Copy();
      SerializedProperty currentProp  = property.Copy();

      if (!currentProp.NextVisible(true))
        return Array.Empty<SerializedProperty>();

      var properties = new List<SerializedProperty>();

      do {
        currentProp = currentProp.Copy();

        if (!currentProp.ChildOf(rootProperty))
          break;

        properties.Add(currentProp.Copy());
      }
      while (currentProp.NextVisible(false) && properties.Count < maxAmount);

      return properties;
    }



    private static bool ChildOf(this SerializedProperty curProp, SerializedProperty rootProperty) => curProp.propertyPath.Contains(rootProperty.propertyPath);
  }
}