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
    public class ShowDeckCommand: AuthenticatedRouteCommand {
        private readonly ICardManager _cardManager;
        private readonly IDeckManager _deckManager;

        private readonly bool _plain;

        public ShowDeckCommand(ICardManager cardManager, IDeckManager deckManager, User identity, bool plain): base(identity) {
            _cardManager = cardManager;
            _deckManager = deckManager;
            _plain = plain;
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
                Card? card = _cardManager.GetCardById(obj.CId);
                if(card == null) {
                    throw new CardNotFoundException();
                }
                cards.Add(card);
            }
            if(_plain) {
                string str = "Deck of " + Identity.UserData.Displayname + "\n";
                foreach(var card in cards) {
                    str += "* " + card.Name + " - " + card.Damage + " Damage - " + card.Id + "\n";
                }
                response = new HttpResponse(StatusCode.Ok, str);
            } else {
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(cards));
            }
           
            return response;
        }
    }
}
