using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration.Extentions.EcsWorld {
  public static class StaticCache {
    public static readonly int[]         Entities   = new int[32];
    public static readonly object[]      Components = new object[32];
    public static readonly IEcsPool[]    Pools      = new IEcsPool[32];
    public static readonly System.Type[] Types      = new System.Type[32];
  }
}