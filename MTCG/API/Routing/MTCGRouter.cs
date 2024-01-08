using Newtonsoft.Json;
using MTCG.API.Routing.Users;
using MTCG.BLL;
using MTCG.HttpServer;
using MTCG.HttpServer.Request;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpMethod = MTCG.HttpServer.Request.HttpMethod;
using MTCG.API.Routing.Cards;
using MTCG.API.Routing.Packages;

namespace MTCG.API.Routing
{
    internal class MTCGRouter : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly ICardManager _cardManager;
        private readonly IPackageManager _packageManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public MTCGRouter(IUserManager userManager, ICardManager cardManager, IPackageManager packageManager)
        {
            _userManager = userManager;
            _cardManager = cardManager;
            _packageManager = packageManager;
            _identityProvider = new IdentityProvider(userManager);
            _routeParser = new IdRouteParser();
        }

        public IRouteCommand? Resolve(HttpRequest request)
        {
            var isMatch = (string path) => _routeParser.IsMatch(path, "/users/{id}");
            var isUsername = (string path) => _routeParser.isUsername(path, "/cards/{username}");
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

                    { Method: HttpMethod.Post, ResourcePath: "/transactions/packages" } => new AquirePackageCommand(_packageManager, _cardManager, GetIdentity(request)),
                    
                    { Method: HttpMethod.Get, ResourcePath: "/cards" } => new ShowUserCardsCommand(_cardManager, GetIdentity(request)),
                    
                    { Method: HttpMethod.Get, ResourcePath: "/deck" } => new ShowDeckCommand(_userManager, GetIdentity(request)),
                    { Method: HttpMethod.Put, ResourcePath: "/deck" } => new ChooseDeckCommand(_userManager, _cardManager, DeserializeIDs(request.Payload), GetIdentity(request)), 
                    
                    { Method: HttpMethod.Get, ResourcePath: "/stats" } => new ShowUsersStatsCommand(_userManager, GetIdentity(request)),
                    //{ Method: HttpMethod.Get, ResourcePath: var path } when isMatch(path) => new ShowMessageCommand(_messageManager, GetIdentity(request), parseId(path)),
                    //{ Method: HttpMethod.Put, ResourcePath: var path } when isMatch(path) => new UpdateMessageCommand(_messageManager, GetIdentity(request), parseId(path), checkBody(request.Payload)),
                    //{ Method: HttpMethod.Delete, ResourcePath: var path } when isMatch(path) => new RemoveMessageCommand(_messageManager, GetIdentity(request), parseId(path)),

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

        private User GetIdentity(HttpRequest request)
        {
            return _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
        }

        private User ValidateIdentity(HttpRequest request, string path) {
            return _identityProvider.ValidateIdentity(request, path) ?? throw new RouteNotAuthenticatedException();
        }
    }
}
