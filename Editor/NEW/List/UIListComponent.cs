using System;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW.List {
   public class UIListComponent : VisualElement {
      private readonly Type  _type;
      private readonly Color _color;
      private readonly bool  _compactName;
      
      private Label _label;
      
      

      public UIListComponent(Type type, bool compactName = false) {
         _type        = type;
         _color       = Utils.GetColorByString(type.Name);
         _compactName = compactName;

         CreateElements();
         AddElements();
         InitElements();
      }

      private void CreateElements() {
         _label = new Label();
      }

      private void AddElements() { Add(_label); }

      private void InitElements() {
         RegisterHover();
         
         _label.text           = _compactName ? _type.Name.Compact() : _type.Name;
         _label.style.color    = Utils.Color_DDD;
         _label.style.fontSize = Utils.METRICS_0750;
         
         style.SetMargin(Utils.METRICS_0125);
         style.SetPadding(Utils.METRICS_0250, Utils.METRICS_0125);
         style.SetBorderRadius(Utils.METRICS_0500);
         style.SetBorderWidth(Utils.METRICS_MIN);
         
         style.flexDirection   = FlexDirection.Row;
         style.alignItems      = Align.Center;
         style.backgroundColor = _color;
         style.flexShrink      = 0;
      }
      
      
      private void RegisterHover() {
         RegisterCallback<MouseOverEvent>(_ => Hover());
         RegisterCallback<MouseOutEvent>(_ => Unhover());
      }

      
      private void Hover() {
         style.backgroundColor = Utils.Color_Primary_L;
         style.SetBorderColor(Utils.Color_Primary_LL);
      }

      private void Unhover() {
         style.backgroundColor = _color;
         style.SetBorderColor(new StyleColor(StyleKeyword.None));
      }
   }
}
