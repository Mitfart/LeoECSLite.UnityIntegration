using System;

namespace Mitfart.LeoECSLite.UnityIntegration {
    public static class TypeExt {
        public static string GetCleanName(this Type type) {
            if (!type.IsGenericType) return type.Name;

            int genericIndex = type.Name.LastIndexOf("`", StringComparison.Ordinal);
            string typeName = genericIndex == -1
                ? type.Name
                : type.Name[..genericIndex];
            return $"{typeName}";
        }
    }
}
