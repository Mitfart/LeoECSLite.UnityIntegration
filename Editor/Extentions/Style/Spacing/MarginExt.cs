﻿using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.Style.Spacing {
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
    ) {
      return style
       .Margin(
          ver,
          ver,
          hor,
          hor
        );
    }

    public static IStyle Margin(this IStyle style, StyleLength length) {
      return style
       .Margin(
          length,
          length
        );
    }
  }
}