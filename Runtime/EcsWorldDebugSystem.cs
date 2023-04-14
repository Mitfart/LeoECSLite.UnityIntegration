#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using LeoECSLite.UnityIntegration.Extentions.EcsWorld;
using LeoECSLite.UnityIntegration.Name;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration {
  public sealed class EcsWorldDebugSystem : IEcsPreInitSystem, IEcsRunSystem, IEcsWorldEventListener {
    public string   WorldName { get; }
    public EcsWorld World     { get; private set; }

    public NameSettings            NameSettings { get; }
    public EcsWorldDebugSystemView View         { get; private set; }

    public HashSet<int> DirtyEntities { get; }



    public EcsWorldDebugSystem(string worldName = null, NameSettings nameSettings = null) {
      WorldName    = worldName;
      NameSettings = nameSettings ?? new NameSettings();

      DirtyEntities = new HashSet<int>();
    }

    public void PreInit(IEcsSystems systems) {
      InitWorld(systems);
      InitView();
      InitEntities();

      World.AddEventListener(this);
      ActiveDebugSystems.Register(this);
    }

    public void Run(IEcsSystems systems) {
      View.RefreshEntities();
      DirtyEntities.Clear();
    }



    public void OnEntityCreated(int e) {
      View.GetEntityView(e)
          .Activate();
      DirtyEntities.Add(e);
    }

    public void OnEntityChanged(int e) {
      DirtyEntities.Add(e);
    }

    public void OnEntityDestroyed(int e) {
      View.GetEntityView(e)
          .Deactivate();
    }


    public void OnWorldResized(int newSize) {
      View.Resize(newSize);
    }

    public void OnWorldDestroyed(EcsWorld world) {
      View.Destroy();
      World.RemoveEventListener(this);
      ActiveDebugSystems.Unregister(this);
    }


    public void OnFilterCreated(EcsFilter filter) { }



    private void InitWorld(IEcsSystems systems) {
      World = systems.GetWorld(WorldName) ?? throw new Exception($"Cant find required world! ({WorldName})");
    }

    private void InitView() {
      View = new EcsWorldDebugSystemView(this);
    }

    private void InitEntities() {
      World.ForeachEntity(OnEntityCreated);
    }
  }
}

#endif