using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Serveur_Puissance_4.serveur;

namespace Serveur_Puissance_4.bdd {
    public class Bdd {
        public static IMongoDatabase Database { get; set; }
        public static IMongoCollection<ScoreForLeaderboard> Leaderboard { get; set; }
        public static IMongoCollection<User> Users { get; set; }
        public Bdd() {

            MongoClient dbClient = new MongoClient("<your login link>");

            Database = dbClient.GetDatabase("inferbodi");
            Leaderboard = Database.GetCollection<ScoreForLeaderboard>("leaderboardPuissance4");
            Users = Database.GetCollection<User>("usersPuissance4");
        }

        public static String Register(String nickname, String password) {
            User newUser = new User(nickname, password);
            Users.InsertOne(newUser);

            User userAdd = Users.Find(user => user.Nickname == nickname && user.Password == password).First();

            ScoreForLeaderboard newScoreObject = new ScoreForLeaderboard(userAdd._id, 1000);
            Leaderboard.InsertOne(newScoreObject);
            Redis.addNewUser(userAdd);

            return userAdd._id.ToString();
        }
        public static String LogIn(String nickname, String password) {
            try {
                return Users.Find(user => user.Nickname == nickname && user.Password == password).First()._id.ToString();
            } catch (InvalidOperationException) {
                throw new InvalidOperationException("User Not Found");
            }
        }
        public static User GetUserById(String id) {
            try {
                return Users.Find(user => user._id == ObjectId.Parse(id)).First();
            } catch (InvalidOperationException) {
                throw new InvalidOperationException("User Not Found");
            }
        }

        public static Boolean UserExist(String nickname) {
            try {
                Users.Find(user => user.Nickname == nickname).First();
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        public static void AddUserScore(Client user, Boolean userWin) {
            uint newScore = userWin ? user.Score + 1 : user.Score == 0 ? 0 : user.Score - 1;
            ScoreForLeaderboard newScoreObject = new ScoreForLeaderboard(user._id, newScore);
            Leaderboard.InsertOne(newScoreObject);
        }

        public static List<User> GetAllUsers() {
            return Users.Find(_ => true).ToList();
        }
        public static List<ScoreForLeaderboard> GetAllScores() {
            return Leaderboard.Find(_ => true).ToList();
        }

        public static List<ScoreForLeaderboard> GetAllLastScores() {
            List<User> users = GetAllUsers();

            List<ScoreForLeaderboard> result = new List<ScoreForLeaderboard>();
            SortDefinition<ScoreForLeaderboard> sort = Builders<ScoreForLeaderboard>.Sort.Descending("Date");
            foreach (User user in users) {
                FilterDefinition<ScoreForLeaderboard> filter = Builders<ScoreForLeaderboard>.Filter.Eq("UserId", user._id);
                try {
                    result.Add(Leaderboard.Find(filter).Sort(sort).First());
                } catch (System.Exception) {
                    Console.WriteLine($"No score for {user._id}   ---   ({user.Nickname})");
                }
            }
            return result;
        }

    }
}