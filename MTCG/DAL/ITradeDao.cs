using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL {
    internal interface ITradeDao {

        List<Trade> GetAllTrades();
        Trade GetTradeById(string id);
        bool CreateTradingDeal(Trade trade);
        bool DeleteTradingDeal(string id);
    }
}
