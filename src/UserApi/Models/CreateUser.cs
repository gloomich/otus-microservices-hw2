using System.ComponentModel.DataAnnotations;

namespace UserApi.Models
{
    public class CreateUser
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
    }
}
