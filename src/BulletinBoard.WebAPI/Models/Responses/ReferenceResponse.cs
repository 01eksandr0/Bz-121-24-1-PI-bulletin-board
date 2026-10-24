namespace BulletinBoard.WebAPI.Models.Responses
{
    /// <summary>Коротке посилання на пов'язаний об'єкт: Id і назва.</summary>
    public class ReferenceResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
