using MTCG.BLL.Exceptions;
using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL.Managers {
    public class CouponManager: ICouponManager {

        private readonly ICouponDao _couponDao;

        public CouponManager(ICouponDao couponDao) {
            _couponDao = couponDao;
        }

        public Coupon? GetCouponById(string id) {
            return _couponDao.GetCouponById(id) ?? throw new CouponNotFoundException();
        }

        public void InsertCoupon(Coupon coupon) {
            if(_couponDao.InsertCoupon(coupon) == false) {
                throw new DuplicateCouponException();
            }
        }

        public bool DeleteCouponById(string id) {
            return _couponDao.DeleteCouponById(id);
        }
    }
}
