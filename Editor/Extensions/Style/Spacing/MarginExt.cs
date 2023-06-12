using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style {
  public static class MarginExt {
    public static IStyle Margin(
      this IStyle style,
      StyleLength top,
      StyleLength bottom,
      StyleLength left,
      StyleLength right
    ) {
      style.marginTop    = top;
      style.marginBottom = bottom;
      style.marginLeft   = left;
      style.marginRight  = right;

      return style;
    }

    public static IStyle Margin(
      this IStyle style,
      StyleLength hor,
      StyleLength ver
    )
      => style
       .Margin(
          ver,
          ver,
          hor,
          hor
        );

    public static IStyle Margin(this IStyle style, StyleLength length)
      => style
       .Margin(
          length,
          length
        );
  }
}