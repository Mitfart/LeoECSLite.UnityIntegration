using System;
using System.Collections.Generic;
using System.Linq;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW.Entity.List {
   public sealed class EntitiesList : VisualElement {
      private Action<IEnumerable<object>> _selectEntities;

      private NewEDW   _window;
      private ListView _listView;

      private bool _isDirty;

      
      
      public EntitiesList() {
         CreateElements();
         AddElements();
         InitElements();
      }

      
      
      public void Init(NewEDW window, Action<IEnumerable<object>> selectEntities) {
         Reset();
         
         _window               = window;
         _selectEntities       = selectEntities;
         _listView.itemsSource = _window.ActiveDebugSystem.Sort.SortedAliveEntities;

         Refresh(true);
      }

      public void Refresh(bool anyway = false) {
         if (!anyway && !_isDirty) return;
         
         _listView.RefreshItems();
         _isDirty = false;
      }
      
      public void Reset() {
         _selectEntities = null;
#if UNITY_2022
         _listView.itemsSource = null;
#else
         _listView.itemsSource = Enumerable.Empty<int>().ToList();
#endif
         _listView.Rebuild();
      }



      public void MarkDirty(int entity) {
         _isDirty = true;
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
         _listView.style.SetPadding(Utils.METRICS_0750);
         _listView.style.paddingRight = Utils.METRICS_0250;
         _listView.style.height       = Utils.GetPercentsLength(100);
         _listView.fixedItemHeight    = Utils.METRICS_1750;
         _listView.makeItem           = MakeItem;
         _listView.bindItem           = BindItem;
#if UNITY_2022
         _listView.selectionChanged += OnSelectionChanged;
#else
         _listView.onSelectionChange += OnSelectionChanged;
#endif

         style.SetMargin(Utils.METRICS_0750);
         style.SetBorderRadius(Utils.METRICS_0750);
         style.height          = Utils.GetPercentsLength(100);
         style.overflow        = Overflow.Hidden;
         style.backgroundColor = Utils.Color_DD;
      }
      
      
      
      private static VisualElement MakeItem() {
         return new UIListEntity();
      }
      
      private void BindItem(VisualElement item, int i) {
         EcsWorldDebugSystem system     = _window.ActiveDebugSystem;
         int                 entity     = system.Sort.SortedAliveEntities[i];
         var                 entityView = (UIListEntity) item;
         
         if (!system.View.TryGetEntityView(entity, out MonoEntityView monoView)) return;
         if (entityView.MonoView == monoView) return;
         
         entityView.Reset();
         entityView.Init(monoView);
      }

      private void OnSelectionChanged(IEnumerable<object> objs) {
         _selectEntities?.Invoke(objs);
      }
   }
}
