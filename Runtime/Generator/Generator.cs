#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;

namespace Mitfart.LeoECSLite.UnityIntegration.Generator{
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
         string directory  = settings.GetFileDirectoryPath(type);
         string name       = settings.GetName(type);
         string fileByPath = GeneratorSettings.GetFilePath(directory, name);


         if (File.Exists(fileByPath)){
            if (!settings.rewriteExisting) return false;

            File.Delete(fileByPath);
         }

         if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);


         using FileStream fileStream    = File.Create(fileByPath);
         string           script        = GenerateScript(type, name);
         byte[]           encodedScript = Script_Encoding.GetBytes(script);
         fileStream.Write(encodedScript, 0, encodedScript.Length);

         return true;
      }

      private static string GenerateScript(Type type, string name){
         string systemNamespace = typeof(Ecv<>).Namespace;
         string ecvClassName    = typeof(Ecv<>).GetCleanName();

         return
            $"{START_IF} \n" +
            $"{USING} {systemNamespace}; \n" +
            $"{PARTIAL_CLASS} {name} : {ecvClassName}<{type}>{{ }} \n" +
            $"{END_IF}";
      }
   }
}
#endif
