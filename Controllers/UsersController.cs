using DotNetCardsServer.Auth;
using DotNetCardsServer.Exceptions;
using DotNetCardsServer.Models.MockData;
using DotNetCardsServer.Models.Users;
using DotNetCardsServer.Services.Users;
using DotNetCardsServer.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCardsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UsersService _usersService;

        public UsersController(IMongoClient mongoClient)
        {
            _usersService = new UsersService(mongoClient);
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine(HttpContext.User.FindFirst("id").Value);
            List<User> users =await _usersService.GetUsersAsync();
            return Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
            User? u = await _usersService.GetOneUserAsync(id);
            return Ok(u);
            }
            catch(UserNotFoundException e)
            {
            return NotFound(e.Message);
            }
           
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User newUser)
        {
            //if (ModelState.IsValid)
            //{
            //    return BadRequest("validation");
            //}

            try
            {
              object DTOuser = await  _usersService.CreateUserAsync(newUser);
              return CreatedAtAction(nameof(Get),new{Id=newUser.Id}, DTOuser);

            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
           
            
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] User updatedUser)
        {
            try
            {
            User newUser = await _usersService.EditUserAsync(id, updatedUser);
            }
            catch(UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _usersService.DeleteUserAsync(id);
            }
            catch (UserNotFoundException e)
            {
                return NotFound(e.Message);
            }
          
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult>  Login([FromBody] LoginModel loginModel)
        {
            try
            {
            User? u = await _usersService.LoginAsync(loginModel);
            string token = JwtHelper.GenerateAuthToken(u);
            return Ok(token);

            }
            catch (AuthenticationException ex)
            {
                return Unauthorized("Email or Password wrong");

            }


        }
    }
}
