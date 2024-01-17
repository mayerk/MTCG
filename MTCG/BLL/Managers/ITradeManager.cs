using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.BLL.Managers
{
    internal interface ITradeManager
    {

        List<Trade> GetAllTrades();
        Trade GetTradeById(string id);
        void CreateTradingDeal(Trade trade);
        void DeleteTradingDealById(string id);

    }
}
