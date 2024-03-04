using DotNetCardsServer.Models.Cards;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DotNetCardsServer.Services.Data.Repositories.Cards
{
    public class CardRepositoryMongoDb
    {
        private IMongoCollection<Card> _cards;

        public CardRepositoryMongoDb(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("dotnet_business_card_app");
            _cards = database.GetCollection<Card>("cards");
        }
        public async Task<Card> CreateCardAsync(Card newCard) {
            try
            {
                await _cards.InsertOneAsync(newCard);
                return newCard;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Card>> GetAllCardsAsync() {
            return await _cards.Find(_ => true).ToListAsync();
        }
        public async Task<Card> GetOneCardAsync(string cardId) {
            var card = await _cards.Find<Card>(c => c.Id == new ObjectId(cardId)).FirstOrDefaultAsync();
            return card;
        }
        public async Task<bool> DeleteCardAsync(string cardId) {

            var result = await _cards.DeleteOneAsync(c => c.Id == new ObjectId(cardId));
            return (result.DeletedCount > 0);
            
        
        }
        public async Task<Card> EditCardAsync(string cardId, Card updatedCard) {
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
                return null;
            }
            return updatedCard;
        }
        public async Task<List<Card>> GetMyCardsAsync(string userId)
        {
            return await _cards.Find(card => card.User_Id == userId).ToListAsync();
        }
        public async Task<bool> DeleteLike(string userId, string cardId) {

            var update = Builders<Card>.Update.Pull(c => c.Likes, userId);
            var result = await _cards.UpdateOneAsync(c => c.Id == new ObjectId( cardId), update);
            return (result.MatchedCount > 0);
           
        }
        public async Task<bool> AddLike(string userId, string cardId) {

            var update = Builders<Card>.Update.Push(c => c.Likes, userId);
            var result = await _cards.UpdateOneAsync(c => c.Id == new ObjectId(cardId), update);
            return (result.MatchedCount > 0);
        }
    }
}
