using System.ComponentModel.DataAnnotations;

namespace ChatNest.Models.ViewModels
{

    public class vmLogin
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
