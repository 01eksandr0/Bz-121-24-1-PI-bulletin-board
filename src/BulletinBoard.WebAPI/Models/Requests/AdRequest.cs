using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.WebAPI.Models.Requests
{
    /// <summary>
    /// Тіло запиту POST і PUT для оголошення.
    /// Числові поля nullable, щоб [Required] міг помітити, що поле не передали
    /// (інакше відсутнє поле мовчки стало б нулем).
    /// </summary>
    public class AdRequest
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();
    }
}
