using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL {
    internal interface IPackageManager {
        void CreatePackage(Card[] cards);
        Package GetFirstPackage();
        void DeletePackage(string pid);
    }
}
