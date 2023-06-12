#if UNITY_EDITOR
namespace Mitfart.LeoECSLite.UnityIntegration.Name {
  public class NameSettings {
    public bool   BakeComponents { get; }
    public string Format         { get; }

    public NameSettings(bool bakeComponents = false, string format = null) { // "X8"
      BakeComponents = bakeComponents;
      Format         = format;
    }
  }
}
#endif