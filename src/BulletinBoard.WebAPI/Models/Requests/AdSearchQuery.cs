namespace BulletinBoard.WebAPI.Models.Requests
{
    /// <summary>Параметри пошуку з рядка запиту: /api/ads?categoryId=1&amp;tagId=2&amp;text=авто</summary>
    public class AdSearchQuery
    {
        public int? CategoryId { get; set; }

        public int? TagId { get; set; }

        public string Text { get; set; }
    }
}
