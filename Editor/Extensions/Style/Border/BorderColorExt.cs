using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style {
  public static class BorderColorExt {
    public static IStyle BorderColor(
      this IStyle style,
      StyleColor  top,
      StyleColor  bot,
      StyleColor  left,
      StyleColor  right
    ) {
      style.borderTopColor    = top;
      style.borderBottomColor = bot;
      style.borderLeftColor   = left;
      style.borderRightColor  = right;

      return style;
    }

    public static IStyle BorderColor(
      this IStyle style,
      StyleColor  color
    ) {
      style.BorderColor(color, color, color, color);

      return style;
    }
  }
}