using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Exceptions;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Response;
using MTCG.Models;

namespace MTCG.API.Routing.Trading
{
    public class CreateTradingDealCommand: AuthenticatedRouteCommand
    {
        private readonly ITradeManager _tradeManager;
        private readonly ICardManager _cardManager;
        private readonly IDeckManager _deckManager;

        private readonly Trade _trade;

        public CreateTradingDealCommand(ITradeManager tradeManager, ICardManager cardManager, IDeckManager deckManager, User identity, Trade trade): base(identity)
        {
            _tradeManager = tradeManager;
            _cardManager = cardManager;
            _deckManager = deckManager;
            _trade = trade;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            try {
                Card? card = _cardManager.GetCardById(_trade.CId);
                Deck? deck = _deckManager.GetDeckByCId(_trade.CId);
                if(card.UId != Identity.Id || deck != null) {
                    response = new HttpResponse(StatusCode.Forbidden);
                    return response;
                }
                _tradeManager.CreateTradingDeal(_trade);
                response = new HttpResponse(StatusCode.Created);
            } catch(CardNotFoundException) {
                response = new HttpResponse(StatusCode.NotFound);
            } catch(DuplicateTradeException) {
                response = new HttpResponse(StatusCode.Conflict);
            }
            return response;            
        }
    }
}