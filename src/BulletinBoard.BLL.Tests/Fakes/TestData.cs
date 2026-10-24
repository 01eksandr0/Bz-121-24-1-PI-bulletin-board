using System;
using System.Collections.Generic;
using BulletinBoard.DAL.Abstractions.Entities;

namespace BulletinBoard.BLL.Tests.Fakes
{
    /// <summary>
    /// Невеликий набір даних для тестів:
    /// користувачі 1 - Олена, 2 - Петро;
    /// категорії 1 Транспорт > 2 Авто > 3 Легкові, 4 Електроніка;
    /// теги 1..6; оголошення 1 (Петро, Легкові), 2 (Олена, Електроніка).
    /// </summary>
    public static class TestData
    {
        public const int Olena = 1;
        public const int Petro = 2;
        public const int Transport = 1;
        public const int Cars = 2;
        public const int PassengerCars = 3;
        public const int Electronics = 4;
        public const int PetroAd = 1;
        public const int OlenaAd = 2;

        public static InMemoryUnitOfWork Create()
        {
            var uow = new InMemoryUnitOfWork();
            uow.Users.Add(new User { Name = "Олена", Email = "olena@mail.com" });
            uow.Users.Add(new User { Name = "Петро", Email = "petro@mail.com" });

            uow.Categories.Add(new Category { Name = "Транспорт" });
            uow.Categories.Add(new Category { Name = "Авто", ParentId = Transport });
            uow.Categories.Add(new Category { Name = "Легкові", ParentId = Cars });
            uow.Categories.Add(new Category { Name = "Електроніка" });

            foreach (string name in new[] { "терміново", "торг", "новий", "б/в", "доставка", "обмін" })
            {
                uow.Tags.Add(new Tag { Name = name });
            }

            uow.Ads.Add(NewAd("Продам Golf", Petro, PassengerCars, uow.Tags.GetById(2)));
            uow.Ads.Add(NewAd("Ноутбук Lenovo", Olena, Electronics, uow.Tags.GetById(3), uow.Tags.GetById(5)));
            return uow;
        }

        private static Ad NewAd(string title, int authorId, int categoryId, params Tag[] tags)
        {
            return new Ad
            {
                Title = title,
                Description = "Опис",
                Price = 100m,
                CreatedAt = DateTime.Now,
                IsActive = true,
                AuthorId = authorId,
                CategoryId = categoryId,
                Tags = new List<Tag>(tags)
            };
        }
    }
}
