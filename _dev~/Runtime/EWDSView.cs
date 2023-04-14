#if UNITY_EDITOR
using System;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration {
    public class EWDSView : MonoBehaviour {
        public event Action<int> OnEntityChange;
    
        private EcsWorldDebugSystem _debugSystem;
        private MonoEntityView[]    _monoEntityViews;

    
    
        public EWDSView Init(EcsWorldDebugSystem debugSystem) {
            _debugSystem     = debugSystem;
            _monoEntityViews = new MonoEntityView[_debugSystem.GetWorldSize()];
      
            name      = _debugSystem.GetDebugName();
            hideFlags = HideFlags.NotEditable;
      
            DontDestroyOnLoad(this);
            return this;
        }
    
        public void Destroy() {
            Destroy(gameObject);
        }
    
    
    
        public bool TryGetEntityView(int entity, out MonoEntityView view) {
            if (entity < 0 || entity >= _monoEntityViews.Length){
                view = null;
                return false;
            }
      
            view = _monoEntityViews[entity];
            return view != null;
        }

        public MonoEntityView CreateEntityView(int entity) {
            MonoEntityView view = _debugSystem.CreateEntityView(entity);
      
            view.transform.SetParent(transform);

            return _monoEntityViews[entity] = view;
        }

    


        public void UpdateEntity(int entity) {
            if (!_debugSystem.View.TryGetEntityView(entity, out MonoEntityView view)) return;

            view.UpdateView();
          
            OnEntityChange?.Invoke(entity);
        }

    
        public void SetWorldSize(int newSize) {
            Array.Resize(ref _monoEntityViews, newSize);
        }
    }
}

#endif