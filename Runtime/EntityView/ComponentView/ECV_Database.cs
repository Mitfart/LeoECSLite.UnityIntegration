using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class ECV_Database{
      public static readonly Dictionary<Type, BaseECV> Registered_Ecv = new();


      static ECV_Database(){
         var registerDummy = new GameObject();

         foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
         foreach (var type in assembly.GetTypes()){
            if (!typeof(BaseECV).IsAssignableFrom(type) ||
                type.IsAbstract ||
                type.IsGenericType)
               continue;
            if (registerDummy.AddComponent(type) is not BaseECV view) continue;

            view.Register();
         }

         Object.Destroy(registerDummy);
      }



      public static void Register(this BaseECV view){
         var componentType = view.GetComponentType();
         if (componentType == null) return;

         if (!Registered_Ecv.TryGetValue(componentType, out var prevView) ||
             view.GetPriority() > prevView.GetPriority())
            Registered_Ecv[componentType] = view;
      }


      public static BaseECV AddEcsComponentView(this GameObject go, Type type){
         if (!type.IsValueType || type.IsPrimitive || type.IsEnum)
            throw new Exception("Can't add none struct type as Component!");

         if (Registered_Ecv.TryGetValue(type, out var view)) return go.AddComponent(view.GetType()) as BaseECV;

         var comp = (NotDefinedECV)go.AddComponent(typeof(NotDefinedECV));
         comp.SetComponentType(type);
         return comp;
      }
   }
}
