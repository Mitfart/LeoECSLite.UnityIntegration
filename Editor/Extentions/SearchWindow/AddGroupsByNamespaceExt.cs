using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.SearchWindow {
  public static class AddGroupsByNamespaceExt {
    private const string TYPE_SEPARATOR         = "."; // ! Important !
    private const string GLOBAL_NAMESPACE_GROUP = "_";



    public static ICollection<SearchTreeEntry> AddGroupsByNamespace(
      this ICollection<SearchTreeEntry> items,
      ICollection<string>               groups,
      Type                              type,
      out int                           indentLevel
    ) {
      AddGroupExt.GroupsBuilder.Clear();

      string[] splitName = type
                          .ToString()
                          .Split(TYPE_SEPARATOR);

      indentLevel = splitName.Length;

      if (indentLevel > 1)
        for (var i = 1; i < indentLevel; i++)
          items.AddGroup(
            groups,
            splitName[i],
            i
          );
      else
        items.TryAddGlobalNamespaceGroup(groups);

      return items;
    }



    private static void TryAddGlobalNamespaceGroup(
      this ICollection<SearchTreeEntry> items,
      ICollection<string>               groups
    ) {
      if (!string.IsNullOrWhiteSpace(AddGroupExt.GroupsBuilder.ToString()))
        return;

      if (!groups.Contains(GLOBAL_NAMESPACE_GROUP))
        AddGlobalNamespaceGroup((IList<SearchTreeEntry>) items);
    }



    private static void AddGlobalNamespaceGroup(IList<SearchTreeEntry> items) {
      items.Insert(
        1,
        new SearchTreeGroupEntry(
          new GUIContent(GLOBAL_NAMESPACE_GROUP),
          1
        )
      );
    }
  }
}