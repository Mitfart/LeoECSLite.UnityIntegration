using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityIntegration.Extentions {
   public class NamedWorld {
      private const string DEFAULT_WORLD_NAME = "Default";
      
      public string   Name  { get; }
      public EcsWorld World { get; }
      
      public NamedWorld(EcsWorld world, string name) {
         World = world;
         Name  = name;
      }

      public override string ToString() {
         return string.IsNullOrWhiteSpace(Name) ? DEFAULT_WORLD_NAME : Name;
      }
   }
}
