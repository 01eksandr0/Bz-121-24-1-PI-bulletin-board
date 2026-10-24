using System;
using System.Collections.Generic;

namespace BulletinBoard.DAL.Abstractions.Entities
{
    /// <summary>Оголошення: одна категорія, кілька тегів, один автор.</summary>
    public class Ad : IEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
