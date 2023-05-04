using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.Style.Flex {
  public static class FlexDirectionExt {
    public static IStyle FlexRow(this IStyle style, bool reverse = false) {
      style.flexDirection = !reverse
        ? UnityEngine.UIElements.FlexDirection.Row
        : UnityEngine.UIElements.FlexDirection.RowReverse;
      return style;
    }

    public static IStyle FlexColumn(this IStyle style, bool reverse = false) {
      style.flexDirection = !reverse
        ? UnityEngine.UIElements.FlexDirection.Column
        : UnityEngine.UIElements.FlexDirection.ColumnReverse;
      return style;
    }
  }
}