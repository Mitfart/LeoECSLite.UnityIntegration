using System;
using UnityEditor;

namespace Mitfart.LeoECSLite.UnityIntegration.ComponentView{
   public sealed class NotRegisteredEcv : BaseEcv{
      private Type _componentType;

      
      public void SetComponentType(Type type) { _componentType = type ?? _componentType; }

      public override SerializedProperty GetValueProperty(SerializedObject serializedObject = null) => null;
      public override Type               GetComponentType()                                         => _componentType;
      

      public override void OnUpdateValue() { }
   }
}
