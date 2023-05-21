using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Plugins.Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.UIElement {
  public static class SetTextExt {
    public static TextElement SetText(this TextElement label, string text) {
      label.text = text;
      return label;
    }
  }
}