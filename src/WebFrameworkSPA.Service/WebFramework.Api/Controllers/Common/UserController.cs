using BrockAllen.MembershipReboot.Nh;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq.Dynamic;
using App.Common.InversionOfControl;
using System.Text;
using App.Common.Security;
using BrockAllen.MembershipReboot;
using Web.Areas.Admin.Models;
using System.Web.Http.ModelBinding;
using Web.Infrastructure.JqGrid;
using System.IO;
using Web.Infrastructure;

namespace Web.Controllers.Api
{
    [Authorize(Roles = Constants.ROLE_ADMIN + "," + Constants.ROLE_USERADMIN + "," + Constants.PERMISSION_EDIT_USER)]
    public class UserController : ApiController
    {
        private IUserService _userService;
        private IRoleService _roleService;
        private MembershipRebootConfiguration<NhUserAccount> _membershipConfiguration;
        public UserController()
        {
            _userService = IoC.GetService<IUserService>();
            _roleService = IoC.GetService<IRoleService>();
            _membershipConfiguration = IoC.GetService<MembershipRebootConfiguration<NhUserAccount>>();

        }
        // GET api/role
        public dynamic GetGridData([FromUri] JqGridSearchModel searchModel)
        {
            var query = _userService.Query();
            if (!User.IsInRole(Constants.ROLE_ADMIN))
            {
                query = query.Where(x => !x.Roles.Any(y => y.Name == Constants.ROLE_ADMIN));
            } 
            var data = Web.Infrastructure.Util.GetGridData<NhUserAccount>(searchModel, query);
            var dataList = data.Items.Select(x => new
            {
                x.ID,
                x.Tenant,
                x.Username,
                x.Email,
                x.FirstName,
                x.LastName,
                x.LastUpdated,
                x.Created,
                x.LastLogin,
                x.IsAccountClosed,
                x.AccountClosed,
                x.IsLoginAllowed,
                x.LastFailedLogin,
                x.FailedLoginCount,
                x.PasswordChanged,
                x.RequiresPasswordReset,
                x.IsAccountVerified,
                x.LastFailedPasswordReset,
                x.FailedPasswordResetCount,
                //x.MobileCode,
                //x.MobileCodeSent,
                x.MobilePhoneNumber,
                //x.MobilePhoneNumberChanged,
                //x.AccountTwoFactorAuthMode,
                //x.CurrentTwoFactorAuthStatus,
                x.VerificationKey,
                x.VerificationKeySent,
                x.VerificationPurpose/*,
                x.VerificationStorage,
                x.HashedPassword*/
            }).ToList();
            return new
            {
                total = searchModel.page > 0 ? (int)Math.Ceiling((float)data.TotalNumber / searchModel.rows) : 1,
                page = searchModel.page,
                TotalItems = data.TotalNumber,
                Items = dataList.Select(x => new
                {
                    x.ID,
                    x.Tenant,
                    x.Username,
                    x.Email,
                    x.FirstName,
                    x.LastName,
                    x.LastUpdated,
                    x.Created,
                    x.LastLogin,
                    x.IsAccountClosed,
                    x.AccountClosed,
                    x.IsLoginAllowed,
                    x.LastFailedLogin,
                    x.FailedLoginCount,
                    x.PasswordChanged,
                    x.RequiresPasswordReset,
                    x.IsAccountVerified,
                    x.LastFailedPasswordReset,
                    x.FailedPasswordResetCount,
                    //x.MobileCode,
                    //x.MobileCodeSent,
                    x.MobilePhoneNumber,
                    //x.MobilePhoneNumberChanged,
                    //x.AccountTwoFactorAuthMode,
                    //x.CurrentTwoFactorAuthStatus,
                    x.VerificationKey,
                    x.VerificationKeySent,
                    x.VerificationPurpose/*,
                x.VerificationStorage,
                x.HashedPassword*/
                }).ToArray()
            };
        }
        [Route("api/user/exporttoexcel")]
        [HttpGet]
        public dynamic ExportToExcel([FromUri]Web.Infrastructure.JqGrid.JqGridSearchModel searchModel)
        {
            var query = _userService.Query();
            if (!User.IsInRole(Constants.ROLE_ADMIN))
            {
                query = query.Where(x => !x.Roles.Any(y => y.Name == Constants.ROLE_ADMIN));
            }
            searchModel.rows = 0;
            var data = Web.Infrastructure.Util.GetGridData<NhUserAccount>(searchModel, query);
            var dataList = data.Items.Select(x => new
            {
                Application=x.Tenant,
                x.Username,
                x.Email,
                x.FirstName,
                x.LastName,
                x.LastUpdated,
                x.Created,
                x.LastLogin,
                x.IsAccountClosed,
                x.AccountClosed,
                x.IsLoginAllowed,
                x.LastFailedLogin,
                x.FailedLoginCount,
                x.PasswordChanged,
                x.RequiresPasswordReset,
                x.IsAccountVerified,
                x.LastFailedPasswordReset,
                x.FailedPasswordResetCount,
                //x.MobileCode,
                //x.MobileCodeSent,
                x.MobilePhoneNumber,
                //x.MobilePhoneNumberChanged,
                //x.AccountTwoFactorAuthMode,
                //x.CurrentTwoFactorAuthStatus,
                x.VerificationKey,
                x.VerificationKeySent,
                x.VerificationPurpose/*,
                x.VerificationStorage,
                x.HashedPassword*/
            }).ToList();
            string filePath = ExporterManager.Export("user", ExporterType.CSV, dataList.ToList(), "");
            HttpResponseMessage result = null;

            if (!File.Exists(filePath))
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(filePath);
                result.Content.Headers.ContentLength = new FileInfo(filePath).Length;

            }
            return result;
        }
        // GET api/role/5
        public IHttpActionResult Get(Guid id)
        {
            if (!HasPermission(id, Constants.ROLE_ADMIN))
                return Unauthorized(); 
            var item = _userService.Query().FirstOrDefault((p) => p.ID == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/role
        public IHttpActionResult Post([FromBody] NhUserAccount item)
        {
            StringBuilder message = new StringBuilder();
            NhUserAccount account = null;
            if (item == null)
                return BadRequest("User cannot be empty.");
            if (string.IsNullOrEmpty(item.Email))
                return BadRequest("User email cannot be empty.");
            if (_membershipConfiguration.EmailIsUsername)
                item.Username = item.Email;
            if (string.IsNullOrEmpty(item.Username))
                return BadRequest("Username cannot be empty.");
            item.Email = string.IsNullOrEmpty(item.Email) ? null : item.Email.Trim();
            item.Username = item.Username.Trim();
            if (string.IsNullOrEmpty(item.Tenant))
                item.Tenant = _membershipConfiguration.DefaultTenant;
            if (_membershipConfiguration.UsernamesUniqueAcrossTenants)
            {
                account = _userService.FindBy(x => x.Username == item.Username);
                if (account != null)
                {
                    return BadRequest(string.Format("Username {0} is not available.", item.Username));
                }
            }
            else
            {
                account = _userService.FindBy(x => x.Tenant == item.Tenant && x.Username == item.Username);
                if (account != null)
                {
                    return BadRequest(string.Format("Username {0} is not available.", item.Username));
                }
            }
            if (!string.IsNullOrEmpty(item.Email))
            {
                account = _userService.FindBy(x => x.Tenant == item.Tenant && x.Email == item.Email);
                if (account != null)
                {
                    return BadRequest(string.Format("Email {0} is already used.", item.Email));
                }
            }
            NhUserAccount newUser = _userService.CreateAccountWithTempPassword(item);
            message.AppendFormat("User {0}  is saved successflly.", item.Username);
            return Json<object>(new { Success = true, Message = message.ToString(), RowId = newUser.ID });
        }

        // PUT api/role/5
        public IHttpActionResult Put(Guid id, [FromBody] NhUserAccount item)
        {
            StringBuilder message = new StringBuilder();
            NhUserAccount account = null;
            if (id == default(Guid))
                return BadRequest("User id cannot be empty.");
            if (item == null)
            {
                return BadRequest("User cannot be empty.");
            }
            if (_membershipConfiguration.EmailIsUsername)
            {
                if (string.IsNullOrEmpty(item.Username))
                    return BadRequest("Username cannot be empty.");
                item.Username = item.Email.Trim();
            }

            if (string.IsNullOrEmpty(item.Username))
            {
                return BadRequest("Username cannot be empty.");
            }
            //item.ID = id;
            item.Username = item.Username.Trim();
            item.Email = string.IsNullOrEmpty(item.Email) ? null : item.Email.Trim();
            if (string.IsNullOrEmpty(item.Tenant))
                item.Tenant = _membershipConfiguration.DefaultTenant;
            if (!HasPermission(id, Constants.ROLE_ADMIN))
                return Unauthorized(); 
            NhUserAccount user = _userService.FindById(id);
            if (user == null)
                return NotFound();
            if (_membershipConfiguration.UsernamesUniqueAcrossTenants)
            {
                account = _userService.FindBy(x => x.ID != item.ID && x.Username == item.Username);
                if (account != null)
                {
                    return BadRequest(string.Format("Username {0} is not available.", item.Username));
                }
            }
            else
            {
                account = _userService.FindBy(x => x.Tenant == item.Tenant && x.ID != item.ID && x.Username == item.Username);
                if (account != null)
                {
                    return BadRequest(string.Format("Username {0} is not available.", item.Username));
                }
            }
            if (!string.IsNullOrEmpty(item.Email))
            {
                account = _userService.FindBy(x => x.Tenant == item.Tenant && x.ID != item.ID && x.Email == item.Email);
                if (account != null)
                {
                    return BadRequest(string.Format("Email {0} is already used.", item.Email));
                }
            }
            user.Username = item.Username;
            user.Email = item.Email;
            user.Tenant = item.Tenant;
            user.FailedLoginCount = item.FailedLoginCount;
            user.FailedPasswordResetCount = item.FailedPasswordResetCount;
            user.FirstName = item.FirstName;
            user.IsAccountClosed = item.IsAccountClosed;
            user.IsAccountVerified = item.IsAccountVerified;
            user.IsLoginAllowed = item.IsLoginAllowed;
            user.LastName = item.LastName;
            user.MobilePhoneNumber = item.MobilePhoneNumber;
            user.RequiresPasswordReset = user.RequiresPasswordReset;

            _userService.Update(user);
            message.AppendFormat("User {0}  is saved successflly.", user.Username);
            return Json<object>(new { Success = true, Message = message.ToString(), RowId = user.ID });

        }

        // DELETE api/role/5
        public IHttpActionResult Delete(Guid id)
        {
            if (id == default(Guid))
                return BadRequest("User id cannot be empty.");
            if (!HasPermission(id, Constants.ROLE_ADMIN))
                return Unauthorized();
            if (_userService.Delete(id))
                return Ok();
            else
                return NotFound();
        }
        [Route("api/userroles/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateUserRoles(Guid id, [ModelBinder(typeof(Web.Infrastruture.FieldValueModelBinder))] UserRoleModel item)
        {
            StringBuilder message = new StringBuilder();
            if (id == default(Guid))
                return BadRequest("User id cannot be empty.");
            if (item == null)
            {
                return BadRequest("Role cannot be empty.");
            }
            if (!HasPermission(id, Constants.ROLE_ADMIN))
                return Unauthorized();
            bool hasRole = item.HasRole;//String.IsNullOrEmpty(HasRole) || (HasRole.Trim().ToLower() != "on" && HasRole.Trim().ToLower() != "yes" && HasRole.Trim().ToLower() != "true") ? false : true;
            Guid uId = id;
            string roleName = item.Name;
            if (_roleService.IsUserInRole(uId, roleName))
            {
                if (!hasRole)
                {
                    _roleService.RemoveUsersFromRoles(new List<Guid>() { uId }, new List<string>() { roleName });
                    message.AppendFormat("Role {0} has been removed from user successfully.", roleName);
                }
            }
            else if (hasRole)
            {
                _roleService.AddUsersToRoles(new List<Guid> { uId }, new List<string>() { roleName });
                message.AppendFormat("Role {0} is granted to user successfully.", roleName);
            }
            return Json<Object>(new { Success = true, Message = message.ToString(), RowId = item.Id });

        }
        [Route("api/userroles/{id}")]
        public dynamic GetUserRoles(Guid id, [FromUri] JqGridSearchModel searchModel)
        {
            if (id == default(Guid))
                return BadRequest("User id cannot be empty.");
            int totalRecords;
            int startRow = (searchModel.page * searchModel.rows) + 1;
            int skip = (searchModel.page > 0 ? searchModel.page - 1 : 0) * searchModel.rows;
            if (!HasPermission(id, Constants.ROLE_ADMIN))
                return Unauthorized(); 
            NhUserAccount user = _userService.GetById(id);
            List<Role> allRoles = _roleService.GetAllRoles();
            if (!User.IsInRole(Constants.ROLE_ADMIN))
                allRoles = allRoles.Where(x => x.Name != Constants.ROLE_ADMIN).ToList();
            List<UserRoleModel> userRoles = new List<UserRoleModel>();
            foreach (Role role in allRoles)
            {
                bool hasRole = user.Roles.AsQueryable().Any(x => x.Id == role.Id);
                //userRoleEditModel.Roles.Add(new UserRoleModel { UserId=id, Role = role, HasRole = hasRole });
                userRoles.Add(new UserRoleModel { Id=role.Id,Name=role.Name,Description=role.Description,HasRole = hasRole });
            }
            var query = userRoles.AsQueryable();
            var data = Web.Infrastructure.Util.GetGridData<UserRoleModel>(searchModel, query);
            totalRecords = data.TotalNumber;
            var dataList = data.Items.ToList();
            var totalPages = (int)Math.Ceiling((float)totalRecords / searchModel.rows);
            return new
            {
                total = searchModel.page > 0 ? (int)Math.Ceiling((float)totalRecords / searchModel.rows) : 1,
                page = searchModel.page,
                TotalItems = totalRecords,
                Items = dataList.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.HasRole
                }).ToArray()
            };
        }
        [Route("api/userroles/exporttoexcel")]
        [HttpGet]
        public dynamic ExportToExcelUserRoles(Guid id,[FromUri]Web.Infrastructure.JqGrid.JqGridSearchModel searchModel)
        {
            if (id == default(Guid))
                return BadRequest("User id cannot be empty.");
            searchModel.rows = 0;
            int startRow = (searchModel.page * searchModel.rows) + 1;
            int skip = (searchModel.page > 0 ? searchModel.page - 1 : 0) * searchModel.rows;
            if (!HasPermission(id, Constants.ROLE_ADMIN))
                return Unauthorized();
            NhUserAccount user = _userService.GetById(id);
            List<Role> allRoles = _roleService.GetAllRoles();
            if (!User.IsInRole(Constants.ROLE_ADMIN))
                allRoles = allRoles.Where(x => x.Name != Constants.ROLE_ADMIN).ToList();
            List<UserRoleModel> userRoles = new List<UserRoleModel>();
            foreach (Role role in allRoles)
            {
                bool hasRole = user.Roles.AsQueryable().Any(x => x.Id == role.Id);
                //userRoleEditModel.Roles.Add(new UserRoleModel { UserId=id, Role = role, HasRole = hasRole });
                userRoles.Add(new UserRoleModel { Id = role.Id, Name = role.Name, Description = role.Description, HasRole = hasRole });
            }

            var query = userRoles.AsQueryable();
            var data = Web.Infrastructure.Util.GetGridData<UserRoleModel>(searchModel, query);
            var dataList = data.Items.Select(x=>new {x.Name,x.Description,x.HasRole}).ToList();

            string filePath = ExporterManager.Export("userroles", ExporterType.CSV, dataList, "");
            HttpResponseMessage result = null;

            if (!File.Exists(filePath))
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(filePath);
                result.Content.Headers.ContentLength = new FileInfo(filePath).Length;

            }
            return result;
        }
        private bool HasPermission(Guid userId, string targetRole)
        {
            bool allow = false;
            if (User.IsInRole(Constants.ROLE_ADMIN))
            {
                allow = true;
                return allow;
            }
            List<Role> roles = _roleService.GetRolesForUser(userId);
            if (roles == null || !roles.Any(x => x.Name == targetRole))
            {
                allow = true;
            }
            return allow;
        }

    }
}
