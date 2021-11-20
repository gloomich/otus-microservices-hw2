using System.ComponentModel.DataAnnotations;

namespace UserApi.Models
{
    public class User
    {
        public User() { }

        public User(DataAccess.User src)
        {
            Id = src.Id;
            FirstName = src.FirstName;
            LastName = src.LastName;
        }

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
    }
}
