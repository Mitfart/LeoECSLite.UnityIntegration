using UnityEngine.UIElements;

namespace Git.Extensions.Editor.Style.Size {
  public static class SquareExt {
    public static IStyle Square(
      this IStyle style,
      StyleLength size
    ) {
      style.width  = size;
      style.height = size;

      return style;
    }
  }
}