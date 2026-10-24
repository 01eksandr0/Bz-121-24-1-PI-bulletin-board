using System;
using System.Collections.Generic;

namespace BulletinBoard.BLL.Models
{
    /// <summary>Оголошення в тому вигляді, в якому його отримують верхні рівні.</summary>
    public class AdModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IReadOnlyList<string> Tags { get; set; }
    }
}
