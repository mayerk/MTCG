using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Users {
    internal class UserByUsernameCommand: AuthenticatedRouteCommand {

        private readonly IUserManager _userManager;

        public UserByUsernameCommand(IUserManager userManager, User identity): base(identity) {
            _userManager = userManager;
        }

        public override HttpResponse Execute() {
            User? user;
            try {
                user = _userManager.GetUserByUsername(Identity.Username);
            } catch (UserNotFoundException) {
                user = null;
            }

            HttpResponse response;
            if (user == null) {
                response = new HttpResponse(StatusCode.Unauthorized);
            } else {
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(user.UserData));
            }

            return response;
        }
    }
}
