Task Management Web Service
Веб-сервис для работы с задачами.
Быстрый старт
Для запуска используйте Docker Compose:
```bash
docker-compose -f docker/docker-compose.yml up -d --build
```
Файл docker-compose.yml находится в папке docker.
Миграции базы данных
Для добавления миграций в каждом сервисе выполните команду:
```bash
dotnet ef migrations add <MigrationName> --project Infrastructure --startup-project <API-Project-Name>
```
Миграции применяются автоматически при запуске приложения.
Технологии
- Работа с базой данных осуществляется через Dapper.
- Entity Framework Core используется только для создания миграций.
Документация API
Краткая документация доступна через Swagger UI в каждом сервисе.
Основные API эндпоинты
Задачи
GET /api/tasks
Получить список задач с фильтрами.
Параметры запроса:
- filter — поиск по заголовку задачи (title).
- pageSize — количество задач на странице.
- pageNumber — номер страницы.
Возвращает задачи, доступные пользователю.
POST /api/tasks
Создать задачу. Тело запроса (JSON):
```json
{
  "title": "string",
  "description": "string",
  "assignedUserId": 0,
  "status": "Planned",
  "isDeleted": true
}
```
status — строка из enum JobStatus:
```csharp
namespace Domain.Enum;
public enum JobStatus { Planned, Testing, Completed, Failed }
```
assignedUserId — ID назначенного пользователя, может быть null.

GET /api/tasks/{id}

Получить задачу по ID, если она доступна пользователю.

PUT /api/tasks/{id}

Обновить задачу по ID.

Тело запроса — те же поля, что и при создании, с изменёнными значениями.

Обновление возможно при наличии прав у пользователя.

DELETE /api/tasks/{id}

Мягкое удаление задачи (помечается как удалённая в БД, поле isDeleted).

DELETE /api/tasks/{id}/hard-delete

Жёсткое удаление — задача полностью удаляется из базы.

PUT /api/tasks/{id}/assign

Назначить исполнителя. Нужно указать ID задачи и ID пользователя.

Исполнитель назначится, если у пользователя есть права и указанный пользователь существует.

GET /api/tasks/get-history-by-job/{jobId}

Получить историю изменений задачи.
Параметры запроса:
- pageSize — количество записей истории на странице.
- pageNumber — номер страницы.

Возвращает всю историю изменений для задачи с указанным jobId.

Особенности

NotificationService и TaskService взаимодействуют с AuthService для валидации логина нестандартным способом.
Все миграции создаются через EF Core, но доступ к базе данных осуществляется через Dapper.
