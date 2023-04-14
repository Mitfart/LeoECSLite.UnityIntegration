using System;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Nav{
   public class StringsDropdown : DropdownField{
      public event Action<string, string> OnChoose;

      
      public StringsDropdown(params object[] choices){
         foreach (object choice in choices)
            Add(choice.ToString());

         this.RegisterValueChangedCallback(
            evt => {
               OnChoose?.Invoke(evt.newValue, value);
               value = evt.newValue;
            });
      }
      
      
      public void Add   (string choice) => choices.Add(choice);
      public void Remove(string choice) => choices.Remove(choice);

      
      public new void Clear() {
         base.Clear();
         OnChoose = null;
      }
   }
}
