﻿using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL {
    public interface ICouponDao {

        Coupon? GetCouponById(string id);
        bool InsertCoupon(Coupon coupon);
        bool DeleteCouponById(string id);
    }
}
