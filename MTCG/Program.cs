using MTCG.API.Routing;
using MTCG.BLL;
using MTCG.BLL.Managers;
using MTCG.DAL;
using MTCG.Models;
using Npgsql;
using System.Net;

namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

            IUserDao userDao = new DatabaseUserDao(connectionString);
            ICardDao cardDao = new DatabaseCardDao(connectionString);
            IPackageDao packageDao = new DatabasePackageDao(connectionString);
            IDeckDao deckDao = new DatabaseDeckDao(connectionString);
            ITradeDao tradeDao = new DatabaseTradeDao(connectionString);

            IUserManager userManager = new UserManager(userDao);
            ICardManager cardManager = new CardManager(cardDao);
            IPackageManager packageManager = new PackageManager(packageDao);
            IDeckManager deckManager = new DeckManager(deckDao);
            ITradeManager tradeManager = new TradeManager(tradeDao);
            IGameManager gameManager = new GameManager();

            var router = new MTCGRouter(userManager, cardManager, packageManager, deckManager, tradeManager, gameManager);
            var server = new HttpServer.HttpServer(router, IPAddress.Any, 10001);
            server.Start();            
        }
    }
}