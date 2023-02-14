#if UNITY_EDITOR
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Extentions {
    public static class DebugSystemViewExt {
        public static EwdsView CreateView(this EcsWorldDebugSystem debugSystem) {
            return 
                new GameObject()
                   .AddComponent<EwdsView>()
                   .Init(debugSystem);
        }
    
        public static MonoEntityView CreateEntityView(this EcsWorldDebugSystem debugSystem, int entity) {
            return 
                new GameObject()
                   .AddComponent<MonoEntityView>()
                   .Init(debugSystem, entity);
        }
    
        public static MonoEntityView GetOrCreate(this EwdsView debugSystemView, int entity) {
            if (!debugSystemView.TryGetEntityView(entity, out MonoEntityView view)) 
                view = debugSystemView.CreateEntityView(entity);
            return view;
        }
    }
}

#endif