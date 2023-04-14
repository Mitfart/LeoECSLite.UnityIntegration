using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.Style {
  public static class PercentsLengthExt {
    public static StyleLength PercentsLength(this float length) {
      return new StyleLength(
        new Length(
          length,
          LengthUnit.Percent
        )
      );
    }

    public static StyleLength PercentsLength(this int length) {
      return ((float) length).PercentsLength();
    }
  }
}