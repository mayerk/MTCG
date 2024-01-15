using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using System.Linq.Expressions;

namespace MTCG.API.Routing.Cards
{
    internal class ConfigureDeckCommand : AuthenticatedRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly IDeckManager _deckManager;

        private readonly List<string> _cardIDs;
        public ConfigureDeckCommand(ICardManager cardManager, IDeckManager deckManager, List<string> cardIDs, User identity) : base(identity)
        {
            _cardManager = cardManager;
            _deckManager = deckManager;
            _cardIDs = cardIDs;
        }

        public override HttpResponse Execute()
        {
            HttpResponse response;
            List<Card> cards = new List<Card>();

            foreach (string id in _cardIDs) {
                Card? card;
                try {
                    card = _cardManager.GetCardById(id);
                }
                catch (CardNotFoundException e) {
                    card = null;
                }

                if (card != null) {
                    if (card.UId != Identity.Id)
                    {
                        response = new HttpResponse(StatusCode.Forbidden);
                        break;
                    }
                    cards.Add(card);
                } else {
                    response = new HttpResponse(StatusCode.Forbidden);
                    break;
                }
            }

            if (cards.Count == 4) {
                _deckManager.ConfigureDeck(cards);
                response = new HttpResponse(StatusCode.Ok);
            }
            else {
                response = new HttpResponse(StatusCode.BadRequest);
            }
            return response;
        }
    }
}