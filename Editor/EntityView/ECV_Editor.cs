using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.EntityView{
   [CanEditMultipleObjects]
   [CustomEditor(typeof(BaseEcv), true)]
   public class EcvEditor : UnityEditor.Editor{
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

         if (target is not BaseEcv script) return;
         script.UpdateValue();
      }

      public override VisualElement CreateInspectorGUI(){
         var root = new VisualElement();
         if (target is not BaseEcv script) 
            return root;
         
         var property = script.GetValueProperty(serializedObject);

         root
           .AddScriptProperty(serializedObject)
           .AddPropertyVisualElement(property, 1);

         return root;
      }
   }
}
