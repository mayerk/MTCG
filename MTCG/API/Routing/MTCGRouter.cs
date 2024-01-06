﻿using Newtonsoft.Json;
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

namespace MTCG.API.Routing
{
    internal class MTCGRouter : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public MTCGRouter(IUserManager userManager)
        {
            _userManager = userManager;
            _identityProvider = new IdentityProvider(userManager);
            _routeParser = new IdRouteParser();
        }

        public IRouteCommand? Resolve(HttpRequest request)
        {
            var isMatch = (string path) => _routeParser.IsMatch(path, "/messages/{id}");
            var isUsername = (string path) => _routeParser.isUsername(path, "users/{username}");
            var parseId = (string path) => int.Parse(_routeParser.ParseParameters(path, "/messages/{id}")["id"]);
            var checkBody = (string? payload) => payload ?? throw new InvalidDataException();

            try
            {
                return request switch
                {
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)), 
                    { Method: HttpMethod.Get, ResourcePath: var path } when isUsername(path) => new UserByUsernameCommand(_userManager, GetIdentity(request)), 
                    { Method: HttpMethod.Put, ResourcePath: var path } when isUsername(path) => new UpdateUserCommand(_userManager, GetIdentity(request), Deserialize<UserData>(request.Payload)), 

                    { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),

                    //{ Method: HttpMethod.Post, ResourcePath: "/messages" } => new AddMessageCommand(_messageManager, GetIdentity(request), checkBody(request.Payload)),
                    //{ Method: HttpMethod.Get, ResourcePath: "/messages" } => new ListMessagesCommand(_messageManager, GetIdentity(request)),

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

        private User GetIdentity(HttpRequest request)
        {
            return _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
        }
    }
}