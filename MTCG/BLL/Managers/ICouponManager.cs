using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL.Managers {
    public interface ICouponManager {

        Coupon? GetCouponById(string id);
        void InsertCoupon(Coupon coupon);
        bool DeleteCouponById(string id);
    }
}
