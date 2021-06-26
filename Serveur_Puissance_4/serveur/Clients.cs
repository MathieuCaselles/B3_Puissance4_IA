using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Serveur_Puissance_4.serveur {
    public class Clients : List<Client> {

        public override String ToString() {
            String result = "";
            foreach ((Client client, int index) in this.Select((client, index) => (client, index))) {
                result += $"Index:{index} - Nickname:{client.Nickname} - NicknameSearch:{client.NicknameSearch} \n";
            }
            return result;
        }

        public List<Client> getTwoFirstClients() {
            return new List<Client> { this [0], this [1] };
        }

        public Client getClientByNickname(String nicknameClient, String nicknameSearch) {
            return this.Find(client => client.Nickname == nicknameSearch &&
                client.NicknameSearch == nicknameClient &&
                client.NicknameSearch != client.Nickname) ?? null;
        }

        public Client getClientByScore(Client currentClient) {
            return this.Find(targetClient => targetClient.Score <= (currentClient.Score + currentClient.MargeSearchRanked) &&
                targetClient.Score >= (currentClient.Score - currentClient.MargeSearchRanked) &&
                targetClient.Nickname != currentClient.Nickname) ?? null;
        }

    }
}