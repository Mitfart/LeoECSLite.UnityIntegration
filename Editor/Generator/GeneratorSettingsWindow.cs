using Mitfart.LeoECSLite.UnityIntegration.Generator;
using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Generator{
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
         UnityEditor.Editor.CreateEditor(_settings).DrawDefaultInspector();
      }
   }
}
