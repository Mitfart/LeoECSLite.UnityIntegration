using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class GeneratorSettingsWindow : EditorWindow{
      private static GeneratorSettingsWindow _window;
      private static GeneratorSettings       _settings;
      
      [MenuItem(MenuPath.Generator_Settings)]
      public static void OpenWindow(){
         _window = GetWindow<GeneratorSettingsWindow>(nameof(GeneratorSettingsWindow));
         _window.Show();
      }
      
      private void OnEnable(){
         _settings           =  GeneratorSettings.instance;
         _settings.hideFlags &= ~HideFlags.NotEditable;
      }

      private void OnGUI(){
         Editor.CreateEditor(_settings).DrawDefaultInspector();
      }
   }
}
