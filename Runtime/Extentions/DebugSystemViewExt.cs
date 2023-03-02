#if UNITY_EDITOR
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
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
            if (!debugSystemView.TryGetEntityView(entity, out MonoEntityView view)) 
                view = debugSystemView.CreateEntityView(entity);
            return view;
        }
    }
}

#endif