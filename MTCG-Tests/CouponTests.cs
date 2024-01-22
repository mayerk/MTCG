using MTCG.BLL.Managers;
using MTCG.DAL;
using MTCG.Models;
using MTCG.BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Tests {
    internal class CouponTests {

        private static readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

        ICouponDao _couponDao = new DatabaseCouponDao(connectionString);
        ICouponManager _couponManager;
        
        private readonly string _couponId = Guid.NewGuid().ToString();

        [SetUp] 
        public void Setup() {
            _couponManager = new CouponManager(_couponDao);
        }

        [Test][Order(0)]
        public void Test_InsertCoupon() {
            Coupon coupon = new(_couponId, 20);
            _couponManager.InsertCoupon(coupon);
            Coupon? vest = _couponManager.GetCouponById(_couponId);
            if(vest == null) {
                Assert.Fail("Could not retreive Coupon with Id '" + _couponId + "'.");
                return;
            }
            Assert.That(vest.Coins, Is.EqualTo(20));
        }

        [Test][Order(1)]
        public void Test_DeleteCoupon() {
            bool deleted = _couponManager.DeleteCouponById(_couponId);
            Assert.That(deleted, Is.True);
        }
    }
}
