using System;
using System.Collections.Generic;
using System.Reflection;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.Ecv{
   [CustomEditor(typeof(NotDefinedEcv))]
   public class NotDefinedEcvEditor : BaseEcvEditor {
      private BaseEcv     _typedTarget;
      private FieldInfo[] _fields;
      private Label[]     _fieldsValuesLabels;
      private object      _prevComponent;


      
      protected override void CreateEditor(VisualElement root) {
         _typedTarget = (NotDefinedEcv) target;
         
         var componentType = _typedTarget.GetComponentType();
         if (componentType == null) return;
         
         AddComponentLabel(root, componentType);

         _fields             = GetFields(componentType);
         _fieldsValuesLabels = AddFields(root, _fields, _typedTarget);
      }
      
      
      protected override void OnEditorUpdate() {
         _typedTarget.UpdateValue();
         if (_typedTarget == null || _fields.Length <= 0 || !_typedTarget.MonoEntityView.IsActive) return;
         
         
         var component = _typedTarget.EcsPool.GetRaw(_typedTarget.Entity);
         if (component == _prevComponent) return;


         for (var i = 0; i < _fields.Length; i++) 
            _fieldsValuesLabels[i].text = _fields[i].GetValue(component).ToString();
         _prevComponent = component;
      }

      
      
      private static void AddComponentLabel(VisualElement root, Type componentType){
         root
           .AddAndGet(CreateContainer())
           .AddAndGet(CreateMainLabel(componentType.Name));
      }
      
      
      private static Label[] AddFields(VisualElement root, IReadOnlyList<FieldInfo> fields, BaseEcv componentView) {
         if (fields.Count <= 0) return null;
         
         var fieldsValuesLabels = new Label[fields.Count];
         
         var fieldsContainer = root.AddAndGet(CreateContainer());
         var component       = componentView.EcsPool.GetRaw(componentView.Entity);

         for (var i = 0; i < fields.Count; i++){
            var field = fields[i];

            var fieldContainer = CreateFieldContainer();
            var fieldLabel     = new Label(field.Name);
            var fieldValue     = new Label(field.GetValue(component).ToString());
            fieldsValuesLabels[i] = fieldValue;

            fieldLabel.style.width = fieldValue.style.width = StyleUtils.GetPercentsLength(50);

            fieldsContainer
              .AddChild(
                  fieldContainer
                    .AddChild(fieldLabel)
                    .AddChild(fieldValue));
         }

         return fieldsValuesLabels;
         
         
         VisualElement CreateFieldContainer() {
            return new VisualElement {
               style = {
                  flexDirection = FlexDirection.Row
               }
            };
         }
      }
      
      private static FieldInfo[] GetFields(Type componentType) {
         return componentType.GetFields(
            BindingFlags.Instance 
          | BindingFlags.Public 
          | BindingFlags.NonPublic);
      }
      
      
      private static Box CreateContainer(){
         var box = new Box();

         box.style.SetPadding(5);
         box.style.SetMargin(5);
         box.style.SetBorderRadius(5);
         box.style.whiteSpace = WhiteSpace.Normal;

         return box;
      }
      
      private static Label CreateMainLabel(string label){
         var labelElement = new Label(label) { 
            style = {
               unityFontStyleAndWeight = FontStyle.Bold
            } 
         };
         return labelElement;
      }
   }
}
