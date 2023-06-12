using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style {
  public static class FontStyleExt {
    public static IStyle FontStyle(this IStyle style, FontStyle fontStyle) {
      style.unityFontStyleAndWeight = fontStyle;
      return style;
    }
  }
}