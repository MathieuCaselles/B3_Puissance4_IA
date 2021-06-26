using System.Threading.Tasks;
using Serveur_Puissance_4.bdd;
using Serveur_Puissance_4.serveur;

namespace Serveur_Puissance_4 {
    class Program {
        static void Main(string[] args) {
            new Bdd();
            new Redis();
            Task.Run(() => {
                new RankedMatchmaking();
            });
            Task.Run(() => {
                new QuickMatchmaking();
            });

            ReceiverOfClients.StartServer();
        }
    }
}