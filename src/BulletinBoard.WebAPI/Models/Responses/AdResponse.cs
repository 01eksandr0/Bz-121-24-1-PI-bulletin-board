using System;
using System.Collections.Generic;

namespace BulletinBoard.WebAPI.Models.Responses
{
    /// <summary>Оголошення у форматі відповіді API: автор і категорія - вкладені об'єкти.</summary>
    public class AdResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public ReferenceResponse Author { get; set; }

        public ReferenceResponse Category { get; set; }

        public IReadOnlyList<string> Tags { get; set; }
    }
}
