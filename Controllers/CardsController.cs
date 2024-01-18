using DotNetCardsServer.Models.Cards;
using DotNetCardsServer.Services.Cards;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace DotNetCardsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardsService _cardsService;

        public CardsController(IMongoClient mongoClient)
        {
            _cardsService = new CardsService(mongoClient);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            List<Card> cards = await _cardsService.GetAllCardsAsync();
            return Ok(cards);
        }

        [HttpGet("{id}")]
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
        public async Task<IActionResult> CreateCard([FromBody] Card newCard)
        {
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

        [HttpGet("my-cards/{userId}")]
        public async Task<IActionResult> GetMyCards(string userId)
        {
            List<Card> cards = await _cardsService.GetMyCardsAsync(userId);
            return Ok(cards);
        }

        [HttpPatch("{cardId}/like/{userId}")]
        public async Task<IActionResult> LikeCard(string cardId, string userId)
        {
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
