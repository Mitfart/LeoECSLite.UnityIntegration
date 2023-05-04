#if UNITY_EDITOR
using System;
using System.Text;
using LeoECSLite.UnityIntegration.Extentions.EcsWorld;
using LeoECSLite.UnityIntegration.Extentions.Type;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityIntegration.Name {
  public sealed class NameBuilder {
    private const    string        COMPONENTS_SEPARATOR = ", ";
    private const    string        START_DESCRIPTION    = " | ";
    private readonly StringBuilder _builder;

    private readonly NameSettings _settings;


    public NameBuilder(NameSettings settings) {
      _settings = settings;
      _builder  = new StringBuilder();
    }


    public NameBuilder Clear() {
      _builder.Clear();
      return this;
    }


    public NameBuilder AddEntityIndex(int e) {
      _builder.Append(e.ToString(_settings.Format));
      return this;
    }


    public NameBuilder BakeComponents(int e, EcsWorld world) {
      Type[] types = StaticCache.Types;
      int    count = world.GetComponentTypes(e, ref types);

      if (count <= 0)
        return this;

      _builder.Append(START_DESCRIPTION);

      for (var i = 0; i < count; i++) {
        _builder.Append(
                   types[i]
                    .GetCleanName()
                 )
                .Append(COMPONENTS_SEPARATOR);
      }

      return this;
    }


    public string End() => _builder.ToString();
  }
}

#endif