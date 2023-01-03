# Интеграция в редактор Unity для LeoECS Lite
Интеграция в редактор Unity с мониторингом состояния мира

> Проверено на Unity 2020.3 (зависит от Unity) и содержит asmdef-описания для компиляции в виде отдельных сборок и уменьшения времени рекомпиляции основного проекта.

# Содержание
* [Социальные ресурсы](#Социальные-ресурсы)
* [Установка](#Установка)
  * [В виде unity модуля](#В-виде-unity-модуля)
  * [В виде исходников](#В-виде-исходников)
* [Интеграция](#Интеграция)
  * [В коде](#В-коде)
  * [В редакторе](#В-редакторе)
* [Лицензия](#Лицензия)
* [Почему?](#Почему?)
* [Оптимизация](#Оптимизация)
* [ЧаВо](#ЧаВо)

# Социальные ресурсы
[![discord](https://img.shields.io/discord/404358247621853185.svg?label=enter%20to%20discord%20server&style=for-the-badge&logo=discord)](https://discord.gg/5GZVde6)



# Установка

> **ВАЖНО!** Зависит от [LeoECS Lite](https://github.com/Leopotam/ecslite) - фреймворк должен быть установлен до этого расширения.

## В виде unity модуля
Поддерживается установка в виде unity-модуля через git-ссылку в PackageManager или прямое редактирование `Packages/manifest.json`:
```
"com.mitfart.leoecslite.unity-integration": "https://github.com/Mitfart/LeoECSLite.UnityIntegration.git",
```

## В виде исходников
Код так же может быть склонирован или получен в виде архива со страницы релизов.



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
        // .Add (new Mitfart.LeoECSLite.UnityIntegration.EcsWorldDebugSystem("events"))
        .Add(new Mitfart.LeoECSLite.UnityIntegration.EcsWorldDebugSystem())
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

### Объявление компенента
> Для отображения компонента в редакторе, необходимо создать ECV<Comp>  `(ECV - ECS Component View)`

```c#
// IEcsSerializedComponent - интерфейс для кодогенерации стандартного ECV<Comp> для компонента 
// Компонент обязательно должен быть с атрибутом: [Serializable]
[Serializable]
public struct Comp : IEcsSerializedComponent {
    public string value;
}
 
// WILL GENERATE:
#if UNITY_EDITOR 
using Mitfart.LeoECSLite.UnityIntegration; 
// using Comp.Namespace
public partial class ECV_Comp : ECV<Comp>{ }
#endif
```


> **ВАЖНО!** По умолчанию названия компонентов **НЕ** записываются в имя `GameObject`
> (как в системе от `Leopotam`)
> Если такое поведение нужно, то его
> можно добавить создав пресет настроек для системы `MonoEntityView.NameBuilder.Settings`
> ```cs
> var nameSettings = new MonoEntityView.NameBuilder.Settings();
> nameSettings.bakeComponents = true;
> ...
> .Add(new Mitfart.LeoECSLite.UnityIntegration.EcsWorldDebugSystem(null, nameSettings))
> ...
> ```



## В редакторе

Для просмотра / `Debug`-а, желательно, использовать специальное окно.
Открыть которое можно **в верхнем меню**: \
`LeoEcs Lite > Debug Window`

> **ВАЖНО!** По умолчанию в проекте включена **кодогенеоация**!
> Для настойки или отключения откройте **в верхнем меню**:
> `LeoEcs Lite > Settings`



# Лицензия
Фреймворк выпускается под двумя лицензиями, [подробности тут](./LICENSE.md).

В случаях лицензирования по условиям MIT-Red не стоит расчитывать на
персональные консультации или какие-либо гарантии.



# Преимущества и недостатки

### Плюсы
+ Автоматическое отображение компонентов в инспекторе
  + `По умолчанию - как сериализуемые структуры в Unity`
  + `Поддержка кастомных Editor / PropertyDrawer-скриптов`
+ Возможность добавления `оформленных` компонентов через инспектор
+ Возможность Удаления `Entity` и/или её `Компонентов`
+ Возможность расширения логики `View-компонентов ` (`EcsComponentView`)
+ Задание **Тега** для `Entity` (при помощи компонента `DebugTag`)
+ Возможность фильтрации сущностей по:
  + `компонентам / пулам`

### Минусы
+ Необходимость **оформления** компонентов с помощью `IEcsViewedComponent` \
  `(для возмодности редактировать их)`

### Планируется
+ Возможность фильтрации сущностей по:
  + `тэгу`



# Оптимизация
+ Использование [специального окна](#В редакторе)
+ Открытие только необходимых Entity и Component-ов
+ Сворачивание списка Entity \
  `(как в специальном окне, так и в инспекторе)`
+ Использлвание **ЛИБО** специального окна **ЛИБО** инспектора \
  `(При использование обоих тратится вдвое больше ресурсов)`



# ЧаВо

### Я хочу создавать сущности в `IEcsPreInitSystem`-системе, но отладочные системы бросают исключения в этом случае. Как это исправить??

Проблема в том, что `EcsWorldDebugSystem` тоже является `IEcsPreInitSystem`-системой и происходит конфликт из-за порядка систем.
Решение - все отладочные системы следует вынести в отдельный `IEcsSystems` и вызвать его инициализацию раньше основного кода:
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
      .Add(new Mitfart.LeoECSLite.UnityIntegration.EcsWorldDebugSystem())
      .Init ();
#endif
  _systems
    .Add (new Sys())
    .Init();
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
###### Это можно сделать через:

- реализацию своего **ECV**:
```c#
public struct Comp {
    public string value;
}
  
// Файл должен лежать где-то в проекте - будет обнаружен и подключен автоматически
#if UNITY_EDITOR
public sealed class ECV_Comp : ECV<Comp>{
    // Реальзовать необходимые методы
    // Или расширить имеющиеся
}
#endif
```

- расширение сгенерированного **ECV**:
```c#
#if UNITY_EDITOR
public partial class ECV_Comp : ECV<Comp>{
    // Реализовать необходимые методы
    // Или расширить имеющиеся
}
#endif
```

- написание своего `Editor`- скрипта для **ECV**:
  ( [подробности тут](https://docs.unity3d.com/Manual/UIE-HowTo-CreateCustomInspector) )
###### использование стандартного GUILayout-а, возможно, но не рекомендуется
```c#
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Mitfart.LeoECSLite.UnityIntegration;

[CustomEditor(typeof(ECV_Comp))]
public class ECV_Comp_Inspector : Editor {
  public override VisualElement CreateInspectorGUI() {
    // Создание главного/родительского элемента
    var container = new VisualElement();

    // Создание редактора
    // ...

    return container;
  }
}
#endif
```
