using System.Collections.Generic;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class AddGroupExt {
    private const string HIERARCHY_SEPARATOR = "/"; // ! Important !

    private static readonly StringBuilder _GroupBuilder = new();



    public static void AddGroups(
      this IList<SearchTreeEntry> items,
      IList<string>               existingGroups,
      out    int                  indentLevel,
      params string[]             addGroups
    ) {
      _GroupBuilder.Clear();

      for (var i = 0; i < addGroups.Length;) {
        items.AddGroup(
          existingGroups,
          addGroups[i],
          ++i
        );
      }

      indentLevel = addGroups.Length + 1;
    }


    private static IList<SearchTreeEntry> AddGroup(
      this IList<SearchTreeEntry> items,
      IList<string>               groups,
      string                      groupName,
      int                         indentLevel
    ) {
      var currentGroup = _GroupBuilder
                        .Append(groupName)
                        .Append(HIERARCHY_SEPARATOR)
                        .ToString();

      if (groups.Contains(currentGroup))
        return items;

      groups.Add(currentGroup);
      items.AddGroupRaw(groupName, indentLevel);
      return items;
    }


    private static IList<SearchTreeEntry> AddGroupRaw(
      this IList<SearchTreeEntry> items,
      string                      groupName,
      int                         indentLevel
    ) {
      items.Add(
        new SearchTreeGroupEntry(
          new GUIContent(groupName),
          indentLevel
        )
      );
      return items;
    }
  }
}