using System;
using System.Collections.Generic;
using System.Text;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed partial class MonoEntityView{
      public sealed class NameBuilder {
         private const string ComponentsSeparator = ", ";
         private const string StartDescription    = ": ";
         private const string Bracket_Open        = "[ ";
         private const string Bracket_Close       = " ]";
         
         private readonly MonoEntityView _monoEntityView;
         private readonly StringBuilder  _nameBuilder;
         private readonly Settings       _settings;

         private readonly StringBuilder _bakedCompsBuilder;
         private readonly string        _bakedIndex;
         
         
         
         public NameBuilder(MonoEntityView monoEntityView, Settings settings){
            _monoEntityView = monoEntityView;
            _nameBuilder    = new StringBuilder();
            _settings       = settings;

            _bakedIndex = _monoEntityView.Entity.ToString(_settings.Format);
            
            if (_settings.BakeComponents) 
               _bakedCompsBuilder = new StringBuilder();
         }
         
         
         
         public void Update(){
            Clear();
            AddTag();
            if (_settings.BakeIndexAtRuntime) AddEntityIndex();
            if (_settings.BakeComponents) AddBakedComponents();
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
                 .Append(Bracket_Open)
                 .Append(_monoEntityView.Tag)
                 .Append(Bracket_Close)
                 .Append(StartDescription);
         }
         private void AddBakedComponents(){
            _nameBuilder
              .Append(_bakedCompsBuilder);
         }
         private void AddEntityIndex(){
            _nameBuilder
              .Append(_bakedIndex)
              .Append(StartDescription);
         }
         private void Set(){
            _monoEntityView.name = _nameBuilder.ToString();
         }



         public void BakeComponents(IEnumerable<Type> bakedComps){
            _bakedCompsBuilder.Clear();

            foreach (var bakedComp in bakedComps)
               _bakedCompsBuilder
                 .Append(bakedComp.Name)
                 .Append(ComponentsSeparator);
         }
         
         public void ChangeTag(string newTag){
            if (string.Equals(newTag, _monoEntityView.Tag)) return;

            if (string.IsNullOrWhiteSpace(newTag) && string.IsNullOrWhiteSpace(_monoEntityView.Tag)) 
               _nameBuilder.Replace(_monoEntityView.Tag, newTag);
            _monoEntityView.Tag = newTag;
            Set();
         }

         

         public struct Settings{
            public bool   BakeComponents;
            public bool   BakeIndexAtRuntime;
            public string Format;

            public Settings(string format = "X8", bool bakeComponents = false, bool bakeIndexAtRuntime = true){
               Format             = format;
               BakeComponents     = bakeComponents;
               BakeIndexAtRuntime = bakeIndexAtRuntime;
            }
         }
      }
   }
}
