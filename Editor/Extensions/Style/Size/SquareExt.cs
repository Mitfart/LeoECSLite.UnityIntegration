using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Plugins.Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style.Size {
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