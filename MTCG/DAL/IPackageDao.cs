using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    internal interface IPackageDao {
        void CreatePackage(Package package);
        Package? GetFirstPackage();
        Package? GetPackageById(string id);
        void DeletePackage(string pid);
    }
}
