using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class Extentions{
      public static T AddAndGet<T>(this VisualElement root, T visualElement) where T : VisualElement{
         root.Add(visualElement);
         return visualElement;
      }

      public static T AddChild<T, TChild>(this T root, TChild visualElement)
         where T : VisualElement where TChild : VisualElement{
         root.Add(visualElement);
         return root;
      }
   }
}
