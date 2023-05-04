/*
* source: https://stackoverflow.com/questions/68132287/unity3d-dont-compile-if-package-name-doesnt-exist
*/

namespace LeoECSLite.UnityIntegration {
  public static class Defines {
#if !LEOECSLITE_INTEGRATION
    [InitializeOnLoadMethod] private static void Init() => DefineUtils.Define("LEOECSLITE_INTEGRATION");
#endif
  }
}