namespace BulletinBoard.DAL.Abstractions.Entities
{
    /// <summary>
    /// Рубрика, категорія або підкатегорія. Рівень визначається батьком:
    /// ParentId = null - рубрика (верхній рівень).
    /// </summary>
    public class Category : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
    }
}
