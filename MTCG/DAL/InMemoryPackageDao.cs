using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.DAL {
    public class InMemoryPackageDao: IPackageDao {
        private readonly List<Package> _packages = new();

        public bool InsertPackage(Package package) {
            _packages.Add(package);
            return true;
        }
        public Package? GetFirstPackage() {
            return _packages.FirstOrDefault();
        }
        public Package? GetPackageById(string id) {
            return _packages.FirstOrDefault(p => p.Id == id);
        }
        public bool DeletePackage(string pid) {
            Package? package = GetPackageById(pid);
            if (package != null) {
                _packages.Remove(package);
            }
            return true;
        }

        public List<Package> GetPackagesByPId(string pid) {
            throw new NotImplementedException();
        }
    }
}
