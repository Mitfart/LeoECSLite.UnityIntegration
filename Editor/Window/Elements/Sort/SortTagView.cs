using System;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Sort{
  public sealed class SortTagView : Button{
    private readonly Action _clickAction;

      
      
    public SortTagView(string text, Action action = null) : base(action){
      _clickAction =  action;
      clicked      += InvokeClickAction;

      this.text = text;
         
      Create();
      SetStyle();
    }
      
      
    private void Create() {
      Add(new Label(text));
    }
      
    private void SetStyle() {
      style.SetPadding(StyleUtils.METRICS_SSS);
      style.SetMargin(StyleUtils.METRICS_SSS);
      style.SetBorderRadius(StyleUtils.METRICS_SSS);
    }
      
      
      
    private void InvokeClickAction() {
      _clickAction?.Invoke();
      clicked -= InvokeClickAction;
    }
  }
}
