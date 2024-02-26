using DotNetCardsServer.Models.Cards;

namespace DotNetCardsServer.Interfaces
{
    public interface ICardsService
    {
        Task<Card> CreateCardAsync(Card newCard);
        Task<List<Card>> GetAllCardsAsync();
        Task<Card> GetCardByIdAsync(string cardId);
        Task<Card> UpdateCardAsync(string cardId, Card updatedCard);
        Task DeleteCardAsync(string cardId);
        Task<List<Card>> GetMyCardsAsync(string userId);
        Task LikeCardAsync(string cardId, string userId);
        Task<bool> IsOwner(string cardId, string userId);
    }
}
