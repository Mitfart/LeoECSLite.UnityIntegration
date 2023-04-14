using System;
using System.Collections.Generic;
using System.Linq;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.NEW.Component;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW.Entity.List {
   public sealed class UIListEntity : VisualElement {
      private readonly Dictionary<BaseEcv, UIComponentLabel> _uiComponentViews = new();

      public MonoEntityView MonoView { get; private set; }
      public int            Entity   => MonoView.Entity;
      
      private Label         _tagLabel;
      private Label         _indexLabel;
      private VisualElement _componentsContainer;

      

      public UIListEntity() {
         CreateElements();
         AddElements();
         InitElements();
      }
      
      
      
      public void Init(MonoEntityView monoView) {
         if (monoView == MonoView) return;
         MonoView = monoView;
         
         MonoView.OnAddComponent    += AddComponent;
         MonoView.OnRemoveComponent += RemoveComponent;
         
         InitEntityLabel();
         AddAllComponents();
      }

      public void Reset() {
         _indexLabel.text = string.Empty;
         _tagLabel.text   = string.Empty;

         if (MonoView != null) {
            MonoView.OnAddComponent    -= AddComponent;
            MonoView.OnRemoveComponent -= RemoveComponent;
         }
         
         _uiComponentViews?.Clear();
         _componentsContainer?.Clear();
      }
      
      
      
      private void CreateElements() {
         _tagLabel            = new Label();
         _indexLabel          = new Label();
         _componentsContainer = new VisualElement();
      }

      private void AddElements() {
         this
           .AddChild(_tagLabel)
           .AddChild(_indexLabel)
           .AddChild(_componentsContainer)
            ;
      }

      private void InitElements() {
         style.flexDirection  = FlexDirection.Row;
         style.justifyContent = _componentsContainer.style.justifyContent = Justify.FlexStart;
         style.alignItems     = _componentsContainer.style.alignItems     = Align.Center;      
         style.SetBorderRadius(Utils.METRICS_0250);
         
         _componentsContainer.style.flexDirection  = FlexDirection.Row;
         _componentsContainer.style.justifyContent = _componentsContainer.style.justifyContent = Justify.FlexStart;
         _componentsContainer.style.alignItems     = _componentsContainer.style.alignItems     = Align.Center;      
      }

      
      private void AddAllComponents() {
         foreach (BaseEcv componentView in MonoView.Components.Values)
            AddComponent(componentView);
      }
      
      

      private void AddComponent(BaseEcv monoComp) {
         UIComponentLabel comp = _componentsContainer.AddAndGet(CreateComponent(monoComp));
         _uiComponentViews.Add(monoComp, comp);
      }
      
      private UIComponentLabel CreateComponent(BaseEcv monoComp) {
         return new UIComponentLabel(monoComp.GetComponentType());
      }
      
      private void RemoveComponent(BaseEcv monoComp) {
         if (!_uiComponentViews.TryGetValue(monoComp, out UIComponentLabel uiComp)) return;
         
         _uiComponentViews.Remove(monoComp);
         Remove(uiComp);
      }
      
      
      private void InitEntityLabel() {
         _tagLabel.text   = MonoView.Tag;
         _indexLabel.text = MonoView.Entity.ToString();
         
         
         _indexLabel.style.marginRight = 
            string.IsNullOrWhiteSpace(_indexLabel.text) 
               ? 0
               : Utils.METRICS_0500;
         
         _tagLabel.style.marginRight = 
            string.IsNullOrWhiteSpace(_tagLabel.text) 
               ? 0
               : Utils.METRICS_0500;
      }
   }
}
