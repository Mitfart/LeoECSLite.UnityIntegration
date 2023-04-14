using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.Style.Size {
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