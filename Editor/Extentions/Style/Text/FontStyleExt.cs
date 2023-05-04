using UnityEngine;
using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.Text {
  public static class FontStyleExt {
    public static IStyle FontStyle(this IStyle style, FontStyle fontStyle) {
      style.unityFontStyleAndWeight = fontStyle;
      return style;
    }
  }
}