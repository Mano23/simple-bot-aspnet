using MongoDB.Bson;
using MongoDB.Driver;

namespace SimpleBot.Repo
{
    public class UserMongoRepo : IUserRepo
    {
        public UserProfile GetProfile(string id)
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var db = cliente.GetDatabase("bot");
            var col = db.GetCollection<UserProfile>("perfil");

            var builder = Builders<UserProfile>.Filter;
            var perfil = col.Find(builder.Eq("Id", id)).SingleOrDefault();
            if (perfil == null)
                perfil = new UserProfile()
                {
                    Id = id,
                    Visitas = 0
                };
            return perfil;
        }

        public void SalvarHistorico(Message message)
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var db = cliente.GetDatabase("bot");
            var col = db.GetCollection<BsonDocument>("historico");

            var bson = new BsonDocument()
            {
                { "User", message.User },
                { "Text", message.Text }
            };

            col.InsertOne(bson);
        }

        public void SetProfile(string id, UserProfile profile)
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var db = cliente.GetDatabase("bot");
            var col = db.GetCollection<UserProfile>("perfil");

            col.ReplaceOne(e => e.Id == id, profile, new UpdateOptions() { IsUpsert = true });
        }
    }
}