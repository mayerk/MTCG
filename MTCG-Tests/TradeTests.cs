using MTCG.BLL.Managers;
using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Tests {
    public class TradeTests {

        private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

        private ITradeDao _tradeDao = new DatabaseTradeDao(connectionString);
        private ITradeManager _tradeManager;

        private readonly string _tradeId = Guid.NewGuid().ToString();

        [SetUp]
        public void Setup() {
            _tradeManager = new TradeManager(_tradeDao);
        }

        [Test][Order(0)]
        public void Test_CreateTradingDeal() {
            Trade trade = new(_tradeId, "2272ba48-6662-404d-a9a1-41a9bed316d9", "monster", 15);
            _tradeManager.CreateTradingDeal(trade);
            Trade? tmp = _tradeManager.GetTradeById(trade.Id);
            if(tmp == null) { 
                Assert.Fail("Could not retreive trade with Id '" + trade.Id + "'.");
            }
            Assert.That(tmp.Id, Is.EqualTo(trade.Id));
        }

        [Test][Order(1)]
        public void Test_GetTradeById() {
            Trade? trade = _tradeManager.GetTradeById(_tradeId);
            if (trade == null) {
                Assert.Fail("Could not retreive trade with Id '" + _tradeId + "'.");
                return;
            }
            Assert.That(trade.Id, Is.EqualTo(_tradeId));
        }

        [Test][Order(2)]
        public void Test_GetAllTrades() {
            List<Trade> trades = _tradeManager.GetAllTrades();
            if(!trades.Any()) {
                Assert.Fail("No trades available.");
            }
            Assert.That(trades.Count, Is.EqualTo(1));
        }

        [Test][Order(3)]
        public void Test_DeleteTradingDeal() {
            _tradeManager.DeleteTradingDealById(_tradeId);
            Trade? trade = _tradeManager.GetTradeById(_tradeId);
            Assert.That(trade, Is.Null);
        }
    }
}
