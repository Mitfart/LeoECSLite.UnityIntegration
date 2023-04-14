#if UNITY_EDITOR
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;

namespace Mitfart.LeoECSLite.UnityIntegration.Tag  {
   public sealed class DebugTagEcv : Ecv<DebugTag>{
      public override int GetPriority() => 1;

      
      protected override void OnInit(){
         base.OnInit();
         MonoEntityView.SetTag(value.tag);
      }

      protected override ref DebugTag GetValidatedComponent(ref DebugTag component){
         MonoEntityView.SetTag(component.tag);
         return ref component;
      }
   }
}
#endif
