using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

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
        public bool IsBusiness { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User() { }

        public User(UserSqlModel userModel)
        {
            Id = new ObjectId(userModel.Id);
            Name = new Name
            {
                First = userModel.FirstName,
                Last = userModel.LastName,
                Middle = userModel.MiddleName
            };
            Email = userModel.Email;
            Password = userModel.Password;
            Phone = userModel.Phone;
            Image = new Image
            {
                Url = userModel.ImageUrl,
                Alt = userModel.ImageAlt
            };
            Address = new Address
            {
                State = userModel.State,
                Country = userModel.Country,
                City = userModel.City,
                Street = userModel.Street,
                HouseNumber = userModel.HouseNumber,
                Zip = userModel.Zip
            };
            IsAdmin = userModel.IsAdmin;
            IsBusiness = userModel.IsBusiness;
            CreatedAt = userModel.CreatedAt;
        }
    }
}
