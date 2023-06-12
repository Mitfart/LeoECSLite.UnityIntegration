#if UNITY_EDITOR
using System;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Name;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.View {
  public sealed class EntityView : MonoBehaviour {
    public int             componentsCount;
    public ComponentView[] components = Array.Empty<ComponentView>();

    private NameBuilder _nameBuilder;
    public  Type[]      ComponentsTypes = Array.Empty<Type>();

    public EcsWorld World      { get; private set; }
    public int      Entity     { get; private set; }
    public bool     IsAlive    { get; private set; }
    public string   BakedIndex { get; private set; }

    private bool BakeComponents => _nameBuilder.Settings.BakeComponents;



    public EntityView Construct(EcsWorld world, int entity, NameBuilder nameBuilder) {
      World        = world;
      Entity       = entity;
      _nameBuilder = nameBuilder;

      BakedIndex = _nameBuilder.StartName().AddEntityIndex(Entity).End();
      return this;
    }

    public void Refresh() {
      if (!IsAlive)
        return;

      RefreshData();

      if (!EnoughViews())
        ResizeForNewViews();

      RefreshViews();
      RefreshName();
    }


    public void Activate()   => gameObject.SetActive(IsAlive = true);
    public void Deactivate() => gameObject.SetActive(IsAlive = false);



    private void RefreshData() {
      componentsCount = World.GetComponentTypes(Entity, ref ComponentsTypes);
    }

    private void RefreshViews() {
      for (var i = 0; i < componentsCount; i++) {
        GetView(i)
         .Init(ComponentsTypes[i])
         .Refresh();
      }
    }

    private void RefreshName() {
      if (!BakeComponents)
        return;

      _nameBuilder
       .StartName()
       .Append(BakedIndex)
       .StartDescription();

      foreach (ComponentView component in components) {
        if (component == null)
          break;

        _nameBuilder.BakeComponent(component.ComponentType);
      }

      name = _nameBuilder.End();
    }



    private ComponentView GetView(int i) => components[i] ??= new ComponentView(this);

    private void ResizeForNewViews() => Array.Resize(ref components, componentsCount);
    private bool EnoughViews()       => componentsCount <= components.Length;
  }
}
#endif