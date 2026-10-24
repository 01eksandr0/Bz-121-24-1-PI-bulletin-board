namespace BulletinBoard.WebAPI.Models.Responses
{
    public class CategoryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        /// <summary>0 - рубрика, 1 - категорія, 2 - підкатегорія.</summary>
        public int Level { get; set; }
    }
}
