using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions {
  public static class SetTextExt {
    public static TElement SetText<TElement>(this TElement target, string text)
      where TElement : TextElement {
      target.text = text;
      return target;
    }
  }
}