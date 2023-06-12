#if UNITY_EDITOR
using System;
using System.Text;
using Mitfart.LeoECSLite.UnityIntegration.Extensions;

namespace Mitfart.LeoECSLite.UnityIntegration.Name {
  public sealed class NameBuilder {
    private const string COMPONENTS_SEPARATOR = ", ";
    private const string START_DESCRIPTION    = " | ";

    private readonly StringBuilder _builder;

    public NameSettings Settings { get; }



    public NameBuilder(NameSettings settings) {
      Settings = settings;
      _builder = new StringBuilder();
    }



    public NameBuilder StartName() {
      _builder.Clear();
      return this;
    }

    public NameBuilder AddEntityIndex(int e) {
      _builder.Append(e.ToString(Settings.Format));
      return this;
    }

    public NameBuilder Append(string str) {
      _builder.Append(str);
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