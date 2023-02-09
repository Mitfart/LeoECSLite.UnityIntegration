using System;

namespace Mitfart.LeoECSLite.UnityIntegration.Extentions{
   public static class TypeExt{
      public static string GetCleanName(this Type type){
         if (!type.IsGenericType) return type.Name;

         var genericIndex = type.Name.LastIndexOf("`", StringComparison.Ordinal);
         var typeName = genericIndex == -1
                              ? type.Name
                              : type.Name[..genericIndex];
         return $"{typeName}";
      }
   }
}
