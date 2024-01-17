using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.BLL.Managers
{
    internal interface IPackageManager
    {
        void CreatePackage(Card[] cards);
        Package GetFirstPackage();
        void DeletePackage(string pid);

        Package? GetPackageById(string id);
    }
}
