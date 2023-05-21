using System.Collections.Generic;
using System.Linq;

namespace Extensions.Runtime.String {
  public static class CompactExt {
    public static string Compact(this string str) {
      const int CHARS_TO_SHOW = 3;

      char[] chars = str.ToCharArray();
      var uppercaseChars = chars
                          .Where(char.IsUpper)
                          .ToList();

      List<char> result = uppercaseChars.Count <= 0
        ? chars
         .Take(CHARS_TO_SHOW)
         .ToList()
        : uppercaseChars;

      return new string(result.ToArray());
    }
  }
}