using System;
using System.Collections.Generic;
using System.Text;

namespace Mitfart.LeoECSLite.UnityIntegration.EntityView{
   public sealed partial class MonoEntityView{
      public sealed class NameBuilder {
         private const string COMPONENTS_SEPARATOR = ", ";
         private const string START_DESCRIPTION    = ": ";
         private const string BRACKET_OPEN        = "[ ";
         private const string BRACKET_CLOSE       = " ]";
         
         private readonly MonoEntityView _monoEntityView;
         private readonly StringBuilder  _nameBuilder;
         private readonly Settings       _settings;

         private readonly StringBuilder _bakedCompsBuilder;
         private readonly string        _bakedIndex;
         
         
         
         public NameBuilder(MonoEntityView monoEntityView, Settings settings){
            _monoEntityView = monoEntityView;
            _nameBuilder    = new StringBuilder();
            _settings       = settings;

            _bakedIndex = _monoEntityView.Entity.ToString(_settings.format);
            
            if (_settings.bakeComponents) 
               _bakedCompsBuilder = new StringBuilder();
         }
         
         
         
         public void Update(){
            Clear();
            AddTag();
            if (_settings.bakeIndexAtRuntime) AddEntityIndex();
            if (_settings.bakeComponents) AddBakedComponents();
            Set();
         }
         
         public void Reset(){
            Clear();
            AddEntityIndex();
            Set();
         }
         
         
         private void Clear(){
            _nameBuilder.Clear();
         }
         private void AddTag(){
            if (!string.IsNullOrWhiteSpace(_monoEntityView.Tag))
               _nameBuilder
                 .Append(BRACKET_OPEN)
                 .Append(_monoEntityView.Tag)
                 .Append(BRACKET_CLOSE)
                 .Append(START_DESCRIPTION);
         }
         private void AddBakedComponents(){
            _nameBuilder
              .Append(_bakedCompsBuilder);
         }
         private void AddEntityIndex(){
            _nameBuilder
              .Append(_bakedIndex)
              .Append(START_DESCRIPTION);
         }
         private void Set(){
            _monoEntityView.name = _nameBuilder.ToString();
         }



         public void BakeComponents(IEnumerable<Type> bakedComps){
            _bakedCompsBuilder.Clear();

            foreach (var bakedComp in bakedComps)
               _bakedCompsBuilder
                 .Append(bakedComp.Name)
                 .Append(COMPONENTS_SEPARATOR);
         }
         
         public void ChangeTag(string newTag){
            if (string.Equals(newTag, _monoEntityView.Tag)) return;

            if (string.IsNullOrWhiteSpace(newTag) && string.IsNullOrWhiteSpace(_monoEntityView.Tag)) 
               _nameBuilder.Replace(_monoEntityView.Tag, newTag);
            _monoEntityView.Tag = newTag;
            Set();
         }

         

         public struct Settings{
            public bool   bakeComponents;
            public bool   bakeIndexAtRuntime;
            public string format;

            public Settings(string format = "X8", bool bakeComponents = false, bool bakeIndexAtRuntime = true){
               this.format             = format;
               this.bakeComponents     = bakeComponents;
               this.bakeIndexAtRuntime = bakeIndexAtRuntime;
            }
         }
      }
   }
}
