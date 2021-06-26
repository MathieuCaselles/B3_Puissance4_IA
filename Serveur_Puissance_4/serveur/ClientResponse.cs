using System;
using Newtonsoft.Json.Linq;

namespace Serveur_Puissance_4.serveur {
    public class ClientResponse {
        public String Mode { get; set; } = "";
        public String UserId { get; set; } = "";
        public String Nickname { get; set; } = "";
        public String Password { get; set; } = "";
        public String NicknameSearch { get; set; } = "";
        public ushort NbMax { get; set; } = 0;

        //
        //
        //constructor
        //
        //
        public ClientResponse(String mode, String id, String nickname, String password, String nicknameSearch, ushort nbrMaxLeaderboard) {
            Mode = mode;
            UserId = id;
            Nickname = nickname;
            Password = password;
            NicknameSearch = nicknameSearch;
            NbMax = nbrMaxLeaderboard;
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