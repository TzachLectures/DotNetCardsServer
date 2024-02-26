using DotNetCardsServer.Models.Users;
using System;
using System.ComponentModel.DataAnnotations;

public class UserSqlModel
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string ImageUrl { get; set; }
    public string ImageAlt { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int HouseNumber { get; set; }
    public int Zip { get; set; } = 0;
    public bool IsAdmin { get; set; } = false;
    public bool IsBusiness { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public UserSqlModel() { }


    public UserSqlModel(User user)
    {
        Id = user.Id.ToString();
        FirstName = user.Name?.First;
        LastName = user.Name?.Last;
        MiddleName = user.Name?.Middle;
        Email = user.Email;
        Password = user.Password;
        Phone = user.Phone;
        ImageUrl = user.Image?.Url;
        ImageAlt = user.Image?.Alt;
        State = user.Address?.State;
        Country = user.Address?.Country;
        City = user.Address?.City;
        Street = user.Address?.Street;
        HouseNumber = user.Address?.HouseNumber ?? 0; // Assuming HouseNumber is a non-nullable int
        Zip = user.Address?.Zip ?? 0; // Assuming Zip is a non-nullable int
        IsAdmin = user.IsAdmin;
        IsBusiness = user.IsBusiness;
        CreatedAt = user.CreatedAt;
    }
}
