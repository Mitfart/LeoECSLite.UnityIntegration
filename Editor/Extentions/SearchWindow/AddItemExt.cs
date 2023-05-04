﻿using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Editor.Extentions {
  public static class AddItemExt {
    public static readonly Texture2D IndentationIcon = GetIndentationIcon();


    public static void AddItem(
      this ICollection<SearchTreeEntry> items,
      string                            title,
      int                               indentLevel,
      object                            data
    )
      => items.Add(new SearchTreeEntry(new GUIContent(title, IndentationIcon)) { level = indentLevel, userData = data });



    private static Texture2D GetIndentationIcon() {
      var icon = new Texture2D(1, 1);

      icon.SetPixel(0, 0, Color.clear);
      icon.Apply();

      return icon;
    }
  }
}