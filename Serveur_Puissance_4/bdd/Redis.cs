using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using Serveur_Puissance_4.serveur;
using StackExchange.Redis;

namespace Serveur_Puissance_4.bdd {
    public class Redis {
        public static IDatabase Database { get; set; }
        public static String usersKeyHash { get; set; } = "user";
        public static String usersIdKeyList { get; set; } = "usersId";
        public static String leaderboardKey { get; set; } = "leaderboard";
        public Redis() {
            ConfigurationOptions options = ConfigurationOptions.Parse("<ip>:<port>,password=<yourPassword>");
            options.AllowAdmin = true;
            ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(options);

            EndPoint[] endpoint = muxer.GetEndPoints();
            IServer server = muxer.GetServer(endpoint[0]);
            server.FlushDatabase();

            Database = muxer.GetDatabase();

            InitializeDatabase();
        }

        private void InitializeDatabase() {
            Console.WriteLine("Initialiaze user...");
            InitializeUsers();
            Console.WriteLine("Initialiaze leaderboard...");
            InitializeLeaderboard();
        }

        private void InitializeUsers() {
            List<User> users = Bdd.GetAllUsers();

            foreach (User user in users) {
                Database.ListRightPush(usersIdKeyList, user._id.ToString());
                HashEntry[] newUser = {
                    new HashEntry("Nickname", user.Nickname),
                    new HashEntry("Password", user.Password),
                };

                Database.HashSet($"{usersKeyHash}:{user._id}", newUser);
            }
        }

        private void InitializeLeaderboard() {
            List<ScoreForLeaderboard> scores = Bdd.GetAllLastScores();

            foreach (ScoreForLeaderboard score in scores) {
                String nickname = Database.HashGet($"{usersKeyHash}:{score.UserId}", "Nickname");
                Database.SortedSetAdd(leaderboardKey, nickname, score.Score);
            }
        }

        public static SortedSetEntry[] getTopLeaderboard(int nbrMax) {
            return Database.SortedSetRangeByScoreWithScores(leaderboardKey, 0, double.PositiveInfinity, Exclude.None, Order.Descending, 0, nbrMax);
        }

        public static String getUserInformation(String nickname) {
            return$"{ Database.SortedSetRank(leaderboardKey, nickname, Order.Descending) + 1} {Database.SortedSetScore(leaderboardKey, nickname)}";
        }

        public static uint getScoreOfuser(String nickname) {
            return (ushort) Database.SortedSetScore(leaderboardKey, nickname);
        }

        public static void setUserScore(Client user, Boolean userWin) {
            if (userWin) {
                Database.SortedSetIncrement(leaderboardKey, user.Nickname, 1);
            } else {
                if (user.Score > 0) {
                    Database.SortedSetDecrement(leaderboardKey, user.Nickname, 1);
                }
            }
        }

        public static void addNewUser(User user) {
            Database.ListRightPush(usersIdKeyList, user._id.ToString());
            HashEntry[] newUser = {
                new HashEntry("Nickname", user.Nickname),
                new HashEntry("Password", user.Password),
            };

            Database.HashSet($"{usersKeyHash}:{user._id}", newUser);

            Database.SortedSetAdd(leaderboardKey, user.Nickname, 1000);
        }

    }
}