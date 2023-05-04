﻿using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Editor.Extentions {
  public static class AddTitleExt {
    public static void AddTitle(this ICollection<SearchTreeEntry> items, string text)
      => items.Add(
        new SearchTreeGroupEntry(
          new GUIContent(
            text,
            AddItemExt.IndentationIcon
          )
        )
      );
  }
}