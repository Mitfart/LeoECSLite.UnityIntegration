using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;

namespace LeoECSLite.UnityIntegration.Editor.Window.Layout {
  public class TabsMenu<TData> : Toolbar where TData : class {
    private const string MAIN_CL = "tabs";

    private readonly Action<TData> _onChangeTab;

    private readonly Dictionary<TData, Tab<TData>> _tabs;

    public TData ActiveTabData  { get; private set; }
    public int   ActiveTabIndex { get; private set; }



    public TabsMenu(Action<TData> onChangeTab) {
      _onChangeTab = onChangeTab;

      _tabs = new Dictionary<TData, Tab<TData>>();

      AddToClassList(MAIN_CL);
    }

    public void Reset() {
      Clear();
      _tabs.Clear();
    }



    public void AddTab(TData data) {
      if (_tabs.ContainsKey(data))
        throw new Exception($"Cant add existing Tab for {data}");

      _tabs.Add(data, AddTabFor(data));

      if (ActiveTabData == null)
        SetActiveTab(data);
    }

    public void RemoveTab(TData data) {
      if (!_tabs.ContainsKey(data))
        throw new Exception($"Cant find {data}");

      Tab<TData> tab = _tabs[data];
      Remove(tab);
      _tabs.Remove(data);
      tab.Destroy();


      if (data != ActiveTabData || _tabs.Count <= 0)
        return;

      int closestIndex = ActiveTabIndex - 1 >= 0
        ? ActiveTabIndex - 1
        : ActiveTabIndex + 1;

      TData closestTab = _tabs.Keys.ToArray()[closestIndex];
      SetActiveTab(closestTab);
    }

    public void SetActiveTab(TData data) {
      if (ActiveTabData != null)
        _tabs[ActiveTabData]
         .SetActive(false);

      ActiveTabData = data;

      ActiveTabIndex = _tabs
                      .Keys
                      .ToList()
                      .IndexOf(ActiveTabData);

      _tabs[ActiveTabData]
       .SetActive(true);

      _onChangeTab?.Invoke(data);
    }


    public IEnumerable<TData> GetWhere(Func<TData, bool> where)
      => _tabs
        .Keys
        .Where(where.Invoke);


    private Tab<TData> AddTabFor(TData data) {
      var tab = new Tab<TData>(this, data);
      Add(tab);
      return tab;
    }
  }
}