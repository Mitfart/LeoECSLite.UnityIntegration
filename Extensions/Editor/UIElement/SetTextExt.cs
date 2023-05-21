using UnityEngine.UIElements;

namespace Git.Extensions.Editor {
  public static class SetTextExt {
    public static TextElement SetText(this TextElement label, string text) {
      label.text = text;
      return label;
    }
  }
}