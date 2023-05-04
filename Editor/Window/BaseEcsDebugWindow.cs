using System;
using LeoECSLite.UnityIntegration.Editor.Window.World;
using Leopotam.EcsLite;
using UnityEditor;

namespace LeoECSLite.UnityIntegration.Editor.Window {
  public abstract class BaseEcsDebugWindow : EditorWindow, IEcsWorldEventListener {
    public EcsWorldDebugSystem ActiveDebugSystem { get; private set; }



    private void CreateGUI() {
      CreateElements();
      AddElements();
      InitElements();
    }


    public virtual void OnEntityCreated(int   e)      { }
    public virtual void OnEntityChanged(int   entity) { }
    public virtual void OnEntityDestroyed(int entity) { }

    public virtual void OnWorldResized(int        newSize) { }
    public virtual void OnWorldDestroyed(EcsWorld world)   { }

    public virtual void OnFilterCreated(EcsFilter filter) { }


    protected abstract void CreateElements();
    protected abstract void AddElements();
    protected abstract void InitElements();


    protected abstract void InitInspector();
    protected abstract void ResetInspector();


    protected void ChangeWorld(WorldTabData newWorldTabData) {
      if (TryGetSystem(newWorldTabData, out EcsWorldDebugSystem debugSystem)) {
        SetActiveWorldDebugSystem(debugSystem);
      }
      else {
        return;

        throw new Exception($"Can't find System relative to `{newWorldTabData}` world!");
      }
    }


    private static bool TryGetSystem(WorldTabData newWorldTabData, out EcsWorldDebugSystem debugSystem) => ActiveDebugSystems.TryGet(newWorldTabData.Name, out debugSystem);

    private void SetActiveWorldDebugSystem(EcsWorldDebugSystem newActiveDebugSystem) {
      if (ActiveDebugSystem == newActiveDebugSystem)
        return;

      ResetActiveSystem();

      ActiveDebugSystem = newActiveDebugSystem;

      InitActiveSystem();
    }

    private void InitActiveSystem() {
      if (ActiveDebugSystem == null)
        return;

      ActiveDebugSystem.World.AddEventListener(this);
      InitInspector();
    }

    private void ResetActiveSystem() {
      if (ActiveDebugSystem == null)
        return;

      ActiveDebugSystem.World.RemoveEventListener(this);
      ResetInspector();
    }


    #region OnPlayModeStateChanged

    private void OnEnable() {
      EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
      EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

    private void OnPlayModeStateChanged(PlayModeStateChange state) {
      switch (state) {
        case PlayModeStateChange.EnteredEditMode:
        case PlayModeStateChange.ExitingEditMode:
        case PlayModeStateChange.EnteredPlayMode:
          break;
        case PlayModeStateChange.ExitingPlayMode:
          ResetInspector();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(state), state, null);
      }
    }

    #endregion
  }
}