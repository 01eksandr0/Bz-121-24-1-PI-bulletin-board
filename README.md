# BulletinBoard — Дошка оголошень (варіант 1)

Лабораторні роботи 2.2 (багаторівнева архітектура) і 2.4 (WebAPI + DI).

Виконав: Крижановський Олександр Романович, група Бз-121-24-1-ПІ, 2026.

Репозиторій: https://github.com/01eksandr0/Bz-121-24-1-PI-bulletin-board

Рубрики / категорії / підкатегорії, теги, користувачі. Оголошення належить одній
категорії і може мати кілька тегів. Видалити й деактивувати оголошення може тільки його автор.

## Структура рішення

| Проєкт | Рівень | Залежить від |
|---|---|---|
| `BulletinBoard.DAL.Abstractions` | сутності, `IRepository<T>`, `IUnitOfWork` | — |
| `BulletinBoard.DAL.EF` | EF Core Code First (SQLite), generic-репозиторій, UoW | DAL.Abstractions, EF Core |
| `BulletinBoard.BLL` | бізнес-логіка, моделі, mapping | тільки DAL.Abstractions |
| `BulletinBoard.Infrastructure` | DI: модулі Autofac для DAL і BLL | BLL, DAL.EF, Autofac |
| `BulletinBoard.WebAPI` | рівень представлення: ASP.NET Core WebAPI | BLL (+ Infrastructure для DI) |
| `BulletinBoard.ConsoleUI` | консольний UI з лаб. 2.2 | BLL (+ DAL.EF у Program.cs) |
| `BulletinBoard.BLL.Tests` | модульні тести BLL (xUnit) | BLL |

## Запуск WebAPI

Потрібен .NET 8 SDK.

    dotnet run --project src/BulletinBoard.WebAPI

API: http://localhost:5080/api/ads  ·  Swagger: http://localhost:5080/swagger

При першому запуску в папці проєкту створюється `board.db` (Code First) з тестовими даними.
Щоб почати з чистими даними — зупиніть програму і видаліть `board.db`.

## Postman

Імпортуйте `postman/BulletinBoard.postman_collection.json` (File → Import)
і запустіть усю колекцію (Run collection). 30 запитів з перевірками статус-кодів.
Поточний користувач передається заголовком `X-User-Id` (1 — Олена, 2 — Петро, 3 — Іван).

## Основні маршрути

| Метод | Маршрут | Опис |
|---|---|---|
| GET | `/api/ads?categoryId=&tagId=&text=` | пошук активних оголошень |
| GET | `/api/ads/{id}` | оголошення (неактивне — тільки автору) |
| GET | `/api/ads/my` | мої оголошення (X-User-Id) |
| POST | `/api/ads` | нове оголошення (X-User-Id) |
| PUT | `/api/ads/{id}` | редагування (тільки автор) |
| POST | `/api/ads/{id}/deactivate`, `/activate` | деактивація / активація (тільки автор) |
| DELETE | `/api/ads/{id}` | видалення (тільки автор) |
| GET, POST | `/api/categories`, `/api/tags`, `/api/users` | довідники і користувачі |
| GET | `/api/users/{id}`, `/api/users?email=` | користувач |

## Тести

    dotnet test

## Консольний UI (лаб. 2.2)

    dotnet run --project src/BulletinBoard.ConsoleUI

## Міграції (за бажанням)

    dotnet tool install --global dotnet-ef
    dotnet ef migrations add Initial --project src/BulletinBoard.DAL.EF

і в `BoardDatabase.EnsureCreated` замінити `EnsureCreated()` на `Migrate()`.
