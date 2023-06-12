using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Window.Layout {
  public class TabsMenu<TData> : Toolbar where TData : class {
    private const string MAIN_CL = "tabs";

    private readonly Dictionary<TData, Tab<TData>> _tabs;

    private readonly Action<TData> _onChangeTab;

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

      Tab<TData> tab = CreateTab(data);
      Add(tab);
      _tabs.Add(data, tab);

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

      if (RemoveActiveTab() && !RemoveLastTab())
        SetActiveTab(ClosestTab());


      bool RemoveLastTab() {
        return _tabs.Count <= 0;
      }

      bool RemoveActiveTab() {
        return data == ActiveTabData;
      }

      TData ClosestTab() {
        int closestIndex = ActiveTabIndex - 1 >= 0
          ? ActiveTabIndex - 1
          : ActiveTabIndex + 1;

        return _tabs.Keys.ToArray()[closestIndex];
      }
    }

    public void SetActiveTab(TData data) {
      if (ActiveTabData != null)
        _tabs[ActiveTabData].SetActive(false);

      ActiveTabData = data;

      _tabs[ActiveTabData]
       .SetActive(true);

      ActiveTabIndex =
        _tabs
         .Keys
         .ToList()
         .IndexOf(ActiveTabData);

      _onChangeTab?.Invoke(data);
    }



    public IEnumerable<TData> Where(Func<TData, bool> where) => _tabs.Keys.Where(where.Invoke);

    private Tab<TData> CreateTab(TData data) => new(this, data);
  }
}