using System;
using System.Collections.Generic;
using BulletinBoard.DAL.Abstractions.Entities;

namespace BulletinBoard.DAL.EF
{
    /// <summary>Набір вхідних даних для демонстрації. Виконується один раз при створенні БД.</summary>
    internal static class DemoDataSeeder
    {
        public static void Seed(BoardContext context)
        {
            var olena = new User { Name = "Олена", Email = "olena@mail.com" };
            var petro = new User { Name = "Петро", Email = "petro@mail.com" };
            var ivan = new User { Name = "Іван", Email = "ivan@mail.com" };
            context.Users.AddRange(olena, petro, ivan);

            var urgent = new Tag { Name = "терміново" };
            var bargain = new Tag { Name = "торг" };
            var brandNew = new Tag { Name = "новий" };
            var used = new Tag { Name = "б/в" };
            var delivery = new Tag { Name = "доставка" };
            context.Tags.AddRange(urgent, bargain, brandNew, used, delivery);

            var transport = AddCategory(context, "Транспорт", null);
            var realty = AddCategory(context, "Нерухомість", null);
            var electronics = AddCategory(context, "Електроніка", null);
            context.SaveChanges();

            var cars = AddCategory(context, "Авто", transport.Id);
            var bikes = AddCategory(context, "Велосипеди", transport.Id);
            var flats = AddCategory(context, "Квартири", realty.Id);
            var phones = AddCategory(context, "Телефони", electronics.Id);
            var laptops = AddCategory(context, "Ноутбуки", electronics.Id);
            context.SaveChanges();

            var rent = AddCategory(context, "Оренда", flats.Id);
            var sale = AddCategory(context, "Продаж", flats.Id);
            var passengerCars = AddCategory(context, "Легкові", cars.Id);
            context.SaveChanges();

            context.Ads.AddRange(
                NewAd("Продам Volkswagen Golf 2012", "Пробіг 180 тис. км, один власник.", 7500m, petro, passengerCars, used, bargain),
                NewAd("Здам 1-кімнатну квартиру", "Поруч метро, є меблі та техніка.", 12000m, olena, rent, urgent),
                NewAd("iPhone 13 128GB", "Повний комплект, гарантія 3 місяці.", 18000m, ivan, phones, used, delivery),
                NewAd("Гірський велосипед Author", "Рама 19, нові шини.", 9000m, petro, bikes, bargain),
                NewAd("Ноутбук Lenovo ThinkPad", "Новий, запакований.", 32000m, olena, laptops, brandNew, delivery));
            context.SaveChanges();
        }

        private static Category AddCategory(BoardContext context, string name, int? parentId)
        {
            var category = new Category { Name = name, ParentId = parentId };
            context.Categories.Add(category);
            return category;
        }

        private static Ad NewAd(string title, string description, decimal price,
            User author, Category category, params Tag[] tags)
        {
            return new Ad
            {
                Title = title,
                Description = description,
                Price = price,
                CreatedAt = DateTime.Now,
                IsActive = true,
                Author = author,
                Category = category,
                Tags = new List<Tag>(tags)
            };
        }
    }
}
