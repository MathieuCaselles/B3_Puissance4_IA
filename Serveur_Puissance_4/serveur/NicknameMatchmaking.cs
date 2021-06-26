using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Serveur_Puissance_4.serveur {
    public class NicknameMatchmaking {
        public static Clients waitingClients = new Clients();

        public static void AddClient(Client client) {
            Console.WriteLine($"{client.Nickname} has been sent for matchmaking by pseudo");

            Client clientFound = waitingClients.getClientByNickname(client.Nickname, client.NicknameSearch);
            if (clientFound != null) {
                if (clientFound.IsSocketConnected()) {
                    clientFound.Color = "RED";
                    client.Color = "YELLOW";

                    //Evite d'enboyer null en paramètre dans la tâche qui se lance trop tard
                    Task.Run(() => {
                        new Game(clientFound, client);
                    });

                    waitingClients.Remove(clientFound);

                    ReceiverOfClients.NbrGame++;
                    Console.WriteLine("\n\n\n===***===***===***===***===***===***===***===***===***===\n");
                    Console.WriteLine($"             Number of games in progress : {ReceiverOfClients.NbrGame} ");
                    Console.WriteLine("\n===***===***===***===***===***===***===***===***===***===\n\n\n");
                } else {
                    waitingClients.Remove(clientFound);
                }
            } else {
                Console.WriteLine($"{client.Nickname} is in the queue");

                //signale au joueur d'attendre un adversaire
                ServerResponse sayWaitingToclient = new ServerResponse("Success", $"Waiting for opponent : {client.NicknameSearch} ...");
                client.sendResponseToClient(sayWaitingToclient);
                waitingClients.Add(client);
            }
        }
    }
}