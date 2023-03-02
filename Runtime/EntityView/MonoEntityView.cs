#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.EntityView{
    public sealed class MonoEntityView : MonoBehaviour{
        public EcsWorldDebugSystem DebugSystem { get; private set; }
        public EcsWorld            World       { get; private set; }
        public int                 Entity      { get; private set; }
        public bool                IsActive    { get; private set; }
        public string              Tag         { get; private set; }

        public Dictionary<Type, BaseEcv> Components         { get; } = new();
        public Dictionary<Type, BaseEcv> ComponentsToRemove { get; } = new();
      
        public event Action<BaseEcv> OnAddComponent;
        public event Action<BaseEcv> OnRemoveComponent;
      
        private EntityNameBuilder _entityNameBuilder;

      
      
        public MonoEntityView Init(EcsWorldDebugSystem debugSystem, int entity) {
            DebugSystem = debugSystem;
            Entity      = entity;
            World       = DebugSystem.World;
         
            _entityNameBuilder ??= new EntityNameBuilder(this, debugSystem.NameSettings);

            ComponentsToRemove.Clear();
            Components.Clear();
            return this;
        }



        public void Activate() {
            gameObject.SetActive(IsActive = true);
            UpdateName();
        }
      
        public void Deactivate() {
            gameObject.SetActive(IsActive = false);
            _entityNameBuilder.Reset();
        }



        public void UpdateView() {
            UpdateComponents(updateValues: true);
            UpdateName();
        }
      
        public void UpdateName() {
            _entityNameBuilder.Update();
        }
      
        public void UpdateComponents(bool updateValues = false) {
            RemoveComponents();
         
            DebugSystem.ForeachComponent(
                Entity, component => {
                    BaseEcv view = GetOrAddComponentView(component.GetType());

                    if (updateValues) 
                        view.UpdateValue();
                });
            _entityNameBuilder.BakeComponents(Components.Keys);
        }
      
        public void UpdateComponentsValues() {
            foreach (BaseEcv view in Components.Values) 
                view.UpdateValue();
        }
      
        public void SetTag(string newTag) {
            if (string.Equals(newTag, Tag)) return;
         
            _entityNameBuilder.SetTag(newTag);
            Tag = newTag;
        }
      
        private void RemoveComponents() {
            foreach (Type componentType in ComponentsToRemove.Keys)
                Components.Remove(componentType);
            ComponentsToRemove.Clear();
        }
      
      
      
        public BaseEcv GetOrAddComponentView(Type compType) {
            if (Components.TryGetValue(compType, out BaseEcv view)) return view;

            view = gameObject.AddEcsComponentView(compType);
            view.Init(this);
            Components[compType] = view;
         
            OnAddComponent?.Invoke(view);
         
            return view;
        }
      
        public void RemoveComponentView(BaseEcv compView) {
            Type componentType = compView.GetComponentType();
         
            OnRemoveComponent?.Invoke(compView);
            ComponentsToRemove[componentType] = compView;
        }

      

        public void Delete() {
            if (!IsActive) return;
            foreach (BaseEcv componentView in Components.Values)
                componentView.Remove();
        }
    }
}

#endif