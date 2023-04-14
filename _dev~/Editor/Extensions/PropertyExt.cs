using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class PropertyExt {
     public static void Draw(
        this SerializedProperty property, 
        bool drawChildren = true, 
        int skipFoldoutsCount = 0) 
     {
         var lastPropPath = string.Empty;

         foreach (SerializedProperty prop in property)
            if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic){
               if (skipFoldoutsCount <= 0){
                  EditorGUILayout.BeginHorizontal();
                  prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName);
                  EditorGUILayout.EndHorizontal();

                  if (!prop.isExpanded) continue;

                  EditorGUI.indentLevel++;
                  Draw(prop);
                  EditorGUI.indentLevel--;
               }
               else{ skipFoldoutsCount--; }

               EditorGUI.indentLevel++;
               Draw(prop);
               EditorGUI.indentLevel--;
            }
            else{
               if (!string.IsNullOrWhiteSpace(lastPropPath) && prop.propertyPath.Contains(lastPropPath)) continue;

               lastPropPath = prop.propertyPath;

               EditorGUILayout.PropertyField(prop, drawChildren);
            }
      }
     
     
     public static VisualElement AddPropertyVisualElement(
        this VisualElement root,
        SerializedProperty property,
        int                skipFoldoutsCount = 0) 
     {
        var lastPropPath = string.Empty;

        foreach (SerializedProperty prop in property)
           if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic){
              if (skipFoldoutsCount <= 0){
                 var foldout = new Foldout{ text = prop.displayName };
                 foldout.AddPropertyVisualElement(prop);
                 root.Add(foldout);
              }
              else{
                 root.AddPropertyVisualElement(prop);
                 skipFoldoutsCount--;
              }
           }
           else{
              if (!string.IsNullOrWhiteSpace(lastPropPath) && prop.propertyPath.Contains(lastPropPath)) continue;

              lastPropPath = prop.propertyPath;
              root.Add(new PropertyField(prop));
           }

        return root;
     }
  }
}
