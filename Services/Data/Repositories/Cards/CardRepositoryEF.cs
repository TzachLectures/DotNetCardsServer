using DotNetCardsServer.Models.Cards;
using DotNetCardsServer.Services.Data.Repositories.Intrefaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DotNetCardsServer.Services.Data.Repositories.Cards
{
    public class CardRepositoryEF: ICardRepository
    {
        private readonly ApplicationDbContext _context;
        public CardRepositoryEF(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Card> CreateCardAsync(Card newCard)
        {
            Console.WriteLine("test");
            var cardSqlModel = new CardSqlModel(newCard);

            _context.Cards.Add(cardSqlModel);
            await _context.SaveChangesAsync();
            return new Card(cardSqlModel);
        }
        public async Task<List<Card>> GetAllCardsAsync()
        {
            var cardSqlModels = await _context.Cards.ToListAsync();
            Dictionary<string, List<string>> likesByCardId = await _context.CardLikes.GroupBy(like => like.Card_Id)
                                                            .ToDictionaryAsync(group => group.Key, group => group.Select((like) => like.User_Id).ToList());

            return cardSqlModels.Select(cardSqlModel => new Card(cardSqlModel,
                                likesByCardId.GetValueOrDefault(cardSqlModel.Id, new List<string>()))).ToList();
        }
        public async Task<Card> GetOneCardAsync(string cardId)
        {
            var cardSqlModel = await _context.Cards.FindAsync(cardId);
            if (cardSqlModel == null) return null;
            List<string> likesOfThisCard = await _context.CardLikes.Where((like) => like.Card_Id == cardId).Select((like) => like.User_Id).ToListAsync();
            return new Card(cardSqlModel, likesOfThisCard);
        }
        public async Task<bool> DeleteCardAsync(string cardId)
        {
            var cardSqlModel = await _context.Cards.FindAsync(cardId);
            if (cardSqlModel == null) return false;
            _context.Cards.Remove(cardSqlModel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Card> EditCardAsync(string cardId, Card updatedCard)
        {
            var cardSqlModel = await _context.Cards.FindAsync(cardId);
            if (cardSqlModel == null) return null;
            cardSqlModel.Title = updatedCard.Title;
            cardSqlModel.Subtitle = updatedCard.Subtitle;
            cardSqlModel.Description = updatedCard.Description;
            cardSqlModel.Phone = updatedCard.Phone;
            cardSqlModel.Email = updatedCard.Email;
            cardSqlModel.Web = updatedCard.Web;
            cardSqlModel.ImageUrl = updatedCard.Image?.Url;
            cardSqlModel.ImageAlt = updatedCard.Image?.Alt;
            cardSqlModel.State = updatedCard.Address?.State;
            cardSqlModel.Country = updatedCard.Address?.Country;
            cardSqlModel.City = updatedCard.Address?.City;
            cardSqlModel.Street = updatedCard.Address?.Street;
            cardSqlModel.HouseNumber = updatedCard.Address?.HouseNumber ?? 0;
            cardSqlModel.Zip = updatedCard.Address?.Zip ?? 0;
            await _context.SaveChangesAsync();

            List<string> likesOfThisCard = await _context.CardLikes.Where((like) => like.Card_Id == cardId).Select((like) => like.User_Id).ToListAsync();

            return new Card(cardSqlModel, likesOfThisCard);
        }

        public async Task<List<Card>> GetMyCardsAsync(string userId)
        {
            var cardSqlModels = await _context.Cards.Where(c => c.User_Id == userId).ToListAsync();
            Dictionary<string, List<string>> likesByCardId = await _context.CardLikes
                                                            .GroupBy(like => like.Card_Id)
                                                            .ToDictionaryAsync(group => group.Key, group => group.Select((like) => like.User_Id).ToList());

            return cardSqlModels.Select(cardSqlModel => new Card(cardSqlModel,
                                        likesByCardId.GetValueOrDefault(cardSqlModel.Id, new List<string>()))).ToList();
        }

        public async Task<bool> DeleteLike(string userId, string cardId)
        {
            UserCardLike? cardLike = await _context.CardLikes.FirstOrDefaultAsync(like => like.Card_Id == cardId && like.User_Id == userId);

            if (cardLike == null) return false;
            _context.CardLikes.Remove(cardLike);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> AddLike(string userId, string cardId) 
        {
            UserCardLike? cardLike = await _context.CardLikes.FirstOrDefaultAsync(like => like.Card_Id == cardId && like.User_Id == userId);
            if (cardLike == null)
            {
                UserCardLike userCardLike = new UserCardLike { Card_Id=cardId, User_Id=userId};
                _context.CardLikes.Add(userCardLike);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
