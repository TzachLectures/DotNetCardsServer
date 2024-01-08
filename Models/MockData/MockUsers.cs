using DotNetCardsServer.Models.Users;

namespace DotNetCardsServer.Models.MockData
{
    public class MockUsers
    {
        static public List<User> UserList = new List<User>
        {
            new User
            {
       
                Name = new Name { First = "John", Last = "Doe", Middle = "T" },
                Email = "john.doe@example.com",
                Password = "yourSecurePassword",
                Phone = "123-456-7890",
                Image = new Image { Url = "http://example.com/image1.jpg", Alt = "User Profile Image" },
                Address = new Address { State = "State1", Country = "Country1", City = "City1", Street = "Street1", HouseNumber = 101, Zip = 1001 },
                IsAdmin = false,
                IsBussines = true
            },
            new User
            {
        
                Name = new Name { First = "Alice", Last = "Smith", Middle = "B" },
                Email = "alice.smith@example.com",
                Password = "anotherSecurePassword",
                Phone = "234-567-8901",
                Image = new Image { Url = "http://example.com/image2.jpg", Alt = "Alice's Profile Image" },
                Address = new Address { State = "State2", Country = "Country2", City = "City2", Street = "Street2", HouseNumber = 202, Zip = 2002 },
                IsAdmin = true,
                IsBussines = false
            },
            new User
            {
         
                Name = new Name { First = "Bob", Last = "Johnson", Middle = "C" },
                Email = "bob.johnson@example.com",
                Password = "securePassword123",
                Phone = "345-678-9012",
                Image = new Image { Url = "http://example.com/image3.jpg", Alt = "Bob's Profile Image" },
                Address = new Address { State = "State3", Country = "Country3", City = "City3", Street = "Street3", HouseNumber = 303, Zip = 3003 },
                IsAdmin = false,
                IsBussines = false
            }
        };




    }
}
