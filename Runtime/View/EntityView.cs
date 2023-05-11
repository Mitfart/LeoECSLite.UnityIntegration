#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using LeoECSLite.UnityIntegration.Editor.Component;
using LeoECSLite.UnityIntegration.Extensions;
using LeoECSLite.UnityIntegration.Name;
using Leopotam.EcsLite;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Entity {
  public sealed class EntityView : MonoBehaviour {
    [SerializeField] private List<ComponentView>             components       = new(16);
    private                  Dictionary<Type, ComponentView> _componentsViews = new(16);

    public EcsWorld World   { get; private set; }
    public int      Entity  { get; private set; }
    public bool     IdDirty { get; private set; }
    public bool     IsAlive { get; private set; }



    public EntityView Construct(EcsWorld world, int entity) {
      World  = world;
      Entity = entity;
      return this;
    }



    public void Refresh(NameSettings nameSettings, NameBuilder nameBuilder) {
      if (!IsAlive || !IdDirty)
        return;

      RefreshComponents();
      RefreshName(nameSettings, nameBuilder);

      IdDirty = false;
    }



    public void Activate()   => gameObject.SetActive(IsAlive = true);
    public void MarkDirty()  => IdDirty = true;
    public void Deactivate() => gameObject.SetActive(IsAlive = false);



    private void RefreshComponents() {
      Type[] components      = StaticCache.Types;
      int    componentsCount = World.GetComponentTypes(Entity, ref components);

      SetComponentsLength(componentsCount);

      for (var i = 0; i < componentsCount; i++)
        SetComponent(at: i, to: components[i]);
    }

    private void RefreshName(NameSettings nameSettings, NameBuilder nameBuilder) {
      nameBuilder
       .StartName()
       .AddEntityIndex(Entity);

      if (nameSettings.BakeComponents)
        BakeComponents(nameBuilder);

      name = nameBuilder.End();
    }



    private void BakeComponents(NameBuilder nameBuilder) {
      nameBuilder.StartDescription();

      foreach (ComponentView componentView in components)
        nameBuilder.BakeComponent(componentView.ComponentType);
    }



    private void SetComponent(int at, Type to) => components[at] = GetView(to);

    private void SetComponentsLength(int count) => components.RemoveRange(count, components.Count - count - 1);



    private ComponentView GetView(Type component) {
      if (_componentsViews.TryGetValue(component, out ComponentView compView))
        return compView;

      return _componentsViews[component] = new ComponentView(component, Entity, World);
    }
  }
}
#endif