using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.API.Routing.Users
{
    internal class ShowScoreboardCommand: AuthenticatedRouteCommand {

        private readonly IUserManager _userManager;
        public ShowScoreboardCommand(IUserManager userManager, User identity) : base(identity) {
            _userManager = userManager;
        }

        public override HttpResponse Execute() {
            List<User> users = _userManager.GetScoreboard();
            List<UserStats> stats = new List<UserStats>();
            foreach(User user in users) {
                stats.Add(new(user.UserData.Displayname, user.Elo, user.Wins, user.Losses));
            }
            HttpResponse response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(stats));
            return response;
        }
    }
}