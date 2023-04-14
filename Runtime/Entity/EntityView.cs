#if UNITY_EDITOR
using Leopotam.EcsLite;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Entity {
  public sealed class EntityView : MonoBehaviour {
    public EcsWorld World  { get; private set; }
    public int      Entity { get; private set; }



    public EntityView Construct(EcsWorld world, int entity) {
      World  = world;
      Entity = entity;
      return this;
    }


    public void Activate() {
      gameObject.SetActive(true);
    }

    public void Deactivate() {
      gameObject.SetActive(false);
    }
  }
}
#endif