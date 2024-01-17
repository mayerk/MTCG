using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL {
    internal interface IPackageDao {
        bool InsertPackage(Package package);
        Package? GetFirstPackage();
        Package? GetPackageById(string id);

        List<Package> GetPackagesByPId(string pid);
        bool DeletePackage(string pid);
    }
}
