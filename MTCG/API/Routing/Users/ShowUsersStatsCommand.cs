﻿using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.API.Routing.Users
{
    internal class ShowUsersStatsCommand : AuthenticatedRouteCommand
    {
        private IUserManager _userManager;

        public ShowUsersStatsCommand(IUserManager userManager, User identity): base(identity) 
        {
            _userManager = userManager;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            User user = _userManager.GetUserByUsername(Identity.Username);
            UserStats stats = new(user.Username, user.Elo, user.Wins, user.Losses);
            response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(stats));
            return response;
        }
    }
}