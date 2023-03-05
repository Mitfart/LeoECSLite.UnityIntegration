using System;
using System.Collections.Generic;
using System.Linq;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.NEW.List;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW {
   public sealed class UIListEntity : VisualElement {
      public int        Entity     { get; private set; } = -1;
      public List<Type> Components { get; private set; }
      
      private Label         _tagLabel;
      private Label         _indexLabel;
      private VisualElement _componentsContainer;




      public UIListEntity() {
         CreateElements();
         AddElements();
         InitElements();
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

      
      
      public void Init(int entity, string tag, params Type[] components) {
         if (Entity == entity) return;
         
         Entity           = entity;
         Components       = components.ToList();
         _tagLabel.text   = tag;
         _indexLabel.text = entity.ToString();
         
         foreach (Type component in Components) {
            AddComponent(component);
         }
         

         if (!string.IsNullOrWhiteSpace(  _tagLabel.text)) _tagLabel.style.marginRight   = Utils.METRICS_0500;
         if (!string.IsNullOrWhiteSpace(_indexLabel.text)) _indexLabel.style.marginRight = Utils.METRICS_0500;
      }

      public void Reset() {
         Entity           = -1;
         _indexLabel.text = string.Empty;
         _tagLabel.text   = string.Empty;
         
         Components?.Clear();
         _componentsContainer.Clear();
      }


      private UIListComponent AddComponent(Type type) {
         return _componentsContainer.AddAndGet(GetUIComponentView(type));
      }
      private UIListComponent GetUIComponentView(Type type) {
         return new UIListComponent(type);
      }
   }
}
