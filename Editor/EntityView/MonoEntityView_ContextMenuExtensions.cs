using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public class MonoEntityView_ContextMenuExtensions{
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
         ComponentsSearchWindow.CreateAndInit((MonoEntityView)menuCommand.context);
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
