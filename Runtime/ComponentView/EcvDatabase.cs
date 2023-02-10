using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mitfart.LeoECSLite.UnityIntegration.ComponentView{
   public static class EcvDatabase{
      public static readonly Dictionary<Type, BaseEcv> Registered_Ecv = new();


      
      static EcvDatabase(){
         var registerDummy = new GameObject();
         var ecvTypes = TypeCache.GetTypesDerivedFrom<BaseEcv>();

         foreach (var type in ecvTypes){
            if (type.IsAbstract || type.IsGenericType) continue;
            if (registerDummy.AddComponent(type) is not BaseEcv view) continue;

            view.Register();
         }

         Object.DestroyImmediate(registerDummy);
      }



      public static void Register(this BaseEcv view){
         var componentType = view.GetComponentType();
         if (componentType == null) return;

         if (!Registered_Ecv.TryGetValue(componentType, out var prevView) ||
             view.GetPriority() > prevView.GetPriority())
            Registered_Ecv[componentType] = view;
      }


      public static BaseEcv AddEcsComponentView(this GameObject go, Type type){
         if (!type.IsValueType || type.IsPrimitive || type.IsEnum)
            throw new Exception("Can't add none struct type as Component!");

         if (Registered_Ecv.TryGetValue(type, out var view)) return go.AddComponent(view.GetType()) as BaseEcv;

         var comp = (NotRegisteredEcv)go.AddComponent(typeof(NotRegisteredEcv));
         comp.SetComponentType(type);
         return comp;
      }
   }
}
