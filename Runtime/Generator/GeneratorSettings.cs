#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class GeneratorSettings : ScriptableSingleton<GeneratorSettings>{
      public string prefix = "ECV_";
      public string postfix;
      public string relativeSaveFolderPath = "Generated_ECV";

      public bool autoGenerate = true;
      public bool rewriteExisting;
      public bool groupByNamespaces = true;


      private void OnEnable(){
         hideFlags &= ~HideFlags.NotEditable;
      }


      private void OnValidate(){
         if (string.IsNullOrWhiteSpace(prefix) && string.IsNullOrWhiteSpace(postfix)) prefix = "ECV_";
      }

      public string GetName(Type type){
         return $"{prefix}{type.Name}{postfix}";
      }

      public string GetSaveDirectoryPath(){
         return $"{Application.dataPath}/{relativeSaveFolderPath}/";
      }

      public string GetFileDirectoryPath(Type type){
         var rootPath = GetSaveDirectoryPath();
         if (!groupByNamespaces) return rootPath;

         var subPath = type.Namespace == null ? "_" : type.Namespace.Replace('.', '/');
         return $"{rootPath}{subPath}/";
      }

      public static string GetFilePath(string directory, string name){
         return $"{directory}{name}.cs";
      }

      public string GetFilePath(Type type){
         return GetFilePath(GetFileDirectoryPath(type), GetName(type));
      }
   }
}
#endif
