using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
   public static class AddGroupsByNamespaceExt {
      private const string _TYPE_SEPARATOR         = ".";
      private const string _GLOBAL_NAMESPACE_GROUP = "_";



      public static IList<SearchTreeEntry> AddNamespaceGroups(
         this IList<SearchTreeEntry> items,
         IList<string>               existingGroups,
         Type                        type,
         out int                     indentLevel
      ) {
         if (type.FromGlobalNamespace())
            items.AddGlobalNamespaceGroup(existingGroups, out indentLevel);
         else
            items.AddGroups(existingGroups, out indentLevel, NamespaceGroups(type));

         return items;
      }



      private static IList<SearchTreeEntry> AddGlobalNamespaceGroup(
         this IList<SearchTreeEntry> items,
         IList<string>               groups,
         out int                     indentLevel
      ) {
         if (!groups.Contains(_GLOBAL_NAMESPACE_GROUP)) {
            items.AddGlobalNamespaceGroupRaw();
            groups.Add(_GLOBAL_NAMESPACE_GROUP);
         }

         indentLevel = 2; // "1" - group => 2
         return items;
      }

      private static IList<SearchTreeEntry> AddGlobalNamespaceGroupRaw(
         this IList<SearchTreeEntry> items
      ) {
         items.Insert(
            index: 1, // under the title
            new SearchTreeGroupEntry(
               new GUIContent(_GLOBAL_NAMESPACE_GROUP),
               level: 1 // in the root of search window
            )
         );
         return items;
      }



      private static bool     FromGlobalNamespace(this Type type) => string.IsNullOrWhiteSpace(type.Namespace);
      private static string[] NamespaceGroups(Type          type) => type.Namespace?.Split(_TYPE_SEPARATOR);
   }
}