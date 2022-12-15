using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class Extentions{
      public static T AddAndGet<T>(this VisualElement root, T visualElement) where T : VisualElement{
         root.Add(visualElement);
         return visualElement;
      }

      public static T AddChild<T, TChild>(this T root, TChild visualElement)
         where T : VisualElement where TChild : VisualElement{
         root.Add(visualElement);
         return root;
      }
      
      
      

      public static Type GetPropertyDrawer(this Type classType){
         var assembly               = Assembly.GetAssembly(typeof(Editor));
         var scriptAttributeUtility = assembly.CreateInstance("UnityEditor.ScriptAttributeUtility");
         if (scriptAttributeUtility == null) return null;

         var scriptAttributeUtilityType = scriptAttributeUtility.GetType();

         const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
         var getDrawerTypeForType = scriptAttributeUtilityType.GetMethod("GetDrawerTypeForType", bindingFlags);

         if (getDrawerTypeForType == null) return null;

         return (Type)getDrawerTypeForType.Invoke(scriptAttributeUtility, new object[]{ classType });
      }


      public static void Draw(this SerializedProperty property, bool drawChildren = true, int skipFoldoutsCount = 0){
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
         int                skipFoldoutsCount = 0){
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


      public static void DrawScriptProperty(this SerializedObject serializedObject){
         var enabled =  GUI.enabled;

         GUI.enabled = false;
         var prop = serializedObject.FindProperty("m_Script");
         EditorGUILayout.PropertyField(prop, true);
         GUI.enabled = enabled;
      }
      public static VisualElement AddScriptProperty(this VisualElement root, SerializedObject serializedObject){
         root.Add(
            new IMGUIContainer(
               () => {
                  if (serializedObject == null) return;
                  GUI.enabled = false;

                  var prop = serializedObject.FindProperty("m_Script");
                  if (prop == null) return;

                  EditorGUILayout.PropertyField(prop, true);
               }));

         return root;
      }
   }
}
