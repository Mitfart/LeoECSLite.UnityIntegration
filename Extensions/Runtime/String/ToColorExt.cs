using System.Text;
using UnityEngine;

namespace Extensions.Runtime.String {
  public static class ToColorExt {
    public static Color ToColor(this string str) {
      byte[] bytes = Encoding.ASCII.GetBytes(str);

      var r     = 0f;
      var g     = 0f;
      var b     = 0f;
      var queue = 0;

      for (var i = 0; i < bytes.Length; i++, queue++) {
        switch (queue) {
          case 0:
            b += bytes[i];
            break;
          case 1:
            g += bytes[i];
            break;
          case 2:
            r     += bytes[i];
            queue =  0;
            break;
        }
      }

      return new Color(
        r.Normalized(),
        g.Normalized(),
        b.Normalized()
      );
    }

    private static float Normalized(this float a) {
      a /= 256f;
      a %= 1;
      return a;
    }
  }
}