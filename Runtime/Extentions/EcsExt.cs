using System;
using System.Reflection;
using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityIntegration {
    public static class EcsExt {
        private static readonly MethodInfo GetPoolMethodInfo = typeof(EcsWorld).GetMethod(nameof(EcsWorld.GetPool));
        private static readonly MethodInfo FilterMethodInfo   = typeof(EcsWorld).GetMethod(nameof(EcsWorld.Filter));

        private static readonly MethodInfo ExcMethodInfo =
            typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Exc));

        private static readonly MethodInfo IncMethodInfo =
            typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Inc));


        public static IEcsPool GetPool(this EcsWorld world, Type type) {
            IEcsPool pool = world.GetPoolByType(type);
            if (pool != null) return pool;

            MethodInfo getPool = GetPoolMethodInfo.MakeGenericMethod(type);
            pool = (IEcsPool)getPool.Invoke(world, null);

            return pool;
        }


        public static EcsWorld.Mask Filter(this EcsWorld world, Type type) {
            MethodInfo getFilter = FilterMethodInfo.MakeGenericMethod(type);
            return (EcsWorld.Mask)getFilter.Invoke(world, null);
        }


        public static EcsWorld.Mask Inc(this EcsWorld.Mask mask, Type type) {
            MethodInfo method = IncMethodInfo.MakeGenericMethod(type);
            return (EcsWorld.Mask)method.Invoke(mask, null);
        }


        public static EcsWorld.Mask Exc(this EcsWorld.Mask mask, Type type) {
            MethodInfo method = ExcMethodInfo.MakeGenericMethod(type);
            return (EcsWorld.Mask)method.Invoke(mask, null);
        }
    }
}
