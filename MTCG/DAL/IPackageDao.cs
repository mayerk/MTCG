using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    internal interface IPackageDao {
        bool InsertPackage(Package package);
        Package? GetFirstPackage();
        Package? GetPackageById(string id);

        List<Package> GetPackagesByPId(string pid);
        void DeletePackage(string pid);
    }
}
