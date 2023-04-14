using System;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW.Component.Filter {
   public class FilterTagView : Button {
      public FilterTag Tag { get; }

      private Label         _filterMethod;
      private Label         _filterLabel;
      private VisualElement _separator;
      

      public FilterTagView(FilterTag tag) {
         Tag = tag;

         CreateElements();
         AddElements();
         InitElements();
      }

      
      private void CreateElements() {
         _filterMethod = new Label();
         _filterLabel  = new Label();
         _separator    = new VisualElement();
      }

      private void AddElements() {
         this
           .AddChild(_filterMethod)
           .AddChild(_separator)
           .AddChild(_filterLabel);
      }

      private void InitElements() {
         const string include = "In";
         const string exclude = "Ex";
         
         _filterLabel.style.SetMargin(Utils.METRICS_0500, Utils.METRICS_0125);
         _filterLabel.text = Tag.Type.Name;
         
         _filterMethod.text = Tag.FilterMethod switch {
            FilterMethod.Include => include,
            FilterMethod.Exclude => exclude,
            _                    => throw new ArgumentOutOfRangeException()
         };
         _filterMethod.style.SetMargin(Utils.METRICS_0250, Utils.METRICS_0125);
         _filterMethod.style.color    = _filterLabel.style.color    = Utils.Color_DDD;
         _filterMethod.style.fontSize = _filterLabel.style.fontSize = Utils.METRICS_0750;

         style.SetPadding(0);
         style.SetMargin(Utils.METRICS_0125);
         style.SetBorderRadius(Utils.METRICS_0500);
         
         Color color = Utils.GetColorByString(Tag.Type.Name);
         style.flexDirection   = FlexDirection.Row;
         style.alignItems      = Align.Center;
         style.justifyContent  = Justify.FlexStart;
         style.flexWrap        = Wrap.Wrap;
         style.backgroundColor = color;
         style.flexShrink      = 0;

         _separator.style.height          = Utils.GetPercentsLength(100);
         _separator.style.width           = Utils.METRICS_MIN;
         _separator.style.backgroundColor = Utils.Color_DDD;
      }
   }
}
