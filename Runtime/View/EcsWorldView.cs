#if UNITY_EDITOR
using System;
using LeoECSLite.UnityIntegration.Entity;
using LeoECSLite.UnityIntegration.Name;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LeoECSLite.UnityIntegration {
  public sealed class EcsWorldView {
    private const string ENTITIES_ROOT_NAME = "Entities";

    private readonly EcsWorldDebugSystem _debugSystem;

    private readonly Transform    _root;
    private readonly Transform    _entitiesRoot;
    private          EntityView[] _entitiesViews;

    private readonly NameBuilder _entitiesNameBuilder;

    private EcsWorld World         => _debugSystem.World;
    private string   DebugName     => _debugSystem.DebugName;
    private int      EntitiesCount => _entitiesViews.Length;

    private NameSettings NameSettings { get; }



    public EcsWorldView(EcsWorldDebugSystem debugSystem, NameSettings nameSettings = null) {
      _debugSystem = debugSystem;
      NameSettings = nameSettings ?? new NameSettings();

      _entitiesViews       = new EntityView[World.GetWorldSize()];
      _entitiesNameBuilder = new NameBuilder(NameSettings);

      _root         = CreateRootObject(DebugName);
      _entitiesRoot = CreateRootObject(ENTITIES_ROOT_NAME, _root);
      Object.DontDestroyOnLoad(_root);

      EditorApplication.update += Refresh;
    }

    public void Destroy() {
      EditorApplication.update -= Refresh;
      Object.Destroy(_root.gameObject);
    }



    public void Refresh() {
      for (var e = 0; e < _entitiesViews.Length; e++)
        GetEntityView(e).Refresh(NameSettings, _entitiesNameBuilder);
    }

    public EntityView GetEntityView(int e) {
      if (EntityOutOfRange(e))
        throw new Exception($"Entity out of range! [ e: {e} | max: {EntitiesCount - 1} ]");

      return _entitiesViews[e] ??= CreateEntityView(e);
    }


    public void Resize(int newSize) => Array.Resize(ref _entitiesViews, newSize);



    private EntityView CreateEntityView(int e) {
      var entityObject = new GameObject();

      entityObject.transform.SetParent(_entitiesRoot);

      return entityObject
            .AddComponent<EntityView>()
            .Construct(World, e);
    }



    private bool EntityOutOfRange(int e) => e < 0 || e >= EntitiesCount;

    private static Transform CreateRootObject(string name, Transform parent = null) {
      var obj = new GameObject { name = name, hideFlags = HideFlags.NotEditable };

      obj.transform.SetParent(parent);

      return obj.transform;
    }
  }
}

#endif