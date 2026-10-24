using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BulletinBoard.ConsoleUI
{
    /// <summary>Зчитування і перевірка формату введених даних (не бізнес-правил).</summary>
    internal static class ConsoleInput
    {
        public static string ReadText(string prompt)
        {
            Console.Write(prompt);
            return (Console.ReadLine() ?? string.Empty).Trim();
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                int value;
                if (int.TryParse(ReadText(prompt), out value))
                {
                    return value;
                }
                Console.WriteLine("Введіть ціле число.");
            }
        }

        /// <summary>Порожній ввід означає "не вказано".</summary>
        public static int? ReadOptionalInt(string prompt)
        {
            while (true)
            {
                string text = ReadText(prompt);
                if (text.Length == 0)
                {
                    return null;
                }
                int value;
                if (int.TryParse(text, out value))
                {
                    return value;
                }
                Console.WriteLine("Введіть ціле число або залиште порожнім.");
            }
        }

        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                decimal value;
                string text = ReadText(prompt).Replace(',', '.');
                if (decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                {
                    return value;
                }
                Console.WriteLine("Введіть число, наприклад 1500 або 99.90");
            }
        }

        /// <summary>Список чисел через кому, наприклад "1, 3, 5". Порожньо - порожній список.</summary>
        public static List<int> ReadIntList(string prompt)
        {
            while (true)
            {
                string[] parts = ReadText(prompt).Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int number;
                if (parts.All(p => int.TryParse(p, out number)))
                {
                    return parts.Select(int.Parse).ToList();
                }
                Console.WriteLine("Введіть номери через кому, наприклад: 1, 3");
            }
        }

        public static bool Confirm(string prompt)
        {
            string answer = ReadText(prompt + " (так/ні): ").ToLowerInvariant();
            return answer == "так" || answer == "т" || answer == "y" || answer == "yes";
        }
    }
}
