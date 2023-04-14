using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration.Editor.Window.World {
  public class WorldTabData {
    private const string DEFAULT_WORLD_NAME = "Default";

    public EcsWorld World { get; }
    public string   Name  { get; }

    public WorldTabData(EcsWorld world, string name) {
      World = world;
      Name  = name;
    }

    public override string ToString() {
      return string.IsNullOrWhiteSpace(Name) ? DEFAULT_WORLD_NAME : Name;
    }
  }
}