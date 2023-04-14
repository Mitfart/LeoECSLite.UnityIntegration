using System;
using System.Collections.Generic;
using System.Linq;
using LeoECSLite.UnityIntegration.Editor.Extentions.SearchWindow;
using LeoECSLite.UnityIntegration.Extentions.EcsWorld;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Editor.Search {
  public class ComponentsSearchWindow : ScriptableObject, ISearchWindowProvider {
    private const string TITLE = "Components";

    private static ComponentsSearchWindow _Window;

    private EcsWorldDebugSystem _debugSystem;
    private Func<Type, bool>    _select;



    public static void OpenFor(EcsWorldDebugSystem debugSystem, Func<Type, bool> onChoose) {
      _Window ??= CreateInstance<ComponentsSearchWindow>();
      _Window.Init(debugSystem, onChoose);

      var context = new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition));

      SearchWindow.Open(
        context,
        _Window
      );
    }

    private void Init(EcsWorldDebugSystem debugSystem, Func<Type, bool> select) {
      _debugSystem = debugSystem;
      _select      = select;
    }



    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
      var items  = new HashSet<SearchTreeEntry>();
      var groups = new HashSet<string>();


      items.AddTitle(TITLE);

      _debugSystem.World.ForeachPool(
        pool => {
          Type component = pool.GetComponentType();
          items
           .AddGroupsByNamespace(
              groups,
              component,
              out int indentLevel
            )
           .AddItem(
              component.Name,
              indentLevel,
              component
            );
        }
      );

      return items.ToList();
    }


    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
      var component = (Type) searchTreeEntry.userData;
      return _select?.Invoke(component) ?? false;
    }
  }
}