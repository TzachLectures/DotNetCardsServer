using MongoDB.Bson;

namespace DotNetCardsServer.Models.Users
{
    public class User
    {
        public ObjectId Id { get; set; }
        public Name Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public Image Image { get; set; }
        public Address Address { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsBussines { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
