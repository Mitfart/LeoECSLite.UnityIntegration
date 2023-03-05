using System;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW {
   public class UIComponent : Foldout {
      public Type    ComponentType { get; }
      public BaseEcv MonoView      { get; }
      
      private Label            _label;
      private InspectorElement _inspector;



      public UIComponent(BaseEcv monoCompView) {
         MonoView      = monoCompView;
         ComponentType = MonoView.GetComponentType();
         
         CreateElements();
         AddElements();
         InitElements();
      }

      private void CreateElements() {
         _label     = new Label();
         _inspector = new InspectorElement(MonoView);
      }

      private void AddElements() {
         this
           .AddChild(_label)
           .AddChild(_inspector)
            ;
      }

      private void InitElements() {
         _label.text = ComponentType.Name;
      }
   }
}
