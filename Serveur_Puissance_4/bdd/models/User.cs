using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Serveur_Puissance_4.bdd {
    public class User {
        [BsonId]
        public ObjectId _id { get; set; }
        public String Nickname { get; set; }
        public String Password { get; set; }

        //
        //
        //constructor
        //
        //
        public User(ObjectId id, String nickname, String password, uint lastScore) {
            _id = id;
            Nickname = nickname;
            Password = password;
        }
        public User(String nickname, String password) {
            _id = ObjectId.GenerateNewId();
            Nickname = nickname;
            Password = password;
        }
    }
}