#if UNITY_EDITOR
using System;
using System.Text;
using LeoECSLite.UnityIntegration.Extensions;

namespace LeoECSLite.UnityIntegration.Name {
  public sealed class NameBuilder {
    private const string COMPONENTS_SEPARATOR = ", ";
    private const string START_DESCRIPTION    = " | ";

    private readonly StringBuilder _builder;
    private readonly NameSettings  _settings;



    public NameBuilder(NameSettings settings) {
      _settings = settings;
      _builder  = new StringBuilder();
    }



    public NameBuilder StartName() {
      _builder.Clear();
      return this;
    }
    
    public NameBuilder AddEntityIndex(int e) {
      _builder.Append(e.ToString(_settings.Format));
      return this;
    }

    public NameBuilder StartDescription() {
      _builder.Append(START_DESCRIPTION);
      return this;
    }

    public NameBuilder BakeComponent(Type component) {
      _builder
       .Append(component.GetCleanName())
       .Append(COMPONENTS_SEPARATOR);
      return this;
    }

    public string End() => _builder.ToString();
  }
}

#endif