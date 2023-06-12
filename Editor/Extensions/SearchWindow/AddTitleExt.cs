using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class AddTitleExt {
    public static void AddTitle(
      this ICollection<SearchTreeEntry> items,
      string                            text
    ) {
      items.Add(
        new SearchTreeGroupEntry(
          new GUIContent(
            text,
            AddItemExt.IndentationIcon
          )
        ) { level = 0 }
      );
    }
  }
}