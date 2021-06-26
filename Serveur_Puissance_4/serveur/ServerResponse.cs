using System;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Serveur_Puissance_4.serveur {
    public class ServerResponse {
        public String Status { get; set; } = "";
        public String PlayerName { get; set; } = "";
        public String PlayerColor { get; set; } = "";
        public String OpponentName { get; set; } = "";
        public String OpponentColor { get; set; } = "";
        public String Board { get; set; } = "";
        public ushort[][] BoardIa { get; set; }
        public String Message { get; set; } = "";
        public SortedSetEntry[] Leaderboard { get; set; }
        public short[] CompleteColumns { get; set; } = new short[7] {-1, -1, -1, -1, -1, -1, -1 };

        //
        //
        //constructor
        //
        //
        public ServerResponse(String status, String playerName, String playerColor, String opponentName, String opponentColor,
            String board, ushort[][] boardIa, String message, short[] completeColumns) {
            Status = status;
            PlayerName = playerName;
            PlayerColor = playerColor;
            OpponentName = opponentName;

            OpponentColor = opponentColor;
            Board = board;
            BoardIa = boardIa;
            Message = message;
            CompleteColumns = completeColumns;

        }

        public ServerResponse(String status, String message) {
            Status = status;
            Message = message;
        }
        public ServerResponse(String status, SortedSetEntry[] leaderboard) {
            Status = status;
            Leaderboard = leaderboard;
        }

        public ServerResponse(String status, String board, String message) {
            Status = status;
            Board = board;
            Message = message;
        }

        //
        //
        //methodes
        //
        //
        public override string ToString() {
            JObject objectToJson = (JObject) JToken.FromObject(this);
            return objectToJson.ToString();
        }
    }
}