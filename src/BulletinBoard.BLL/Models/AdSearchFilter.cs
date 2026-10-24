namespace BulletinBoard.BLL.Models
{
    /// <summary>Умови пошуку. Порожнє поле означає "не фільтрувати".</summary>
    public class AdSearchFilter
    {
        public int? CategoryId { get; set; }

        public int? TagId { get; set; }

        public string Text { get; set; }
    }
}
