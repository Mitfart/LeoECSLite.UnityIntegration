using System;
using LeoECSLite.UnityIntegration.Editor.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Window.Entity {
  public class UIComponentLabel : VisualElement {
    public const     string MAIN_CL       = "component_label";
    public const     string MAIN_LABEL_CL = "component_label__label";
    private readonly Color  _color;
    private readonly bool   _compactName;


    private readonly Type _type;

    private Label _label;


    public UIComponentLabel(Type type, bool compactName = false) {
      _type        = type;
      _compactName = compactName;
      _color       = type.Name.ToColor();

      CreateElements();
      AddElements();
      InitElements();
    }


    private void CreateElements() => _label = new Label();

    private void AddElements() => Add(_label);

    private void InitElements() {
      _label.text = _compactName
        ? _type.Name.Compact()
        : _type.Name;

      style.backgroundColor = _color;

      AddToClassList(MAIN_CL);
      _label.AddToClassList(MAIN_LABEL_CL);
    }
  }
}