using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.WebAPI.Models.Requests
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; }

        /// <summary>null - нова рубрика (верхній рівень).</summary>
        public int? ParentId { get; set; }
    }
}
