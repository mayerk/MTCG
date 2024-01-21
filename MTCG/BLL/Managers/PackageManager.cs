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
    public class PackageManager : IPackageManager
    {

        private readonly IPackageDao _packageDao;
        public PackageManager(IPackageDao packageDao)
        {
            _packageDao = packageDao;
        }
        public void CreatePackage(Card[] cards)
        {
            Package package = new(cards);
            _packageDao.InsertPackage(package);
        }
        public Package GetFirstPackage()
        {
            Package? package = _packageDao.GetFirstPackage();
            int i = 0;
            if (package == null)
            {
                throw new NoPackageAvailableException();
            }
            List<Package> packages = _packageDao.GetPackagesByPId(package.PId);
            foreach (Package p in packages)
            {
                package.Cards[i] = new(p.tmpCId);
                ++i;
            }
            return package;
        }
        public void DeletePackage(string pid)
        {
            _packageDao.DeletePackage(pid);
        }
    }
}
