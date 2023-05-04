using System;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration.Extentions {
  public static class StaticCache {
    public static int[]      Entities   = new int[32];
    public static object[]   Components = new object[32];
    public static IEcsPool[] Pools      = new IEcsPool[32];
    public static Type[]     Types      = new Type[32];
  }
}