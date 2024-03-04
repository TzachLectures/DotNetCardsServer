using DotNetCardsServer.Interfaces;
using DotNetCardsServer.Models.Cards;
using DotNetCardsServer.Services.Data.Repositories.Intrefaces;
using MongoDB.Driver;

namespace DotNetCardsServer.Services.Cards
{
    public class CardsService : ICardsService
    {
        private readonly ICardRepository _cardRepository;

        public CardsService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public async Task<Card> CreateCardAsync(Card newCard)
        {
            return await _cardRepository.CreateCardAsync(newCard);
        }

        public async Task<List<Card>> GetAllCardsAsync()
        {
            return await _cardRepository.GetAllCardsAsync();
        }

        public async Task<Card> GetCardByIdAsync(string cardId)
        {
            var card = await _cardRepository.GetOneCardAsync(cardId);
            if (card == null)
            {
                throw new Exception("Card not found");
            }
            return card;
        }

        public async Task<Card> UpdateCardAsync(string cardId, Card updatedCard)
        {
            var card = await _cardRepository.EditCardAsync(cardId, updatedCard);
            if (card == null)
            {
               
                    throw new Exception("Card not found");
                
            }
            return card;

        }
        public async Task DeleteCardAsync(string cardId)
        {
            bool result = await _cardRepository.DeleteCardAsync(cardId);
            if (!result)
            {
                throw new Exception("Card not found");
            }
        }

        public async Task<List<Card>> GetMyCardsAsync(string userId)
        {
            return await _cardRepository.GetMyCardsAsync(userId);
        }

        public async Task LikeCardAsync(string cardId, string userId)
        {
            var card = await _cardRepository.GetOneCardAsync(cardId);

            bool result;
            if(card.Likes.Contains(userId))
            {
                result= await _cardRepository.DeleteLike(userId, cardId);
            }
            else
            {
                result= await _cardRepository.AddLike(userId, cardId);

            }
            if (!result)
            {
                throw new Exception("Card not found");
            }
        }
        public async Task<bool> IsOwner(string cardId, string userId)
        {
            var card = await _cardRepository.GetOneCardAsync(cardId);
            return card.User_Id == userId;
        }
    }
    
    
}
