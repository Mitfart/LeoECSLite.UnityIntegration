#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityIntegration.Generator{
   [FilePath(SETTINGS_SAVE_PATH, FilePathAttribute.Location.PreferencesFolder)]
   public sealed class EcvGeneratorSettings : ScriptableSingleton<EcvGeneratorSettings> {
      private const string SETTINGS_SAVE_PATH          = "Plugins/Mitfart/LeoECSLite_UnityIntegration/EcvGeneratorSettings.st";
      private const string DEFAULT_PREFIX              = "ECV_";
      private const string DEFAULT_POSTFIX             = "";
      private const string DEFAULT_SAVE_FOLDER_PATH    = "Generated_ECV";
      private const bool   DEFAULT_AUTO_GENERATE       = true;
      private const bool   DEFAULT_REWRITE_EXISTING    = false;
      private const bool   DEFAULT_GROUP_BY_NAMESPACES = true;
      
      public string prefix                 = DEFAULT_PREFIX;
      public string postfix                = DEFAULT_POSTFIX;
      public string relativeSaveFolderPath = DEFAULT_SAVE_FOLDER_PATH;
      public bool   autoGenerate           = DEFAULT_AUTO_GENERATE;
      public bool   rewriteExisting        = DEFAULT_REWRITE_EXISTING;
      public bool   groupByNamespaces      = DEFAULT_GROUP_BY_NAMESPACES;

      public bool IsDirty => EditorUtility.IsDirty(this);
      
      private void OnValidate(){
         if (string.IsNullOrWhiteSpace(prefix) && string.IsNullOrWhiteSpace(postfix)) 
            prefix = DEFAULT_PREFIX;
         
         if (string.IsNullOrWhiteSpace(relativeSaveFolderPath))
            relativeSaveFolderPath = DEFAULT_SAVE_FOLDER_PATH;
         
         if (!IsDirty) 
            EditorUtility.SetDirty(this);
      }

      public void Save() {
         if (IsDirty) base.Save(true);
      }
      
      
      
      public string GetName(Type type){
         return $"{prefix}{type.Name}{postfix}";
      }
      
      public string GetSaveDirectoryPath(){
         return $"{Application.dataPath}/{relativeSaveFolderPath}/";
      }
      
      public string GetFileDirectoryPath(Type type){
         string rootPath = GetSaveDirectoryPath();
         if (!groupByNamespaces) return rootPath;

         string subPath = type.Namespace == null ? "_" : type.Namespace.Replace('.', '/');
         return $"{rootPath}{subPath}/";
      }
      
      public static string GetFilePath(string directory, string name){
         return $"{directory}{name}.cs";
      }
   }
}
#endif
