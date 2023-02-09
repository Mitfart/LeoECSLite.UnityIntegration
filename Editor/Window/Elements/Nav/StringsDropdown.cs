using System;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Nav{
   public class StringsDropdown : DropdownField{
      public event Action<string, string> OnChangeValue;

      
      public StringsDropdown(params object[] choices){
         foreach (var choice in choices)
            Add(choice.ToString());

         this.RegisterValueChangedCallback(
            evt => {
               OnChangeValue?.Invoke(evt.newValue, value);
               value = evt.newValue;
            });
      }
      
      
      public void Add   (string choice) => choices.Add(choice);
      public void Remove(string choice) => choices.Remove(choice);
   }
}
