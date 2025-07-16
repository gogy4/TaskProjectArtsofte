Task Management Web Service
Веб-сервис для работы с задачами.
Быстрый старт
Для запуска используйте Docker Compose:
docker-compose -f docker/docker-compose.yml up -d --build
Файл docker-compose.yml находится в папке docker.
Миграции базы данных
Для добавления миграций в каждом сервисе выполните команду:
dotnet ef migrations add <MigrationName> --project Infrastructure --startup-project <API-Project-Name>
Миграции применяются автоматически при запуске приложения.
Технологии
Работа с базой данных осуществляется через Dapper.
Entity Framework Core используется только для создания миграций.
Документация API
Краткая документация доступна через Swagger UI в каждом сервисе.
Основные API эндпоинты
Задачи
GET /api/tasks
Получить список задач с фильтрами.
Параметры запроса:
filter — поиск по заголовку задачи (title).
pageSize — количество задач на странице.
pageNumber — номер страницы.
Возвращает задачи, доступные пользователю.
POST /api/tasks
Создать задачу. Тело запроса (JSON):
{
  "title": "string",
  "description": "string",
  "assignedUserId": 0,
  "status": "Planned",
  "isDeleted": true
}
status — строка из enum JobStatus:
namespace Domain.Enum;
public enum JobStatus { Planned, Testing, Completed, Failed }
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
pageSize — количество записей истории на странице.
pageNumber — номер страницы.
Возвращает всю историю изменений для задачи с указанным jobId.
Аутентификация и Пользователи
POST /api/auth/register – регистрация
Регистрация пользователя с проверкой на существующую почту.
Тело запроса (JSON):
{
  "email": "string",
  "password": "string"
}
Решено оставить только почту и пароль без добавления имени.
POST /api/auth/login – вход (получение JWT)
Логин пользователя, который возвращает JWT токен.
Тело запроса (JSON):
{
  "email": "string",
  "password": "string"
}
GET /api/auth/me – информация о текущем пользователе
Возвращает информацию о текущем аутентифицированном пользователе.
Пример ответа:
{
  "id": 1,
  "email": "qwerty@gmail.com",
  "hashedPassword": "5jJhkTqDgTDnLZthskGQczxTSrOYRgDexETz2gvPF4f5dGrh6L6bM86wqhF1gzWGfCSVDHNua+im/bgOTlQwGw==",
  "salt": "07cd5f5d-0efa-4ab2-a8b5-3757a810bfe3"
}
В других сервисах этот метод используется для проверки аутентификации.
POST /auth/api/logout – выход из аккаунта
Завершение сессии пользователя.
GET /auth/api/get-user-by/{id} – получение информации о пользователе по ID
Возвращает информацию о пользователе по его ID.
Используется для проверки существования пользователя, например, при назначении задачи.
Уведомления
GET /api/notifications/{userId} – получить список уведомлений
Для получения уведомлений нужно указать ID пользователя.
POST /api/notifications – создать уведомление
Отправляет уведомление пользователю, если он существует и текущий пользователь авторизован.
PUT /api/notifications/{id}/mark-as-read – отметить уведомление как прочитанное
Отмечает уведомление прочитанным, если оно принадлежит текущему пользователю.
Особенности
NotificationService и TaskService взаимодействуют с AuthService для валидации логина способом.
Все миграции создаются через EF Core, но доступ к базе данных осуществляется через Dapper.
