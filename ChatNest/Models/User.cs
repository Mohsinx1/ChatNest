using System.ComponentModel.DataAnnotations;

namespace ChatNest.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string ConnectionId { get; set; } // For SignalR

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }


}
