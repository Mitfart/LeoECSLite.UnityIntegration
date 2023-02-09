using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mitfart.LeoECSLite.UnityIntegration.ComponentView{
   public static class EcvDatabase{
      public static readonly Dictionary<Type, BaseEcv> Registered_Ecv = new();


      
      static EcvDatabase(){
         var registerDummy = new GameObject();

         foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
         foreach (var type in assembly.GetTypes()){
            if (!typeof(BaseEcv).IsAssignableFrom(type) ||
                type.IsAbstract ||
                type.IsGenericType)
               continue;
            if (registerDummy.AddComponent(type) is not BaseEcv view) continue;

            view.Register();
         }

         Object.Destroy(registerDummy);
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

         var comp = (NotDefinedEcv)go.AddComponent(typeof(NotDefinedEcv));
         comp.SetComponentType(type);
         return comp;
      }
   }
}
