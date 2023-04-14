using System.Collections.Generic;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LeoECSLite.UnityIntegration.Editor.Extentions.SearchWindow {
  public static class AddGroupExt {
    private const string HIERARCHY_SEPARATOR = "/"; // ! Important !

    public static readonly StringBuilder GroupsBuilder = new();



    public static ICollection<SearchTreeEntry> AddGroup(
      this ICollection<SearchTreeEntry> items,
      ICollection<string>               groups,
      string                            groupName,
      int                               indentLevel
    ) {
      var currentGroup = GroupsBuilder
                        .Append(groupName)
                        .Append(HIERARCHY_SEPARATOR)
                        .ToString();

      if (groups.Contains(currentGroup))
        return items;

      groups.Add(currentGroup);

      return AddGroupRaw(items, groupName, indentLevel);
    }


    public static ICollection<SearchTreeEntry> AddGroupRaw(
      this ICollection<SearchTreeEntry> items,
      string                            groupName,
      int                               indentLevel
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