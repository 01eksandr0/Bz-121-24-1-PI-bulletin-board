namespace BulletinBoard.WebAPI
{
    /// <summary>
    /// Заголовок, у якому клієнт передає Id поточного користувача.
    /// Повноцінна автентифікація (JWT, cookie) у завданні не вимагається,
    /// тому користувач визначається найпростішим способом.
    /// </summary>
    public static class ApiHeaders
    {
        public const string UserId = "X-User-Id";
    }
}
