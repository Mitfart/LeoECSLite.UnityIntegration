using System;
using System.Collections.Generic;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Search;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Component;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Elements.Sort{
   public class SortTagsContainerView : Box{
      private readonly Dictionary<Type, SortTagView> _activeSortTags;
      private readonly EcsDebugWindow                _debugWindow;

      public event Action<Type> OnAddSortTag;
      public event Action<Type> OnRemoveSortTag;



      public SortTagsContainerView(EcsDebugWindow debugWindow){
         _activeSortTags = new Dictionary<Type, SortTagView>();
         _debugWindow    = debugWindow;

         Create();
         SetStyle();
      }
      
      
      private void Create() {
         Add(new SortTagView(Icons.Plus, OpenComponentsMenu));
      }
      
      private void SetStyle() {
         style.flexDirection  = FlexDirection.Row;
         style.flexWrap       = Wrap.Wrap;
         style.justifyContent = Justify.FlexStart;
         style.alignItems     = Align.FlexStart;
      }



      public void AddTag(Type type){
         var sortTag = new SortTagView(type.Name, () => RemoveTag(type));
         
         Insert(childCount - 1, sortTag);
         _activeSortTags[type] = sortTag;
         
         OnAddSortTag?.Invoke(type);
      }
      
      public void RemoveTag(Type type){
         if (!Contains(type)) return;
         
         Remove(_activeSortTags[type]);
         _activeSortTags.Remove(type);
         
         OnRemoveSortTag?.Invoke(type);
      }
      
      public bool Contains(Type type){
         return _activeSortTags.ContainsKey(type);
      }
      
      
      
      private void OpenComponentsMenu() {
         if (_debugWindow == null) return;

         ComponentsSearchWindow
           .CreateAndInit(
               _debugWindow.ActiveSystem,
               componentType => {
                  if (Contains(componentType)) return false;

                  AddTag(componentType);
                  return true;
               });
      }
   }
}
