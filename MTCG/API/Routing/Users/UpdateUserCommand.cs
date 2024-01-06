using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Users {
    internal class UpdateUserCommand: AuthenticatedRouteCommand {

        private readonly IUserManager _userManager;
        private readonly UserData _userData;

        public UpdateUserCommand(IUserManager userManager, User identity, UserData userData) : base(identity) {
            _userManager = userManager;
            _userData = userData;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            Identity.UserData = _userData;
            try {
                _userManager.UpdateUser(Identity);
                response = new HttpResponse(StatusCode.Ok);
            } catch (UserNotFoundException) {
                response = new HttpResponse(StatusCode.NotFound);
            }

            return response;
        }
    }
}
