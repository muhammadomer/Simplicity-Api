using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplicityOnlineWebApi.ClientInvoice.Models.Interfaces;
using SimplicityOnlineWebApi.BLL.Entities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserDetailsController : Controller
    {
        private readonly IUserDetailsRepository UserDetailsRepository;
        private readonly IClientInvoiceRepository ClientInvoiceRepository;
        private readonly IAppSettingRepository AppSettingRepository;
        private readonly IPassthroughRepository PassthroughRepository;    
        private readonly IOrdersTagsRepository OrdersTagsRepository;
        private readonly ILogger<RefS4bFormsController> Logger;
        public UserDetailsController(
            IPassthroughRepository passthroughRepository,
            IAppSettingRepository appSettingRepository,
            IClientInvoiceRepository clientInvoiceRepository,
            IUserDetailsRepository userDetailsRepository,
            IOrdersTagsRepository ordersTagsRepository,
            ILogger<RefS4bFormsController> logger
            )
        {
            this.PassthroughRepository = passthroughRepository;
            this.AppSettingRepository = appSettingRepository;
            this.ClientInvoiceRepository = clientInvoiceRepository;
            this.UserDetailsRepository = userDetailsRepository;
            this.OrdersTagsRepository = ordersTagsRepository;
            this.Logger = logger;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(string id)
        {
            if (id.Equals("1"))
            {
                return new ObjectResult("Found");
            }
            else
            {
                return new ObjectResult("Not Found");
            }
        }

        // POST api/values
        [HttpPost]
        [ActionName("Login")]
        [Route("[action]")]
        public UserDetails Login([FromBody]UserDetails userName)
        {
            if(userName == null)
            {
                Logger.LogInformation("Attempted User is null");
            }
            else
            {
                 Logger.LogInformation(userName.UserLogon + " is attempting to login using Project Id " + userName.ProjectId);
            }
         // from below line you get license information
         //Note: You need to pass projectId
         //LicenseInformation licenseInformation = ClientInvoiceRepository.GetLicenseInfo(userName.ProjectId);
         //userName.License = licenseInformation;
         //
         //UserDetails user = UserDetailsRepository.Login(userName.UserLogon, userName.UserEnable, userName.ProjectId,licenseInformation.noOfAllowedUsers);
         UserDetails user = UserDetailsRepository.Login(userName.UserLogon, userName.UserEnable, userName.ProjectId);



         //---Find number of logged in users

         if (user == null)
            {
                user = new UserDetails();
                user.IsSucessfull = false;
                user.ProjectId = userName.ProjectId;
                user.Message = UserDetailsRepository.Message;
               
                return user;
            }else
            {
                
            }
            return user;
        }

        [HttpPost]
        [ActionName("LoginByPassthrough")]
        [Route("[action]")]
        public UserDetails LoginByPassthrough([FromBody]UserPassthroughModel userModel)
        {
          if (string.IsNullOrWhiteSpace(userModel.Passthrough))
            {
                Logger.LogInformation("passthrough string is null");
            }
            else
            {
                Logger.LogInformation(userModel.Passthrough + " is attempting for passthrough using Project Id " + userModel.ProjectId);
            }

           UserDetails user = null;
           string decryptedPassThrough = Utilities.strUFFAFU(userModel.Passthrough);
           if(decryptedPassThrough.Length>8)
           {
               string passThroughDailyPassCode = decryptedPassThrough.Substring(0, 8);
               string dailyPassCode = Utilities.StrHash(Utilities.StrCreateEnabler());
                Logger.LogInformation("Attempting to validate the Passthrough String '" + decryptedPassThrough + "' for Project Id: " + userModel.ProjectId + ". Daily Pass code is " + passThroughDailyPassCode);
               if (dailyPassCode.Equals(passThroughDailyPassCode))
               {
                   long sequence = 0;
                   if (long.TryParse(decryptedPassThrough.Substring(8, decryptedPassThrough.Length - 8), out sequence))
                   {
                       PassthroughModel passthroughModel = PassthroughRepository.GetPassthroughModelBySequence(sequence, userModel.ProjectId);
                       if (passthroughModel != null)
                       {
                           user = UserDetailsRepository.Login(passthroughModel.CreatedBy, userModel.ProjectId, HttpContext.Response);
                           if (user != null)
                           {
                               user.RedirectUrl = "/" + passthroughModel.ComponentName.ToLower() + "/" + passthroughModel.InternalId;
                           }
                       }
                       else
                       {
                            Logger.LogError("Unable to get Attachment Folder Passthrough For Sequence '" + sequence + "'");
                       }
                       if (!PassthroughRepository.DeletePassthroughBySequence(sequence, userModel.ProjectId))
                       {
                            Logger.LogError("Unable to Delete Attachment Folder Passthrough For Sequence '" + sequence + "'.");
                       }
                   }
                   else
                   {
                        Logger.LogError("Passthrough string contains invalid Sequence '" + sequence + "'");
                   }
               }
           }
           else
           {
                Logger.LogError("Invalid Passthrough String '" + userModel.Passthrough + "'. Could not match against Daily Pass Code");
           }
           
            if (user == null)
            {
                user = new UserDetails();
                user.IsSucessfull = false;
                user.ProjectId = userModel.ProjectId;
                user.Message = "Invalid Passthrough string.";
                return user;
            }            
            return user;
        }

        [HttpPost]
        [ActionName("LogOut")]
        [Route("[action]")]
        public IActionResult LogOut([FromBody]UserSessions userName)
        {
            bool isUserLogOut = UserDetailsRepository.LogOut(Request, HttpContext.Response);
            return new ObjectResult(isUserLogOut);
        }

        [HttpGet]
        [ActionName("GetAllUsers")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult GetAllUsers()
        {
            List<UserDetails> users = UserDetailsRepository.GetAllUsers(Request, HttpContext.Response);
            if (users == null)
            {
                return new ObjectResult(false);
            }
           
          
            return new ObjectResult(users);
        }

        [HttpGet]
        [ActionName("GetAuthentication")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAuthentication()
        {
            


            return new ObjectResult(true);
        }

        [HttpPost]
        [ActionName("AddUser")]
        [Route("[action]")]
        public IActionResult AddUser([FromBody]UserDetails userName)
        {
            //---Validation that max no of users are not created
            bool maxLimit = false;
            long nofOfLicense = 0;
            long countUser = UserDetailsRepository.GetTotalUsers(Request);
            ResponseModel res = AppSettingRepository.GetAppSettingById(Request, "MA0");
            List<ApplicationSettings> appSetting = (List<ApplicationSettings>)res.TheObject;
            if (appSetting[0].Setting1 != "Not Set")
            {
                nofOfLicense = Convert.ToInt64(appSetting[0].Setting1);
                if (countUser >= nofOfLicense)
                    maxLimit = true;
                else
                    maxLimit = false;
            }
            else
            {
                maxLimit = false;
            }
            if (maxLimit == false)
            {
                if (userName.ResourceSequence == null) userName.ResourceSequence = -1;
                return new ObjectResult(UserDetailsRepository.AddUser(userName, HttpContext.Request));
            }else
            {
                return new ObjectResult(null);
            }
            
        }

        [HttpPost]
        [ActionName("DisableOrEnableUser")]
        [Route("[action]")]
        public IActionResult DisableOrEnableUser(UserDetails userName)
        {
            UserDetails user = UserDetailsRepository.DisableOrEnable(userName,HttpContext.Request);
            if (user == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(true);
        }

        [HttpGet]
        [ActionName("GetUsersByUserName")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult GetUsersByUserName(string UserName)
        {
            List<UserDetails> users = UserDetailsRepository.GetUsersByUserName(UserName, Request, HttpContext.Response);
            if (users == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(users);
        }

         [HttpGet]
         [ActionName("GetUsersByUserId")]
         [Route("[action]")]
         //[ValidateRequestState]
         public IActionResult GetUsersByUserId(int UserId)
         {
            List<UserDetails> users = UserDetailsRepository.GetUsersByUserId(UserId, Request, HttpContext.Response);
            if (users == null)
            {
               return new ObjectResult(false);
            }
            return new ObjectResult(users);
         }

      [HttpPost]
        [ActionName("Update")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult Update([FromBody]UserDetails userName)
        {
           
          return new ObjectResult(UserDetailsRepository.UpdateUser(userName, HttpContext.Request));
           
        }
        [HttpGet]
        [ActionName("CheckUserAsUniq")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult CheckUserAsUniq(string userName)
        {
            List<UserDetails> User = UserDetailsRepository.CheckUserAsUniq(userName,-1, HttpContext.Request);
            if (User != null && User.Count > 0)
            {
                return new ObjectResult(true);
            }
            else
            {
                return new ObjectResult(false);
            }
        }
        [HttpPost]
        [ActionName("IsUserLogonExist")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult IsUserLogonExist([FromBody]UserDetails user)
        {
            UserDetails User = UserDetailsRepository.CheckUserLogon(user, HttpContext.Request);
            if (User != null)
            {
                return new ObjectResult(User);
            }
            else
            {
                return new ObjectResult(User);
            }
        }
        [HttpPost]
        [ActionName("CheckUserPass")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult CheckUserPass([FromBody]UserDetails user)
        {
            UserDetails User = UserDetailsRepository.CheckPassword(user);
            if (User != null)
            {
                return new ObjectResult(User);
            }
            else
            {
                return new ObjectResult(User);
            }
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult ResetPassword([FromBody]UserDetails userName)
        {
            UserDetails User = UserDetailsRepository.ResetPassword(userName);
            if (User == null)
            {
                return new ObjectResult(User);
            }
            else
            {
                return new ObjectResult(User);
            }

        }

        [HttpGet]
        [ActionName("IsMaxNoOfUserCreated")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult IsMaxNoOfUserCreated()
        {
            bool returnValue = false;
            long nofOfLicense = 0;
            long countUser = UserDetailsRepository.GetTotalUsers(Request);
            ResponseModel res = AppSettingRepository.GetAppSettingById(Request, "MA0");
            List<ApplicationSettings> appSetting = (List<ApplicationSettings>) res.TheObject;
            if(appSetting[0].Setting1 != "Not Set")
            {
                nofOfLicense = Convert.ToInt64( appSetting[0].Setting1);
                if (countUser >= nofOfLicense)
                    returnValue = true;
                else
                    returnValue = false;
            }else
            {
                returnValue = false;
            }
            return new ObjectResult(returnValue);
        }
        [HttpPost]
        [ActionName("ForgotUser")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult ForgotUser([FromBody]UserDetails userName)
        {
            ResponseModel response = new ResponseModel();
            response.Message = "Something went wrong!";
            Request.Headers["ProjectId"] = userName.ProjectId;
            UserDetails user = UserDetailsRepository.GetUserByEmail(userName);
            user.DomainName = userName.DomainName;
            if (user == null || user.UserId == 0)
            {
                response.Message = "User email does not exist!";
                return new ObjectResult(response);
            }
            Random random = new Random();
            user.ForgotPasswordString = random.Next(0, 9999).ToString() + DateTime.Now.Ticks.ToString() + random.Next(0, 9999).ToString();
            var returnStatus = UserDetailsRepository.UpdateUser(user, HttpContext.Request);
            if (returnStatus.IsSucessfull)
            {
                var emailOptions = new EmailOrderTags();
                emailOptions.Subject = "Reset Your Simplicity4Business Password";
                emailOptions.From = new EmailContact();
                emailOptions.To = new List<EmailContact>() { new EmailContact { EmailAddress = userName.UserEmail, FullName = userName.UserName } };
                emailOptions.Body = getMsgBody(user);
                response = OrdersTagsRepository.EmailOrdersWithTagsAndImages(emailOptions, Request, HttpContext.Response);
                return new ObjectResult(response);
            }
            return new ObjectResult(response);
        }
        [HttpPost]
        [ActionName("ForgotPasswordStringExist")]
        [Route("[action]")]
        public IActionResult ForgotPasswordStringExist([FromBody]UserDetails userName)
        {
            ResponseModel response = new ResponseModel();
            response.Message = "Something went wrong!";
            Request.Headers["ProjectId"] = userName.ProjectId;
            UserDetails user = UserDetailsRepository.CheckForgotPasswordString(userName);

            if (user == null || user.UserId == 0)
            {
                response.Message = "Forgot string does not exist!";
                return new ObjectResult(response);
            }
            response.IsSucessfull = true;
            response.TheObject = user;
            return new ObjectResult(response);
        }
        [HttpPost]
        [ActionName("RecoverForgotPassword")]
        [Route("[action]")]
        public IActionResult RecoverForgotPassword([FromBody]UserDetails user)
        {
            ResponseModel response = new ResponseModel();
            response.Message = "Something went wrong!";
            Request.Headers["ProjectId"] = user.ProjectId;
            var existedUser = UserDetailsRepository.GetUserByUserId(user.UserId, Request);
            if (existedUser != null)
            {
                existedUser.UserEnable = user.UserEnable;
                existedUser.ProjectId = user.ProjectId;
                UserDetails user1 = UserDetailsRepository.RecoverForgotPassword(existedUser);
                response.IsSucessfull = true;
                response.Message = "Successfully password updated!";
                return new ObjectResult(response);
            }
            response.Message = "Forgot string does not exist!";
            return new ObjectResult(response);
        }

        private string getMsgBody(UserDetails user)
        {
            string domain = user.DomainName;//"https://demomssql.test-simplicity4business.co.uk:7002";
            string msg = @"<table align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' style='border-collapse:collapse;height:100%;margin:0;padding:0;width:100%;background-color:#fafafa'>
                            <tbody>
                                <tr>
                                    <td align='center' valign='top' style='height:100%;margin:0;padding:10px;width:100%;border-top:0'>
                                        <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse:collapse;border:0;max-width:600px!important'>
                                            <tbody>
                                                <tr>
                                                    <td valign='top' style='background-color:#fafafa;border-top:0;border-bottom:0;padding-top:9px;padding-bottom:9px'></td>
                                                </tr>
                                                 <tr>
                                                    <td valign='top' style='background-color:#ffffff;border-top:0;border-bottom:0;padding-top:9px;padding-bottom:0'>
                                                        <table border='0' cellpadding='0' cellspacing='0' width='100%' style='min-width:100%;border-collapse:collapse'>
                                                            <tbody>
                                                                <tr>
                                                                    <td valign='top' style='padding:9px'>
                                                                        <table align='left' width='100%' border='0' cellpadding='0' cellspacing='0' style='min-width:100%;border-collapse:collapse'>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td valign='top' style='padding-right:9px;padding-left:9px;padding-top:0;padding-bottom:0;text-align:center'>
                                                                                        <img align='center' src='" + domain + @"/assets/modules/pages/common/img/logo-inverse.png' width='246' style='max-width:246px;padding-bottom:0;display:inline!important;vertical-align:bottom;border:0;min-height:auto;outline:none;text-decoration:none' class='CToWUd'>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign='top' style='background-color:#ffffff;border-top:0;border-bottom:2px solid #eaeaea;padding-top:0;padding-bottom:9px'>
                                                        <table border='0' cellpadding='0' cellspacing='0' width='100%' style='min-width:100%;border-collapse:collapse'>
                                                            <tbody>
                                                                <tr>
                                                                    <td valign='top' style='padding-top:9px'>
                                                                        <table align='left' border='0' cellpadding='0' cellspacing='0' style='max-width:100%;min-width:100%;border-collapse:collapse' width='100%'>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td valign='top' style='padding-top:0;padding-right:18px;padding-bottom:9px;padding-left:18px;word-break:break-word;color:#202020;font-family:Helvetica;font-size:16px;line-height:150%;text-align:left'>

                                                                                        <h1 style='text-align:center;display:block;margin:0;padding:0;color:#202020;font-family:Helvetica;font-size:26px;font-style:normal;font-weight:bold;line-height:125%;letter-spacing:normal'>Don't worry, we all forget sometimes</h1>
                                                                                        &nbsp;

                                                                                        <p style='margin:10px 0;padding:0;color:#202020;font-family:Helvetica;font-size:16px;line-height:150%;text-align:left'>
                                                                                            Hi " + user.UserName + @"  <br>
                                                                                            <br>
																	                        To update your password, click the link below:<br>
                                                                                            <a href='" + domain + "/#/recover-password?forgotstring=" + user.ForgotPasswordString + @"' style='color:#2baadf;font-weight:normal;text-decoration:underline' target='_blank'>http://simplicity4business.co.uk/</a><br>
                                                                                            <br>
                                                                                            <br>
                                                                                             We hope you will enjoy the experience and look forward to helping you with this!
                                                                                        </p>

                                                                                        <hr>

                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>";
            return msg;
        }
    }
}
