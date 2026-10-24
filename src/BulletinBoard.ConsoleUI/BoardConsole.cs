using System;
using System.Collections.Generic;
using System.Linq;
using BulletinBoard.BLL.Exceptions;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models;

namespace BulletinBoard.ConsoleUI
{
    /// <summary>
    /// Консольний інтерфейс. Тут тільки ввід даних, виклик сервісів BLL і показ результату.
    /// Усі правила (хто може видаляти, які дані допустимі) перевіряє BLL.
    /// </summary>
    public class BoardConsole
    {
        private readonly IUserService _users;
        private readonly ICategoryService _categories;
        private readonly ITagService _tags;
        private readonly IAdService _ads;
        private readonly List<MenuItem> _menu;

        private UserModel _currentUser;

        public BoardConsole(IUserService users, ICategoryService categories, ITagService tags, IAdService ads)
        {
            _users = users;
            _categories = categories;
            _tags = tags;
            _ads = ads;
            _menu = new List<MenuItem>
            {
                new MenuItem("1", "Увійти за email", false, Login),
                new MenuItem("2", "Зареєструватися", false, Register),
                new MenuItem("3", "Категорії", false, ShowCategories),
                new MenuItem("4", "Теги", false, ShowTags),
                new MenuItem("5", "Пошук оголошень", false, Search),
                new MenuItem("6", "Переглянути оголошення за номером", false, ShowAd),
                new MenuItem("7", "Мої оголошення", true, ShowMyAds),
                new MenuItem("8", "Додати оголошення", true, CreateAd),
                new MenuItem("9", "Редагувати оголошення", true, EditAd),
                new MenuItem("10", "Деактивувати оголошення", true, () => ChangeAd("деактивовано", _ads.Deactivate)),
                new MenuItem("11", "Активувати оголошення", true, () => ChangeAd("активовано", _ads.Activate)),
                new MenuItem("12", "Видалити оголошення", true, DeleteAd),
                new MenuItem("13", "Додати категорію", true, CreateCategory),
                new MenuItem("14", "Додати тег", true, CreateTag),
                new MenuItem("15", "Вийти з акаунта", true, () => _currentUser = null)
            };
        }

        public void Run()
        {
            while (true)
            {
                PrintMenu();
                string key = ConsoleInput.ReadText("Ваш вибір: ");
                if (key == "0")
                {
                    return;
                }

                MenuItem item = _menu.FirstOrDefault(m => m.Key == key);
                if (item == null)
                {
                    Console.WriteLine("Невідомий пункт меню.");
                }
                else if (item.RequiresLogin && _currentUser == null)
                {
                    Console.WriteLine("Спочатку увійдіть або зареєструйтеся.");
                }
                else
                {
                    Execute(item.Action);
                }
            }
        }

        private static void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (BusinessException ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("===== ДОШКА ОГОЛОШЕНЬ =====  Користувач: " +
                              (_currentUser == null ? "гість" : _currentUser.Name + " (" + _currentUser.Email + ")"));
            foreach (MenuItem item in _menu.Where(m => !m.RequiresLogin || _currentUser != null))
            {
                Console.WriteLine(item.Key.PadLeft(2) + " - " + item.Title);
            }
            Console.WriteLine(" 0 - Вихід");
        }

        private void Login()
        {
            _currentUser = _users.GetByEmail(ConsoleInput.ReadText("Email: "));
            Console.WriteLine("Вітаємо, " + _currentUser.Name + "!");
        }

        private void Register()
        {
            string name = ConsoleInput.ReadText("Ім'я: ");
            string email = ConsoleInput.ReadText("Email: ");
            _currentUser = _users.Register(name, email);
            Console.WriteLine("Реєстрація успішна. Ви увійшли як " + _currentUser.Name + ".");
        }

        private void ShowCategories()
        {
            ConsoleView.PrintCategories(_categories.GetTree());
        }

        private void ShowTags()
        {
            ConsoleView.PrintTags(_tags.GetAll());
        }

        private void Search()
        {
            var filter = new AdSearchFilter
            {
                CategoryId = ConsoleInput.ReadOptionalInt("Номер категорії (Enter - усі): "),
                TagId = ConsoleInput.ReadOptionalInt("Номер тегу (Enter - усі): "),
                Text = ConsoleInput.ReadText("Текст для пошуку (Enter - без тексту): ")
            };
            ConsoleView.PrintAds(_ads.Search(filter));
        }

        private void ShowAd()
        {
            int id = ConsoleInput.ReadInt("Номер оголошення: ");
            int? viewerId = _currentUser == null ? (int?)null : _currentUser.Id;
            ConsoleView.PrintAdFull(_ads.GetById(id, viewerId));
        }

        private void ShowMyAds()
        {
            ConsoleView.PrintAds(_ads.GetByAuthor(_currentUser.Id));
        }

        private void CreateAd()
        {
            AdModel ad = _ads.Create(_currentUser.Id, ReadAdInput());
            Console.WriteLine("Оголошення додано:");
            ConsoleView.PrintAdFull(ad);
        }

        private void EditAd()
        {
            int id = ConsoleInput.ReadInt("Номер оголошення: ");
            AdModel ad = _ads.Update(id, _currentUser.Id, ReadAdInput());
            Console.WriteLine("Оголошення змінено:");
            ConsoleView.PrintAdFull(ad);
        }

        private void ChangeAd(string doneText, Action<int, int> change)
        {
            int id = ConsoleInput.ReadInt("Номер оголошення: ");
            change(id, _currentUser.Id);
            Console.WriteLine("Оголошення #" + id + " " + doneText + ".");
        }

        private void DeleteAd()
        {
            int id = ConsoleInput.ReadInt("Номер оголошення: ");
            if (ConsoleInput.Confirm("Видалити оголошення #" + id + " назавжди?"))
            {
                _ads.Delete(id, _currentUser.Id);
                Console.WriteLine("Оголошення #" + id + " видалено.");
            }
        }

        private void CreateCategory()
        {
            ShowCategories();
            int? parentId = ConsoleInput.ReadOptionalInt("Номер батьківської категорії (Enter - нова рубрика): ");
            CategoryModel category = _categories.Create(ConsoleInput.ReadText("Назва: "), parentId);
            Console.WriteLine("Категорію [" + category.Id + "] " + category.Name + " додано.");
        }

        private void CreateTag()
        {
            TagModel tag = _tags.Create(ConsoleInput.ReadText("Назва тегу: "));
            Console.WriteLine("Тег [" + tag.Id + "] " + tag.Name + " додано.");
        }

        private AdInputModel ReadAdInput()
        {
            var input = new AdInputModel
            {
                Title = ConsoleInput.ReadText("Заголовок: "),
                Description = ConsoleInput.ReadText("Опис: "),
                Price = ConsoleInput.ReadDecimal("Ціна, грн: ")
            };
            ShowCategories();
            input.CategoryId = ConsoleInput.ReadInt("Номер категорії: ");
            ShowTags();
            input.TagIds = ConsoleInput.ReadIntList("Номери тегів через кому (Enter - без тегів): ");
            return input;
        }
    }
}
