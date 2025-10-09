# OrderService (ASP.NET Core 8) - три слоя + PostgreSQL

## Структура
- CoreLib - сущности, DTO, интерфейсы
- OrderService.Dal - DbContext, репозитории (EF Core + Npgsql)
- OrderService.Logic - бизнес-логика
- OrderService.Api - Web API, контроллеры, Program.cs

## Как запустить
1. Установите .NET 8 SDK.
2. Установите PostgreSQL и создайте БД `food_delivery` или используйте строку подключения в `OrderService.Api/appsettings.json`.
3. В корне проекта выполните:
```bash
cd /path/to/OrderServiceProject
dotnet build
dotnet run --project OrderService.Api
```

Swagger будет доступен в режиме разработки.

## Примечания
- В проекте использована EnsureCreated() для упрощения. Для продакшена используйте миграции (dotnet ef migrations add ...).
- По умолчанию в `appsettings.json` строка подключения:
  `Host=localhost;Port=5432;Database=food_delivery;Username=postgres;Password=12345`
