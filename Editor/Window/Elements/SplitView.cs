using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class SplitView : TwoPaneSplitView{
      public readonly VisualElement Left;
      public readonly VisualElement Right;

      public SplitView() : base(0, 150, TwoPaneSplitViewOrientation.Horizontal){
         Left  = this.AddAndGet(new VisualElement{ name = "left" });
         Right = this.AddAndGet(new VisualElement{ name = "right" });

         Left.style.minHeight  = Left.style.minWidth  = Styles.Metrics_Percents_Sss;
         Right.style.minHeight = Right.style.minWidth = Styles.Metrics_Percents_Sss;
      }
   }
}
