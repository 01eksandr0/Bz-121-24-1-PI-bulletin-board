using System.Collections.Generic;

namespace BulletinBoard.BLL.Models
{
    /// <summary>Дані, які користувач вводить при створенні або редагуванні оголошення.</summary>
    public class AdInputModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public IReadOnlyList<int> TagIds { get; set; } = new List<int>();
    }
}
