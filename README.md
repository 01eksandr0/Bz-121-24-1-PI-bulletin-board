# Лабораторна робота 2.2 — Багаторівнева архітектура (варіант 1: Дошка оголошень)

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
| `BulletinBoard.ConsoleUI` | консольний інтерфейс | BLL (+ DAL.EF лише в Program.cs) |

## Запуск

Потрібен .NET 8 SDK.

    dotnet run --project src/BulletinBoard.ConsoleUI

При першому запуску поруч із програмою створюється база `board.db`
(Code First, `EnsureCreated`) і заповнюється тестовими даними.
Щоб почати з чистими даними — видаліть `board.db`.

Тестові користувачі: `olena@mail.com`, `petro@mail.com`, `ivan@mail.com`.

## Міграції (за бажанням)

Замість `EnsureCreated` можна використовувати міграції EF:

    dotnet tool install --global dotnet-ef
    dotnet ef migrations add Initial --project src/BulletinBoard.DAL.EF

і в `EfUnitOfWorkFactory` замінити `EnsureCreated()` на `Migrate()`.
Для роботи інструменту в DAL.EF є `BoardContextDesignFactory`.

## Документація

`docs/` — UML-діаграми (PlantUML і PNG).
