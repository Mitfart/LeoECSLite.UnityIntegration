using System;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Nav{
   public sealed class ButtonsContainer : VisualElement{
      public ButtonsContainer(){
         style.flexDirection = FlexDirection.Row;
         style.marginLeft    = style.marginRight = -StyleUtils.METRICS_SSS;
      }

      
      public void AddButton(string newText, Action action){
         var btn = this.AddAndGet(new Button(action){ text = newText });

         btn.WithSquareButtonStyle();
         btn.style.marginLeft = btn.style.marginRight = StyleUtils.METRICS_SSS;
      }
   }
}
