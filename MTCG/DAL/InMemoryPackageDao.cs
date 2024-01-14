﻿using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    internal class InMemoryPackageDao: IPackageDao {
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
        public void DeletePackage(string pid) {
            Package? package = GetPackageById(pid);
            if (package != null) {
                _packages.Remove(package);
            }
        }

        public List<Package> GetPackagesByPId(string pid) {
            throw new NotImplementedException();
        }
    }
}
