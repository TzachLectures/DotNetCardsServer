using DotNetCardsServer.Models.Users;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace DotNetCardsServer.Models.Cards
{
    #nullable disable
    public class Card        
    {
        public ObjectId Id { get; set; }

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

        public int BizNumber { get; set; }

        public List<string> Likes { get; set; } = new List<string>();

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public string User_Id { get; set; }
    }
}
