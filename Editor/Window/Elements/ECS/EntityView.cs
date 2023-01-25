using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration{
   public sealed class EntityView : VisualElement{
      public override  VisualElement      contentContainer{ get; }
      
      private readonly FoldoutWithButtons _contentFoldout;
      
      public string Label     { get => _contentFoldout.text;  set => _contentFoldout.text = value; }
      public bool   IsExpanded{ get => _contentFoldout.value; set => _contentFoldout.value = value; }

      public MonoEntityView      MonoView    { get; private set; }
      public int                 Entity      => MonoView.Entity;
      public EcsWorldDebugSystem DebugSystem => MonoView.DebugSystem;

      private readonly Dictionary<Type, ComponentView> _uiComponentViews = new();



      public EntityView(){
         contentContainer = this;
         contentContainer
            = _contentFoldout
                 = this.AddAndGet(new ContentBox())
                       .AddAndGet(new FoldoutWithButtons());

         _contentFoldout.AddButton(Icons.Plus  , AddComponentToEntity);
         _contentFoldout.AddButton(Icons.Reload, UpdateEntity);
         _contentFoldout.AddButton(Icons.Close,  DeleteEntity);


         #region ClickEvents

         void UpdateEntity(){
            if (MonoView == null) return;
            MonoView.UpdateView();
         }

         void DeleteEntity(){
            if (MonoView == null) return;
            MonoView.Delete();
         }

         void AddComponentToEntity(){
            if (MonoView == null) return;
            var searchWindow = ComponentsSearchWindow.CreateAndInit(MonoView.DebugSystem);
            
            searchWindow.OnSelect = componentType => {
               var pool = MonoView.World.GetPool(componentType);
               if (pool.Has(MonoView.Entity)){
                  Debug.Log($"Can't add another instance of <{componentType}>!");
                  return false;
               }

               pool.AddRaw(MonoView.Entity, Activator.CreateInstance(componentType));
               MonoView.GetOrAdd(componentType);
               return true;
            };
         }

         #endregion
      }


      public EntityView Init(MonoEntityView monoEntityView){
         MonoView = monoEntityView;
         
         _uiComponentViews.Clear();
         AddAllComponents();

         MonoView.OnAddComponent    += AddComponentView;
         MonoView.OnRemoveComponent += DelComponentView;
         
         return this;
      }

      public void Dispose(){
         MonoView.OnAddComponent    -= AddComponentView;
         MonoView.OnRemoveComponent -= DelComponentView;
         
         parent.Remove(this);
      }
      
      
      private void AddComponentView(BaseEcv monoCompView){
         var compType = monoCompView.GetComponentType();
         var comp     = _contentFoldout.AddAndGet(new ComponentView().Init(monoCompView));
         _uiComponentViews.Add(compType, comp);
      }
      private void DelComponentView(BaseEcv monoCompView){
         var compType = monoCompView.GetComponentType();
         if (!_uiComponentViews.TryGetValue(compType, out var uiComp)) return;
         
         Remove(uiComp);
         _uiComponentViews.Remove(compType);
      }



      private void AddAllComponents(){
         foreach (var (compType, monoCompView) in MonoView.Components)
            if (monoCompView != null && !_uiComponentViews.ContainsKey(compType))
               AddComponentView(monoCompView);
      }
      public new class UxmlFactory : UxmlFactory<EntityView, UxmlTraits>{}
   }
}
