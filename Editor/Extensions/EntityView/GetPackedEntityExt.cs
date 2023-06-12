using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.View;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class GetPackedEntityExt {
    public static EcsPackedEntity          PackedEntity(this          EntityView entityView) => entityView.World.PackEntity(entityView.Entity);
    public static EcsPackedEntityWithWorld PackedEntityWithWorld(this EntityView entityView) => entityView.World.PackEntityWithWorld(entityView.Entity);
  }
}