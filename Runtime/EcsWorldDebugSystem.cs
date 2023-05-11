#if UNITY_EDITOR
using System;
using LeoECSLite.UnityIntegration.Extensions;
using LeoECSLite.UnityIntegration.Name;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration {
  public sealed class EcsWorldDebugSystem : IEcsPreInitSystem, IEcsRunSystem, IEcsWorldEventListener {
    public string   DebugName { get; }
    public string   WorldName { get; }
    public EcsWorld World     { get; private set; }

    public EcsWorldView View { get; }

    public int WorldSize { get; private set; }



    public EcsWorldDebugSystem(string worldName = null, NameSettings nameSettings = null) {
      WorldName = worldName;
      DebugName = worldName.ToWorldDebugName();

      View = new EcsWorldView(this, nameSettings);
    }

    public void PreInit(IEcsSystems systems) {
      InitWorld(systems);

      WorldSize = World.GetEntitiesCount();

      World.AddEventListener(this);
      ActiveDebugSystems.Register(this);

      InitEntities();
    }



    public void Run(IEcsSystems systems) => View.Refresh();

    public void OnEntityCreated(int   e) => View.GetEntityView(e).Activate();
    public void OnEntityChanged(int   e) => View.GetEntityView(e).MarkDirty();
    public void OnEntityDestroyed(int e) => View.GetEntityView(e).Deactivate();

    public void OnWorldResized(int newSize) => View.Resize(WorldSize = newSize);

    public void OnWorldDestroyed(EcsWorld world) {
      View.Destroy();
      World.RemoveEventListener(this);
      ActiveDebugSystems.Unregister(this);
    }

    public void OnFilterCreated(EcsFilter filter) { }



    private void InitWorld(IEcsSystems systems) => World = systems.GetWorld(WorldName) ?? throw new Exception($"Cant find required world! ({WorldName})");
    private void InitEntities()                 => World.ForeachEntity(OnEntityCreated);
  }
}

#endif