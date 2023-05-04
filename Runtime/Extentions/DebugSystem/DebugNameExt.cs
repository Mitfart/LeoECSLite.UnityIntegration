#if UNITY_EDITOR
namespace LeoECSLite.UnityIntegration.Extentions {
  public static class DebugNameExt {
    public static string DebugName(this EcsWorldDebugSystem debugSystem) => debugSystem.WorldName.ToWorldDebugName();
  }
}
#endif