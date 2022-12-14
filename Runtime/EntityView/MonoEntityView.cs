using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed partial class MonoEntityView : MonoBehaviour{
      [NonSerialized] private NameBuilder _nameBuilder;
      
      public EcsWorldDebugSystem DebugSystem{ get; private set; }
      public EcsWorld            World      { get; private set; }
      public int                 Entity     { get; private set; }
      public string              Tag        { get; private set; }
      public bool                IsActive   { get; private set; }

      public  Dictionary<Type, BaseECV> Components        { get; } = new();
      private Dictionary<Type, BaseECV> ComponentsToRemove{ get; } = new();
      
      public event Action<BaseECV> OnAddComponent;
      public event Action<BaseECV> OnRemoveComponent;

      
      public void Init(EcsWorldDebugSystem debugSystem, int entity){
         DebugSystem = debugSystem;
         Entity      = entity;
         World       = DebugSystem.World;
         
         _nameBuilder ??= new NameBuilder(this, debugSystem.NameSettings);

         ComponentsToRemove.Clear();
         Components.Clear();
      }



      public void Activate(){
         gameObject.SetActive(IsActive = true);
         UpdateName();
      }
      public void Deactivate(){
         gameObject.SetActive(IsActive = false);
         _nameBuilder.Reset();
      }



      public void UpdateView(){
         UpdateComponents(true);
         UpdateName();
      }

      public void UpdateName(){
         _nameBuilder.Update();
      }
      public void ChangeTag(string newTag){
         _nameBuilder.ChangeTag(newTag);
      }

      public void UpdateComponents(bool updateValues = false){
         RemoveComponents();
         
         DebugSystem.ForeachComponent(
            Entity, component => {
               BaseECV view = GetOrAdd(component.GetType());

               if (updateValues) 
                  view.UpdateValue();
            });
         _nameBuilder.BakeComponents(Components.Keys);
      }
      public void UpdateComponentsValues(){
         foreach (BaseECV view in Components.Values) 
            view.UpdateValue();
      }

      private void RemoveComponents(){
         foreach (Type componentType in ComponentsToRemove.Keys)
            Components.Remove(componentType);
         ComponentsToRemove.Clear();
      }
      
      
      public BaseECV GetOrAdd(Type compType){
         if (Components.TryGetValue(compType, out BaseECV view)) return view;

         view = gameObject.AddEcsComponentView(compType);
         view.Init(this);
         Components[compType] = view;
         
         OnAddComponent?.Invoke(view);
         
         return view;
      }
      public void Remove(BaseECV compView){
         Type componentType = compView.GetComponentType();
         
         OnRemoveComponent?.Invoke(compView);
         ComponentsToRemove[componentType] = compView;
      }


      public void Delete(){
         if (!IsActive) return;
         foreach (BaseECV componentView in Components.Values)
            componentView.Delete();
      }
   }
}
