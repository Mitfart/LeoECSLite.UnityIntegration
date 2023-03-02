using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Nav{
   public sealed class SplitView : TwoPaneSplitView{
      public readonly VisualElement left;
      public readonly VisualElement right;

      public SplitView() : base(0, 150, TwoPaneSplitViewOrientation.Horizontal){
         left  = this.AddAndGet(new VisualElement{ name = "left" });
         right = this.AddAndGet(new VisualElement{ name = "right" });

         left.style.minHeight  = left.style.minWidth  = Utils.Metrics_Percents_Sss;
         right.style.minHeight = right.style.minWidth = Utils.Metrics_Percents_Sss;
      }
   }
}
