using System;
using Mitfart.LeoECSLite.UnityIntegration.Attributes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class GeneratorEditor : Editor{
      private static GeneratorSettings GeneratorSettings => GeneratorSettings.instance;

      [MenuItem(MenuPath.Generator, priority = 11)]
      public static void Generate__Editor(){
         Generate(true);
      }

      [MenuItem(MenuPath.Generator, true)]
      public static bool Generate__Editor_Validate(){
         return !Application.isPlaying;
      }

      [DidReloadScripts]
      public static void Generate__Auto(){
         if (GeneratorSettings.autoGenerate) Generate(false);
      }


      private static void Generate(bool withRefresh){
         var isModified = false;

         foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
         foreach (var type in assembly.GetTypes()){
            if (type.GetCustomAttributes(typeof(GenerateView), false).Length <= 0 ||
                type.IsGenericType)
               continue;

            var create = Generator.Create(type, GeneratorSettings);
            isModified = isModified || create;
         }

         if (isModified && withRefresh) AssetDatabase.Refresh();
      }
   }
}
