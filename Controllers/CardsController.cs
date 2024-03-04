using DotNetCardsServer.Interfaces;
using DotNetCardsServer.Models.Cards;
using DotNetCardsServer.Services.Cards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DotNetCardsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {
        private readonly ICardsService _cardsService;

        public CardsController(ICardsService cardsService)
        {
            _cardsService = cardsService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCards()
        {
            List<Card> cards = await _cardsService.GetAllCardsAsync();
            return Ok(cards);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCard(string id)
        {
            try
            {
                Card card = await _cardsService.GetCardByIdAsync(id);
                return Ok(card);
            }
            catch (Exception e) 
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "MustBeBusinessOrAdmin")]
        public async Task<IActionResult> CreateCard([FromBody] Card newCard)
        {
            var claims = HttpContext.User.Claims;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Card createdCard = await _cardsService.CreateCardAsync(newCard);
            return CreatedAtAction(nameof(GetCard), new { id = createdCard.Id }, createdCard);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCard(string id, [FromBody] Card updatedCard)
        {
            bool.TryParse(HttpContext.User.FindFirstValue("isAdmin"), out bool isAdmin);
            if (!await _cardsService.IsOwner(id, HttpContext.User.FindFirstValue("id") ?? "") && !isAdmin)
            {
                return Unauthorized("You can only edit your own cards");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _cardsService.UpdateCardAsync(id, updatedCard);
                return NoContent();
            }
            catch (Exception e) 
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(string id)
        {
            bool.TryParse(HttpContext.User.FindFirstValue("isAdmin"), out bool isAdmin);
            if (!await _cardsService.IsOwner(id, HttpContext.User.FindFirstValue("id") ?? "") && !isAdmin)
            {
                return Unauthorized("You can only delete your own cards");
            }
            try
            {
                await _cardsService.DeleteCardAsync(id);
                return NoContent();
            }
            catch (Exception e) 
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("my-cards")]
        public async Task<IActionResult> GetMyCards()
        {
            string userId = HttpContext.User.FindFirstValue("Id") ?? "";
            List<Card> cards = await _cardsService.GetMyCardsAsync(userId);
            return Ok(cards);
        }

        [HttpPatch("{cardId}")]
        public async Task<IActionResult> LikeCard(string cardId)
        {
            string userId = HttpContext.User.FindFirstValue("Id") ?? "";

            try
            {
                await _cardsService.LikeCardAsync(cardId, userId);
                return NoContent();
            }
            catch (Exception e) 
            {
                return NotFound(e.Message);
            }
        }
    }
}
