using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style {
  public static class PaddingExt {
    public static IStyle Padding(
      this IStyle style,
      StyleLength lengthTop,
      StyleLength lengthBottom,
      StyleLength lengthLeft,
      StyleLength lengthRight
    ) {
      style.paddingTop    = lengthTop;
      style.paddingBottom = lengthBottom;
      style.paddingLeft   = lengthLeft;
      style.paddingRight  = lengthRight;

      return style;
    }


    public static IStyle Padding(
      this IStyle style,
      StyleLength hor,
      StyleLength ver
    )
      => style
       .Padding(
          ver,
          ver,
          hor,
          hor
        );


    public static IStyle Padding(this IStyle style, StyleLength length)
      => style
       .Padding(
          length,
          length
        );
  }
}