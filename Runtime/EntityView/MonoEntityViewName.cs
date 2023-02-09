using System;
using System.Collections.Generic;
using System.Text;

namespace Mitfart.LeoECSLite.UnityIntegration.EntityView{
  public sealed class EntityNameBuilder {
     private const string COMPONENTS_SEPARATOR = ", ";
     private const string START_DESCRIPTION    = ": ";
     private const string TAG_BRACKET_OPEN     = "[ ";
     private const string TA_BRACKET_CLOSE     = " ]";
         
     private readonly MonoEntityView     _monoEntityView;
     private readonly StringBuilder      _nameBuilder;
     private readonly EntityNameSettings _settings;

     private readonly StringBuilder _bakedCompsBuilder;
     private readonly string        _bakedIndex;
     
         
         
         
     public EntityNameBuilder(MonoEntityView monoEntityView, EntityNameSettings settings){
        _monoEntityView = monoEntityView;
        _nameBuilder    = new StringBuilder();
        _settings       = settings;

        _bakedIndex = _monoEntityView.Entity.ToString(_settings.BakeEntityFormat);
            
        if (_settings.BakeComponents) 
           _bakedCompsBuilder = new StringBuilder();
     }
         
         
         
     public void Update() {
        Clear();
        AddTag();
        if (_settings.BakeEntity) AddEntityIndex();
        if (_settings.BakeComponents) AddBakedComponents();
        Set();
     }
         
     public void Reset() {
        Clear();
        AddEntityIndex();
        Set();
     }

     

     public void SetTag(string newTag) {
        _nameBuilder.Replace(_monoEntityView.Tag, newTag);
        Set();
     }
     
     public void BakeComponents(IEnumerable<Type> bakedComps) {
        if (!_settings.BakeComponents) return;
        _bakedCompsBuilder.Clear();

        foreach (var bakedComp in bakedComps)
           _bakedCompsBuilder
             .Append(bakedComp.Name)
             .Append(COMPONENTS_SEPARATOR);
     }
     
         
     
     private void Clear() {
        _nameBuilder.Clear();
     }
     
     private void AddTag() {
        if (!string.IsNullOrWhiteSpace(_monoEntityView.Tag))
           _nameBuilder
             .Append(TAG_BRACKET_OPEN)
             .Append(_monoEntityView.Tag)
             .Append(TA_BRACKET_CLOSE)
             .Append(START_DESCRIPTION);
     }
     
     private void AddBakedComponents() {
        _nameBuilder
          .Append(START_DESCRIPTION)
          .Append(_bakedCompsBuilder);
     }
     
     private void AddEntityIndex() {
        _nameBuilder
          .Append(_bakedIndex);
     }
     
     private void Set() {
        _monoEntityView.name = _nameBuilder.ToString();
     }
  }
}
