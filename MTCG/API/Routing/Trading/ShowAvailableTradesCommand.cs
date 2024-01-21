using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Response;
using MTCG.Models;
using Newtonsoft.Json;

namespace MTCG.API.Routing.Trading
{
    public class ShowAvailableTradesCommand: AuthenticatedRouteCommand
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