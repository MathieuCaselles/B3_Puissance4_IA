using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Serveur_Puissance_4.serveur {
    public class RankedMatchmaking {
        public static Clients waitingClients = new Clients();
        public RankedMatchmaking() {
            while (true) {
                if (waitingClients.Count > 1) {
                    foreach (Client client in waitingClients) {

                        Client clientFound = waitingClients.getClientByScore(client);
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
                            //signale au joueur d'attendre un adversaire
                            if (client.MargeSearchRanked >= 50) {
                                ServerResponse errorResponse = new ServerResponse("Error", $"No opponent of your level has been found.");
                                client.sendResponseToClient(errorResponse);
                                client.WorkSocket.Shutdown(SocketShutdown.Both);
                                client.WorkSocket.Close();
                            } else {
                                client.MargeSearchRanked += 5;
                            }
                        }

                    }
                }
                Thread.Sleep(10000); //10sec
            }
        }

        public static void AddClient(Client client) {
            Console.WriteLine($"{client.Nickname} has been sent for ranked matchmaking");

            client.UpdateScore();

            Client clientFound = waitingClients.getClientByScore(client);
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
                ServerResponse sayWaitingToclient = new ServerResponse("Success", $"Waiting for opponent de votre niveau ...");
                client.sendResponseToClient(sayWaitingToclient);
                waitingClients.Add(client);
            }
        }
    }
}