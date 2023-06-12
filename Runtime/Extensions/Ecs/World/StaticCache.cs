using System;
using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityIntegration.Extensions {
  public static class StaticCache {
    public static readonly int[]      Entities   = new int[32];
    public static readonly object[]   Components = new object[32];
    public static readonly IEcsPool[] Pools      = new IEcsPool[32];
    public static readonly Type[]     Types      = new Type[32];
  }
}