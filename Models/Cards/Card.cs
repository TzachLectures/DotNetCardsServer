using DotNetCardsServer.Models.Users;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotNetCardsServer.Models.Cards
{
    #nullable disable
    public class Card        
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string _id => Id.ToString();

        [Required, StringLength(maximumLength:256,MinimumLength =2)] 
        public string Title { get; set; }

        [Required, StringLength(maximumLength: 256, MinimumLength = 2)]
        public string Subtitle { get; set; }

        [Required, StringLength(1024)]
        public string Description { get; set; }

        [Required,Phone]
        public string Phone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Url]
        public string Web { get; set; }

        public Image Image { get; set; }
        public Address Address { get; set; }

        [Required]
        public int BizNumber { get; set; }

        public List<string> Likes { get; set; } = new List<string>();

        public DateTime CreateAt { get; set; } = DateTime.Now;

        [JsonPropertyName("user_id")]
        public string User_Id { get; set; }

        public Card() { }

        public Card(CardSqlModel cardSqlModel, List<string> likes = null)
        {
            Id = (cardSqlModel.Id);
            Title = cardSqlModel.Title;
            Subtitle = cardSqlModel.Subtitle;
            Description = cardSqlModel.Description;
            Phone = cardSqlModel.Phone;
            Email = cardSqlModel.Email;
            Web = cardSqlModel.Web;
            Image = new Image
            {
                Url = cardSqlModel.ImageUrl,
                Alt = cardSqlModel.ImageAlt
            };
            Address = new Address
            {
                State = cardSqlModel.State,
                Country = cardSqlModel.Country,
                City = cardSqlModel.City,
                Street = cardSqlModel.Street,
                HouseNumber = cardSqlModel.HouseNumber,
                Zip = cardSqlModel.Zip
            };
            BizNumber = cardSqlModel.BizNumber;
            CreateAt = cardSqlModel.CreateAt;
            User_Id = cardSqlModel.User_Id;
            // Assuming Likes is a collection of User IDs in Card and UserCardLike entities have a UserId property
            Likes = likes ?? new List<string>();
        }
    }
}
