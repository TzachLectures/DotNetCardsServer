using DotNetCardsServer.Auth;
using DotNetCardsServer.Exceptions;
using DotNetCardsServer.Models.Users;
using DotNetCardsServer.Services.Users;
using DotNetCardsServer.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Claims;

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
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Get()
        {    
            List<User> users =await _usersService.GetUsersAsync();
            return Ok(users);
        }


        // GET api/<UsersController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            bool.TryParse(HttpContext.User.FindFirstValue("isAdmin"), out bool isAdmin);
            bool isMyId = HttpContext.User.FindFirstValue("id") == id;
            if(!isAdmin && !isMyId)
            {
                return Unauthorized("You can watch only your own profile");
            }

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            bool.TryParse(HttpContext.User.FindFirstValue("isAdmin"), out bool isAdmin);
            bool isMyId = HttpContext.User.FindFirstValue("id") == id;
            if (!isAdmin && !isMyId)
            {
                return Unauthorized("You can watch only your own profile");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            bool.TryParse(HttpContext.User.FindFirstValue("isAdmin"), out bool isAdmin);
            bool isMyId = HttpContext.User.FindFirstValue("id") == id;
            if (!isAdmin && !isMyId)
            {
                return Unauthorized("You can watch only your own profile");
            }

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
