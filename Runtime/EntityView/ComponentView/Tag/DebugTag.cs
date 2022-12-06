#if UNITY_EDITOR
using System;

namespace Mitfart.LeoECSLite.UnityIntegration  {
   public sealed class DebugTagEcv : ECV<DebugTag>{
      public override int GetPriority() => 1;

      
      protected override void OnInit(){
         base.OnInit();
         MonoEntityView.ChangeTag(value.tag);
      }

      protected override ref DebugTag GetValidatedComponent(ref DebugTag component){
         MonoEntityView.ChangeTag(component.tag);
         return ref component;
      }
   }

   [Serializable]
   public struct DebugTag : IEcsSerializedComponent{
      public string tag;
   }
}
#endif
