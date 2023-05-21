using UnityEngine;
using UnityEngine.UIElements;

namespace Git.Extensions.Editor.Style.Text {
  public static class FontStyleExt {
    public static IStyle FontStyle(this IStyle style, FontStyle fontStyle) {
      style.unityFontStyleAndWeight = fontStyle;
      return style;
    }
  }
}