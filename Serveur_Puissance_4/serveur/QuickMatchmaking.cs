using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
namespace Serveur_Puissance_4.serveur {
    public class QuickMatchmaking {
        public static Clients waitingClients = new Clients();

        public QuickMatchmaking() {
            while (true) {
                if (waitingClients.Count > 1) {

                    List<Client> clientsFound = waitingClients.getTwoFirstClients();

                    Client p1 = clientsFound[0];
                    Client p2 = clientsFound[1];
                    if (!p1.IsSocketConnected()) {
                        waitingClients.Remove(p1);
                        continue;
                    }
                    if (!p2.IsSocketConnected()) {
                        waitingClients.Remove(p2);
                        continue;
                    }

                    p1.Color = "RED";
                    p2.Color = "YELLOW";

                    //Evite d'enboyer null en paramètre dans la tâche qui se lance trop tard
                    Task.Run(() => {
                        new Game(p1, p2);
                    });

                    waitingClients.Remove(p1);
                    waitingClients.Remove(p2);

                    ReceiverOfClients.NbrGame++;
                    Console.WriteLine("\n\n\n===***===***===***===***===***===***===***===***===***===\n");
                    Console.WriteLine($"             Number of games in progress : {ReceiverOfClients.NbrGame} ");
                    Console.WriteLine("\n===***===***===***===***===***===***===***===***===***===\n\n\n");

                }
                Thread.Sleep(1); //1ms

            }
        }

        public static void AddClient(Client client) {
            Console.WriteLine($"{client.Nickname} has been sent for quick matchmaking");
            //signale au joueur d'attendre un adversaire
            ServerResponse sayWaitingToclient = new ServerResponse("Success", "Waiting for opponent...");
            client.sendResponseToClient(sayWaitingToclient);
            waitingClients.Add(client);
        }
    }
}