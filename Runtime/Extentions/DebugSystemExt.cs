using Mitfart.LeoECSLite.UnityIntegration.System;

namespace Mitfart.LeoECSLite.UnityIntegration.Extentions {
  public static class DebugSystemExt {
    public static string GetDebugName(this EcsWorldDebugSystem debugSystem) {
      return !string.IsNullOrWhiteSpace(debugSystem.WorldName) 
        ? $"[ECS-WORLD {debugSystem.WorldName}]" 
        : "[ECS-WORLD]";
    }

    public static int GetWorldSize(this EcsWorldDebugSystem debugSystem) {
      return debugSystem.World.GetWorldSize();
    }
  }
}
