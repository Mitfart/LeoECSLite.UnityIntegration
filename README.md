# Интеграция в редактор Unity для LeoECS Lite
Интеграция в редактор Unity с мониторингом состояния мира
(не подразусевает [конвертацию / создание через инспектор](https://github.com/Mitfart/LeoECSLite.UniLeo), лишь просмотр!)

> Проверено на Unity ***2021.3 LTS*** (зависит от Unity) 


# Содержание
* [Установка](#Установка)
  * [В виде unity модуля](#В-виде-unity-модуля)
  * [В виде исходников](#В-виде-исходников)
* [Интеграция](#Интеграция)
  * [В коде](#В-коде)
  * [В редакторе](#В-редакторе)
* [Почему?](#Почему?)
* [Обратная связь](#Обратная-связь)
* [Good practices](#Good-practices)
* [ЧаВо](#ЧаВо)



# Установка

> **ВАЖНО!** Зависит от: \
> [LeoECS Lite](https://github.com/Leopotam/ecslite) \
> [GenericUnityObjects](https://github.com/SolidAlloy/GenericUnityObjects) (в будещем планируется убрать зависимость)
- `фреймворки должены быть установлены до этого расширения.`

### Через Package manager:
- Откройте Package manager
- Нажмите плюс в левом верхнем углу
- "Add from git url" и вставте:

```
https://github.com/Mitfart/LeoECSLite.UnityIntegration
```

или добавьте *Packages/manifest.json*:
```
"com.mitfart.leoecslite.unity-integration": "https://github.com/Mitfart/LeoECSLite.UnityIntegration.git",
```

### В виде исходников
Код так же может быть склонирован или получен в виде архива со страницы релизов



<br/>



# Интеграция

## В коде
### Подключение системы
```c#
// ecs-startup code:
IEcsSystems _systems;

void Start() {        
    _systems = new EcsSystems (new EcsWorld());
    _systems
        .Add (new TestSystem1())
#if UNITY_EDITOR
        // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
        // .Add (new LeoECSLite.UnityIntegration.EcsWorldDebugSystem("events"))
        .Add(new LeoECSLite.UnityIntegration.EcsWorldDebugSystem())
        // Для коректного отображения данных, системы необходимо подключать в конце
#endif
        .Init();
}

void Update() {
    // Отладочные системы являюся обычными ECS-системами и для их корректной работы
    // требуется выполнять их запуск через EcsSystems.Run()
    _systems?.Run();
}
```
> **ВАЖНО!** По умолчанию названия компонентов **НЕ** записываются в имя
> (как в системе от `Leopotam`)
> Если такое поведение нужно, то его
> можно добавить создав пресет настроек для системы
> ```cs
> var nameSettings = new EntityNameSettings(bakeComponents: true);
> ...
> .Add(new LeoECSLite.UnityIntegration.EcsWorldDebugSystem(null, nameSettings))
> ...
> ```

### Объявление компенента

```c#
[Serializable] // --- Для отображения данных компонент долже быть сериализован
public struct Comp {
    public string value;
}
```



## В редакторе

Для просмотра / Debug-а, желательно, использовать специальное окно.
Открыть которое можно **в верхнем меню**:
> **LeoEcs Lite > Debug Window**

` СКОРО ПОЯВЯТСЯ ИЗОБРАЖЕНИЯ! `



# Преимущества и недостатки

###### ``времено недоступно`` - обновляю эдитор, переписывая с нуля, поэтому не всё пока перенес на новую версию

### Плюсы
+ Полностью автоматическое отображение компонентов в инспекторе
  + `По умолчанию - как сериализуемые структуры в Unity`
  + `Поддержка кастомных Editor / PropertyDrawer-скриптов`
+ Возможность добавления компонентов через инспектор
+ Возможность Удаления `Entity` и/или её `Компонентов` ``времено недоступно``
+ Возможность расширения логики `View-компонентов ` (`ECV<>`) ``времено недоступно``
+ Задание **Тега** для `Entity` (при помощи компонента `DebugTag`) ``времено недоступно``
+ Возможность фильтрации сущностей по: `только в спец окне`
  + `компонентам / пулам`

### Минусы `пока не выявлены`

### Планируется
+ Закончить ``времено недоступные`` плюсы
+ `Возможность фильтрации сущностей по DebugTag`



# Обратная связь
```
@Mitfart
```
> #### Discord [Группа по LeoEcsLite](https://discord.gg/5GZVde6)
> #### Telegram [Группа по Ecs](https://t.me/ecschat)



# Good practices
+ Использование [специального окна](#В редакторе)
+ Сворачивание списка Entity в инспекторе
+ Использлвание **ЛИБО** специального окна **ЛИБО** инспектора \
  `(При использование обоих тратится вдвое больше ресурсов)`



# ЧаВо

### Я хочу создавать сущности в `IEcsPreInitSystem`, ноотладочные системы бросают исключения в этом случае. Как это исправить??

**Причина** -`EcsWorldDebugSystem` тоже является `IEcsPreInitSystem` и происходит конфликт из-за порядка систем. \
**Решение_1** - вместо `IEcsPreInitSystem` использовать `IEcsInitSystem` \
**Решение_2** - все отладочные системы следует вынести в отдельный `IEcsSystems` и 
вызвать его инициализацию раньше основного кода:
```c#
IEcsSystems _systems;
#if UNITY_EDITOR
IEcsSystems _editorSystems;
#endif


void Awake() {
    _systems = new EcsSystems (new EcsWorld());
    
#if UNITY_EDITOR
    // Создаем отдельную группу для отладочных систем.
    _editorSystems = new EcsSystems (_systems.GetWorld());
    _editorSystems
      .Add(new LeoECSLite.UnityIntegration.EcsWorldDebugSystem(null, nameSettings))
      .Init ();
#endif

  _systems
    .Add (new Sys())
    .Init();
  }
}


void Update() {
    _systems?.Run();
    
#if UNITY_EDITOR
    // Выполняем обновление состояния отладочных систем. 
    _editorSystems?.Run ();
#endif
}
    
    
void OnDestroy () {
#if UNITY_EDITOR
  // Выполняем очистку отладочных систем.
  if (_editorSystems != null) {
      _editorSystems.Destroy ();
      _editorSystems = null;
  }
#endif
  if (_systems != null) {
      _systems.Destroy();
      _systems.GetWorld().Destroy();
      _systems = null;
  }
}
```

### Я хочу добавить свою логику для компонента в окне инспектора. Как я могу это сделать?

Написав свой `Editor / PropertyDrawer` для компонента:

- В стандартном эдиторе: ```@/Editor/Component/ComponentEditor.cs``` \
используется [UIToolkit](https://docs.unity3d.com/Manual/UIE-HowTo-CreateCustomInspector)
- использование стандартного GUILayout, возможно, но не рекомендуется \

```c#
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Mitfart.LeoECSLite.UnityIntegration;

[CustomEditor(typeof(ComponentData<Comp>))] 
public class ComponentDataEditor : Editor {
  public override VisualElement CreateInspectorGUI() {
    // Создание главного/родительского элемента
    var container = new VisualElement();
    // ...
    return container;
  }
}
#endif
```