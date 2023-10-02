using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Extensions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Search {
   public class ComponentsSearchWindow : ScriptableObject, ISearchWindowProvider {
      private const string _TITLE = "Components";

      private static ComponentsSearchWindow _window;

      private int                   _entity;
      private ComponentsSearchScope _searchScope;
      private Func<Type, bool>      _select;
      private EcsWorld              _world;



      public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
         var items  = new List<SearchTreeEntry>();
         var groups = new List<string>();

         items.AddTitle(_TITLE);

         return _searchScope switch {
            ComponentsSearchScope.World  => CreateSearchTreeForWorld(_world, items, groups),
            ComponentsSearchScope.Entity => CreateSearchTreeForEntity(_entity, _world, items, groups),
            ComponentsSearchScope.Unset  => throw new Exception(message: "Search Scope not set! (internal)"),
            _                            => throw new ArgumentOutOfRangeException()
         };
      }

      public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
         var component = (Type)searchTreeEntry.userData;
         return _select?.Invoke(component) ?? false;
      }



      private void Init(EcsWorld world, Func<Type, bool> select) {
         Init(world, entity: -1, select);
         _searchScope = ComponentsSearchScope.World;
      }

      private void Init(EcsWorld world, int entity, Func<Type, bool> select) {
         _world       = world;
         _select      = select;
         _entity      = entity;
         _searchScope = ComponentsSearchScope.Entity;
      }



      public static void OpenFor(EcsWorld world, Func<Type, bool> onChoose) {
         Window().Init(world, onChoose);
         SearchWindow.Open(MousePosition(), Window());
      }

      public static void OpenFor(EcsWorld world, int entity, Func<Type, bool> onChoose) {
         Window().Init(world, entity, onChoose);
         SearchWindow.Open(MousePosition(), Window());
      }



      private static List<SearchTreeEntry> CreateSearchTreeForWorld(EcsWorld world, IList<SearchTreeEntry> items, IList<string> groups) {
         world.ForeachPool(pool => { AddComponent(pool.GetComponentType(), items, groups); });
         return items.ToList();
      }

      private static List<SearchTreeEntry> CreateSearchTreeForEntity(
         int                    entity,
         EcsWorld               world,
         IList<SearchTreeEntry> items,
         IList<string>          groups
      ) {
         Type[] components = StaticCache.Types;
         int    count      = world.GetComponentTypes(entity, ref components);

         for (var i = 0; i < count; i++) {
            AddComponent(components[i], items, groups);
         }

         return items.ToList();
      }

      private static void AddComponent(Type component, IList<SearchTreeEntry> items, IList<string> groups)
         => items
           .AddNamespaceGroups(groups, component, out int indentLevel)
           .AddItem(component.Name, indentLevel, component);



      private static ComponentsSearchWindow Window() => _window ??= CreateInstance<ComponentsSearchWindow>();

      private static SearchWindowContext MousePosition() {
         Vector2 mousePos  = Event.current.mousePosition;
         Vector2 openPoint = GUIUtility.GUIToScreenPoint(mousePos);
         var     context   = new SearchWindowContext(openPoint);

         return context;
      }
   }
}