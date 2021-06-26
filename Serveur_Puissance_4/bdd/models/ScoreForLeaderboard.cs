using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Serveur_Puissance_4.bdd {
    public class ScoreForLeaderboard {
        [BsonId]
        public ObjectId _id { get; set; }
        public ObjectId UserId { get; set; }
        public uint Score { get; set; }
        public DateTime Date { get; set; }

        //
        //
        //constructor
        //
        //
        public ScoreForLeaderboard(ObjectId id, ObjectId userId, uint score, DateTime date) {
            _id = id;
            UserId = userId;
            Score = score;
            Date = date;
        }
        public ScoreForLeaderboard(ObjectId userId, uint score) {
            _id = ObjectId.GenerateNewId();
            UserId = userId;
            Score = score;
            Date = DateTime.Now;
        }
    }
}