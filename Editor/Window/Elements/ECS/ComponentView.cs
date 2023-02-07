using System;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Buttons;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.ECS{
   public sealed class ComponentView : VisualElement{
      public override  VisualElement      contentContainer{ get; }
      
      private readonly FoldoutWithButtons _contentFoldout;

      public Type             ComponentType     { get; private set; }
      public BaseEcv          MonoView          { get; private set; }
      public InspectorElement ComponentInspector{ get; private set; }

      public string Label{ get => _contentFoldout.text; set => _contentFoldout.text = value; }

      
      
      public ComponentView(){
         contentContainer = this;
         contentContainer =
            _contentFoldout =
               this.AddAndGet(new ContentBox())
                   .AddAndGet(new FoldoutWithButtons());

         _contentFoldout.AddButton(Icons.Reload, () => { if (MonoView != null) MonoView.UpdateValue(); });
         _contentFoldout.AddButton(Icons.Close,  () => { if (MonoView != null) MonoView.Delete();      });
      }

      

      public ComponentView Init(BaseEcv monoCompView){
         MonoView      = monoCompView;
         ComponentType = MonoView.GetComponentType();
         Label         = ComponentType.Name;

         if (ComponentInspector != null) Remove(ComponentInspector);
         ComponentInspector = this.AddAndGet(new InspectorElement(monoCompView));

         return this;
      }
      
      public new class UxmlFactory : UxmlFactory<ComponentView, UxmlTraits>{}
   }
}
