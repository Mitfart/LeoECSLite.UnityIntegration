using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class GeneratorSettingsWindow : EditorWindow{
      private static GeneratorSettingsWindow _window;
      
      [MenuItem(MenuPath.GENERATOR_SETTINGS)]
      public static void OpenWindow(){
         _window = GetWindow<GeneratorSettingsWindow>(nameof(GeneratorSettingsWindow));
         _window.Show();
      }

      private void OnGUI(){
         var settings = GeneratorSettings.instance;
         settings.hideFlags &= ~HideFlags.NotEditable;

         Editor.CreateEditor(settings).DrawDefaultInspector();
      }
   }
}
