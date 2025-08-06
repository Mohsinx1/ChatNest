namespace ChatNest.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        // Foreign Keys
        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int? ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }

}
