using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.EntityView{
   public static class EcvContextMenuExtensions{
      [MenuItem("CONTEXT/BaseECV/UpdateValue")]
      public static void UpdateValue(MenuCommand menuCommand){
         ((BaseEcv)menuCommand.context).UpdateValue();
      }
      [MenuItem("CONTEXT/BaseECV/UpdateValue", true)]
      public static bool UpdateValueValidate(MenuCommand menuCommand){
         return Application.isPlaying && menuCommand.context != null;
      }


      [MenuItem("CONTEXT/BaseECV/Remove")]
      public static void Remove(MenuCommand menuCommand){
         ((BaseEcv)menuCommand.context).Delete();
      }
      [MenuItem("CONTEXT/BaseECV/Remove", true)]
      public static bool RemoveValidate(MenuCommand menuCommand){
         return Application.isPlaying && menuCommand.context != null;
      }
   }
}
