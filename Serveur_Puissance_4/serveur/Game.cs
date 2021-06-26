using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Serveur_Puissance_4.game;
namespace Serveur_Puissance_4.serveur {

    public class Game {
        public Client FirstClient { get; set; }
        public Client SecondClient { get; set; }
        public Board Board { get; set; }
        public Boolean GameInProgress { get; set; }

        public Game(Client FirstClient, Client SecondClient) {

            this.FirstClient = FirstClient;
            this.SecondClient = SecondClient;

            this.Board = new Board();
            this.GameInProgress = true;

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            while (this.GameInProgress) {

                if (this.Board.Win() == 0) {
                    // savoir qui joue et qui attend
                    Client playingClient = this.getPlayingClient();

                    Client waitingClient = this.getOpponentClient();

                    //envoie des données en fonction de si le joueur attend ou joue
                    ServerResponse jsonObjectClientJoue = new ServerResponse("Success", playingClient.ToString(), playingClient.Color,
                        waitingClient.ToString(), waitingClient.Color, this.Board.ToString(), this.Board.getBoardForIa(), "It's your turn !", this.Board.getCompleteColumns());

                    ServerResponse jsonObjectClientAttente = new ServerResponse("Success", waitingClient.ToString(), waitingClient.Color,
                        playingClient.ToString(), playingClient.Color, this.Board.ToString(), this.Board.getBoardForIa(), $"Waiting for {playingClient}...", this.Board.getCompleteColumns());

                    this.Send(playingClient.WorkSocket, jsonObjectClientJoue.ToString());
                    this.Send(waitingClient.WorkSocket, jsonObjectClientAttente.ToString());

                    // récupérer colonne joué.
                    try {
                        playingClient.appendStringData(playingClient.WorkSocket.Receive(playingClient.Buffer));
                        int reponse = int.Parse(playingClient.StringData);
                        this.Board.Play(reponse);

                    } catch (Exception) {
                        this.SendError();
                        this.GameInProgress = false;
                    }

                    this.Board.changePlayer();
                    playingClient.clearStringData();
                    waitingClient.clearStringData();

                    if (this.Board.thereIsEquality()) {
                        this.SendEquality();
                        this.GameInProgress = false;
                    }

                } else {

                    this.SendWinner();
                    this.GameInProgress = false;
                }
            }
            ReceiverOfClients.NbrGame--;

            Console.WriteLine("\n\n\n===***===***===***===***===***===***===***===***===***===\n");
            Console.WriteLine($"            Number of games in progress : {ReceiverOfClients.NbrGame}");
            Console.WriteLine("\n===***===***===***===***===***===***===***===***===***===\n\n\n");

            //on coupe la connexion
            this.FirstClient.WorkSocket.Shutdown(SocketShutdown.Both);
            this.FirstClient.WorkSocket.Close();
            this.SecondClient.WorkSocket.Shutdown(SocketShutdown.Both);
            this.SecondClient.WorkSocket.Close();

        }

        public Client getPlayingClient() {
            return Board.WhoPlay == 1 ? this.FirstClient : this.SecondClient;
        }

        public Client getOpponentClient() {
            return Board.WhoPlay == 1 ? this.SecondClient : this.FirstClient;
        }

        private void Send(Socket socketClient, String data) {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.  
            socketClient.Send(byteData);
        }

        private void SendWinner() {
            Client winner = this.Board.Win() == 1 ? FirstClient : SecondClient;
            Client loser = this.Board.Win() == 1 ? SecondClient : FirstClient;

            String resultat = $"{winner.ToString().Replace("\n", String.Empty)} won !";
            ServerResponse obj = new ServerResponse("Success", this.Board.ToString(), resultat);
            // Convert the string data to byte data using UTF8 encoding.  

            byte[] byteData = Encoding.UTF8.GetBytes(obj.ToString());
            //Sending the data to the remote device.  
            this.FirstClient.WorkSocket.Send(byteData);
            this.SecondClient.WorkSocket.Send(byteData);

            if (FirstClient.CurrentClientResponse.Mode == "ranked") {
                winner.Win();
                loser.Lose();
            }
        }

        private void SendEquality() {
            ServerResponse obj = new ServerResponse("Success", this.Board.ToString(), "Tied ! You're the best ! ");
            // Convert the string data to byte data using UTF8 encoding.  

            byte[] byteData = Encoding.UTF8.GetBytes(obj.ToString());
            //Sending the data to the remote device.  
            this.FirstClient.WorkSocket.Send(byteData);
            this.SecondClient.WorkSocket.Send(byteData);
        }

        private void SendError() {
            ServerResponse obj = new ServerResponse("Error", this.Board.ToString(), "A player has disconnected. Please restart a game.");

            byte[] byteData = Encoding.UTF8.GetBytes(obj.ToString());
            // sending the data to the remote device.  
            try {
                this.FirstClient.WorkSocket.Send(byteData);
            } catch (Exception) {

                Console.WriteLine("j1 has disconnected");
            }

            try {
                this.SecondClient.WorkSocket.Send(byteData);
            } catch (Exception) {

                Console.WriteLine("j2 has disconnected");
            }

        }
    }
}