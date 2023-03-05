using System;
using System.Collections.Generic;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW {
   public class UIEntity : VisualElement {
      private readonly Dictionary<BaseEcv, UIComponent> _uiComponentViews = new();

      public MonoEntityView MonoView { get; private set; }
      public int            Entity   => MonoView.Entity;
      
      private ScrollView _labelContainer;
      private Label         _tagLabel;
      private Label         _indexLabel;
      private VisualElement _componentsContainer;


      
      public UIEntity() {
         CreateElements();
         AddElements();
         InitElements();
      }


      private void CreateElements() {
         _labelContainer      = new ScrollView(ScrollViewMode.Horizontal);
         _tagLabel            = new Label();
         _indexLabel          = new Label();
         _componentsContainer = new VisualElement();
      }

      private void AddElements() {
         this
           .AddChild(
               _labelContainer
                 .AddChild(_tagLabel)
                 .AddChild(_indexLabel)
            )
           .AddChild(_componentsContainer);
      }

      private void InitElements() {
         style.SetBorderRadius(Utils.METRICS_0250);
         
         _labelContainer.style.backgroundColor = Utils.Color_L;
         
         _tagLabel.style.SetMargin(0);
         _tagLabel.style.SetPadding(Utils.METRICS_1500, Utils.METRICS_0500);
         _tagLabel.style.opacity   = .75f;
         _indexLabel.style.SetMargin(0);
         _indexLabel.style.SetPadding(Utils.METRICS_1500, Utils.METRICS_0500);
         _indexLabel.style.opacity = .75f;
         
         _tagLabel.style.display   = DisplayStyle.Flex;
         _indexLabel.style.display = DisplayStyle.Flex;

         _componentsContainer.style.SetMargin(Utils.METRICS_0750);
         _componentsContainer.style.SetPadding(Utils.METRICS_0750);
         _componentsContainer.style.SetBorderRadius(Utils.METRICS_0750);
         _componentsContainer.style.backgroundColor = Utils.Color_DD;
      }

      
      public UIEntity Init(MonoEntityView monoView) {
         if (MonoView == monoView) return this;

         MonoView         = monoView;
         _tagLabel.text   = MonoView.Tag;
         _indexLabel.text = Entity.ToString();
         
         MonoView.OnAddComponent    += AddComponent;
         MonoView.OnRemoveComponent += RemoveComponentView;
         
         AddAllComponents();
         
         if (string.IsNullOrWhiteSpace(  _tagLabel.text)) _tagLabel.style.display   = DisplayStyle.None;
         if (string.IsNullOrWhiteSpace(_indexLabel.text)) _indexLabel.style.display = DisplayStyle.None;
         
         return this;
      }

      
      public void Reset() {
         _indexLabel.text = string.Empty;
         _tagLabel.text   = string.Empty;
         
         MonoView.OnAddComponent    -= AddComponent;
         MonoView.OnRemoveComponent -= RemoveComponentView;
         
         _componentsContainer.Clear();
      }

      
      
      private void AddComponent(BaseEcv monoCompView) {
         UIComponent comp = _componentsContainer.AddAndGet(GetUIComponentView(monoCompView));
         _uiComponentViews.Add(monoCompView, comp);
      }
      
      private void RemoveComponentView(BaseEcv monoCompView) {
         if (!_uiComponentViews.TryGetValue(monoCompView, out UIComponent uiComp)) return;
         
         _uiComponentViews.Remove(monoCompView);
         Remove(uiComp);
      }
      

      private UIComponent GetUIComponentView(BaseEcv monoCompView) {
         return new UIComponent(monoCompView);
      }
      
      
      
      private void AddAllComponents() {
         foreach (BaseEcv componentView in MonoView.Components.Values)
            AddComponent(componentView);
      }
   }
}
