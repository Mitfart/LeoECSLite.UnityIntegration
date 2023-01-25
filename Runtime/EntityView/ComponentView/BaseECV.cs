using System;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public abstract class BaseEcv : MonoBehaviour{
      public MonoEntityView MonoEntityView{ get; private set; }
      public IEcsPool       EcsPool       { get; private set; }
      public EcsWorld       EcsWorld      { get; private set; }
      public int            Entity        { get; private set; }
      public bool           IsActive      { get; private set; }


      private void OnValidate(){
         if (!IsActive) return;

         OnSetValue();
      }

      public virtual  int                GetPriority() => -1;
      public abstract Type               GetComponentType();
      public abstract SerializedProperty GetValueProperty(SerializedObject serializedObject = null);
      public abstract void               OnUpdateValue();

      protected virtual void OnInit()    {}
      protected virtual void OnSetValue(){}
      protected virtual void OnDelete()  {}



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
            Delete();
            return;
         }
         
         OnUpdateValue();
      }
      

      public void Delete(){
         if (IsActive) {
            OnDelete();
            if (EcsPool.Has(Entity)) 
               EcsPool.Del(Entity);
         }
         IsActive = false;

         MonoEntityView.Remove(this);
         DestroyImmediate(this);
      }
   }
}
