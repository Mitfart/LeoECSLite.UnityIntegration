using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.World {
  public class WorldTabData {
    private const string DEFAULT_WORLD_NAME = "Default";

    public string   Name  { get; }
    public EcsWorld World { get; }

    public WorldTabData(string name, EcsWorld world) {
      Name  = name;
      World = world;
    }

    public override string ToString()
      => string.IsNullOrWhiteSpace(Name)
        ? DEFAULT_WORLD_NAME
        : Name;
  }
}