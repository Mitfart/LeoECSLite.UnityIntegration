using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style {
  public static class OverflowExt {
    public static IStyle OverflowHidden(this IStyle style) {
      style.overflow = Overflow.Hidden;
      return style;
    }

    public static IStyle OverflowVisible(this IStyle style) {
      style.overflow = Overflow.Visible;
      return style;
    }
  }
}