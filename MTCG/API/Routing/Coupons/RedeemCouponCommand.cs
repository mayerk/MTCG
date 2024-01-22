using MTCG.BLL.Exceptions;
using MTCG.BLL.Managers;
using MTCG.HttpServer.Response;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MTCG.API.Routing.Coupons {
    public  class RedeemCouponCommand: AuthenticatedRouteCommand {

        ICouponManager _couponManager;
        IUserManager _userManager;

        string _couponId;

        public RedeemCouponCommand(ICouponManager couponManager, IUserManager userManager, string couponId, User identity): base(identity) {
            _couponManager = couponManager;
            _userManager = userManager;
            _couponId = couponId;
        }

        public override HttpResponse Execute() {
            HttpResponse response;
            try {
                Coupon? coupon = _couponManager.GetCouponById(_couponId);
                if(coupon == null) {
                    response = new HttpResponse(StatusCode.NotFound);
                    return response;
                }
                Identity.Coins += coupon.Coins;
                _userManager.UpdateUser(Identity);
                if(_couponManager.DeleteCouponById(coupon.Id) == false) {
                    response = new HttpResponse(StatusCode.NotFound);
                }
                response = new HttpResponse(StatusCode.Ok);
            } catch(CouponNotFoundException) {
                response = new HttpResponse(StatusCode.NotFound);
                return response;
            }
            return response;
        }
    }
}
