using System.Collections.Generic;
using UnityEditor;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class GetChildrenExt {
    public static IList<SerializedProperty> GetChildren(this SerializedProperty property, int maxAmount = int.MaxValue) {
      var                properties = new List<SerializedProperty>();
      SerializedProperty prop       = property.Copy();

      if (!HasProperties())
        return properties;

      do
        StoreProperty();
      while (HasNextProperty());

      return properties;


      bool HasProperties() {
        return prop.NextVisible(true);
      }

      void StoreProperty() {
        properties.Add(prop.Copy());
      }

      bool HasNextProperty() {
        return prop.NextVisible(false) && properties.Count < maxAmount;
      }
    }
  }
}