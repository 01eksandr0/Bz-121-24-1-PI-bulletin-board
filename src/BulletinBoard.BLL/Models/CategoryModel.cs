namespace BulletinBoard.BLL.Models
{
    /// <summary>Категорія для показу. Level: 0 - рубрика, 1 - категорія, 2 - підкатегорія.</summary>
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public int Level { get; set; }
    }
}
