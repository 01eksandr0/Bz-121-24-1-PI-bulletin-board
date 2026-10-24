using System;

namespace BulletinBoard.ConsoleUI
{
    /// <summary>Пункт меню: клавіша, назва, дія і чи потрібен вхід у систему.</summary>
    internal class MenuItem
    {
        public MenuItem(string key, string title, bool requiresLogin, Action action)
        {
            Key = key;
            Title = title;
            RequiresLogin = requiresLogin;
            Action = action;
        }

        public string Key { get; }

        public string Title { get; }

        public bool RequiresLogin { get; }

        public Action Action { get; }
    }
}
