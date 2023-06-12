using System;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Filter.View {
  public class FilterTagView : Button {
    private const string TAG_CL           = "tag";
    private const string TAG_LABEL_CL     = "tag__label";
    private const string TAG_EXTRA_CL     = "tag__extra";
    private const string TAG_SEPARATOR_CL = "tag__separator";

    private const string INCLUDE = "In";
    private const string EXCLUDE = "Ex";
    private       Label  _filterLabel;


    private Label         _filterMethod;
    private VisualElement _separator;

    public Type         Type   { get; }
    public FilterMethod Method { get; }



    public FilterTagView(Type componentType, FilterMethod filterMethod) {
      Type   = componentType;
      Method = filterMethod;

      CreateElements();
      AddElements();
      InitElements();
    }


    private void CreateElements() {
      _filterMethod = new Label();
      _filterLabel  = new Label();
      _separator    = new VisualElement();
    }

    private void AddElements()
      => this
        .AddChild(_filterMethod)
        .AddChild(_separator)
        .AddChild(_filterLabel);

    private void InitElements() {
      SetLabel();
      SetMethod();

      AddToClassList(TAG_CL);
      _filterMethod.AddToClassList(TAG_EXTRA_CL);
      _filterLabel.AddToClassList(TAG_LABEL_CL);
      _separator.AddToClassList(TAG_SEPARATOR_CL);
    }



    private void SetMethod()
      => _filterMethod.text = Method switch {
        FilterMethod.Include => INCLUDE,
        FilterMethod.Exclude => EXCLUDE,
        var _                => throw new ArgumentOutOfRangeException()
      };

    private void SetLabel() => _filterLabel.text = Type.Name;
  }
}