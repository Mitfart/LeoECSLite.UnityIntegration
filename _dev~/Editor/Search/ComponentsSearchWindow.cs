using System;
using System.Collections.Generic;
using System.Text;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using NodeEngine.Editor.Search;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Search{
   public class ComponentsSearchWindow : ScriptableObject, ISearchWindowProvider{
      private const string TITLE                  = "Components";
      private const string NOT_REGISTERED_GROUP      = "Not Defined";
      private const string GLOBAL_NAMESPACE_GROUP = "_";
      private const string TYPE_SEPARATOR         = "."; // ! Important !
      private const string HIERARCHY_SEPARATOR    = "/"; // ! Important !

      private static readonly StringBuilder GroupsBuilder = new();
      private static          Texture2D     _indentationIcon;
      
      public Func<Type, bool> OnChoose { get; private set; }
      
      private EcsWorldDebugSystem _ecsWorldDebugSystem;

      
      
      public ComponentsSearchWindow Init(EcsWorldDebugSystem ecsWorldDebugSystem, Func<Type, bool> onChoose) {
         _ecsWorldDebugSystem = ecsWorldDebugSystem;
         _indentationIcon     = SearchWindowUtils.GetIndentationIcon();
         OnChoose             = onChoose;
         return this;
      }
      
      
      
      public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context){
         var items  = new List<SearchTreeEntry>();
         var groups = new HashSet<string>();
         
         var notRegisteredGroupExist   = false;
         var globalNamespaceGroupExist = false;

         
         AddTitle(items);

         AddRegisteredComponents();

         //AddNotRegisteredComponents();

         return items;

         
         void AddRegisteredComponents() {
            var componentTypes = EcvDatabase.RegisteredEcv.Keys;

            foreach (var componentType in componentTypes) {
               GroupsBuilder.Clear();
            
               AddGroupsByNamespace(items, groups, componentType, out var indentLevel);
               //TryAddGlobalNamespaceGroup(ref indentLevel);

               AddItem(items, componentType.Name, indentLevel, componentType);
            }
         }

         void TryAddGlobalNamespaceGroup(ref int indentLevel) {
            if (!string.IsNullOrWhiteSpace(GroupsBuilder.ToString())) return;

            if (!globalNamespaceGroupExist) {
               AddGlobalNamespaceGroup(items);
               globalNamespaceGroupExist = true;
            }

            indentLevel++;
         }
         
         void AddNotRegisteredComponents() {
            _ecsWorldDebugSystem.ForeachPool(pool => {
               var componentType = pool.GetComponentType();
            
               if (EcvDatabase.RegisteredEcv.ContainsKey(componentType)) return;
            
               if (!notRegisteredGroupExist) {
                  AddGroup(items, groups, NOT_REGISTERED_GROUP, 1);
                  notRegisteredGroupExist = true;
               }

               AddItem(items, componentType.Name, 2, componentType);
            });
         }
      }

      
      private static void AddTitle(ICollection<SearchTreeEntry> items) {
         items.Add(new SearchTreeGroupEntry(new GUIContent(TITLE)));
      }
      
      private static void AddGroupsByNamespace(
         ICollection<SearchTreeEntry> items,
         ICollection<string>          groups,
         Type                         componentType,
         out int                      indentLevel
      ) {
         var splitName = componentType.ToString().Split(TYPE_SEPARATOR);
         
         indentLevel = splitName.Length;
         
         for (var i = 0; i < indentLevel-1; i++)
            AddGroup(
               items,
               groups,
               splitName[i],
               i + 1);
      }
      
      private static void AddGroup(
         ICollection<SearchTreeEntry> items, 
         ICollection<string>          groups, 
         string                       groupName,
         int                          indentLevel) 
      {
         var groupFullName = 
            GroupsBuilder
              .Append(groupName)
              .Append(HIERARCHY_SEPARATOR)
              .ToString();
         
         if (groups.Contains(groupFullName)) return;
            
         items.Add(
            new SearchTreeGroupEntry(
               new GUIContent(groupName), 
               indentLevel
            )
         );
         groups.Add(groupFullName);
      }

      private static void AddItem(
         ICollection<SearchTreeEntry> items,
         string                       name,
         int                          indentLevel,
         object                       data
      ) {
         items.Add(
            new SearchTreeEntry(
               new GUIContent(name, _indentationIcon)){
               level    = indentLevel, 
               userData = data
            });
      }
      
      private static void AddGlobalNamespaceGroup(IList<SearchTreeEntry> items, int i = 1) {
         items.Insert(
            i, new SearchTreeGroupEntry(
               new GUIContent(GLOBAL_NAMESPACE_GROUP), 1));
      }
      
      
      
      public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context){
         var componentType = (Type) searchTreeEntry.userData;
         return OnChoose?.Invoke(componentType) ?? false;
      }
      
      
      
      public static ComponentsSearchWindow CreateAndInit(EcsWorldDebugSystem debugSystem, Func<Type, bool> onChoose) {
         var search = 
            CreateInstance<ComponentsSearchWindow>()
           .Init(debugSystem, onChoose);
         
         SearchWindow.Open(
            new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
            search);
         
         return search;
      }
   }
}
