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
        public static string Reply(Message message)
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var db = cliente.GetDatabase("bot");
            var bson = new BsonDocument()
            {
                { "id", message.Id },
                { "user", message.User },
                { "text", message.Text }
            };
            var col = db.GetCollection<BsonDocument>("message");
            col.InsertOne(bson);
            return $"{message.User} disse '{message.Text}'";
        }

        public static UserProfile GetProfile(string id)
        {
            return null;
        }

        public static void SetProfile(string id, UserProfile profile)
        {
        }
    }
}