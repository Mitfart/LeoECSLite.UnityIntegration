using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class EcvContextMenuExtensions{
      [MenuItem("CONTEXT/BaseECV/UpdateValue")]
      public static void UpdateValue(MenuCommand menuCommand){
         ((BaseECV)menuCommand.context).UpdateValue();
      }
      [MenuItem("CONTEXT/BaseECV/UpdateValue", true)]
      public static bool UpdateValueValidate(MenuCommand menuCommand){
         return Application.isPlaying && menuCommand.context != null;
      }


      [MenuItem("CONTEXT/BaseECV/Remove")]
      public static void Remove(MenuCommand menuCommand){
         ((BaseECV)menuCommand.context).Delete();
      }
      [MenuItem("CONTEXT/BaseECV/Remove", true)]
      public static bool RemoveValidate(MenuCommand menuCommand){
         return Application.isPlaying && menuCommand.context != null;
      }
   }
}
