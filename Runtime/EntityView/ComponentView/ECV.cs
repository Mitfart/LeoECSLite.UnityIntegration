using System;
using Leopotam.EcsLite;
using UnityEditor;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public interface IEcsSerializedComponent{} // Tag-Interface for Generator

   public abstract class ECV<T> : BaseECV where T : struct{
      public T value;

      private EcsPool<T> _typedEcsPool;
      public  EcsPool<T> TypedEcsPool => _typedEcsPool ??= (EcsPool<T>)EcsPool;

      public override Type GetComponentType(){
         return typeof(T);
      }
      public override SerializedProperty GetValueProperty(SerializedObject serializedObject = null){
         serializedObject ??= new SerializedObject(this);
         return serializedObject.FindProperty(nameof(value));
      }



      public override void OnUpdateValue(){
         value = TypedEcsPool.Get(Entity);
      }


      protected override void OnSetValue(){
         _typedEcsPool.Get(Entity) = GetValidatedComponent(ref value);
      }
      protected virtual ref T GetValidatedComponent(ref T component){
         return ref component;
      }
   }
}
