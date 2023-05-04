/*
* source: https://stackoverflow.com/questions/68132287/unity3d-dont-compile-if-package-name-doesnt-exist
*/

using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace LeoECSLite.UnityIntegration {
  public static class DefineUtils {
    public static void Define(string define) {
#if UNITY_2021_2_OR_NEWER
      NamedBuildTarget namedBuildTarget = GetActiveNamedBuildTarget();
      PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out string[] defines);
#else
      var buildTargetGroup = GetActiveBuildTargetGroup();
      PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup, out var defines);
#endif

      var defineList = defines.ToList();

      if (!defineList.Contains(define))
        defineList.Add(define);

#if UNITY_2021_2_OR_NEWER
      PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, defineList.ToArray());
#else
      PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineList.ToArray());
#endif
    }



#if UNITY_2021_2_OR_NEWER
    private static NamedBuildTarget GetActiveNamedBuildTarget() {
      BuildTargetGroup buildTargetGroup = GetActiveBuildTargetGroup();
      var              namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);

      return namedBuildTarget;
    }
#endif

    private static BuildTargetGroup GetActiveBuildTargetGroup() {
      BuildTarget      buildTarget      = EditorUserBuildSettings.activeBuildTarget;
      BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);

      return buildTargetGroup;
    }
  }
}