# Цель мини проекта - создать базовую архитектуру для быстрого старта разработки чего-либо на unity
В данном репозитории собраны все мои (и не совсем мои) наработки для создания поддерживаемого и расширяемого кода для unity. В целом это исключительно инфраструктурный проект. Буду расширять по мере появления идей.
>https://youtu.be/tIt1cMfe7r4

"Геймпей" предельно простой- две случайно сгенерированные воксельные фигуры сталкиваются и перекрашиваются. Счетчик столкновений работает некорректно (чтобы чинить надо по-другому меши собирать) :)

## Что интересного:
- Сисетма менеджеров с двумя точками входа - GameController-ом и GUIController-ом. Все остальные менеджеры создаются с помощью метода ```GetAllInstances<T>()``` в классе ```MonoBehaviourServiceHelper```. Этот метод возвращает список всех классов наследников интерфейса ```T``` и класса ```MonoBehaviourService``` в сборке. Чрезвычайно удобная фича, не нужно руками создавать GameObject-ы или удалять их.
>> TODO: сделать чтобы нельзя было передавать в ```T``` не интерфейс. Сейчас если это сделать то ничего не происходит, что может привести к плохоотслеживаемым ошибкам. Ну и может оптимизировать поиск нужных классов в сборке. Сейчас оно работает с точки зрения RAM модели где-то за квадрат от количества типов в сборке, но по факту дольше. Можно переписать с linq на циклы, но выглядеть это будет некрасиво. Польза не очень очевидна от этих действий - этот поиск экземпляров выполняется исключительно во время билда.
- Класс ```MonoBehaviourService```. Как известно, в unity классы наследуемые от MonoBehaviour не могут быть статическими, что иногда печально. Поэтому для менеджеров (пусть не всегда по своей сути статических, главное единственных) приходится использовать такой синглтон. 
- Шина ивентов взята из интернета и существенно отрефакторена, добавлены мелкие возможности для упрощения жизни.
- Для поддержания единственности точки входа создан интерфейс ```ICallable``` с методом ```Call```. Тем самым все Update-ы скручиваются в один в GameManager
- Структура разделена на слои на столько на сколько у меня это получилось, MVC с unity не особо дружит, но тут почти
>> TODO: нарисовать UML-ку
- Минимальное общение с движком unity
>> Для кого-то это может быть минусом :)


### Глобальный TODO: сделать инъекцию зависимостей для менеджеров которые работают с UI, а то сейчас это делается так: 
```csharp
private readonly GUIManager guiManager = GUIManager.Get()
```
Проблема: в unity нельзя NuGet-ы использовать (можно, но проще самому написать чем через такие костыли лезть)
