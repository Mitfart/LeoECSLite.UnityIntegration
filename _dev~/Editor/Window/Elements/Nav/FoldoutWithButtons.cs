using System;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Nav{
   public sealed class FoldoutWithButtons : Foldout{
      private const    string           EMPTY_STRING = "_";
      private readonly ButtonsContainer _buttonsContainer;

      
      public FoldoutWithButtons(){
         text = EMPTY_STRING;

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

      
      public Button AddButton(string newText, Action action) {
         return _buttonsContainer.AddButton(newText, action);
      }
      public Button AddButton(Texture2D icon, Action action) {
         return _buttonsContainer
           .AddButton(string.Empty, action)
               .AddChild(new VisualElement {
                   style = {
                      backgroundImage = icon, 
                      width           = Utils.GetPercentsLength(100),
                      height          = Utils.GetPercentsLength(100)
                   }
                });
            ;
      }
   }
}
