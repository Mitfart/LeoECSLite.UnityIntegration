using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration.Extentions.EcsWorld {
  public static class StaticCache {
    public static int[]         Entities   = new int[32];
    public static object[]      Components = new object[32];
    public static IEcsPool[]    Pools      = new IEcsPool[32];
    public static System.Type[] Types      = new System.Type[32];
  }
}