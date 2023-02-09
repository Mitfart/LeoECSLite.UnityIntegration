using System;
using System.IO;
using System.Text;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;

namespace Mitfart.LeoECSLite.UnityIntegration.Generator{
   public static class EcvGenerator{
      private const           string       START_IF        = "#if UNITY_EDITOR";
      private const           string       END_IF          = "#endif";
      private const           string       USING           = "using";
      private const           string       PARTIAL_CLASS   = "public partial class";
      private static readonly UTF8Encoding Script_Encoding = new(true);


      
      public static bool Create(Type type, EcvGeneratorSettings settings){
         return CreateScriptFile(type, settings);
      }

      private static bool CreateScriptFile(Type type, EcvGeneratorSettings settings){
         var directory  = settings.GetFileDirectoryPath(type);
         var name       = settings.GetName(type);
         var fileByPath = EcvGeneratorSettings.GetFilePath(directory, name);


         if (File.Exists(fileByPath)){
            if (!settings.rewriteExisting) return false;

            File.Delete(fileByPath);
         }

         if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);


         using var fileStream    = File.Create(fileByPath);
         var       script        = GenerateScript(type, name);
         var       encodedScript = Script_Encoding.GetBytes(script);
         fileStream.Write(encodedScript, 0, encodedScript.Length);

         return true;
      }

      private static string GenerateScript(Type type, string name){
         var ecvNamespace = typeof(Ecv<>).Namespace;
         var ecvClassName = typeof(Ecv<>).GetCleanName();

         return
            $"{START_IF} \n" +
            $"{USING} {ecvNamespace}; \n" +
            $"{PARTIAL_CLASS} {name} : {ecvClassName}<{type}>{{ }} \n" +
            $"{END_IF}";
      }
   }
}
