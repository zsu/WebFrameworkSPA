﻿using System;
using System.Text;
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
        private IUserService _userService;
        public AccountController()
        {
            _userService = IoC.GetService<IUserService>();
        }
        [Route("api/account/changepassword")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult ChangePassword([ModelBinder(typeof(Web.Infrastruture.FieldValueModelBinder))] ChangePasswordModel item)
        {
            StringBuilder message = new StringBuilder();
            Guid userId=User==null || !User.HasUserID() || !User.Identity.IsAuthenticated?Guid.Empty:User.GetUserID();
            NhUserAccount user = null;
            if (!string.IsNullOrEmpty(item.UserName))
            {
                user = _userService.FindBy(x=>x.Username==item.UserName.Trim());
                if (user == null)
                    return BadRequest(string.Format("Invalid user name or password."));
                userId = user.ID;
            }
            else
                item.UserName = User.Identity.Name;
            if (userId == default(Guid))
                return Unauthorized();
            //if (!User.HasUserID())
            //{
            //    return Unauthorized();
            //}
            if (item.NewPassword != item.NewPasswordConfirm)
            {
                message.Append("New password and password confirm do not match.");
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            try
            {
                _userService.ChangePassword(userId, item.OldPassword, item.NewPassword);
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            message.Append("Password is changed successfully.");
            return Json<object>(new { Success = true, Message = message.ToString() });
        }
        [Route("api/account/resetpassword")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult ResetPassword([ModelBinder(typeof(Web.Infrastruture.FieldValueModelBinder))] PasswordResetModel item)
        {
            StringBuilder message = new StringBuilder();
            if(string.IsNullOrEmpty(item.Email))
                return BadRequest("Email cannot be empty.");
            var account = _userService.FindBy(x=>x.Email==item.Email.Trim());
            if (account == null)
                return BadRequest("Invalid email.");
            try
            {
                _userService.ResetPassword(item.Email.Trim());
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            message.Append("Password reset email has been sent out successfully.");
            return Json<object>(new { Success = true, Message = message.ToString() });
        }
        [Route("api/account/confirmpasswordreset")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult ConfirmPasswordReset([ModelBinder(typeof(Web.Infrastruture.FieldValueModelBinder))] ResetPasswordConfirmModel item)
        {
            StringBuilder message = new StringBuilder();
            if (string.IsNullOrEmpty(item.Key))
                return BadRequest("Key cannot be empty.");
            NhUserAccount account;
            if (item.Password != item.ConfirmPassword)
            {
                message.Append("New password and password confirm do not match.");
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            try
            {
                if(!_userService.ChangePasswordFromResetKey(item.Key, item.Password, out account))
                {
                    message.Append("Failed to change password.");
                    return Json<object>(new { Success = false, Message = message.ToString() });
                }
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            message.Append("Password was changed successfully.");
            return Json<object>(new { Success = true, Message = message.ToString() });
        }
        [Route("api/account/confirmemail")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult ConfirmEmail([ModelBinder(typeof(Web.Infrastruture.FieldValueModelBinder))] ChangeEmailFromKeyInputModel item)
        {
            StringBuilder message = new StringBuilder();
            NhUserAccount account;
            try
            {
                _userService.VerifyEmailFromKey(item.Key, item.Password, out account);
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            message.Append("Email address was confirmed.");
            return Json<object>(new { Success = true, Message = message.ToString() });
        }
        [Route("api/account/cancelverificationrequest")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult CancelVerificationRequest(string id)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                bool closed;
                _userService.CancelVerification(id, out closed);
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            message.Append("The request that was emailed to you is now cancelled.");
            return Json<object>(new { Success = true, Message = message.ToString() });
        }
        [Route("api/account/register")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Register([ModelBinder(typeof(Web.Infrastruture.FieldValueModelBinder))] RegisterInputModel item)
        {
            StringBuilder message = new StringBuilder();
            if (string.IsNullOrEmpty(item.Email))
                return BadRequest("Email cannot be empty.");
            if (string.IsNullOrEmpty(item.Username))
                return BadRequest("Username cannot be empty.");
            if (string.IsNullOrEmpty(item.Password))
                return BadRequest("Password cannot be empty.");
            if (item.Password!=item.ConfirmPassword)
                return BadRequest("Password and confirm password do not match.");
            if (string.IsNullOrEmpty(item.FirstName))
                return BadRequest("Firstname cannot be empty.");
            try
            {
                NhUserAccount account = new NhUserAccount { Username = item.Username, HashedPassword = item.Password, Email = item.Email, FirstName = item.FirstName, LastName = item.LastName };
                account = _userService.CreateAccount(account);
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
                return Json<object>(new { Success = false, Message = message.ToString() });
            }
            message.AppendFormat("User {0} is created successfully.",item.Username);
            return Json<object>(new { Success = true, Message = message.ToString() });
        }
    }
}
