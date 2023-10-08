using api.Data;
using api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserServices(IOptions<DatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.Connection);
            var mongoDb = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _userCollection = mongoDb.GetCollection<User>("users");
        }

        // add new user
        public async Task CreateAsync(User newUser) => await _userCollection.InsertOneAsync(newUser);

        // get all users
        public async Task<List<User>> GetAsync() => await _userCollection.Find(_ => true).ToListAsync();

        // get user by id
        public async Task<User> GetAsync(String id) => await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // update user
        public async Task UpdateAsync(string id, User updateUser) => await _userCollection.ReplaceOneAsync(x => x.Id == id, updateUser);

        // delete user
        public async Task RemoveAsync(String id) => await _userCollection.DeleteOneAsync(x => x.Id == id);
    }
}
