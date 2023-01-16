using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leopotam.EcsLite;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class ComponentsSearchWindow : ScriptableObject, ISearchWindowProvider{
      private const string TITLE                  = "Components";
      private const string TYPE_SEPARATOR         = ".";
      private const string HIERARCHY_SEPARATOR    = "/";
      private const string NOT_DEFINED_GROUP      = "Not Defined";
      private const string GLOBAL_NAMESPACE_GROUP = "_";

      private static readonly StringBuilder       Groups_Builder  = new();
      private static readonly Texture2D           IndentationIcon = new(1, 1);
      private                 EcsWorldDebugSystem _ecsWorldDebugSystem;

      public Func<Type, bool> OnSelect;

      
      
      
      
      
      public static ComponentsSearchWindow CreateAndInit(EcsWorldDebugSystem ecsWorldDebugSystem){
         var searchWindow = CreateInstance<ComponentsSearchWindow>().Init(ecsWorldDebugSystem);
         SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), searchWindow);
         
         IndentationIcon.SetPixel(0, 0, Color.clear);
         IndentationIcon.Apply();
         return searchWindow;
      }
      
      public ComponentsSearchWindow Init(EcsWorldDebugSystem ecsWorldDebugSystem){
         _ecsWorldDebugSystem = ecsWorldDebugSystem;
         return this;
      }

      
      public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context){
         var list   = new List<SearchTreeEntry>();
         var groups = new List<string>();
         
         var notDefindedGroupExist     = false;
         var globalNamespaceGroupExist = false;
         var globalNamespaceGroupIndex = 1;

         list.Add(new SearchTreeGroupEntry(new GUIContent(TITLE)));

         foreach (var componentType in ECV_Database.Registered_Ecv.Keys){
            var splitedName = componentType.ToString().Split(TYPE_SEPARATOR);
            var depth       = splitedName.Length;

            Groups_Builder.Clear();

            for (var i = 0; i < depth - 1; i++){
               Groups_Builder.Append(splitedName[i]);
               var group = Groups_Builder.ToString();

               if (!groups.Contains(group)){
                  list.Add(new SearchTreeGroupEntry(new GUIContent(splitedName[i]), i + 1));
                  groups.Add(group);
               }

               Groups_Builder.Append(HIERARCHY_SEPARATOR);
            }

            if (!string.IsNullOrWhiteSpace(Groups_Builder.ToString())){
               list.Add(
                  new SearchTreeEntry(new GUIContent(splitedName.Last(), IndentationIcon)){
                     level = depth, userData = componentType
                  });
               continue;
            }

            if (!globalNamespaceGroupExist){
               list.Insert(globalNamespaceGroupIndex, new SearchTreeGroupEntry(new GUIContent(GLOBAL_NAMESPACE_GROUP), 1));
               globalNamespaceGroupExist = true;
            }
            globalNamespaceGroupIndex++;
            depth++;
            
            list.Insert(globalNamespaceGroupIndex, new SearchTreeEntry(new GUIContent(splitedName.Last())){
                  level = depth, userData = componentType
               });
         }

         _ecsWorldDebugSystem.ForeachPool(pool => {
               var componentType = pool.GetComponentType();
               if (ECV_Database.Registered_Ecv.ContainsKey(componentType)) return;
               if (!notDefindedGroupExist){
                  list.Add(new SearchTreeGroupEntry(new GUIContent(NOT_DEFINED_GROUP), 1));
                  notDefindedGroupExist = true;
               }
               list.Add(new SearchTreeEntry(new GUIContent(componentType.Name)){ 
                  level = 2, userData = componentType 
               });
            });

         return list;
      }
      
      
      public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context){
         var componentType = (Type)searchTreeEntry.userData;
         return OnSelect?.Invoke(componentType) ?? false;
      }
   }
}
