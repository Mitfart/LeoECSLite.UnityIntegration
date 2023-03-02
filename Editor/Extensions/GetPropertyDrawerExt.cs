using System;
using System.Reflection;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions {
  public static class GetPropertyDrawerExt {
    private const string       GET_DRAWER_METHOD        = "GetDrawerTypeForType";
    private const string       SCRIPT_ATTRIBUTE_UTILITY = "UnityEditor.ScriptAttributeUtility";
    private const BindingFlags BINDING_FLAGS            = BindingFlags.NonPublic | BindingFlags.Static;

    
    private static readonly object ScriptAttributeUtility = 
      Assembly
       .GetAssembly(typeof(UnityEditor.Editor))
       .CreateInstance(SCRIPT_ATTRIBUTE_UTILITY);
    
    private static readonly MethodInfo GetDrawerType_Method_Info = 
      ScriptAttributeUtility
      ?.GetType()
       .GetMethod(GET_DRAWER_METHOD, BINDING_FLAGS);

    

    public static Type GetPropertyDrawerTypeFor(this Type classType) {
      return (Type) GetDrawerType_Method_Info
       .Invoke(
          ScriptAttributeUtility, 
          new object[]{ classType });
    }
  }
}
