using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Exceptions;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.API.Routing.Users
{
    public class UserByUsernameCommand: AuthenticatedRouteCommand {

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
                response = new HttpResponse(StatusCode.NotFound);
            } else {
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(user.UserData));
            }

            return response;
        }
    }
}
