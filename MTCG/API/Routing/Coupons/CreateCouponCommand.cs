using MTCG.BLL.Exceptions;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Response;
using MTCG.Models;

namespace MTCG.API.Routing.Coupons
{
    public class CreateCouponCommand: AuthenticatedRouteCommand
    {
        private ICouponManager _couponManager;
        private Coupon _coupon;

        public CreateCouponCommand(ICouponManager couponManager, Coupon coupon, User identity) : base(identity) {
            _couponManager = couponManager;
            _coupon = coupon;
        }

        public override HttpResponse Execute() {
            HttpResponse response;

            if(Identity.Token != "admin-mtcgToken") {
                response = new HttpResponse(StatusCode.Forbidden);
                return response;
            }

            bool inserted = true;
            try {
                _couponManager.InsertCoupon(_coupon);
            } catch(DuplicateCouponException) { 
                inserted = false;
            }

            if(inserted) {
                response = new HttpResponse(StatusCode.Ok);
            } else {
                response = new HttpResponse(StatusCode.Conflict);
            }
            return response;
        }
    }
}