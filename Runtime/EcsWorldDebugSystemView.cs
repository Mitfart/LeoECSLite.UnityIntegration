#if UNITY_EDITOR
using System;
using LeoECSLite.UnityIntegration.Entity;
using LeoECSLite.UnityIntegration.Extentions.DebugSystem;
using LeoECSLite.UnityIntegration.Name;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LeoECSLite.UnityIntegration {
  public sealed class EcsWorldDebugSystemView {
    private const string ENTITIES_ROOT_NAME = "Entities";

    private readonly EcsWorldDebugSystem _debugSystem;
    private readonly Transform           _entitiesRoot;
    private readonly NameBuilder         _nameBuilder;

    private readonly Transform    _root;
    private          EntityView[] _entities;

    private EcsWorld     World         => _debugSystem.World;
    private NameSettings NameSettings  => _debugSystem.NameSettings;
    private int          EntitiesCount => _entities.Length;



    public EcsWorldDebugSystemView(EcsWorldDebugSystem debugSystem) {
      _debugSystem = debugSystem;

      _entities    = new EntityView[World.GetWorldSize()];
      _nameBuilder = new NameBuilder(NameSettings);

      _root         = CreateRootObject(_debugSystem.DebugName());
      _entitiesRoot = CreateRootObject(ENTITIES_ROOT_NAME, _root);
      Object.DontDestroyOnLoad(_root);

      EditorApplication.update += OnEditorUpdate;
    }

    public void Destroy() {
      EditorApplication.update -= OnEditorUpdate;
      Object.Destroy(_root.gameObject);
    }

    private void OnEditorUpdate() {
      RefreshEntities();
    }



    public EntityView GetEntityView(int e) {
      if (EntityOutOfRange(e))
        throw new Exception($"Entity out of range! [ e: {e} | max: {EntitiesCount - 1}] ]");

      return _entities[e] ??= CreateEntityView(e);
    }

    public void RefreshEntities() {
      for (var e = 0; e < _entities.Length; e++) {
        if (_entities[e] == null)
          continue;

        if (NameSettings.BakeComponents && _debugSystem.DirtyEntities.Contains(e))
          UpdateEntityName(GetEntityView(e));
      }
    }



    public void Resize(int newSize) {
      Array.Resize(ref _entities, newSize);
    }



    private void UpdateEntityName(EntityView entityView) {
      int      e     = entityView.Entity;
      EcsWorld world = entityView.World;

      _nameBuilder.Clear()
                  .AddEntityIndex(e);

      if (NameSettings.BakeComponents)
        _nameBuilder.BakeComponents(e, world);

      entityView.name = _nameBuilder.End();
    }

    private bool EntityOutOfRange(int e) {
      return e < 0 || e >= EntitiesCount;
    }



    private EntityView CreateEntityView(int e) {
      var entityObject = new GameObject();

      entityObject.transform.SetParent(_entitiesRoot);

      EntityView view = entityObject.AddComponent<EntityView>()
                                    .Construct(World, e);

      UpdateEntityName(view);

      return view;
    }

    private static Transform CreateRootObject(string name, Transform parent = null) {
      var obj = new GameObject {
        name      = name,
        hideFlags = HideFlags.NotEditable
      };

      obj.transform.SetParent(parent);

      return obj.transform;
    }
  }
}

#endif