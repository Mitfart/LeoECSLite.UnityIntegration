using System;
using System.Collections.Generic;
using System.Linq;
using LeoECSLite.UnityIntegration.Editor.Extentions.SearchWindow;
using LeoECSLite.UnityIntegration.Extentions.EcsWorld;
using Leopotam.EcsLite;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Editor.Search {
  public class ComponentsSearchWindow : ScriptableObject, ISearchWindowProvider {
    private const string TITLE = "Components";

    private static ComponentsSearchWindow _Window;

    private EcsWorld              _world;
    private int                   _entity;
    private Func<Type, bool>      _select;
    private ComponentsSearchScope _searchScope;



    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
      var items  = new HashSet<SearchTreeEntry>();
      var groups = new HashSet<string>();


      items.AddTitle(TITLE);

      switch (_searchScope) {
        case ComponentsSearchScope.World:
          _world.ForeachPool(pool => { AddComponent(pool.GetComponentType()); });
          break;
        case ComponentsSearchScope.Entity:
          object[] components = StaticCache.Components;
          int      count      = _world.GetComponents(_entity, ref components);

          for (var i = 0; i < count; i++) {
            AddComponent(
              components[i]
               .GetType()
            );
          }

          break;
        case ComponentsSearchScope.unset:
          throw new Exception("Search Scope not set! (internal error)");
        default:
          throw new ArgumentOutOfRangeException();
      }

      return items.ToList();


      void AddComponent(Type componentType) {
        items
         .AddGroupsByNamespace(
            groups,
            componentType,
            out int indentLevel
          )
         .AddItem(
            componentType.Name,
            indentLevel,
            componentType
          );
      }
    }


    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
      var component = (Type) searchTreeEntry.userData;
      return _select?.Invoke(component) ?? false;
    }



    public static void OpenFor(EcsWorld world, Func<Type, bool> onChoose) {
      _Window ??= CreateWindow();
      _Window.Init(
        world,
        onChoose
      );
      OpenInMousePosition();
    }

    public static void OpenFor(EcsWorld world, int entity, Func<Type, bool> onChoose) {
      _Window ??= CreateWindow();
      _Window.Init(
        world,
        entity,
        onChoose
      );
      OpenInMousePosition();
    }



    private static ComponentsSearchWindow CreateWindow() {
      return CreateInstance<ComponentsSearchWindow>();
    }

    private static void OpenInMousePosition() {
      Vector2 mousePos  = Event.current.mousePosition;
      Vector2 openPoint = GUIUtility.GUIToScreenPoint(mousePos);
      var     context   = new SearchWindowContext(openPoint);

      SearchWindow.Open(
        context,
        _Window
      );
    }



    private void Init(EcsWorld world, Func<Type, bool> select) {
      Init(
        world,
        -1,
        select
      );
      _searchScope = ComponentsSearchScope.World;
    }

    private void Init(EcsWorld world, int entity, Func<Type, bool> select) {
      _world       = world;
      _select      = select;
      _entity      = entity;
      _searchScope = ComponentsSearchScope.Entity;
    }
  }
}