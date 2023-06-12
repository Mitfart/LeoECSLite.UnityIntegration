#if UNITY_EDITOR
using System;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Name;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mitfart.LeoECSLite.UnityIntegration.View {
  public sealed class WorldView {
    private const string ENTITIES_ROOT_NAME = "Entities";

    private readonly EcsWorldDebugSystem _debugSystem;
    private readonly Transform           _root;
    private readonly Transform           _entitiesRoot;
    private readonly NameBuilder         _nameBuilder;

    private EntityView[] _entitiesViews;

    private EcsWorld World     => _debugSystem.World;
    private int      WorldSize => _debugSystem.WorldSize;
    private string   DebugName => _debugSystem.DebugName;



    public WorldView(EcsWorldDebugSystem debugSystem, NameSettings nameSettings = null) {
      _debugSystem   = debugSystem;
      _entitiesViews = new EntityView[World.GetWorldSize()];
      _nameBuilder   = new NameBuilder(nameSettings ?? new NameSettings());

      _root         = CreateRootObject(DebugName);
      _entitiesRoot = CreateRootObject(ENTITIES_ROOT_NAME, _root);

      Object.DontDestroyOnLoad(_root);
    }



    public void Destroy()           => Object.Destroy(_root.gameObject);
    public void Refresh()           => World.ForeachEntity(RefreshView);
    public void Resize(int newSize) => Array.Resize(ref _entitiesViews, newSize);



    public EntityView GetEntityView(int e) {
      if (EntityOutOfRange(e))
        throw new Exception($"Entity out of range! [ e: {e} | max: {WorldSize - 1} ]");

      return _entitiesViews[e] ??= CreateEntityView(e);
    }



    private EntityView CreateEntityView(int e) {
      var entityObject = new GameObject();

      entityObject.transform.SetParent(_entitiesRoot);

      return entityObject
            .AddComponent<EntityView>()
            .Construct(World, e, _nameBuilder);
    }



    private void RefreshView(int      e) => GetEntityView(e).Refresh();
    private bool EntityOutOfRange(int e) => e < 0 || e >= WorldSize;

    private static Transform CreateRootObject(string name, Transform parent = null) {
      var obj = new GameObject { name = name, hideFlags = HideFlags.NotEditable };

      obj.transform.SetParent(parent);

      return obj.transform;
    }
  }
}

#endif