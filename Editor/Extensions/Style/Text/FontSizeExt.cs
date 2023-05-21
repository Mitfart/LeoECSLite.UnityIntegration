using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Plugins.Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style.Text {
  public static class FontSizeExt {
    public static IStyle FontSize(this IStyle style, StyleLength value) {
      style.fontSize = value;
      return style;
    }
  }
}