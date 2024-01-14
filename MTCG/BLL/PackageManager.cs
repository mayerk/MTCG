using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL {
    internal class PackageManager: IPackageManager {

        private readonly IPackageDao _packageDao;
        public PackageManager(IPackageDao packageDao) {
            _packageDao = packageDao;
        }
        public void CreatePackage(Card[] cards) {
            Package package = new(cards);
            _packageDao.InsertPackage(package);
        }
        public Package GetFirstPackage() {
            return _packageDao.GetFirstPackage() ?? throw new NoPackageAvailableException();
        }
        public void DeletePackage(string pid) {
            _packageDao.DeletePackage(pid);
        }
    }
}
