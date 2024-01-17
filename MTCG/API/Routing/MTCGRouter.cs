using Newtonsoft.Json;
using MTCG.API.Routing.Users;
using MTCG.HttpServer;
using MTCG.HttpServer.Request;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using HttpMethod = MTCG.HttpServer.Request.HttpMethod;
using MTCG.API.Routing.Cards;
using MTCG.API.Routing.Packages;
using MTCG.API.Routing.Trading;
using MTCG.BLL.Managers;

namespace MTCG.API.Routing
{
    internal class MTCGRouter : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly ICardManager _cardManager;
        private readonly IPackageManager _packageManager;
        private readonly IDeckManager _deckManager;
        private readonly ITradeManager _tradeManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public MTCGRouter(IUserManager userManager, ICardManager cardManager, IPackageManager packageManager, IDeckManager deckManager, ITradeManager tradeManager)
        {
            _userManager = userManager;
            _cardManager = cardManager;
            _packageManager = packageManager;
            _deckManager = deckManager;
            _tradeManager = tradeManager;
            _identityProvider = new IdentityProvider(userManager);
            _routeParser = new IdRouteParser();
        }

        public IRouteCommand? Resolve(HttpRequest request)
        {
            var isMatch = (string path) => _routeParser.IsMatch(path, "/users/{id}");
            var isUsername = (string path) => _routeParser.isUsername(path, "/cards/{username}");
            var isTrade = (string path) => _routeParser.isTrade(path, "/tradings/{tradingdealid}");
            //var userMatchesToken = (string path, User user) => _identityProvider.ParsedUserMatchesToken(path, user);
            var parseId = (string path) => int.Parse(_routeParser.ParseParameters(path, "/messages/{id}")["id"]);
            var checkBody = (string? payload) => payload ?? throw new InvalidDataException();

            try
            {
                return request switch
                {
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)), 
                    { Method: HttpMethod.Get, ResourcePath: var path } when isMatch(path) => new UserByUsernameCommand(_userManager, ValidateIdentity(request, path)), 
                    { Method: HttpMethod.Put, ResourcePath: var path } when isMatch(path) => new UpdateUserCommand(_userManager, ValidateIdentity(request, path), Deserialize<UserData>(request.Payload)), 

                    { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)), 
                    
                    { Method: HttpMethod.Post, ResourcePath: "/packages" } => new CreatePackageCommand(_packageManager, _cardManager, Deserialize<Card[]>(request.Payload), GetIdentity(request)),

                    { Method: HttpMethod.Post, ResourcePath: "/transactions/packages" } => new AquirePackageCommand(_packageManager, _cardManager, _userManager, GetIdentity(request)),
                    
                    { Method: HttpMethod.Get, ResourcePath: "/cards" } => new ShowUserCardsCommand(_cardManager, GetIdentity(request)),
                    
                    { Method: HttpMethod.Get, ResourcePath: "/deck" } => new ShowDeckCommand(_cardManager, _deckManager, GetIdentity(request)),
                    { Method: HttpMethod.Put, ResourcePath: "/deck" } => new ConfigureDeckCommand(_cardManager, _deckManager, DeserializeIDs(request.Payload), GetIdentity(request)), 
                    
                    { Method: HttpMethod.Get, ResourcePath: "/stats" } => new ShowUsersStatsCommand(_userManager, GetIdentity(request)), 
                    
                    { Method: HttpMethod.Get, ResourcePath: "/scoreboard" } => new ShowScoreboardCommand(_userManager, GetIdentity(request)), 
                    
                    { Method: HttpMethod.Get, ResourcePath: "/tradings" } => new ShowAvailableTradesCommand(_tradeManager, GetIdentity(request)), 
                    { Method: HttpMethod.Post, ResourcePath: "/tradings" } => new CreateTradingDealCommand(_tradeManager, _cardManager, _deckManager, GetIdentity(request), Deserialize<Trade>(request.Payload)), 
                    { Method: HttpMethod.Delete, ResourcePath: var path } when isTrade(path) => new DeleteTradingDealCommand(_tradeManager, _cardManager, GetIdentity(request), GetParameterFromRequest(path)), 
                    { Method: HttpMethod.Post, ResourcePath: var path } when isTrade(path) => new ProcessTradingDealCommand(_tradeManager, _cardManager, _deckManager, GetIdentity(request), GetParameterFromRequest(path), DeserializeString(request.Payload)),

                    _ => null
                };
            }
            catch(InvalidDataException)
            {
                return null;
            }            
        }

        private T Deserialize<T>(string? body) where T : class
        {
            var data = body is not null ? JsonConvert.DeserializeObject<T>(body) : null;
            return data ?? throw new InvalidDataException();
        }

        private List<string> DeserializeIDs(string? body) {
            var data = body is not null ? JsonConvert.DeserializeObject<List<string>>(body) : null;
            return data ?? throw new InvalidDataException();
        }

        private string DeserializeString(string? body) {
            var data = body is not null ? JsonConvert.DeserializeObject<string>(body) : null;
            return data ?? throw new InvalidDataException();
        }

        private User GetIdentity(HttpRequest request)
        {
            return _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
        }

        private User ValidateIdentity(HttpRequest request, string path) {
            return _identityProvider.ValidateIdentity(request, path) ?? throw new RouteNotAuthenticatedException();
        }

        private string GetParameterFromRequest(string path) {
            return path.Substring(path.LastIndexOf("/") + 1);
        }
    }
}
