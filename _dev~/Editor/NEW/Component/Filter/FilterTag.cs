using System;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW.Component.Filter {
   public readonly struct FilterTag {
      public Type         Type         { get; }
      public FilterMethod FilterMethod { get; }

      
      public FilterTag(Type type, FilterMethod filterMethod) {
         Type         = type;
         FilterMethod = filterMethod;
      }


      public override string ToString() {
         return $"{FilterMethod} | {Type.Name}";
      }
   }
}
