using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style {
  public static class PercentsLengthExt {
    public static StyleLength Percents(this float length)
      => new(
        new Length(
          length,
          LengthUnit.Percent
        )
      );

    public static StyleLength Percents(this int length) => ((float) length).Percents();
  }
}