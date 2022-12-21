using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class TagsContainerView : Box{
      private readonly Dictionary<Type, TagView> _tags;

      public EcsWorldDebugSystem EcsWorldDebugSystem = null;

      public event Action<Type> OnAddTag; 
      public event Action<Type> OnRemoveTag; 



      public TagsContainerView(){
         _tags = new Dictionary<Type, TagView>();

         Add(new TagView(Icons.Plus, this, OpenComponentsMenu));

         style.flexDirection  = FlexDirection.Row;
         style.flexWrap       = Wrap.Wrap;
         style.justifyContent = Justify.FlexStart;
         style.alignItems     = Align.FlexStart;
         

         void OpenComponentsMenu(){
            if (EcsWorldDebugSystem == null) return;
            var searchWindow = ComponentsSearchWindow.CreateAndInit(EcsWorldDebugSystem);
            
            searchWindow.OnSelect = componentType => {
               if (Contains(componentType)) 
                  return false;
               
               AddTag(componentType, this);
               return true;
            };
         }
      }

      
      
      public void AddTag(Type type, TagsContainerView container, TagView tag = null){
         tag ??= new TagView(type, container);
         
         Insert(childCount-1, tag);
         _tags[type] = tag;
         
         OnAddTag?.Invoke(type);
      }
      public void RemoveTag(Type type){
         if (!Contains(type)) return;
         
         Remove(_tags[type]);
         _tags.Remove(type);
         
         OnRemoveTag?.Invoke(type);
      }
      public bool Contains(Type type){
         return _tags.ContainsKey(type);
      }
   }
}
