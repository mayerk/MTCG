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
    internal class DeleteTradingDealCommand: AuthenticatedRouteCommand
    {
        private readonly ITradeManager _tradeManager;
        private readonly ICardManager _cardManager;

        private readonly string _tradeId;

        public DeleteTradingDealCommand(ITradeManager tradeManager, ICardManager cardManager, User identity, string id): base(identity) {
            _tradeManager = tradeManager;
            _cardManager = cardManager;
            _tradeId = id;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            Trade? trade = _tradeManager.GetTradeById(_tradeId);
            if(trade == null) {
                response = new HttpResponse(StatusCode.NotFound);
                return response;
            }
            try {
                Card? card = _cardManager.GetCardById(trade.CId);
                if(card.UId != Identity.Id) {
                    response = new HttpResponse(StatusCode.Forbidden);
                    return response;
                }
                _tradeManager.DeleteTradingDealById(_tradeId);
                response = new HttpResponse(StatusCode.Ok);
            } catch (CardNotFoundException) {
                response = new HttpResponse(StatusCode.NotFound);
            } catch(TradeNotFoundException) {
                response = new HttpResponse(StatusCode.NotFound);
            }
            return response;
        }
    }
}