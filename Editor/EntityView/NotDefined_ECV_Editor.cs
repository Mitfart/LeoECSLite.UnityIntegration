using System;
using System.Reflection;
using Mitfart.LeoECSLite.UnityIntegration.ComponentView;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Window;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.EntityView{
   [CustomEditor(typeof(NotDefinedEcv))]
   public class NotDefinedEcvEditor : UnityEditor.Editor{
      private BaseEcv     _typedTarget;
      private FieldInfo[] _fields;
      private Label[]     _fieldsValues;
      private object      _prevComponent;



      private void OnEnable(){
         EditorApplication.update -= UpdateValue;
         EditorApplication.update += UpdateValue;
      }
      private void OnDisable(){
         EditorApplication.update -= UpdateValue;
      }



      public override VisualElement CreateInspectorGUI(){
         _typedTarget = (BaseEcv) target;
         
         var root = new VisualElement();
         
         var componentType = _typedTarget.GetComponentType();
         if (componentType == null) return root;
         
         AddComponentLabel(root, componentType);
         
         _fields = componentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
         if (_fields.Length <= 0) return root;

         AddFields(root);

         return root;
      }
      private void UpdateValue(){
         if (!Application.isPlaying || _typedTarget == null){
            EditorApplication.update -= UpdateValue;
            return;
         }
         
         _typedTarget.UpdateValue();
         if (_typedTarget == null || _fields.Length <= 0 || !_typedTarget.MonoEntityView.IsActive) return;
         
         
         var component = _typedTarget.EcsPool.GetRaw(_typedTarget.Entity);
         if (component == _prevComponent) return;


         for (var i = 0; i < _fields.Length; i++) 
            _fieldsValues[i].text = _fields[i].GetValue(component).ToString();
         _prevComponent = component;
      }



      private void AddFields(VisualElement root){
         _fieldsValues = new Label[_fields.Length];

         var fieldsContainer = root.AddAndGet(CreateBox());
         var component       = _typedTarget.EcsPool.GetRaw(_typedTarget.Entity);

         for (var i = 0; i < _fields.Length; i++){
            var field = _fields[i];

            var fieldContainer = new VisualElement{ style ={ flexDirection = FlexDirection.Row } };
            var fieldLabel     = new Label(field.Name);
            var fieldValue     = new Label(field.GetValue(component).ToString());
            _fieldsValues[i] = fieldValue;

            fieldLabel.style.width = fieldValue.style.width = Styles.GetPercentsLength(50);

            fieldsContainer
              .AddChild(
                  fieldContainer
                    .AddChild(fieldLabel)
                    .AddChild(fieldValue));
         }
      }
      

      private static void AddComponentLabel(VisualElement root, Type componentType){
         var componentTypeName = componentType == null ? "Null" : componentType.Name;

         root
           .AddAndGet(CreateBox())
           .AddAndGet(CreateMainLabel(componentTypeName));
      }
      

      private static Box CreateBox(){
         var box = new Box();

         box.style.SetPadding(5);
         box.style.SetMargin(5);
         box.style.SetBorderRadius(5);
         box.style.whiteSpace = WhiteSpace.Normal;

         return box;
      }
      private static Label CreateMainLabel(string label){
         var labelElement = new Label(label);
         labelElement.style.unityFontStyleAndWeight = FontStyle.Bold;
         return labelElement;
      }
   }
}
