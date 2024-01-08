using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Users {
    internal class ShowDeckCommand: AuthenticatedRouteCommand {
        private readonly IUserManager _userManager;
        public ShowDeckCommand(IUserManager userManager, User identity): base(identity) {
            _userManager = userManager;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            List<Card> deck = _userManager.GetDeckByAuthToken(Identity.Token);
            if(!deck.Any() ) {
                response = new HttpResponse(StatusCode.NoContent);
            } else {
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(deck));
            }
            return response;
        }
    }
}
