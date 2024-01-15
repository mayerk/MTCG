using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.API.Routing.Users
{
    internal class ShowScoreBoardCommand: AuthenticatedRouteCommand {

        private readonly IUserManager _userManager;
        public ShowScoreBoardCommand(IUserManager userManager, User identity) : base(identity) {
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