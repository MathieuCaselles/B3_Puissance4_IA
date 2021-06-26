using System;
using System.Net.Sockets;
using System.Text;
using MongoDB.Bson;
using Newtonsoft.Json;
using Serveur_Puissance_4.bdd;

namespace Serveur_Puissance_4.serveur {
    public class Client {
        // Client  socket.  
        public Socket WorkSocket { get; set; } = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] Buffer { get; set; } = new byte[BufferSize];
        // Received data string.  
        private StringBuilder _sb = new StringBuilder();
        public String StringData {
            get { return _sb.ToString(); }
        }

        public ObjectId _id { get; set; }
        public string Nickname { get; set; }
        public string NicknameSearch { get; set; }

        public string Color { get; set; }
        public uint Score { get; set; }
        public uint MargeSearchRanked { get; set; } = 5;

        public ClientResponse CurrentClientResponse { get; set; }

        //
        //
        //constructor
        //
        //
        public Client(Socket workSocket) {
            WorkSocket = workSocket;
        }

        //
        //
        //methodes
        //
        //
        public override String ToString() {
            return Nickname;
        }
        public void Win() {
            Redis.setUserScore(this, true);
            Bdd.AddUserScore(this, true);
        }
        public void Lose() {
            Redis.setUserScore(this, false);
            Bdd.AddUserScore(this, false);
        }

        public void UpdateScore() {
            Score = Redis.getScoreOfuser(Nickname);
        }

        public void sendResponseToClient(ServerResponse responseToSend) {
            WorkSocket.Send(Encoding.UTF8.GetBytes(responseToSend.ToString()));
        }
        public void appendStringData(int bytesRead) {
            _sb.Append(Encoding.UTF8.GetString(
                Buffer, 0, bytesRead));
        }

        public void clearStringData() {
            _sb.Clear();
        }

        public void readClientResponse(int clientResponse) {
            StringBuilder reader = new StringBuilder();

            reader.Append(Encoding.UTF8.GetString(
                Buffer, 0, clientResponse));

            CurrentClientResponse = JsonConvert.DeserializeObject<ClientResponse>(reader.ToString());

            Nickname = CurrentClientResponse.Nickname;
            NicknameSearch = CurrentClientResponse.NicknameSearch;
            if (CurrentClientResponse.UserId != "")
                _id = ObjectId.Parse(CurrentClientResponse.UserId);
        }

        public bool IsSocketConnected() {
            // this.WorkSocket.Poll returns true if 
            // connection is closed, reset, terminated or pending(meaning no active connection)
            // connection is active and there is data available
            bool part1 = this.WorkSocket.Poll(1000, SelectMode.SelectRead);

            // this.WorkSocket.Available returns number of bytes available
            bool part2 = this.WorkSocket.Available == 0;
            if ((part1 && part2) || !this.WorkSocket.Connected) {
                // there is no data available to read so connection is not active
                this.WorkSocket.Shutdown(SocketShutdown.Both);
                this.WorkSocket.Close();
                return false;
            } else
                return true;
        }
    }
}