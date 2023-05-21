using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace LeoECSLite.UnityIntegration.PackedEntity {
  public static class ComponentsWithPackedEntities {
    private static readonly HashSet<Type> _Components = new();

    static ComponentsWithPackedEntities() {
      foreach (FieldInfo fieldInfo in TypeCache.GetFieldsWithAttribute<PackedEntityAttribute>())
        _Components.Add(fieldInfo.ReflectedType);
    }

    public static bool Contains(Type component) => _Components.Contains(component);
  }
}