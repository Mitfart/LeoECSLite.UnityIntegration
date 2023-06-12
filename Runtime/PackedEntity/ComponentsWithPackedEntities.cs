using System;
using System.Collections.Generic;
using System.Reflection;
using Mitfart.LeoECSLite.UnityIntegration.Attributes;
using UnityEditor;

namespace Mitfart.LeoECSLite.UnityIntegration.PackedEntity {
  public static class ComponentsWithPackedEntities {
    private static readonly HashSet<Type> _Components = new();

    static ComponentsWithPackedEntities() {
      foreach (FieldInfo fieldInfo in TypeCache.GetFieldsWithAttribute<PackedEntityAttribute>())
        _Components.Add(fieldInfo.ReflectedType);
    }

    public static bool Contains(Type component) => _Components.Contains(component);
  }
}