using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Git.Extensions.Editor {
  public static class AddGroupsByNamespaceExt {
    private const string TYPE_SEPARATOR = ".";

    private const string GLOBAL_NAMESPACE_GROUP = "_";



    public static IList<SearchTreeEntry> AddNamespaceGroups(
      this IList<SearchTreeEntry> items,
      IList<string>               groups,
      Type                        type,
      out int                     indentLevel
    ) {
      if (type.FromGlobalNamespace())
        items.AddGlobalNamespaceGroup(groups, out indentLevel);
      else
        items.AddGroups(groups, out indentLevel, NamespaceGroup(type));

      return items;
    }



    private static IList<SearchTreeEntry> AddGlobalNamespaceGroup(this IList<SearchTreeEntry> items, IList<string> groups, out int indentLevel) {
      if (!groups.Contains(GLOBAL_NAMESPACE_GROUP)) {
        items.AddGlobalNamespaceGroupRaw();
        groups.Add(GLOBAL_NAMESPACE_GROUP);
      }

      indentLevel = 2; // "1" - group => 2
      return items;
    }

    private static IList<SearchTreeEntry> AddGlobalNamespaceGroupRaw(this IList<SearchTreeEntry> items) {
      items.Insert(
        1, // under the title
        new SearchTreeGroupEntry(
          new GUIContent(GLOBAL_NAMESPACE_GROUP),
          1 // in the root of search window
        )
      );
      return items;
    }



    private static bool     FromGlobalNamespace(this Type type) => string.IsNullOrWhiteSpace(type.Namespace);
    private static string[] NamespaceGroup(Type           type) => type.Namespace?.Split(TYPE_SEPARATOR);
  }
}