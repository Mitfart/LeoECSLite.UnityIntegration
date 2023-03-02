using System;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Search;
using Mitfart.LeoECSLite.UnityIntegration.EntityView;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;
using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Menu{
   public static class MonoEntityViewContextualMenu{
      [MenuItem("CONTEXT/MonoEntityView/UpdateView")]
      public static void UpdateView(MenuCommand menuCommand){
         ((MonoEntityView)menuCommand.context).UpdateView();
      }
      
      [MenuItem("CONTEXT/MonoEntityView/UpdateView", true)]
      public static bool UpdateViewValidate(MenuCommand menuCommand){
         return Application.isPlaying;
      }
      
      
      
      [MenuItem("CONTEXT/MonoEntityView/Add Component")]
      public static void AddComponent(MenuCommand menuCommand){
         var monoView = (MonoEntityView) menuCommand.context;
         
         ComponentsSearchWindow.CreateAndInit(
            monoView.DebugSystem, componentType => {
               var pool = monoView.World.GetPool(componentType);
               if (pool.Has(monoView.Entity)){
                  Debug.Log($"Can't add another instance of <{componentType}>!");
                  return false;
               }

               pool.AddRaw(monoView.Entity, Activator.CreateInstance(componentType));
               monoView.GetOrAddComponentView(componentType);
               return true;
            });
      }
      
      [MenuItem("CONTEXT/MonoEntityView/Add Component", true)]
      public static bool AddComponentValidate(MenuCommand menuCommand){
         return Application.isPlaying;
      }
      

      
      [MenuItem("CONTEXT/MonoEntityView/Delete")]
      public static void Delete(MenuCommand menuCommand){
         ((MonoEntityView)menuCommand.context).Delete();
      }
      
      [MenuItem("CONTEXT/MonoEntityView/Delete", true)]
      public static bool DeleteValidate(MenuCommand menuCommand){
         return Application.isPlaying;
      }
   }
}
