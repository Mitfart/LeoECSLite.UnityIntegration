using System;
using System.Reflection;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class Extensions{
      #region Type

      public static string GetCleanName(this Type type){
         if (!type.IsGenericType) return type.Name;

         var genericIndex = type.Name.LastIndexOf("`", StringComparison.Ordinal);
         var typeName = genericIndex == -1
                           ? type.Name
                           : type.Name[..genericIndex];
         return $"{typeName}";
      }

      #endregion


      #region EcsLite

      private static readonly MethodInfo Get_Pool_Method_Info = typeof(EcsWorld).GetMethod(nameof(EcsWorld.GetPool));
      private static readonly MethodInfo Filter_Method_Info   = typeof(EcsWorld).GetMethod(nameof(EcsWorld.Filter));

      private static readonly MethodInfo Exc_Method_Info =
         typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Exc));

      private static readonly MethodInfo Inc_Method_Info =
         typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Inc));


      public static IEcsPool GetPool(this EcsWorld world, Type type){
         var pool = world.GetPoolByType(type);
         if (pool != null) return pool;

         var getPool = Get_Pool_Method_Info.MakeGenericMethod(type);
         pool = (IEcsPool)getPool.Invoke(world, null);

         return pool;
      }


      public static EcsWorld.Mask Filter(this EcsWorld world, Type type){
         var getFilter = Filter_Method_Info.MakeGenericMethod(type);
         return (EcsWorld.Mask)getFilter.Invoke(world, null);
      }


      public static EcsWorld.Mask Inc(this EcsWorld.Mask mask, Type type){
         var method = Inc_Method_Info.MakeGenericMethod(type);
         return (EcsWorld.Mask)method.Invoke(mask, null);
      }


      public static EcsWorld.Mask Exc(this EcsWorld.Mask mask, Type type){
         var method = Exc_Method_Info.MakeGenericMethod(type);
         return (EcsWorld.Mask)method.Invoke(mask, null);
      }

      #endregion


      #region Editor

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

      #endregion
   }
}
