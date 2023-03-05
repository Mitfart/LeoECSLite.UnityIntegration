using System;
using System.Collections.Generic;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW {
   public sealed class EntitiesList : VisualElement {
      public event Action<IEnumerable<object>> OnSelectEntities;
      
      private List<int> _entities;
      private ListView  _listView;


      public EntitiesList() {
         CreateElements();
         AddElements();
         InitElements();
      }
      

      private void CreateElements() {
         _listView = new ListView();
      }

      private void AddElements() {
         this
           .AddChild(_listView)
            ;
      }
      
      private void InitElements() {
         _listView.fixedItemHeight = Utils.METRICS_1750;
         _listView.makeItem        = MakeItem;
         _listView.bindItem        = BindItem;
         
#if UNITY_2022
         _listView.selectionChanged += SelectEntities;
#else
         _listView.onSelectionChange += SelectEntities;
#endif
         
         
         style.SetMargin(Utils.METRICS_0750);
         style.SetBorderRadius(Utils.METRICS_0750);
         style.height          = Utils.GetPercentsLength(100);
         style.overflow        = Overflow.Hidden;
         style.backgroundColor = Utils.Color_DD;

         _listView.style.SetPadding(Utils.METRICS_0750);
         _listView.style.paddingRight = Utils.METRICS_0250;
         _listView.style.height       = Utils.GetPercentsLength(100);
      }

      
      public void Init(List<int> entities) {
         _listView.Clear();
         _listView.itemsSource = _entities = entities;
         _listView.RefreshItems();
         _listView.ClearSelection();
      }

      
      
      private static VisualElement MakeItem() {
         return new UIListEntity();
      }
      
      private void BindItem(VisualElement item, int i) {
         int entity     = _entities[i];
         var entityView = (UIListEntity) item;
         
         if (entityView.Entity == entity) return;
         
         UnbindItem(item, i);

         entityView.Init(entity, string.Empty, typeof(EntitiesList), typeof(Filter));
      }
      
      private static void UnbindItem(VisualElement item, int i) {
         var entityView = (UIListEntity) item;
         entityView.Reset();
      }
      
      

      private void SelectEntities(IEnumerable<object> objs) {
         OnSelectEntities?.Invoke(objs);
      }

      
      public void Reset() {
         _listView.Clear();
         _listView.itemsSource = null;
         _listView.RefreshItems();
         _listView.ClearSelection();
      }
   }
}
