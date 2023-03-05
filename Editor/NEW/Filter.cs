using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Search;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Style;
using Mitfart.LeoECSLite.UnityIntegration.Extentions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor.NEW {
   public class Filter : VisualElement {
      private readonly NewEDW                               _debugWindow;
      private readonly Dictionary<FilterTag, FilterTagView> _tagViews;
      private readonly List<FilterTag>                      _tags;

      private Button _addTagBtn;
      private Button _clearBtn;
      
      
      
      public Filter(NewEDW debugWindow) {
         _tags        = new List<FilterTag>();
         _tagViews    = new Dictionary<FilterTag, FilterTagView>();
         _debugWindow = debugWindow;
         
         CreateElements();
         AddElements();
         InitElements();
      }
      

      private void CreateElements() {
         _addTagBtn = new Button(OpenComponentsMenu).AddIcon(Icons.Plus_More);
         _clearBtn  = new Button(Reset).AddIcon(Icons.Close);
      }

      private void AddElements() {
         this.AddChild(_addTagBtn)
             .AddChild(_clearBtn);
      }

      private void InitElements() {
         style.SetMargin(Utils.METRICS_0250);
         style.SetBorderColor(Utils.Color_DDD);
         style.SetBorderWidth(Utils.METRICS_0125);
         style.SetBorderRadius(Utils.METRICS_0250);
         style.backgroundColor = Utils.Color_DD;
         style.flexDirection   = FlexDirection.Row;
         style.alignItems      = Align.Center;
         
         SetButtonStyle(_addTagBtn);
         SetButtonStyle(_clearBtn);

         _clearBtn.style.position = Position.Absolute;
         _clearBtn.style.top      = 0;
         _clearBtn.style.right    = 0;
         
         HideButton(_clearBtn);
      }



      public EcsFilter GetEcsFilter() {
         EcsWorld.Mask mask = _debugWindow.ActiveDebugSystem.World.Filter(_tags[0].Type);

         for (var i = 1; i < _tags.Count; i++) {
            FilterTag tag = _tags[i];
            switch (tag.FilterMethod) {
               case FilterMethod.Include: mask.Inc(tag.Type); break;
               case FilterMethod.Exclude: mask.Exc(tag.Type); break;
               default:                   throw new ArgumentOutOfRangeException();
            }
         }

         return mask.End();
      }
      
      

      public void AddTag(FilterTag tag) {
         if (_tags.Contains(tag)) 
            Debug.Log($"Tag {tag} is already added!");
         
         FilterTagView view = CreateTagView(tag);

         _tags.Add(tag);
         _tagViews.Add(tag, view);

         if (_tags.Count >= 1) 
            ShowButton(_clearBtn);
      }

      public void RemoveTag(FilterTag tag) {
         if (!_tags.Contains(tag)) 
            throw new Exception($"Can't remove {tag}!");
         
         Remove(_tagViews[tag]);
         
         _tags.Remove(tag);
         _tagViews.Remove(tag);

         if (_tags.Count <= 0) 
            HideButton(_clearBtn);
      }
      
      public void Reset() {
         while (_tags.Count > 0)
            RemoveTag(_tags[^1]);
      }

      
      private FilterTagView CreateTagView(FilterTag tag) {
         var newView = new FilterTagView(tag);
         int index   = _tags.Count - 1 >= 0 ? _tags.Count - 1 : 0;
         Insert(index, newView);
         return newView;
      }
      
      
      
      private static void SetButtonStyle(VisualElement button) {
         button.style.Square(Utils.METRICS_1250);
         button.style.SetPadding(0);
         button.style.SetMargin(Utils.METRICS_0250);
         button.style.backgroundColor = Utils.Color_DD;
      }

      private static void HideButton(VisualElement button) { button.style.display = DisplayStyle.None; }
      private static void ShowButton(VisualElement button) { button.style.display = DisplayStyle.Flex; }
      
      
      
      private void OpenComponentsMenu() {
         if (_debugWindow == null) return;
         
         ComponentsSearchWindow
           .CreateAndInit(
               _debugWindow.ActiveDebugSystem,
               componentType => {
                  var tag = new FilterTag(
                     componentType, 
                     FilterMethod.Include);
                  
                  if (_tags.Contains(tag)) return false;

                  AddTag(tag);
                  return true;
               });
      }
   }
   

   public class FilterTagView : Button {
      public FilterTag Tag { get; }

      private Label         _filterMethod;
      private Label         _filterLabel;
      private VisualElement _separator;
      

      public FilterTagView(FilterTag tag) {
         Tag = tag;

         CreateElements();
         AddElements();
         InitElements();
      }

      
      private void CreateElements() {
         _filterMethod = new Label();
         _filterLabel  = new Label();
         _separator    = new VisualElement();
      }

      private void AddElements() {
         this
           .AddChild(_filterMethod)
           .AddChild(_separator)
           .AddChild(_filterLabel);
      }

      private void InitElements() {
         const string include = "In";
         const string exclude = "Ex";
         
         _filterLabel.style.SetMargin(Utils.METRICS_0500, Utils.METRICS_0125);
         _filterLabel.text = Tag.Type.Name;
         
         _filterMethod.text = Tag.FilterMethod switch {
            FilterMethod.Include => include,
            FilterMethod.Exclude => exclude,
            _                    => throw new ArgumentOutOfRangeException()
         };
         _filterMethod.style.SetMargin(Utils.METRICS_0250, Utils.METRICS_0125);
         _filterMethod.style.color    = _filterLabel.style.color    = Utils.Color_DDD;
         _filterMethod.style.fontSize = _filterLabel.style.fontSize = Utils.METRICS_0750;

         style.SetPadding(0);
         style.SetMargin(Utils.METRICS_0125);
         style.SetBorderRadius(Utils.METRICS_0500);
         
         Color color = Utils.GetColorByString(Tag.Type.Name);
         style.flexDirection   = FlexDirection.Row;
         style.alignItems      = Align.Center;
         style.justifyContent  = Justify.FlexStart;
         style.flexWrap        = Wrap.Wrap;
         style.backgroundColor = color;
         style.flexShrink      = 0;

         _separator.style.height          = Utils.GetPercentsLength(100);
         _separator.style.width           = Utils.METRICS_MIN;
         _separator.style.backgroundColor = Utils.Color_DDD;
      }
   }
    
   
   public readonly struct FilterTag {
      public Type         Type         { get; }
      public FilterMethod FilterMethod { get; }

      
      public FilterTag(Type type, FilterMethod filterMethod) {
         Type         = type;
         FilterMethod = filterMethod;
      }


      public override string ToString() {
         return $"{FilterMethod} | {Type.Name}";
      }
   }
   
   
   public enum FilterMethod {
      Include, 
      Exclude
   }
}
