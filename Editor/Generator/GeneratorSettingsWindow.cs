using Mitfart.LeoECSLite.UnityIntegration.Generator;
using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Generator{
   public class GeneratorSettingsWindow : EditorWindow{
      private static EcvGeneratorSettings SETTINGS;
      
      
      
      private void OnEnable(){
         SETTINGS           =  EcvGeneratorSettings.instance;
         SETTINGS.hideFlags &= ~HideFlags.NotEditable;
      }

      private void OnGUI(){
         UnityEditor
           .Editor
           .CreateEditor(SETTINGS)
           .DrawDefaultInspector();

         bool enabled = GUI.enabled;
         GUI.enabled = SETTINGS.IsDirty;
         if (GUILayout.Button(nameof(EcvGeneratorSettings.Save)))
            SETTINGS.Save();
         GUI.enabled = enabled;
      }
   }
}
