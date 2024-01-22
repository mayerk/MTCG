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
    public class ShowScoreboardCommand: AuthenticatedRouteCommand {

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
            string scoreboard = "";
            if (stats.Count > 0) {
                int i = 1;
                foreach (var stat in stats) {
                    scoreboard += i.ToString() + ") " + stat.Name + " - Elo: " + stat.Elo.ToString() + " - Wins: " + stat.Wins.ToString() + " - Losses: " + stat.Losses.ToString() + "\n";
                    ++i;
                }
            }
            
            HttpResponse response = new HttpResponse(StatusCode.Ok, scoreboard);
            return response;
        }
    }
}