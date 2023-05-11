using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class SetTextExt {
    public static TextElement SetText(this TextElement label, string text) {
      label.text = text;
      return label;
    }
  }
}