using System;
using UnityEditor;
using UnityEngine;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;

namespace Mitfart.LeoECSLite.UnityIntegration.ComponentView{
   public abstract class BaseEcv : MonoBehaviour{
      public MonoEntityView MonoEntityView { get; private set; }
      public IEcsPool       EcsPool        { get; private set; }
      public EcsWorld       EcsWorld       { get; private set; }
      public int            Entity         { get; private set; }
      public bool           IsActive       { get; private set; }

      
      public virtual  int                GetPriority() => -1;
      public abstract Type               GetComponentType();
      public abstract SerializedProperty GetValueProperty(SerializedObject serializedObject = null);
      public abstract void               OnUpdateValue();
      
      protected virtual void OnInit()     {}
      protected virtual void OnSetValue() {}
      protected virtual void OnRemove()   {}

      

      public void Init(MonoEntityView monoEntityView){
         if (IsActive) return;
         IsActive = true;

         MonoEntityView = monoEntityView;
         Entity         = monoEntityView.Entity;
         EcsWorld       = MonoEntityView.World;
         EcsPool        = EcsWorld.GetPool(GetComponentType());

         OnUpdateValue();
         OnInit();
      }
      
      public void UpdateValue(){
         if (!IsActive || !MonoEntityView.IsActive) return;

         if (!EcsPool.Has(Entity)){
            Remove();
            return;
         }
         
         OnUpdateValue();
      }
      
      public void Remove(){
         if (IsActive) {
            OnRemove();
            if (EcsPool.Has(Entity)) 
               EcsPool.Del(Entity);
         }
         IsActive = false;

         MonoEntityView.RemoveComponentView(this);
         DestroyImmediate(this);
      }
      
      
      
      protected virtual void OnValidate() {
         if (!IsActive) return;

         OnSetValue();
      }
   }
}
