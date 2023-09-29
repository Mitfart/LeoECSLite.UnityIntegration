using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
   public static class AddItemExt {
      public static readonly Texture2D IndentationIcon = GetIndentationIcon();



      public static void AddItem(
         this ICollection<SearchTreeEntry> items,
         string                            title,
         int                               indentLevel,
         object                            data = null
      )
         => items.Add(
            new SearchTreeEntry(
               new GUIContent(
                  title,
                  IndentationIcon
               )
            ) { level = indentLevel, userData = data }
         );



      private static Texture2D GetIndentationIcon() {
         var icon = new Texture2D(width: 1, height: 1);

         icon.SetPixel(x: 0, y: 0, Color.clear);
         icon.Apply();

         return icon;
      }
   }
}