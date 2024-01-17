using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    internal interface ITradeDao {

        List<Trade> GetAllTrades();
        Trade GetTradeById(string id);
        bool CreateTradingDeal(Trade trade);
        bool DeleteTradingDeal(string id);
    }
}
