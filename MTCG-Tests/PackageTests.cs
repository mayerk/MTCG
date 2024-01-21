using MTCG.BLL.Managers;
using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Tests {
    public class PackageTests {

        private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

        private IPackageDao _packageDao = new DatabasePackageDao(connectionString);
        private IPackageManager _packageManager;

        private readonly string _packageId = Guid.NewGuid().ToString();

        [SetUp]
        public void Setup() {
            _packageManager = new PackageManager(_packageDao);
        }

        [Test][Order(0)]
        public void Test_CreatePackage() {
            List<Card> cards = new List<Card> {
                new("2272ba48-6662-404d-a9a1-41a9bed316d9", "WaterGoblin", 10.0),
                new("3871d45b-b630-4a0d-8bc6-a5fc56b6a043", "Dragon", 70.0),
                new("166c1fd5-4dcb-41a8-91cb-f45dcd57cef3", "Knight", 22.0),
                new("237dbaef-49e3-4c23-b64b-abf5c087b276", "WaterSpell", 40.0),
                new("27051a20-8580-43ff-a473-e986b52f297a", "FireElf", 28.0)
            };
            _packageManager.CreatePackage(cards.ToArray());

            Package? package = _packageManager.GetFirstPackage();
            if(package == null) {
                Assert.Fail("No package available.");
                return;
            }
            Assert.IsTrue(packageContainsCards(cards, package));
        }

        private bool packageContainsCards(List<Card> cards, Package package) {
            List<string> cardIDs = new List<string>();
            foreach(Card card in cards) {
                cardIDs.Add(card.Id);
            }
            foreach(var card in package.Cards) { 
                if(!cardIDs.Contains(card.Id)) {
                    return false;
                }
            }
            return true;
        }

        [Test][Order(1)]
        public void Test_DeletePackage() {
            Package? package = _packageDao.GetFirstPackage();
            if( package == null ) {
                Assert.Fail("No package available.");
                return;
            }
            _packageDao.DeletePackage(package.PId);
            package = _packageDao.GetFirstPackage();
            Assert.That(package, Is.Null);
        }

    }
}
