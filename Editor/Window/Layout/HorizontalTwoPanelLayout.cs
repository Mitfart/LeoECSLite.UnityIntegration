using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Layout {
  public sealed class HorizontalTwoPanelLayout : TwoPaneSplitView {
    public VisualElement Left  { get; private set; }
    public VisualElement Right { get; private set; }



    public HorizontalTwoPanelLayout(float startSize = 250f) : base(
      0,
      startSize,
      TwoPaneSplitViewOrientation.Horizontal
    ) {
      CreateElements();
      AddElements();

      //Left.style.minHeight  = Left.style.minWidth  = Utils.METRICS_1250;
      //Right.style.minHeight = Right.style.minWidth = Utils.METRICS_1250;
    }


    private void CreateElements() {
      Left  = new VisualElement { name = "left" };
      Right = new VisualElement { name = "right" };
    }

    private void AddElements()
      => this
        .AddChild(Left)
        .AddChild(Right);
  }
}