#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class GeneratorSettings : ScriptableSingleton<GeneratorSettings>{
      private const string DEFAULT_PREFIX              = "ECV_";
      private const string DEFAULT_POSTFIX             = "";
      private const string DEFAULT_SAVE_FOLDER_PATH    = "Generated_ECV";
      private const bool   DEFAULT_AUTO_GENERATE       = true;
      private const bool   DEFAULT_REWRITE_EXISTING    = false;
      private const bool   DEFAULT_GROUP_BY_NAMESPACES = true;
      
      public string prefix;
      public string postfix;
      public string relativeSaveFolderPath;
      public bool   autoGenerate;
      public bool   rewriteExisting;
      public bool   groupByNamespaces;
      

      private void OnValidate(){
         if (string.IsNullOrWhiteSpace(prefix) && string.IsNullOrWhiteSpace(postfix)) 
            prefix = DEFAULT_PREFIX;
         
         if (string.IsNullOrWhiteSpace(relativeSaveFolderPath))
            relativeSaveFolderPath = DEFAULT_SAVE_FOLDER_PATH;
         Save(false);
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
