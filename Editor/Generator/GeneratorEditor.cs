using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class GeneratorEditor : Editor{
      private static GeneratorSettings GeneratorSettings => GeneratorSettings.instance;

      [MenuItem(MenuPath.GENERATOR, priority = 11)]
      public static void Generate__Editor(){
         Generate();
      }

      [MenuItem(MenuPath.GENERATOR, true)]
      public static bool Generate__Editor_Validate(){
         return !Application.isPlaying;
      }

      [DidReloadScripts]
      public static void Generate__Auto(){
         if (GeneratorSettings.autoGenerate) Generate();
      }


      private static void Generate(){
         var isModified = false;

         foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
         foreach (var type in assembly.GetTypes()){
            if (!(type.IsValueType && !type.IsPrimitive && !type.IsEnum) ||
                !typeof(IEcsSerializedComponent).IsAssignableFrom(type) ||
                type.IsGenericType ||
                type.IsInterface)
               continue;

            var create = Generator.Create(type, GeneratorSettings);
            isModified = isModified || create;
         }

         if (isModified && !GeneratorSettings.autoGenerate) AssetDatabase.Refresh();
      }
   }
}
