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
    internal class ProcessTradingDealCommand: AuthenticatedRouteCommand
    {
        private readonly ITradeManager _tradeManager;
        private readonly ICardManager _cardManager;
        private readonly IDeckManager _deckManager;

        private readonly string _tradeId;
        private readonly string _cardToTrade;

        public ProcessTradingDealCommand(ITradeManager tradeManager, ICardManager cardManager, IDeckManager deckManager, User identity, string tradeId, string cardId): base(identity)
        {
            _tradeManager = tradeManager;
            _cardManager = cardManager;
            _deckManager = deckManager;
            _tradeId = tradeId;
            _cardToTrade = cardId;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
             Trade? trade = _tradeManager.GetTradeById(_tradeId);
            if (trade == null) {
                response = new HttpResponse(StatusCode.NotFound);
                return response;
            }
            Card? cardInTradeDeal, offeredCard;
            try {
                cardInTradeDeal = _cardManager.GetCardById(trade.CId);
                offeredCard = _cardManager.GetCardById(_cardToTrade);
            } catch(CardNotFoundException) {
                response = new HttpResponse(StatusCode.NotFound);
                return response;
            }
            Deck? deck = _deckManager.GetDeckByCId(offeredCard.Id);
            
            if(offeredCard.UId != Identity.Id || !meetsRequirements(cardInTradeDeal, offeredCard, trade) || deck != null || cardInTradeDeal.UId == Identity.Id) {
                response = new HttpResponse(StatusCode.Forbidden);
                return response;
            }
            string tmpUId = cardInTradeDeal.UId;
            cardInTradeDeal.UId = offeredCard.UId;
            offeredCard.UId = tmpUId;

            _cardManager.UpdateCardUId(cardInTradeDeal);
            _cardManager.UpdateCardUId(offeredCard);
            _tradeManager.DeleteTradingDealById(trade.Id);

            response = new HttpResponse(StatusCode.Ok);
            return response;
        }

        private bool meetsRequirements(Card cardInTradeDeal, Card offeredCard, Trade trade) {
            return (cardInTradeDeal.CardSpecification.cardType == offeredCard.CardSpecification.cardType) && offeredCard.Damage >= trade.MinimumDamage;
        }
    }
}