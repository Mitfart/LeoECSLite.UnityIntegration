namespace Mitfart.LeoECSLite.UnityIntegration.Plugins.Mitfart.LeoECSLite.UnityIntegration.Runtime.Extensions.String {
  public static class ToWorldDebugNameExt {
    public static string ToWorldDebugName(this string name)
      => !string.IsNullOrWhiteSpace(name)
        ? $"[ECS-WORLD {name}]"
        : "[ECS-WORLD]";
  }
}