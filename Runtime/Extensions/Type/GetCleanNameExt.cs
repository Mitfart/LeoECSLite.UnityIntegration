using System;

namespace Mitfart.LeoECSLite.UnityIntegration.Extensions {
  public static class GetCleanNameExt {
    private const string GENERIC_SYMBOL = "`";


    public static string GetCleanName(this Type type) {
      if (!type.IsGenericType)
        return type.Name;

      int genericIndex = type.Name.LastIndexOf(GENERIC_SYMBOL, StringComparison.Ordinal);

      string cleanName = genericIndex == -1
        ? type.Name
        : type.Name[..genericIndex];

      return cleanName;
    }
  }
}