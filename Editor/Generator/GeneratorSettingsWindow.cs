using UnityEditor;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class GeneratorSettingsWindow : EditorWindow{
      private void OnGUI(){
         Editor.CreateEditor(GeneratorSettings.instance).DrawDefaultInspector();
      }

      [MenuItem(MenuPath.GENERATOR_SETTINGS)]
      public static void OpenWindow(){
         GetWindow(typeof(GeneratorSettingsWindow)).Show();
      }
   }
}
