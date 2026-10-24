using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.WebAPI.Models.Requests
{
    public class CreateTagRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
