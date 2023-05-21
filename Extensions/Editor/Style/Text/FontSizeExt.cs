using UnityEngine.UIElements;

namespace Git.Extensions.Editor.Style.Text {
  public static class FontSizeExt {
    public static IStyle FontSize(this IStyle style, StyleLength value) {
      style.fontSize = value;
      return style;
    }
  }
}