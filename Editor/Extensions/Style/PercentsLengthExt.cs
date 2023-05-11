using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class PercentsLengthExt {
    public static StyleLength PercentsLength(this float length)
      => new(
        new Length(
          length,
          LengthUnit.Percent
        )
      );

    public static StyleLength PercentsLength(this int length) => ((float) length).PercentsLength();
  }
}