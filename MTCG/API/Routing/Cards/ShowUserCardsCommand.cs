using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Cards {
    internal class ShowUserCardsCommand: AuthenticatedRouteCommand {

        ICardManager _cardManager;
        public ShowUserCardsCommand(ICardManager cardManager, User identity): base(identity) {
            _cardManager = cardManager;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            List<Card> cards = _cardManager.GetAllUsersCards(Identity.Id);
            if(!cards.Any()) {
                response = new HttpResponse(StatusCode.NoContent);
                return response;
            }
            response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(cards));
            return response;
        }
    }
}
