using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   [CanEditMultipleObjects]
   [CustomEditor(typeof(BaseECV), true)]
   public class EcvEditor : Editor{
      private void OnEnable(){
         EditorApplication.update -= UpdateValue;
         EditorApplication.update += UpdateValue;
      }
      private void OnDisable(){
         EditorApplication.update -= UpdateValue;
      }

      private void UpdateValue(){
         if (!Application.isPlaying){
            EditorApplication.update -= UpdateValue;
            return;
         }

         if (target is not BaseECV script) return;
         script.UpdateValue();
      }

      public override VisualElement CreateInspectorGUI(){
         var root = new VisualElement();
         if (target is not BaseECV script) 
            return root;
         
         var property = script.GetValueProperty(serializedObject);

         root.AddScriptProperty(serializedObject);
         if (property != null) root.AddPropertyVisualElement(property, 1);

         return root;
      }
   }
}
