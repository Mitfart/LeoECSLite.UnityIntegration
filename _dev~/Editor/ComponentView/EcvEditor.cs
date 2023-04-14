using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using UnityEditor;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.ComponentView{
   [CanEditMultipleObjects]
   [CustomEditor(typeof(BaseEcv), true)]
   public class EcvEditor : BaseEcvEditor{
      protected override void CreateEditor(VisualElement root) {
         root
           .AddScriptProperty(serializedObject)
           .AddPropertyVisualElement(((BaseEcv) target).GetValueProperty(serializedObject), 1);
      }

      
      protected override void OnEditorUpdate() {
         if (target is BaseEcv ecv) ecv.UpdateValue();
      }
   }
}
