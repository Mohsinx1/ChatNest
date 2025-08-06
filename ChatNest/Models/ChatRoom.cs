namespace ChatNest.Models
{
    public class ChatRoom
    {
        // ChatRoom properties
        // Making Changes to the ChatRoom class
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Message> Messages { get; set; }
    }

}
