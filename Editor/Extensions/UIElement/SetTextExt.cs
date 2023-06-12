using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class SetTextExt {
    public static TextElement SetText(this TextElement label, string text) {
      label.text = text;
      return label;
    }
  }
}