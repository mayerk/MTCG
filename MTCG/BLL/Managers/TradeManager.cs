using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.BLL.Exceptions;
using MTCG.DAL;
using MTCG.Models;

namespace MTCG.BLL.Managers
{
    internal class TradeManager : ITradeManager
    {

        private readonly ITradeDao _tradeDao;
        public TradeManager(ITradeDao tradeDao)
        {
            _tradeDao = tradeDao;
        }

        public List<Trade> GetAllTrades()
        {
            return _tradeDao.GetAllTrades();
        }

        public Trade GetTradeById(string id)
        {
            return _tradeDao.GetTradeById(id);
        }

        public void CreateTradingDeal(Trade trade)
        {
            if (_tradeDao.CreateTradingDeal(trade) == false)
            {
                throw new DuplicateTradeException();
            }
        }

        public void DeleteTradingDealById(string id)
        {
            if (_tradeDao.DeleteTradingDeal(id) == false)
            {
                throw new TradeNotFoundException();
            }
        }
    }
}
