#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mitfart.LeoECSLite.UnityIntegration.ComponentView{
    public static class EcvDatabase{
        public static readonly Dictionary<Type, BaseEcv> RegisteredEcv = new();


      
        static EcvDatabase(){
            var                      registerDummy = new GameObject();
            TypeCache.TypeCollection ecvTypes      = TypeCache.GetTypesDerivedFrom<BaseEcv>();

            foreach (Type type in ecvTypes){
                if (type.IsAbstract || type.IsGenericType) continue;
                if (registerDummy.AddComponent(type) is not BaseEcv view) continue;

                view.Register();
            }

            Object.DestroyImmediate(registerDummy);
        }



        public static void Register(this BaseEcv view) {
            Type componentType = view.GetComponentType();
            if (componentType == null) return;

            if (!RegisteredEcv.TryGetValue(componentType, out BaseEcv prevView) ||
                view.GetPriority() > prevView.GetPriority())
                RegisteredEcv[componentType] = view;
        }


        public static BaseEcv AddEcsComponentView(this GameObject go, Type type) {
            if (!type.IsValueType || type.IsPrimitive || type.IsEnum)
                throw new Exception("Can't add none struct type as Component!");

            if (RegisteredEcv.TryGetValue(type, out BaseEcv view)) return go.AddComponent(view.GetType()) as BaseEcv;

            var comp = (NotRegisteredEcv)go.AddComponent(typeof(NotRegisteredEcv));
            comp.SetComponentType(type);
            return comp;
        }
    }
}

#endif