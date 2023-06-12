using System;
using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Filter {
  public class FilterTag {
    public FilterMethod Method { get; }
    public Type         Type   { get; }
    public EcsWorld     World  { get; private set; }
    public IEcsPool     Pool   { get; private set; }



    public FilterTag(Type type, FilterMethod method, EcsWorld world) {
      Type   = type;
      Method = method;

      SetWorld(world);
    }



    public void SetWorld(EcsWorld world) {
      if (World == world)
        return;

      World = world;
      Pool  = World.GetPoolByType(Type);
    }
  }
}