using System;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class TagView : Button{
      private readonly TagsContainerView _container;
      private readonly Type              _tagType;
      
      public readonly Action ClickAction;

      
      
      public TagView(Action action = null) : base(action){
         style.SetPadding(Styles.Metrics_Sss);
         style.SetMargin(Styles.Metrics_Sss);
         style.SetBorderRadius(Styles.Metrics_Sss);

         ClickAction =  action;
         clicked     += RemoveFromParent;
      }
      public TagView(Type type, TagsContainerView container, Action action = null) : this(action){
         _tagType   = type;
         _container = container;
         
         this.AddAndGet(new Label(_tagType.Name));
      }
      public TagView(string text, TagsContainerView container, Action action = null) : this(action){
         _tagType   = null;
         _container = container;
         
         this.AddAndGet(new Label(text));
      }
      
      
      private void RemoveFromParent(){
         if (ClickAction != null) return;
         
         _container.RemoveTag(_tagType);
         clicked -= RemoveFromParent;
      }
   }
}
