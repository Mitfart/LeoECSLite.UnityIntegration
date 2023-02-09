using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using Mitfart.LeoECSLite.UnityIntegration.System;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Extentions {
  public static class DebugSystemViewExt {
    public static EWDSView CreateView(this EcsWorldDebugSystem debugSystem) {
      return 
        new GameObject()
         .AddComponent<EWDSView>()
         .Init(debugSystem);
    }
    
    public static MonoEntityView CreateEntityView(this EcsWorldDebugSystem debugSystem, int entity) {
      return 
        new GameObject()
         .AddComponent<MonoEntityView>()
         .Init(debugSystem, entity);
    }
    
    public static MonoEntityView GetOrCreate(this EWDSView debugSystemView, int entity) {
      if (!debugSystemView.TryGetEntityView(entity, out var view)) 
        view = debugSystemView.CreateEntityView(entity);
      return view;
    }
  }
}
