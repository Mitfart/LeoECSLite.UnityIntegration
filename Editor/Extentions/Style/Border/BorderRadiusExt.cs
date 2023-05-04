using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.Border {
  public static class BorderRadiusExt {
    public static IStyle BorderRadius(
      this IStyle style,
      StyleLength tr,
      StyleLength br,
      StyleLength bl,
      StyleLength tl
    ) {
      style.borderTopRightRadius    = tr;
      style.borderBottomRightRadius = br;
      style.borderBottomLeftRadius  = bl;
      style.borderTopLeftRadius     = tl;

      return style;
    }

    public static IStyle BorderRadius(
      this IStyle style,
      StyleLength length
    ) {
      style.BorderRadius(length, length, length, length);

      return style;
    }
  }
}