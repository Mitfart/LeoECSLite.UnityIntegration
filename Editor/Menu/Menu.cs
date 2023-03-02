using Mitfart.LeoECSLite.UnityIntegration.Attributes;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Generator;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window;
using Mitfart.LeoECSLite.UnityIntegration.Generator;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Menu{
   public static class Menu {
      private static EcvGeneratorSettings EcvGeneratorSettings => EcvGeneratorSettings.instance;

      
      
      [MenuItem(MenuPath.DEBUG_WINDOW)]
      public static void OpenEcsDebugWindow() {
         EditorWindow.GetWindow<EcsDebugWindow>(nameof(EcsDebugWindow)).Show();
      }
      
      [MenuItem(MenuPath.GENERATOR_SETTINGS)]
      public static void OpenGeneratorSettingsWindow() {
         EditorWindow.GetWindow<GeneratorSettingsWindow>(nameof(GeneratorSettingsWindow)).Show();
      }
      
      
      [MenuItem(MenuPath.GENERATOR_GENERATE, priority = 11)]
      public static void Generate(){
         Generate(withRefresh: true);
      }
      
      [MenuItem(MenuPath.GENERATOR_GENERATE, true)]
      public static bool Generate_Validate(){
         return !Application.isPlaying;
      }

      [DidReloadScripts]
      public static void Generate__Auto() {
         if (!EcvGeneratorSettings.autoGenerate) return;
         Generate(withRefresh: false);
      }


      
      private static void Generate(bool withRefresh){
         var isModified        = false;
         var generateViewTypes = TypeCache.GetTypesWithAttribute<GenerateView>();

         foreach (var type in generateViewTypes) {
            var viewCreated = EcvGenerator.Create(type, EcvGeneratorSettings);
            isModified = isModified || viewCreated;
         }

         if (isModified && withRefresh) 
            AssetDatabase.Refresh();
      }
   }
}
