﻿using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.Style.Overflow {
  public static class OverflowExt {
    public static IStyle OverflowHidden(this IStyle style) {
      style.overflow = UnityEngine.UIElements.Overflow.Hidden;
      return style;
    }
    
    public static IStyle OverflowVisible(this IStyle style) {
      style.overflow = UnityEngine.UIElements.Overflow.Visible;
      return style;
    }
  }
}