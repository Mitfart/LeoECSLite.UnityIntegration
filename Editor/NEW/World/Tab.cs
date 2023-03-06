using System;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW.World {
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
