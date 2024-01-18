using DotNetCardsServer.Models.Cards;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DotNetCardsServer.Services.Cards
{
    public class CardsService
    {
        private readonly IMongoCollection<Card> _cards;

        public CardsService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("dotnet_business_card_app");
            _cards = database.GetCollection<Card>("cards");
        }

        public async Task<Card> CreateCardAsync(Card newCard)
        {
            await _cards.InsertOneAsync(newCard);
            return newCard;
        }

        public async Task<List<Card>> GetAllCardsAsync()
        {
            return await _cards.Find(_ => true).ToListAsync();
        }

        public async Task<Card> GetCardByIdAsync(string cardId)
        {
            var card = await _cards.Find<Card>(c => c.Id == new ObjectId(cardId)).FirstOrDefaultAsync();
            if (card == null)
            {
                throw new Exception("Card not found"); 
            }
            return card;
        }

        public async Task<Card> UpdateCardAsync(string cardId, Card updatedCard)
        {
            var filter = Builders<Card>.Filter.Eq(u => u.Id, new ObjectId(cardId));
            var update = Builders<Card>.Update
                .Set(c => c.Title, updatedCard.Title)
                .Set(c => c.Subtitle, updatedCard.Subtitle)
                .Set(c => c.Description, updatedCard.Description)
                .Set(c => c.Phone, updatedCard.Phone)
                .Set(c => c.Email, updatedCard.Email)
                .Set(c => c.Web, updatedCard.Web)
                .Set(c => c.Image, updatedCard.Image)
                .Set(c => c.Address, updatedCard.Address);
            var result = await _cards.UpdateOneAsync(filter, update);
            // Check if the update was successful
            if (result.MatchedCount == 0)
            {
                throw new Exception("Card not found");
            }
            return updatedCard;
        }
        public async Task DeleteCardAsync(string cardId)
        {
            var result = await _cards.DeleteOneAsync(c => c.Id == new ObjectId(cardId));
            if (result.DeletedCount == 0)
            {
                throw new Exception("Card not found");
            }
        }

        public async Task<List<Card>> GetMyCardsAsync(string userId)
        {
            return await _cards.Find(card => card.User_Id == userId).ToListAsync();
        }

        public async Task LikeCardAsync(string cardId, string userId)
        {
            var card = await GetCardByIdAsync(cardId);
            var update = card.Likes.Contains(userId)
                ? Builders<Card>.Update.Pull(c => c.Likes, userId)
                : Builders<Card>.Update.Push(c => c.Likes, userId);

            var result = await _cards.UpdateOneAsync(c => c.Id == card.Id, update);
            if (result.MatchedCount == 0)
            {
                throw new Exception("Card not found"); 
            }
        }
    }
}
