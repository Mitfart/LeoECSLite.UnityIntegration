using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class ContentBox : Box{
      private Label _label;

      public string Label{
         get => _label.text;
         set{
            if (_label == null) Insert(0, _label = new Label());

            _label.text = value;
         }
      }

      public ContentBox WithLabel(){
         _label = this.AddAndGet(new Label{ style ={ unityTextAlign = TextAnchor.MiddleLeft } });
         return this;
      }
      public new class UxmlFactory : UxmlFactory<ContentBox, UxmlTraits>{}
   }
}
