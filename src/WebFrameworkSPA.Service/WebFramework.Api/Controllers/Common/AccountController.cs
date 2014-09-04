using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using App.Common.InversionOfControl;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Nh;
using Service;
using Web.Models;

namespace Web.Controllers.Common
{
    [Authorize]
    public class AccountController : ApiController
    {
        private UserAccountService<NhUserAccount> _accountService;
        public AccountController()
        {
            _accountService = IoC.GetService<UserAccountService<NhUserAccount>>();
        }
        [Route("api/account/changepassword")]
        [HttpPut]
        public IHttpActionResult ChangePassword([ModelBinder(typeof(Web.Infrastruture.FieldValueModelBinder))] ChangePasswordModel item)
        {
            StringBuilder message = new StringBuilder();
            var user = HttpContext.Current.User;
            var claimsIdentity = user == null ? null : user.Identity as ClaimsIdentity;
            if (!User.HasUserID())
            {
                return Unauthorized();
            }
            if (item.NewPassword != item.NewPasswordConfirm)
            {
                message.Append("New password and password confirm do not match.");
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            try
            {
                _accountService.ChangePassword(user.GetUserID(), item.OldPassword, item.NewPassword);
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            message.Append("Password is changed successflly.");
            return Json<object>(new { Success = true, Message = message.ToString() });
        }
        [Route("api/account/confirmemail")]
        [HttpPut]
        public IHttpActionResult ConfirmEmail([ModelBinder(typeof(Web.Infrastruture.FieldValueModelBinder))] ChangeEmailFromKeyInputModel item)
        {
            StringBuilder message = new StringBuilder();
            NhUserAccount account;
            try
            {
                _accountService.VerifyEmailFromKey(item.Key, item.Password, out account);
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            message.Append("Email address was confirmed.");
            return Json<object>(new { Success = true, Message = message.ToString() });
        }
    }
}
