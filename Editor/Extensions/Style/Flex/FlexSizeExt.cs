using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extensions.Flex {
  public static class FlexSizeExt {
    public static IStyle FlexGrow(this IStyle style, bool grow = true) {
      style.flexGrow = grow
        ? 1
        : 0;
      return style;
    }

    public static IStyle FlexShrink(this IStyle style, bool grow = true) {
      style.flexShrink = grow
        ? 1
        : 0;
      return style;
    }
  }
}