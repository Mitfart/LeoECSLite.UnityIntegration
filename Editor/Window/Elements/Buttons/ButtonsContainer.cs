using System;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class ButtonsContainer : VisualElement{
      public ButtonsContainer(){
         style.flexDirection = FlexDirection.Row;
         style.marginLeft    = style.marginRight = -Styles.Metrics_Sss;
      }

      public void AddButton(string newText, Action action, string newName = null){
         var btn = this.AddAndGet(new Button(action){ text = newText, name = newName });

         btn.WithSquareButtonStyle();
         btn.style.marginLeft = btn.style.marginRight = Styles.Metrics_Sss;
      }
   }
}
