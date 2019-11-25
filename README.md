# Hive
Framework быстрой разработки микросервисов декомпозированных по субдомену.


## Проблема
При производстве программного обеспечения на основе архитектурного стиля микросервисы очень важным выступает предметно-ориентированное проектирование. У разработчиков очень часто возникают проблемы обучения, фокусировки на тактических паттернах и в принципе чисто технических проблемах. При этом теряется самое главное - моделирование, коммуникация, фиксация на главных задачах бизнеса.

Передовой опыт многих корпараций состоит в ещё большем обобществлении труда разработчиков и его специализации - разделение команд не только на фронтовые и бэкендовые, но архитектурные и продуктовые. При всём этом, далеко не все готовы оплачивать поиск идеальной архитектуры. Цель архитектурной комманды при этом - дать продуктовым пример для разработки микросервисов, с помощью которого разработчики, которые всё ещё нуждаются в обучении смогут начать разработку быстрее, одновременно проводя своё обучение.

Противоречия антогонизма бизнесовых и инженерных требований так или иначе должны будут найти выход, и возможно этот проект кому-то поможет. Это и универсальная инфраструктура микросервиса, и объяснение того что именно представляет из себя его архитектура, и пример использования.

**Никаких тренингов, инфобизнеса, деформаций и дисфункций** - проектируйте и исследуйте!

## Описание архитектуры
Гексогональная архитектура. Микросервис при этом представлен следующими слоями:
1. Доменная модель.
2. Доменные сервисы.
3. Уровень приложения.
4. Уровень инфраструктуры.
5. Уровень хостинга.

### Строение модели
Очень часто разработчики имеют разное представление о строекнии модели в аспекте тактических паттернов. Здесь используется следующая терминология:
**Подобласть** - идеально выделенный экспертами предметной области ограниченный контекст.
**Ограниченный контекст** состоит из
1. **Агрегат**
1.1. **Анемичная модель**
1.1.1. **Корень агрегата** - версионированная сущность.
1.1.2. **Объект-значения**.
1.2. **Границы области** - искуственный контейнер валидаторов и операций.
1.2.1. **Валидаторы** предметной области.
1.2.2. **Бизнес-операции** являющиеся объектами-фабриками.
2. **Доменные сообщения**.

**Корень агрегата** в модели представлен рядом интерфейсов, в результате чего это и корневое объект-значение, и сущность с иднтификаторм и версией.
В результате вызова обработчика **Бизнес-операции** сначала валидируется входящая **Анемичная модель**, а затем создаётся экземляр агрегата, с новой версией. Все изменения агригатов при этом проецируются в виде **Доменного сообщения** в интерфейс ШИНЫ ДОМЕННЫХ СООБЩЕНИЙ.

### Состав доменных сервисов.
1. Провайдер агрегатов.
2. Нотификатор шины событий.

### Важные оссобенности.
1. В данной реализации не используются механимы отслеживания состояния агрегатов, такие как **Снимки**, или логика в Setter'ах полей. Вместо этого все классы интерфейсы **намеренно иммутабельные**. Изменение объектов-значений, аргрегатов в целом обязоно проводиться путём вызова бизнес-операции агрегата. Это приводит к генерированию новой версии, генерированию сообщения предметной области, позволяет соблюсти принцип инвариата агрегата и границы транзакционной целостности. **В свойствах любых класоов модели не должно быть доступных setter'ов!**
2. База данных для данной архитектуры всего лишь адаптер. В центре действия всех комманд находится ШИНА ДОМЕННЫХ СОБЫТИЙ. Это означает следующее:
- Если в результате бизнес-операции валидатор пометит результат как валидный, событие будет обработано и передано остальным потребителям.
- Если в результате бизнес-операции валидатор пометит результат как не валидный, отправка события будет зависить от стратегии передачи не валидных результатов (иногда не валидные результаты приемлимы по требованиям бизнеса).
- Если в результате бизнес-операции событие будет передано в обработку, но возникнет инфраструктурная ошибка, весь сервис будет приостановлен с отказом в обслживании (todo).
- Если в результате бизнес-операции валидатор пометит результат как валидный, событие будет отправлено в шину, но в результате обработки возникнет отказ в обслуживании БД - сервис всё равно продолжит работу с командами.


## RoadMap
1. Построение инфраструктуры 
1. Слои доменной модели и доменных сервисов.
2. Kafka как Domain Event Bus.
3. EventBus как центральный порт комманд в слой приложения.
4. Метапрограммирование моделей.
4.1. Метамодель субдомена.
4.2. Конструктор субдомена.
5. Универсальное noSQL хранилище для метамоделей.
6. Универсальное реляционное хранилище для метамоделей.
7. Универсальный адаптер REST.
