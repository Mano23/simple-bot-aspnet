﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBot
{
    public class SimpleBotUser
    {
        static Dictionary<string, UserProfile> _perfil = new Dictionary<string, UserProfile>();
        public static string Reply(Message message)
        {
            string userId = message.Id;

            var perfil = GetProfile(userId);

            perfil.Visitas += 1;

            SetProfile(userId, perfil);

            return $"{message.User} conversou '{perfil.Visitas}'";
        }

        public static string SalvarHistorico(Message message)
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

            return $"{message.User} disse '{message.Text}'";
        }

        public static UserProfile GetProfile(string id)
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

        public static void SetProfile(string id, UserProfile profile)
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var db = cliente.GetDatabase("bot");
            var col = db.GetCollection<UserProfile>("perfil");

            col.ReplaceOne(e => e.Id == id, profile, new UpdateOptions() {IsUpsert = true });
        }
    }
}