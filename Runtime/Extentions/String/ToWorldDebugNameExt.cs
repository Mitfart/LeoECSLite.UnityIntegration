namespace LeoECSLite.UnityIntegration.Extentions.String {
  public static class ToWorldDebugNameExt {
    public static string ToWorldDebugName(this string name) {
      return !string.IsNullOrWhiteSpace(name)
        ? $"[ECS-WORLD {name}]"
        : "[ECS-WORLD]";
    }
  }
}