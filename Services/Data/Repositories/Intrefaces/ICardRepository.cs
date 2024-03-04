using DotNetCardsServer.Models.Cards;

namespace DotNetCardsServer.Services.Data.Repositories.Intrefaces
{
    public interface ICardRepository
    {
        Task<Card> CreateCardAsync(Card newCard);
        Task<List<Card>> GetAllCardsAsync();
        Task<Card> GetOneCardAsync(string cardId);
        Task<bool> DeleteCardAsync(string cardId);
        Task<Card> EditCardAsync(string cardId, Card updatedCard);
        Task<List<Card>> GetMyCardsAsync(string userId);
        Task<bool> DeleteLike(string userId, string cardId);
        Task<bool> AddLike(string userId, string cardId);
    }
}
