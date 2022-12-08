using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class ComponentsSearchWindow : ScriptableObject, ISearchWindowProvider{
      private const string Title                = "Components";
      private const string TypeSeparator        = ".";
      private const string HierarchySeparator   = "/";
      private const string NotDefindedGroup     = "Not Definded";
      private const string GlobalNamespaceGroup = "_";

      private static readonly StringBuilder  Groups_Builder = new();
      private                 MonoEntityView _entityView;


      public void Init(MonoEntityView entityView){
         _entityView = entityView;
      }

      public static void CreateAndInit(MonoEntityView entityView){
         var searchWindow = CreateInstance<ComponentsSearchWindow>();
         searchWindow.Init(entityView);
         SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), searchWindow);
      }



      public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context){
         var list   = new List<SearchTreeEntry>();
         var groups = new List<string>();
         
         var notDefindedGroupExist     = false;
         var globalNamespaceGroupExist = false;
         var globalNamespaceGroupIndex = 1;

         list.Add(new SearchTreeGroupEntry(new GUIContent(Title)));

         foreach (var componentType in ECV_Database.Registered_Ecv.Keys){
            var splitedName = componentType.ToString().Split(TypeSeparator);
            var depth       = splitedName.Length;

            Groups_Builder.Clear();

            for (var i = 0; i < depth - 1; i++){
               Groups_Builder.Append(splitedName[i]);
               var group = Groups_Builder.ToString();

               if (!groups.Contains(group)){
                  list.Add(new SearchTreeGroupEntry(new GUIContent(splitedName[i]), i + 1));
                  groups.Add(group);
               }

               Groups_Builder.Append(HierarchySeparator);
            }

            if (!string.IsNullOrWhiteSpace(Groups_Builder.ToString())){
               list.Add(
                  new SearchTreeEntry(new GUIContent(splitedName.Last())){
                     level = depth, userData = componentType
                  });
               continue;
            }

            if (!globalNamespaceGroupExist){
               list.Insert(globalNamespaceGroupIndex, new SearchTreeGroupEntry(new GUIContent(GlobalNamespaceGroup), 1));
               globalNamespaceGroupExist = true;
            }
            globalNamespaceGroupIndex++;
            depth++;
            
            list.Insert(globalNamespaceGroupIndex, new SearchTreeEntry(new GUIContent(splitedName.Last())){
                  level = depth, userData = componentType
               });
         }

         _entityView.DebugSystem.ForeachPool( pool => {
               var componentType = pool.GetComponentType();
               if (ECV_Database.Registered_Ecv.ContainsKey(componentType)) return;
               if (!notDefindedGroupExist){
                  list.Add(new SearchTreeGroupEntry(new GUIContent(NotDefindedGroup), 1));
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
         var pool          = _entityView.World.GetPool(componentType);

         if (pool.Has(_entityView.Entity)){
            Debug.Log($"Can't add another instance of <{componentType}>!");
            return false;
         }

         pool.AddRaw(_entityView.Entity, Activator.CreateInstance(componentType));
         _entityView.GetOrAdd(componentType);
         return true;
      }
   }
}
