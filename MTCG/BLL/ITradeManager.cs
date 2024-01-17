using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL {
    internal interface ITradeManager {

        List<Trade> GetAllTrades();
        Trade GetTradeById(string id);
        void CreateTradingDeal(Trade trade);
        void DeleteTradingDealById(string id);

    }
}
