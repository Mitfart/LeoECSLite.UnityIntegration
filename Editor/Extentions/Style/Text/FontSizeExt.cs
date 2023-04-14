﻿using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.Style.Text {
  public static class FontSizeExt {
    public static IStyle FontSize(this IStyle style, StyleLength value) {
      style.fontSize = value;
      return style;
    }
  }
}