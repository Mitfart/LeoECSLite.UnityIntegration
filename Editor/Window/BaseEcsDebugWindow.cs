using System;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.World;
using UnityEditor;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window {
  public abstract class BaseEcsDebugWindow : EditorWindow, IEcsWorldEventListener {
    public EcsWorldDebugSystem ActiveSystem { get; private set; }



    private void OnEnable() {
      EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
      EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable() {
      EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void CreateGUI() {
      CreateElements();
      StructureElements();
      InitElements();
    }



    public virtual void OnEntityCreated(int   e)      { }
    public virtual void OnEntityChanged(int   entity) { }
    public virtual void OnEntityDestroyed(int entity) { }

    public virtual void OnWorldResized(int        newSize) { }
    public virtual void OnWorldDestroyed(EcsWorld world)   { }

    public virtual void OnFilterCreated(EcsFilter filter) { }



    protected abstract void CreateElements();
    protected abstract void StructureElements();
    protected abstract void InitElements();


    protected abstract void InitInspector();
    protected abstract void ResetInspector();



    protected void ChangeWorld(WorldTabData worldTab) {
      if (ActiveDebugSystems.TryGet(worldTab.Name, out EcsWorldDebugSystem debugSystem))
        SetActiveWorldDebugSystem(debugSystem);
      else
        throw new Exception($"Can't find System relative to `{worldTab}` world!");
    }


    private void SetActiveWorldDebugSystem(EcsWorldDebugSystem newActiveDebugSystem) {
      if (ActiveSystem == newActiveDebugSystem)
        return;

      ResetActiveSystem();

      ActiveSystem = newActiveDebugSystem;

      InitActiveSystem();
    }


    private void InitActiveSystem() {
      if (ActiveSystem == null)
        return;

      ActiveSystem.World.AddEventListener(this);
      InitInspector();
    }

    private void ResetActiveSystem() {
      if (ActiveSystem == null)
        return;

      ActiveSystem.World.RemoveEventListener(this);
      ResetInspector();
    }



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
  }
}