using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serveur_Puissance_4.serveur {
    public class ReceiverOfClients {
        public static int NbrGame { get; set; } = 0;

        public static void StartServer() {
            // Establish the local endpoint for the socket.  
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try {
                listener.Bind(localEndPoint);
                listener.Listen(100);
                Console.WriteLine("Matchmaking server start.");

                Console.WriteLine("\n\n\n===***===***===***===***===***===***===***===***===***===\n");
                Console.WriteLine($"             Number of games in progress : {NbrGame} ");
                Console.WriteLine("\n===***===***===***===***===***===***===***===***===***===\n\n\n");

                while (true) {
                    //attente d'un joueur
                    Socket clientSokcet = listener.Accept();
                    Console.WriteLine("A player is connected");
                    Task.Run(() => {
                        ClientTreatment.StartTreatmentUser(clientSokcet);
                    });
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
    }
}