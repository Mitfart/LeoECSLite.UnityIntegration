using System;
using UnityEditor;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class NotDefinedECV : BaseECV{
      private Type _componentType;

      public void SetComponentType(Type type){
         _componentType = type ?? _componentType;
      }

      public override SerializedProperty GetValueProperty(SerializedObject serializedObject = null){
         return null;
      }
      public override Type GetComponentType(){
         return _componentType;
      }

      public override void OnUpdateValue(){}
   }
}
