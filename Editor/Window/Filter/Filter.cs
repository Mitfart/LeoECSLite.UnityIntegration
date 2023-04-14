using System;
using System.Collections.Generic;
using System.Linq;

namespace LeoECSLite.UnityIntegration.Editor.Window.Filter {
  public class Filter {
    public EcsWorldDebugSystem         DebugSystem { get; private set; }
    public Dictionary<Type, FilterTag> Tags        { get; }

    public event Action<Type> OnAddTag;
    public event Action<Type> OnRemoveTag;



    public Filter() {
      Tags = new Dictionary<Type, FilterTag>(4);
    }

    public void Init(EcsWorldDebugSystem debugSystem) {
      DebugSystem = debugSystem;

      foreach (FilterTag tag in Tags.Values)
        tag.SetWorld(DebugSystem.World);
    }

    public void Clear() {
      foreach (Type component in Tags.Keys)
        RemoveTag(component, false);

      Tags.Clear();
    }

    public void Reset() {
      DebugSystem = null;
      Clear();
    }



    public bool AddTag(Type component) {
      if (Tags.TryGetValue(component, out FilterTag tag)) {
        UnityEngine.Debug.Log($"Tag of type: [ {component.Name} ] is already added with method: [ {tag.Method} ]!");
        return false;
      }

      FilterMethod method = FilterMethod.Include;

      Tags.Add(
        component,
        new FilterTag(
          component,
          method,
          DebugSystem.World
        )
      );

      OnAddTag?.Invoke(component);
      return true;
    }

    public void RemoveTag(Type component, bool removeFromCollection = true) {
      if (!Tags.ContainsKey(component)) {
        UnityEngine.Debug.Log($"Tag of type: [ {component.Name} ] not found!");
        return;
      }

      if (removeFromCollection)
        Tags.Remove(component);

      OnRemoveTag?.Invoke(component);
    }



    public bool IsEmpty() {
      return Tags.Count <= 0;
    }

    public bool Has(int e) {
      return Tags
            .Values
            .Select(
               tag => tag.Method switch {
                 FilterMethod.Include => tag.Pool.Has(e),
                 FilterMethod.Exclude => !tag.Pool.Has(e),
                 _                    => throw new ArgumentOutOfRangeException()
               }
             )
            .All(compatible => compatible);
    }
  }
}