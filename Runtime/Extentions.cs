using System;
using System.Reflection;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class Extensions{
      #region Type

      public static string GetCleanName(this Type type){
         if (!type.IsGenericType) return type.Name;

         int genericIndex = type.Name.LastIndexOf("`", StringComparison.Ordinal);
         string typeName = genericIndex == -1
                              ? type.Name
                              : type.Name[..genericIndex];
         return $"{typeName}";
      }

      #endregion


      #region EcsLite

      private static readonly MethodInfo Get_Pool_Method_Info = typeof(EcsWorld).GetMethod(nameof(EcsWorld.GetPool));
      private static readonly MethodInfo Filter_Method_Info   = typeof(EcsWorld).GetMethod(nameof(EcsWorld.Filter));

      private static readonly MethodInfo Exc_Method_Info =
         typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Exc));

      private static readonly MethodInfo Inc_Method_Info =
         typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Inc));


      public static IEcsPool GetPool(this EcsWorld world, Type type){
         IEcsPool pool = world.GetPoolByType(type);
         if (pool != null) return pool;

         MethodInfo getPool = Get_Pool_Method_Info.MakeGenericMethod(type);
         pool = (IEcsPool)getPool.Invoke(world, null);

         return pool;
      }


      public static EcsWorld.Mask Filter(this EcsWorld world, Type type){
         MethodInfo getFilter = Filter_Method_Info.MakeGenericMethod(type);
         return (EcsWorld.Mask)getFilter.Invoke(world, null);
      }


      public static EcsWorld.Mask Inc(this EcsWorld.Mask mask, Type type){
         MethodInfo method = Inc_Method_Info.MakeGenericMethod(type);
         return (EcsWorld.Mask)method.Invoke(mask, null);
      }


      public static EcsWorld.Mask Exc(this EcsWorld.Mask mask, Type type){
         MethodInfo method = Exc_Method_Info.MakeGenericMethod(type);
         return (EcsWorld.Mask)method.Invoke(mask, null);
      }

      #endregion
   }
}
