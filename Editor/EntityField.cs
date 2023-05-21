using System;
using Git.Extensions.Ecs;
using Git.Extensions.Editor.Style.Flex;
using Leopotam.EcsLite;
using UnityEngine.UIElements;
using LeoECSLite.UnityIntegration.View;
using UnityEditor.UIElements;

namespace LeoECSLite.UnityIntegration.Editor {
  public class EntityField : VisualElement {
    private const string UNAVAILABLE_ENTITY = "Unavailable entity";

    public readonly Action<EntityView> OnSet;



    public EntityField(EcsPackedEntityWithWorld packedEntity, Action<EntityView> onSet, string title = null) {
      OnSet = onSet;
      packedEntity.Unpack(out EcsWorld world, out int e);
      DrawEditor(e, world, title);
    }

    public EntityField(EcsPackedEntity packedEntity, EcsWorld world, Action<EntityView> onSet, string title = null) {
      OnSet = onSet;
      packedEntity.Unpack(world, out int e);
      DrawEditor(e, world, title);
    }

    public EntityField(int e, EcsWorld world, Action<EntityView> onSet, string title = null) {
      OnSet = onSet;
      DrawEditor(e, world, title);
    }



    private void DrawEditor(int e, EcsWorld world, string title) {
      Add(
        ActiveDebugSystems.TryGet(world, out EcsWorldDebugSystem system)
          ? EntityViewField(e, world, title, system)
          : new Label(UNAVAILABLE_ENTITY)
      );

      style.FlexRow();
    }


    private ObjectField EntityViewField(
      int                 e,
      EcsWorld            ecsWorld,
      string              title,
      EcsWorldDebugSystem system
    ) {
      var field = new ObjectField(title) {
        objectType = typeof(EntityView), //
        value      = EntityView(e, ecsWorld, system),
        style      = { flexShrink = 1 }
      };

      field.RegisterValueChangedCallback(changeEvt => {
        if (changeEvt.newValue is EntityView entityView)
          OnSet?.Invoke(entityView);
      });
      return field;
    }


    private static EntityView EntityView(int e, EcsWorld world, EcsWorldDebugSystem system) {
      return world.IsEntityAlive(e)
        ? system.View.GetEntityView(e)
        : null;
    }
  }
}