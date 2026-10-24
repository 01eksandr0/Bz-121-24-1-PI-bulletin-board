using System;
using System.Collections.Generic;
using System.Globalization;
using BulletinBoard.BLL.Models;

namespace BulletinBoard.ConsoleUI
{
    /// <summary>Виведення моделей BLL на консоль.</summary>
    internal static class ConsoleView
    {
        private static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("uk-UA");

        public static void PrintAdShort(AdModel ad)
        {
            string status = ad.IsActive ? "" : " [неактивне]";
            Console.WriteLine("  #" + ad.Id + " " + ad.Title + " - " + FormatPrice(ad.Price) + status);
            Console.WriteLine("     " + ad.CategoryName + " | " + FormatTags(ad.Tags) + " | автор: " + ad.AuthorName);
        }

        public static void PrintAdFull(AdModel ad)
        {
            Console.WriteLine("Оголошення #" + ad.Id + (ad.IsActive ? "" : " [неактивне]"));
            Console.WriteLine("  Заголовок: " + ad.Title);
            Console.WriteLine("  Опис:      " + ad.Description);
            Console.WriteLine("  Ціна:      " + FormatPrice(ad.Price));
            Console.WriteLine("  Категорія: " + ad.CategoryName);
            Console.WriteLine("  Теги:      " + FormatTags(ad.Tags));
            Console.WriteLine("  Автор:     " + ad.AuthorName);
            Console.WriteLine("  Додано:    " + ad.CreatedAt.ToString("dd.MM.yyyy HH:mm", Culture));
        }

        public static void PrintAds(IReadOnlyList<AdModel> ads)
        {
            if (ads.Count == 0)
            {
                Console.WriteLine("Оголошень не знайдено.");
                return;
            }
            Console.WriteLine("Знайдено оголошень: " + ads.Count);
            foreach (AdModel ad in ads)
            {
                PrintAdShort(ad);
            }
        }

        public static void PrintCategories(IReadOnlyList<CategoryModel> categories)
        {
            foreach (CategoryModel category in categories)
            {
                Console.WriteLine(new string(' ', 2 + category.Level * 4) + "[" + category.Id + "] " + category.Name);
            }
        }

        public static void PrintTags(IReadOnlyList<TagModel> tags)
        {
            foreach (TagModel tag in tags)
            {
                Console.WriteLine("  [" + tag.Id + "] " + tag.Name);
            }
        }

        private static string FormatPrice(decimal price)
        {
            return price.ToString("N2", Culture) + " грн";
        }

        private static string FormatTags(IReadOnlyList<string> tags)
        {
            return tags.Count == 0 ? "без тегів" : "#" + string.Join(" #", tags);
        }
    }
}
