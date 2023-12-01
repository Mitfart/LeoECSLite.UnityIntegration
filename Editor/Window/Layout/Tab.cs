using UnityEditor.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Layout {
   public sealed class Tab<TData> : ToolbarButton where TData : class {
      private const string _MAIN_CL        = "tab";
      private const string _MAIN_ACTIVE_CL = "tab_active";

      private readonly TData           _data;
      private readonly TabsMenu<TData> _tabsMenu;



      public Tab(TabsMenu<TData> tabsMenu, TData data, string title) {
         _tabsMenu = tabsMenu;
         _data     = data;
         text      = title;

         AddToClassList(_MAIN_CL);

         clicked += SetAsActiveTab;
      }

      public void Destroy() => clicked -= SetAsActiveTab;



      public void SetActive(bool active) {
         if (active)
            AddToClassList(_MAIN_ACTIVE_CL);
         else
            RemoveFromClassList(_MAIN_ACTIVE_CL);
      }



      private void SetAsActiveTab() => _tabsMenu.SetActiveTab(_data);
   }
}