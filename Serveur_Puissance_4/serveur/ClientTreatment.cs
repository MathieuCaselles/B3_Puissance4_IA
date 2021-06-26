using System;
using System.Net.Sockets;
using System.Text;
using Serveur_Puissance_4.bdd;

namespace Serveur_Puissance_4.serveur {
    public class ClientTreatment {

        public static void StartTreatmentUser(Socket clientSokcet) {

            try {
                //création objet client
                Client client = new Client(clientSokcet);

                try {
                    Console.WriteLine($"Waiting player response...");
                    client.readClientResponse(clientSokcet.Receive(client.Buffer));
                    Console.WriteLine($"player response = {client.CurrentClientResponse.ToString()}");
                } catch (System.Exception) {
                    Console.WriteLine($"The player has disconnected");
                    return;
                }

                switch (client.CurrentClientResponse.Mode) {
                    case "quick":
                        QuickMatchmaking.AddClient(client);
                        break;
                    case "nickname":
                        NicknameMatchmaking.AddClient(client);
                        break;
                    case "ranked":
                        RankedMatchmaking.AddClient(client);
                        break;
                    case "login":
                        {
                            try {
                                String idFound = Bdd.LogIn(client.CurrentClientResponse.Nickname, client.CurrentClientResponse.Password);
                                ServerResponse idResponse = new ServerResponse("Success", idFound);
                                client.sendResponseToClient(idResponse);
                            } catch (System.Exception) {
                                Console.WriteLine($"Error login: No User Found with this nickname and password");
                                ServerResponse errorResponse = new ServerResponse("Error", "No User Found with this nickname and password");
                                client.sendResponseToClient(errorResponse);
                            }
                            break;
                        }
                    case "register":
                        {
                            if (!Bdd.UserExist(client.Nickname)) {
                                String idFound = Bdd.Register(client.Nickname, client.CurrentClientResponse.Password);
                                ServerResponse idResponse = new ServerResponse("Success", idFound);
                                client.sendResponseToClient(idResponse);
                            } else {
                                ServerResponse errorResponse = new ServerResponse("Error", "This user already exists");
                                client.sendResponseToClient(errorResponse);
                            }
                            break;
                        }
                    case "userInformations":
                        {
                            ServerResponse successResponse = new ServerResponse("Success", Redis.getUserInformation(client.Nickname));
                            client.sendResponseToClient(successResponse);
                            break;
                        }
                    case "leaderboard":
                        {
                            ServerResponse successResponse = new ServerResponse("Success", Redis.getTopLeaderboard(client.CurrentClientResponse.NbMax));
                            client.sendResponseToClient(successResponse);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine($"Error mode : {client.CurrentClientResponse.Mode} doesn't exist. Shutdown socket player.");
                            ServerResponse errorResponse = new ServerResponse("Error", $"Mode : {client.CurrentClientResponse.Mode} doesn't exist. Shutdown socket player.");
                            client.sendResponseToClient(errorResponse);
                            client.WorkSocket.Shutdown(SocketShutdown.Both);
                            client.WorkSocket.Close();
                            break;
                        }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }
    }
}