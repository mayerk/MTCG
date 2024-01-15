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
        private readonly ICardManager _cardManager;
        private readonly IDeckManager _deckManager;

        public ShowDeckCommand(ICardManager cardManager, IDeckManager deckManager, User identity): base(identity) {
            _cardManager = cardManager;
            _deckManager = deckManager;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            List<Deck> deck = _deckManager.GetDeckByUId(Identity.Id);
            List<Card> cards = new List<Card>();

            if(!deck.Any()) {
                response = new HttpResponse(StatusCode.NoContent);
                return response;
            }
            foreach(var obj in deck) {
                cards.Add(_cardManager.GetCardById(obj.CId));
            }
            response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(cards));

            return response;
        }
    }
}
