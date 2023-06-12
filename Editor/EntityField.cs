using System;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration.Editor.Extensions.Style;
using Mitfart.LeoECSLite.UnityIntegration.View;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Mitfart.LeoECSLite.UnityIntegration.Editor {
  public class EntityField : VisualElement {
    private readonly Action<EntityView> _onSet;



    public EntityField(
      EcsPackedEntityWithWorld packedEntity,
      Action<EntityView>       onSet,
      string                   title = null
    ) {
      _onSet = onSet;
      style.FlexRow();

      Add(
        EntityViewField(
          title,
          AliveEntity(packedEntity, out int e, out EcsWorldDebugSystem system)
            ? system.View.GetEntityView(e)
            : null
        )
      );
    }

    public EntityField(
      EcsPackedEntity    packedEntity,
      EcsWorld           world,
      Action<EntityView> onSet,
      string             title = null
    ) {
      _onSet = onSet;
      style.FlexRow();

      Add(
        EntityViewField(
          title,
          AliveEntity(packedEntity, world, out int e, out EcsWorldDebugSystem system)
            ? system.View.GetEntityView(e)
            : null
        )
      );
    }



    private ObjectField EntityViewField(string title, EntityView entityView) {
      var field = new ObjectField(title) {
        objectType = typeof(EntityView), //
        value      = entityView,
        style      = { flexShrink = 1, flexGrow = 1 }
      };
      RegisterSetCallback(field);
      return field;
    }

    private void RegisterSetCallback(ObjectField field) {
      field.RegisterValueChangedCallback(
        change => {
          if (change.newValue is EntityView entityView)
            _onSet?.Invoke(entityView);
        }
      );
    }



    private static bool AliveEntity(
      EcsPackedEntityWithWorld packedEntity,
      out int                  e,
      out EcsWorldDebugSystem  system
    ) {
      e      = -1;
      system = null;
      return !packedEntity.EqualsTo(default)
          && packedEntity.Unpack(out EcsWorld world, out e)
          && ActiveDebugSystems.TryGet(world, out system);
    }

    private static bool AliveEntity(
      EcsPackedEntity         packedEntity,
      EcsWorld                world,
      out int                 e,
      out EcsWorldDebugSystem system
    ) {
      e      = -1;
      system = null;
      return !packedEntity.EqualsTo(default)
          && packedEntity.Unpack(world, out e)
          && ActiveDebugSystems.TryGet(world, out system);
    }
  }
}