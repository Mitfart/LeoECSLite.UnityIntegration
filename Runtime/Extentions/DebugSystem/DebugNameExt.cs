#if UNITY_EDITOR
using LeoECSLite.UnityIntegration.Extentions.String;

namespace LeoECSLite.UnityIntegration.Extentions.DebugSystem {
  public static class DebugNameExt {
    public static string DebugName(this EcsWorldDebugSystem debugSystem) => debugSystem.WorldName.ToWorldDebugName();
  }
}
#endif