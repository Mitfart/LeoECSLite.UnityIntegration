using Mitfart.LeoECSLite.UnityIntegration.Generator;
using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Generator{
   public class GeneratorSettingsWindow : EditorWindow{
      private static EcvGeneratorSettings _settings;
      
      
      
      private void OnEnable(){
         _settings           =  EcvGeneratorSettings.instance;
         _settings.hideFlags &= ~HideFlags.NotEditable;
      }

      private void OnGUI(){
         UnityEditor
           .Editor
           .CreateEditor(_settings)
           .DrawDefaultInspector();
      }
   }
}
