using System.Collections.Generic;
using UnityEditor;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
   public static class GetChildrenExt {
      public static IList<SerializedProperty> GetChildren(this SerializedProperty property, int maxAmount = int.MaxValue) {
         var                properties = new List<SerializedProperty>();
         SerializedProperty prop       = property.Copy();
         SerializedProperty nextProp   = Next(property);

         if (!HasChildren())
            return properties;

         do {
            StoreChild();
         } while (HasNextChild());

         return properties;


         void StoreChild() {
            properties.Add(prop.Copy());
         }

         bool HasChildren() {
            return prop.NextVisible(enterChildren: true)
                && !EqualNextProperty();
         }


         bool HasNextChild() {
            return prop.NextVisible(enterChildren: false)
                && !EqualNextProperty()
                && properties.Count < maxAmount;
         }

         bool EqualNextProperty() {
            return SerializedProperty.EqualContents(prop, nextProp);
         }
      }

      private static SerializedProperty Next(SerializedProperty property) {
         SerializedProperty nextProp = property.Copy();
         nextProp.NextVisible(enterChildren: false);
         return nextProp;
      }
   }
}