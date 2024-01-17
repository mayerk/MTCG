using MTCG.API.Routing.Users;
using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.API.Routing.Trading
{
    internal class ShowAvailableTradesCommand: AuthenticatedRouteCommand
    {
        private readonly ITradeManager _tradeManager;

        public ShowAvailableTradesCommand(ITradeManager tradeManager, User identity): base(identity) {
            _tradeManager = tradeManager;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            List<Trade> trades = _tradeManager.GetAllTrades();
            if(!trades.Any()) {
                response = new HttpResponse(StatusCode.NoContent);
            }
            else {
                response = new HttpResponse(StatusCode.Ok, JsonConvert.SerializeObject(trades));
            }
           
            return response;
        }
    }
}