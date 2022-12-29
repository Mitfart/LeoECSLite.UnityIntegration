using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class ECV_Database{
      public static readonly Dictionary<Type, BaseECV> Registered_Ecv = new();


      static ECV_Database(){
         GameObject registerDummy = new GameObject();

         foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
         foreach (Type type in assembly.GetTypes()){
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
         Type componentType = view.GetComponentType();
         if (componentType == null) return;

         if (!Registered_Ecv.TryGetValue(componentType, out BaseECV prevView) ||
             view.GetPriority() > prevView.GetPriority())
            Registered_Ecv[componentType] = view;
      }


      public static BaseECV AddEcsComponentView(this GameObject go, Type type){
         if (!type.IsValueType || type.IsPrimitive || type.IsEnum)
            throw new Exception("Can't add none struct type as Component!");

         if (Registered_Ecv.TryGetValue(type, out BaseECV view)) return go.AddComponent(view.GetType()) as BaseECV;

         NotDefinedECV comp = (NotDefinedECV)go.AddComponent(typeof(NotDefinedECV));
         comp.SetComponentType(type);
         return comp;
      }
   }
}
