using System;
using System.Collections.Generic;
using System.Linq;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW {
   public class Tabs<TData> : ScrollView where TData : class {
      private readonly List<TData>                   _data;
      private readonly Dictionary<TData, Tab<TData>> _tabs;

      public TData ActiveTab      { get; private set; }
      public int   ActiveTabIndex { get; private set; }

      public event Action<TData> OnChangeTab;


      
      public Tabs() : base(ScrollViewMode.Horizontal) {
         _data = new List<TData>();
         _tabs = new Dictionary<TData, Tab<TData>>();
         
         InitElements();
      }

      private void InitElements() {
         style.backgroundColor = Utils.Color_L;
      }



      public void AddTab(TData data) {
         if (_data.Contains(data)) 
            throw new Exception($"Cant add existing Tab for {data}");

         _data.Add(data);
         _tabs[data] = CreateTabFor(data);
         
         if (ActiveTab == null)
            SetActiveTab(data);
      }
      
      public void RemoveTab(TData data) {
         if (!_data.Contains(data)) 
            throw new Exception($"Cant find {data}");
         
         _data.Remove(data);
         
         Tab<TData> tab = _tabs[data];
         _tabs.Remove(data);
         tab.Dispose();
         Remove(tab);


         if (_data != ActiveTab || _tabs.Count <= 0) return;

         int   closestIndex = ActiveTabIndex - 1 >= 0 ? ActiveTabIndex - 1 : ActiveTabIndex + 1;
         TData closestTab   = _tabs.Keys.ToList()[closestIndex];
         SetActiveTab(closestTab);
      }
      
      public void SetActiveTab(TData data) {
         if (ActiveTab != null && _tabs.TryGetValue(ActiveTab, out Tab<TData> view)) 
            view.Deactivate();
         
         ActiveTab      = data;
         ActiveTabIndex = _tabs.Keys.ToList().IndexOf(ActiveTab);
         
         _tabs[ActiveTab].Activate();
         
         OnChangeTab?.Invoke(data);
      }
      
      
      private Tab<TData> CreateTabFor(TData data) {
         return this.AddAndGet(new Tab<TData>(this, data));
      }
      
      

      public void Reset() {
         Clear();
         _data.Clear();
         _tabs.Clear();
      }
   }

   
   public sealed class Tab<TData> : Button, IDisposable where TData : class  {
      private readonly TData       _data;
      private readonly Tabs<TData> _tabs;
      
      private bool _isActive;

      
      
      public Tab(Tabs<TData> tabs, TData data) {
         _tabs = tabs;
         _data = data;
         text  = data.ToString();

         InitElements();
         RegisterHover();
         
         clicked += SetAsActiveTab;
      }
      
      
      private void InitElements() {
         style.SetMargin(0);
         style.SetPadding(Utils.METRICS_1500, Utils.METRICS_0500);
         style.SetBorderRadius(0);
         style.SetBorderColor(new StyleColor(StyleKeyword.None));
         
         style.backgroundColor   = new StyleColor(StyleKeyword.None);
         style.borderBottomColor = new StyleColor(StyleKeyword.None);
         style.borderBottomWidth = Utils.METRICS_0125;
         style.opacity           = .5f;
      }

      public void Dispose() {
         clicked -= SetAsActiveTab;
      }


      
      public void Activate() {
         style.borderBottomColor = Utils.Color_Primary;
         _isActive               = true;
      }

      public void Deactivate() {
         style.borderBottomColor = new StyleColor(StyleKeyword.None);
         _isActive               = false;
      }

      
      public void Hover() {
         if (_isActive) return;
         style.borderBottomColor = Utils.Color_LLL;
      }

      public void Unhover() {
         if (_isActive) return;
         style.borderBottomColor = new StyleColor(StyleKeyword.None);
      }
      

      private void SetAsActiveTab() {
         _tabs.SetActiveTab(_data);
      }
      
      
      private void RegisterHover() {
         RegisterCallback<MouseOverEvent>(_ => Hover());
         RegisterCallback<MouseOutEvent>(_ => Unhover());
      }
   }
}
