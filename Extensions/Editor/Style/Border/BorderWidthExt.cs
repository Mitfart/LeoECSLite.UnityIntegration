using UnityEngine.UIElements;

namespace Git.Extensions.Editor.Style.Border {
  public static class BorderWidthExt {
    public static IStyle BorderWidth(
      this IStyle style,
      StyleFloat  top,
      StyleFloat  bot,
      StyleFloat  left,
      StyleFloat  right
    ) {
      style.borderTopWidth    = top;
      style.borderBottomWidth = bot;
      style.borderLeftWidth   = left;
      style.borderRightWidth  = right;

      return style;
    }

    public static IStyle BorderWidth(
      this IStyle style,
      StyleFloat  width
    ) {
      style.BorderWidth(width, width, width, width);

      return style;
    }
  }
}