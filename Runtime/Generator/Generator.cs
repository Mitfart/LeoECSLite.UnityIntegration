using System;
using System.IO;
using System.Text;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public static class Generator{
      private const           string       START_IF        = "#if UNITY_EDITOR";
      private const           string       END_IF          = "#endif";
      private const           string       USING           = "using";
      private const           string       PARTIAL_CLASS   = "public partial class";
      private static readonly UTF8Encoding Script_Encoding = new(true);


      public static bool Create(Type type, GeneratorSettings settings){
         return CreateScriptFile(type, settings);
      }

      private static bool CreateScriptFile(Type type, GeneratorSettings settings){
         var directory  = settings.GetFileDirectoryPath(type);
         var name       = settings.GetName(type);
         var fileByPath = GeneratorSettings.GetFilePath(directory, name);


         if (File.Exists(fileByPath)){
            if (!settings.rewriteExisting) return false;

            File.Delete(fileByPath);
         }

         if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);


         using var fileStream    = File.Create(fileByPath);
         var           script        = GenerateScript(type, name);
         var           encodedScript = Script_Encoding.GetBytes(script);
         fileStream.Write(encodedScript, 0, encodedScript.Length);

         return true;
      }

      private static string GenerateScript(Type type, string name){
         var systemNamespace = typeof(EcsWorldDebugSystem).Namespace;
         var ecvClassName    = typeof(Ecv<>).GetCleanName();

         return
            $"{START_IF} \n" +
            $"{USING} {systemNamespace}; \n" +
            $"{PARTIAL_CLASS} {name} : {ecvClassName}<{type}>{{ }} \n" +
            $"{END_IF}";
      }
   }
}
