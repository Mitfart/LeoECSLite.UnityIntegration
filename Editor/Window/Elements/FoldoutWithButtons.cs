using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class FoldoutWithButtons : Foldout{
      private const    string           EmptyString = "_";
      private readonly ButtonsContainer _buttonsContainer;

      public FoldoutWithButtons(){
         text = EmptyString;

         var toggle        = hierarchy[0];
         var toggleContent = toggle.hierarchy[0];
         var toggleIcon    = toggleContent.hierarchy[0];
         var toggleLabel   = (Label)toggleContent.hierarchy[1];


         toggle.style.marginLeft = 0;

         toggleIcon.style.height                   = new StyleLength(new Length(100, LengthUnit.Percent));
         toggleIcon.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

         toggleLabel.style.flexGrow   = 1;
         toggleLabel.style.flexShrink = 1;
         toggleLabel.style.flexBasis  = 0;
         toggleLabel.style.whiteSpace = WhiteSpace.Normal;

         value = false;

         _buttonsContainer = toggleContent.AddAndGet(new ButtonsContainer());
      }

      public void AddButton(string newText, Action action, string newName = null){
         _buttonsContainer.AddButton(newText, action, newName);
      }
   }
}
