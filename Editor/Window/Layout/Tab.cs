using UnityEditor.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Layout {
  public sealed class Tab<TData> : ToolbarButton where TData : class {
    private const string MAIN_CL        = "tab";
    private const string MAIN_ACTIVE_CL = "tab_active";

    private readonly TData           _data;
    private readonly TabsMenu<TData> _tabsMenu;



    public Tab(TabsMenu<TData> tabsMenu, TData data) {
      _tabsMenu = tabsMenu;
      _data     = data;
      text      = data.ToString();

      AddToClassList(MAIN_CL);

      clicked += SetAsActiveTab;
    }

    public void Destroy() => clicked -= SetAsActiveTab;



    public void SetActive(bool active) {
      if (active)
        AddToClassList(MAIN_ACTIVE_CL);
      else
        RemoveFromClassList(MAIN_ACTIVE_CL);
    }



    private void SetAsActiveTab() => _tabsMenu.SetActiveTab(_data);
  }
}