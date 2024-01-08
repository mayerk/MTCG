using MTCG.API.Routing.Cards;
using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using System.Linq.Expressions;

namespace MTCG.API.Routing {
    internal class ChooseDeckCommand: AuthenticatedRouteCommand {
        private readonly IUserManager _userManager;
        private readonly ICardManager _cardManager;

        private readonly List<string> _cardIDs;
        public ChooseDeckCommand(IUserManager userManager, ICardManager cardManager, List<string> cardIDs, User identity) : base(identity) {
            _userManager = userManager;
            _cardManager = cardManager;
            _cardIDs = cardIDs;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            List<Card> cards = new List<Card>();

            foreach(string id in _cardIDs) {
                Card? card;
                try {
                    card = _cardManager.GetCardById(id);
                } catch(CardNotFoundException e) {
                    card = null;
                }

                if(card != null) {
                    if(card.UId != Identity.Id) {
                        response = new HttpResponse(StatusCode.Forbidden);
                        break;
                    }
                    cards.Add(card);
                } else {
                    response = new HttpResponse(StatusCode.Forbidden);
                    break;
                }
            }

            if(cards.Count == 4) {
                Identity.Deck = cards;
                response = new HttpResponse(StatusCode.Ok);
            } else {
                response = new HttpResponse(StatusCode.BadRequest);
            }
            return response;
        }
    }
}