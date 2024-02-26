using DotNetCardsServer.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class CardSqlModel
{
    [Key]
    public string Id { get; set; } =  Guid.NewGuid().ToString();
    public string _id => Id.ToString();
    [Required, StringLength(maximumLength: 256, MinimumLength = 2)]
    public string Title { get; set; }
    [Required, StringLength(maximumLength: 256, MinimumLength = 2)]
    public string Subtitle { get; set; }
    [Required, StringLength(1024)]
    public string Description { get; set; }
    [Required, Phone]
    public string Phone { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Url]
    public string Web { get; set; }
    public string ImageUrl { get; set; }
    public string ImageAlt { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int HouseNumber { get; set; }
    public int Zip { get; set; } = 0;
    [Required]
    public int BizNumber { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    [ForeignKey("User")]
   public string User_Id { get; set; }
    public UserSqlModel User { get; set; }
}
