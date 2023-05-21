using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Git.Extensions.Editor {
  public static class AddAllOfExt {
    public static VisualElement AddAllOf<T>(this VisualElement root, IEnumerable<T> collection, Func<T, VisualElement> createItem) {
      foreach (T item in collection)
        root.Add(createItem(item));
      return root;
    }

    public static VisualElement AddAllOf(this VisualElement root, IEnumerable collection, Func<object, VisualElement> createItem) {
      foreach (object item in collection)
        root.Add(createItem(item));
      return root;
    }



    public static VisualElement AddAllOf<T>(this VisualElement root, IEnumerable<T> collection, Func<T, int, VisualElement> createItem) {
      T[] enumerable = collection as T[] ?? collection.ToArray();

      for (var i = 0; i < enumerable.Length; i++)
        root.Add(createItem(enumerable[i], i));

      return root;
    }
  }
}