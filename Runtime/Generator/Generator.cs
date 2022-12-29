using System;
using System.IO;
using System.Text;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class Generator{
      private const           string       StartIf         = "#if UNITY_EDITOR";
      private const           string       EndIf           = "#endif";
      private const           string       Using           = "using";
      private const           string       PartialClass    = "public partial class";
      private static readonly UTF8Encoding Script_Encoding = new(true);


      public static bool Create(Type type, GeneratorSettings settings){
         return CreateScriptFile(type, settings);
      }

      private static bool CreateScriptFile(Type type, GeneratorSettings settings){
         string directory  = settings.GetFileDirectoryPath(type);
         string name       = settings.GetName(type);
         string fileByPath = GeneratorSettings.GetFilePath(directory, name);


         if (File.Exists(fileByPath)){
            if (!settings.rewriteExisting) return false;

            File.Delete(fileByPath);
         }

         if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);


         using FileStream fileStream    = File.Create(fileByPath);
         string           script        = GenerateScript(type, name);
         byte[]           encodedScript = Script_Encoding.GetBytes(script);
         fileStream.Write(encodedScript, 0, encodedScript.Length);

         return true;
      }

      private static string GenerateScript(Type type, string name){
         string typeNamespace   = type.Namespace;
         string systemNamespace = typeof(EcsWorldDebugSystem).Namespace;
         string ecvClassName    = typeof(ECV<>).GetCleanName();

         bool needNamespace = !string.IsNullOrWhiteSpace(typeNamespace) && typeNamespace != systemNamespace;
         return
            $"{StartIf} \n" +
            $"{Using} {systemNamespace}; \n" +
            (needNamespace ? $"{Using} {type.Namespace}; \n" : null) +
            $"{PartialClass} {name} : {ecvClassName}<{type.Name}>{{ }} \n" +
            $"{EndIf}";
      }
   }
}
