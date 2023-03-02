using UnityEngine;

namespace NodeEngine.Editor.Search {
  public static class SearchWindowUtils {
    public static Texture2D GetIndentationIcon() {
      var icon = new Texture2D(1, 1);
      icon.SetPixel(0, 0, Color.clear);
      icon.Apply();
      return icon;
    }
  }
}
