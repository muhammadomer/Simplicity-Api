using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SimplicityOnlineWebApi.BLL.Entities;
using Newtonsoft.Json;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class DiaryAppsRepository : IDiaryAppsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public DiaryAppsRepository()
        {
        }

        public List<DiaryApps> GetDiaryAppsByDate(DateTime? appStartDate, DateTime? appEndDate, HttpRequest Request, HttpResponse Response)
        {
            List<DiaryApps> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                int userId = Int32.Parse(Request.Headers["UserId"]);
                string token = Request.Headers["token"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB diaryAppsDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = diaryAppsDB.selectAllDiaryAppsByAppDate(appStartDate, appEndDate);
                        if (returnValue == null)
                        {
                            Response.Headers["message"] = "No Appointment Found.";
                        }
                        else
                        {
                            //OrdersRepository _ordersRepository = new OrdersRepository(this._appEnvironment, null);
                            //for (int i = 0; i < returnValue.Count; i++)
                            //{

                            //  returnValue[i].Order = _ordersRepository.GetOrderDetailsBySequence(returnValue[i].JobSequence, Request);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Headers["message"] = "Exception occured while getting Appointments By Date. " + ex.Message;
            }
            return returnValue;
        }

        public List<DiaryAppsHistory> GetDiaryAppsByJobSequence(HttpRequest Request, HttpResponse Response, long jobSequence)
        {
            List<DiaryAppsHistory> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                int userId = Int32.Parse(Request.Headers["UserId"]);
                string token = Request.Headers["token"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB diaryAppsDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = diaryAppsDB.selectAllDiaryAppsByJobSequence(jobSequence);
                        if (returnValue == null)
                        {
                            Response.Headers["message"] = "No Appointment Found.";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Response.Headers["message"] = "Exception occured while getting Appointments History By Job. " + ex.Message;
            }
            return returnValue;
        }

        public List<DiaryApps> GetDiaryAppsByDateAndJobRef(DateTime? appStartDate, DateTime? appEndDate, string jobRef, HttpRequest Request, HttpResponse Response)
        {
            List<DiaryApps> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                int userId = Int32.Parse(Request.Headers["UserId"]);
                string token = Request.Headers["token"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB diaryAppsDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = diaryAppsDB.selectAllDiaryAppsByAppDate(appStartDate, appEndDate, jobRef);
                        if (returnValue == null)
                        {
                            Response.Headers["message"] = "No Appointment Found.";
                        }
                        else
                        {
                            //OrdersRepository _ordersRepository = new OrdersRepository(this._appEnvironment, null);
                            //for (int i = 0; i < returnValue.Count; i++)
                            //{

                            //  returnValue[i].Order = _ordersRepository.GetOrderDetailsBySequence(returnValue[i].JobSequence, Request);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Headers["message"] = "Exception occured while getting Appointments By Date. " + ex.Message;
            }
            return returnValue;
        }

        public List<DiaryApps> GetDiaryAppsThirdPartyByEntityId(long entityId, HttpRequest Request, HttpResponse Response)
        {
            List<DiaryApps> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                int userId = Int32.Parse(Request.Headers["WebId"]);
                string token = Request.Headers["token"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB diaryAppsDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = diaryAppsDB.selectAllDiaryAppsThirdPartyByEntityId(entityId);
                        if (returnValue == null)
                        {
                            Response.Headers["message"] = "No Appointment Found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Headers["message"] = "Exception occured while getting Appointments By Date. " + ex.Message;
            }
            return returnValue;
        }

        internal bool updateVisitStatusBySequence(HttpRequest request, long daSequence, int visitStatus, bool flgNoAccess,
                                                  int lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB daDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (daDB.updateVisitStatusBySequence(daSequence, visitStatus, flgNoAccess, lastAmendedBy, dateLastAmended))
                        {
                            returnValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool UpdateDiaryAppGPSDetails(HttpRequest request, long daSequence, DiaryAppsGPS diaryAppsGPS,
                                               int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "DiaryAppsRepository.UpdateDiaryAppGPSDetails()";
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB daDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (daDB.UpdateDiaryAppGPSDetails(daSequence, diaryAppsGPS, lastAmendedBy, dateLastAmended))
                        {
                            returnValue = true;
                        }
                        else
                        {
                            Message = daDB.ErrorMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Update Diary App GPS details.", ex);
            }
            return returnValue;
        }

        internal bool updateVisitStatusAndFlgCompletedBySequence(HttpRequest request, long daSequence, int visitStatus, bool flgCompleted,
                                                                 int lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB daDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (daDB.updateVisitStatusAndFlgCompletedBySequence(daSequence, visitStatus, flgCompleted, lastAmendedBy, dateLastAmended))
                        {
                            returnValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public DiaryApps GetDiaryAppsBySequence(long sequence, HttpRequest Request, HttpResponse Response)
        {
            DiaryApps returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB daDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = daDB.selectAllDiaryAppsSequence(sequence);
                        if (returnValue == null)
                        {
                            Message = "No Diary Appointment Found.";
                        }
                        else
                        {
                            if (returnValue.JobSequence > 0)
                            {
                                OrdersRepository _ordersRepository = new OrdersRepository(null);
                                returnValue.Order = _ordersRepository.GetOrderDetailsBySequence(returnValue.JobSequence ?? 0, Request);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting appointment. " + ex.Message;
            }
            return returnValue;
        }
        public long GetDiaryResourceSequenceByUserId(long userId, HttpRequest Request, HttpResponse Response)
        {
            long returnValue = -1;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB daDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = daDB.selectDiaryResourceSequenceByUserId(userId);
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting resource Sequence. " + ex.Message;
            }
            return returnValue;
        }
        public ResponseModel CreateDiaryApps(DiaryApps diaryApp, HttpRequest request)
        {
            //DiaryApps returnValue = null;
			ResponseModel returnValue = new ResponseModel();

			try
            {
                string projectId = request.Headers["ProjectId"];
				bool isOverlapTimeslot = false;

				if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB daDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        long sequence = -1;
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
						if (diaryApp.ClientId > 0)
						{
                            isOverlapTimeslot = daDB.checkClientTimeslotDuplicate(diaryApp.DateAppStart, diaryApp.DateAppEnd, diaryApp.ClientId ?? 0, diaryApp.ResourceSequence??0);
                        }
						if (isOverlapTimeslot == false)
						{
							if (daDB.insertDiaryApps(out sequence, diaryApp.TransType, diaryApp.JoinResource ?? 0, diaryApp.FlgUseClientId, diaryApp.ClientId ?? 0,
																diaryApp.JobSequence ?? 0, diaryApp.JobAddressId ?? 0, diaryApp.FlgBookingRequired,
																diaryApp.ResourceSequence ?? 0, diaryApp.DateAppStart, diaryApp.DateAppEnd,
																diaryApp.FlgAppAllDay, diaryApp.AppPostCode, diaryApp.AppSubject, diaryApp.AppLocation,
																diaryApp.FlgAppReminder, diaryApp.AppReminderSound, diaryApp.AppReminderMins ?? 0, diaryApp.AppNotes,
																diaryApp.AppCategory, diaryApp.AppAttachmentPath, diaryApp.FlgOnlineMeeting,
																diaryApp.FlgUnavailable, diaryApp.RepeatSequence ?? 0, diaryApp.MultiResourceSequence ?? 0,
																diaryApp.AppType ?? 0, diaryApp.FlgAppDeleted, diaryApp.FlgAppCompleted, diaryApp.FlgAppBroken,
																diaryApp.AppBrokenReason ?? 0, diaryApp.FlgNoAccess, diaryApp.FlgAppConfirmed, diaryApp.DateAppConfirmed,
																diaryApp.AppConfirmedBy, diaryApp.CertSequence ?? 0, diaryApp.VisitStatus ?? 0,
																diaryApp.FlgAppFixed, diaryApp.FlgPrint, diaryApp.PrintUserId ?? 0, diaryApp.UnscheduledDeSeq ?? 0, diaryApp.RateSequence ?? 0,
																userId, DateTime.Now))
							{
								diaryApp.Sequence = sequence;
								returnValue.TheObject =  daDB.selectAllDiaryAppsSequence(sequence);
								returnValue.IsSucessfull = true;
								//***************** Send Firebase Notifications *****************
								#region Firebase Notification
								UserNotificationsDB noteDB = new UserNotificationsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
								sequence = -1;
								UserNotifications userNotification = new UserNotifications();
								userNotification.title = "New Appointment";
								userNotification.body = "New Appointment is assigned";
								Microsoft.Extensions.Primitives.StringValues values;
								if (request.Headers.TryGetValue("Origin", out values))
								{
									string action = values.ToString();
									userNotification.click_action = "/diary/" + diaryApp.JobSequence;
								}
                                userNotification.data = JsonConvert.SerializeObject(diaryApp); // new JavaScriptSerializer().Serialize(diaryApp);
								List<NotificationsToken> tokenList = noteDB.getAllTokenList();
								string reg_ids = "";
								foreach (NotificationsToken token in tokenList)
								{
									reg_ids = reg_ids + (reg_ids.Length > 0 ? "," : "") + token.FirebaseToken;
								}
								userNotification.registration_ids = reg_ids;
								userNotification.sound = "default";
								userNotification.UserId = userId;
								userNotification.CreatedBy = userId;
								userNotification.DateCreated = DateTime.Now;
								userNotification.LastAmendedBy = -1;
								userNotification.DateLastAmended = DateTime.Now;
								//----Send Firebase Notification by using Firebase API
								SendUserNotification fbPush = new SendUserNotification(projectId, this.IsSecondaryDatabase, this.SecondaryDatabaseId);
								bool x = fbPush.FirebasePushNotifyAsync(userNotification).Result;
								if (x == true)
								{
									//---insert Notification
									noteDB.insertNotification(out sequence, userNotification);
								}
								//---Write log
								Utilities.WriteLog("Notification has been sent to ids:" + reg_ids);
								#endregion Firebase Notification
								//***************** Send SMS Notifications *****************
								#region send sms Notification
								//---get Client mobile no list
								if (diaryApp.ClientSMSEnabled == true)
								{
									List<DiaryAppsMobileNo> mobilenoList = daDB.getDiaryClientMobileNo(diaryApp.Sequence ?? 0);
									bool isSMSSent = sendSMSMessage(mobilenoList, diaryApp, userId, settings);
								}
								if (diaryApp.DiaryResourceSMSEnabled == true)
								{
									List<DiaryAppsMobileNo> mobilenoList = daDB.getDiaryResourceMobileNo(diaryApp.Sequence ?? 0);
									bool isSMSSent = sendSMSMessage(mobilenoList, diaryApp, userId, settings);
								}
								#endregion sms notification
								//foreach (DiaryAppsMobileNo mobileno in mobilenoList)
								//{
								//    if (mobileno.MobileNo != "" && mobileno.MobileNo != "N/A")
								//    {
								//        sequence = -1;
								//        SMSNotifications smsNote = new SMSNotifications();
								//        DateTime dateAppStart = Convert.ToDateTime( diaryApp.DateAppStart);
								//        smsNote.Message = composeSMSMessage(diaryApp.ResourceSequence, settings, dateAppStart.ToString("dd/MM/yyyy HH:mm tt"));
								//        smsNote.ReceiverId = diaryApp.ResourceSequence;
								//        smsNote.SendTo = mobileno.MobileNo;
								//        smsNote.SentAt = DateTime.Now;
								//        smsNote.CreatedBy = userId;
								//        smsNote.DateCreated = DateTime.Now;
								//        smsNote.LastAmendedBy = -1;
								//        //---insert sms
								//        noteDB.insertSMS(out sequence, smsNote);
								//        ///--- sent sms
								//        SendUserNotification smsPush = new SendUserNotification();
								//        bool y = smsPush.SMSSendNotifyAsync(smsNote.Message, smsNote.SendTo).Result;
								//        if (y==true)
								//        {
								//            //---update marks as sent
								//            smsNote.Sequence = sequence;
								//            smsNote.FlgMarkAsSend = true;
								//            noteDB.updateSMSNotificationAsSent(smsNote);
								//        }
								//    }
								//}

							}
						}else
						{
							returnValue.IsSucessfull = false;
							returnValue.Message = "Client already has appointment in this Time slot ";
						}
                    }
                }
            }
            catch (Exception ex)
            {
				returnValue.IsSucessfull = false;
				returnValue.Message = ex.Message;
            }
            return returnValue;
        }

        public string composeSMSMessage(long resourceSequence, ProjectSettings settings, string appointmentDate)
        {
            string returnMessage = "";
            try
            {
                EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                EntityDetailsCore companyDetail = DetailsCoreDB.getEntityByEntityid(1);
                DiaryResourcesDB drDB = new DiaryResourcesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                DiaryResourceContactDetail resourceContact = drDB.GetDiaryResourceContactByResourceId(resourceSequence);
                if (resourceContact != null)
                {
                    returnMessage = resourceContact.EngineerName + " from " + companyDetail.NameLong + " will be attending an appointment on " + appointmentDate + " if this appointment is not convenient please call " + companyDetail.Telephone + " to rebook. ";
                }
            }
            catch (Exception ex)
            {
                returnMessage = "";
            }
            return returnMessage;
        }

        private bool sendSMSMessage(List<DiaryAppsMobileNo> mobilenoList, DiaryApps diaryApp, int userId, ProjectSettings settings)
        {
            bool returnValue = false;
            UserNotificationsDB noteDB = new UserNotificationsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
            long sequence = -1;
            foreach (DiaryAppsMobileNo mobileno in mobilenoList)
            {
                if (mobileno.MobileNo != "" && mobileno.MobileNo != "N/A")
                {
                    sequence = -1;
                    SMSNotifications smsNote = new SMSNotifications();
                    DateTime dateAppStart = Convert.ToDateTime(diaryApp.DateAppStart);
                    smsNote.Message = composeSMSMessage(diaryApp.ResourceSequence ?? 0, settings, dateAppStart.ToString("dd/MM/yyyy HH:mm tt"));
                    smsNote.ReceiverId = diaryApp.ResourceSequence;
                    smsNote.SendTo = mobileno.MobileNo;
                    smsNote.SentAt = DateTime.Now;
                    smsNote.CreatedBy = userId;
                    smsNote.DateCreated = DateTime.Now;
                    smsNote.LastAmendedBy = -1;
                    smsNote.FlgReminderEnabled = diaryApp.SMSReminderEnabled;
                    //---insert sms
                    noteDB.insertSMS(out sequence, smsNote);
                    ///--- sent sms
                    SendUserNotification smsPush = new SendUserNotification();
                    bool y = smsPush.SMSSendNotifyAsync(smsNote.Message, smsNote.SendTo).Result;
                    if (y == true)
                    {
                        //---update marks as sent
                        smsNote.Sequence = sequence;
                        smsNote.FlgMarkAsSend = true;
                        noteDB.updateSMSNotificationAsSent(smsNote);
                    }
                }
            }
            return returnValue;
        }
        public DiaryApps UpdateDiaryApp(DiaryApps diaryApp, HttpRequest request)
        {
            DiaryApps objDiaryApp = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        DiaryAppsDB daDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        bool isOverlapTimeslot = daDB.checkClientTimeslotDuplicate(diaryApp.DateAppStart, diaryApp.DateAppEnd, (long)diaryApp.ClientId, (long)diaryApp.ResourceSequence, (long)diaryApp.Sequence);
                        if (isOverlapTimeslot == false)
                        {
                            objDiaryApp = daDB.selectAllDiaryAppsSequence(diaryApp.Sequence ?? 0);
                            long oldResourceSequence = objDiaryApp.ResourceSequence ?? 0;
                            if (daDB.updateBySequence(diaryApp.Sequence ?? 0, diaryApp.TransType, diaryApp.JoinResource ?? 0, diaryApp.FlgUseClientId, diaryApp.ClientId ?? 0,
                                                       diaryApp.JobSequence ?? 0, diaryApp.JobAddressId ?? 0, diaryApp.FlgBookingRequired,
                                                       diaryApp.ResourceSequence ?? 0, diaryApp.DateAppStart, diaryApp.DateAppEnd,
                                                       diaryApp.FlgAppAllDay, diaryApp.AppPostCode, diaryApp.AppSubject, diaryApp.AppLocation,
                                                       diaryApp.FlgAppReminder, diaryApp.AppReminderSound, diaryApp.AppReminderMins ?? 0, diaryApp.AppNotes,
                                                       diaryApp.AppCategory, diaryApp.AppAttachmentPath, diaryApp.FlgOnlineMeeting,
                                                       diaryApp.FlgUnavailable, diaryApp.RepeatSequence ?? 0, diaryApp.MultiResourceSequence ?? 0,
                                                       diaryApp.AppType ?? 0, diaryApp.FlgAppDeleted, diaryApp.FlgAppCompleted, diaryApp.FlgAppBroken,
                                                       diaryApp.AppBrokenReason ?? 0, diaryApp.FlgNoAccess, diaryApp.FlgAppConfirmed, diaryApp.DateAppConfirmed,
                                                       diaryApp.AppConfirmedBy, diaryApp.CertSequence ?? 0, diaryApp.VisitStatus ?? 0, diaryApp.VisitVam,
                                                       diaryApp.FlgAppFixed, diaryApp.FlgPrint, diaryApp.PrintUserId ?? 0, diaryApp.UnscheduledDeSeq ?? 0, diaryApp.RateSequence ?? 0,
                                                       userId, DateTime.Now))
                            {
                                objDiaryApp = daDB.selectAllDiaryAppsSequence(diaryApp.Sequence ?? 0);
                                //***************** Send Firebase Notifications if resource is changed *****************
                                if (oldResourceSequence != diaryApp.ResourceSequence)
                                {
                                    #region Firebase Notification
                                    UserNotificationsDB noteDB = new UserNotificationsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                    long sequence = -1;
                                    UserNotifications userNotification = new UserNotifications();
                                    userNotification.title = "Appointment Modified";
                                    userNotification.body = "Appointment is modified";
                                    Microsoft.Extensions.Primitives.StringValues values;
                                    if (request.Headers.TryGetValue("Origin", out values))
                                    {
                                        string action = values.ToString();
                                        userNotification.click_action = "/diary/" + diaryApp.JobSequence;
                                    }

                                    userNotification.data = JsonConvert.SerializeObject(diaryApp); // new JavaScriptSerializer().Serialize(diaryApp);
                                                                                                   //userNotification.data = new JavaScriptSerializer().Serialize(diaryApp);
                                    List<NotificationsToken> tokenList = noteDB.getAllTokenList();
                                    string reg_ids = "";
                                    foreach (NotificationsToken token in tokenList)
                                    {
                                        reg_ids = reg_ids + (reg_ids.Length > 0 ? "," : "") + token.FirebaseToken;
                                    }
                                    userNotification.registration_ids = reg_ids;
                                    userNotification.UserId = userId;
                                    userNotification.CreatedBy = userId;
                                    userNotification.DateCreated = DateTime.Now;
                                    userNotification.LastAmendedBy = -1;
                                    userNotification.DateLastAmended = DateTime.Now;
                                    //----Send Firebase Notification by using Firebase API
                                    SendUserNotification fbPush = new SendUserNotification(projectId, this.IsSecondaryDatabase, this.SecondaryDatabaseId);
                                    bool x = fbPush.FirebasePushNotifyAsync(userNotification).Result;

                                    #endregion Firebase Notification
                                    //***************** Send SMS Notifications *****************
                                    #region send sms Notification
                                    //---get mobile no list
                                    List<DiaryAppsMobileNo> mobilenoList = daDB.getDiaryResourceMobileNo(diaryApp.Sequence ?? 0);
                                    foreach (DiaryAppsMobileNo mobileno in mobilenoList)
                                    {
                                        if (mobileno.MobileNo != "" && mobileno.MobileNo != "N/A")
                                        {
                                            sequence = -1;
                                            SMSNotifications smsNote = new SMSNotifications();
                                            DateTime dateAppStart = Convert.ToDateTime(diaryApp.DateAppStart);
                                            smsNote.Message = composeSMSMessage(diaryApp.ResourceSequence ?? 0, settings, dateAppStart.ToString("dd/MM/yyyy HH:mm tt"));
                                            smsNote.ReceiverId = diaryApp.ResourceSequence;
                                            smsNote.SendTo = mobileno.MobileNo;
                                            smsNote.SentAt = DateTime.Now;
                                            smsNote.CreatedBy = userId;
                                            smsNote.DateCreated = DateTime.Now;
                                            smsNote.LastAmendedBy = -1;
                                            //---insert sms
                                            noteDB.insertSMS(out sequence, smsNote);
                                            ///--- sent sms
                                            SendUserNotification smsPush = new SendUserNotification();
                                            bool y = smsPush.SMSSendNotifyAsync(smsNote.Message, smsNote.SendTo).Result;
                                            if (y == true)
                                            {
                                                //---update marks as sent
                                                smsNote.Sequence = sequence;
                                                smsNote.FlgMarkAsSend = true;
                                                noteDB.updateSMSNotificationAsSent(smsNote);
                                            }
                                        }
                                    }
                                    #endregion sms notification
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objDiaryApp = null;
            }
            return objDiaryApp;
        }

        public Boolean DeleteDiaryApp(DiaryApps diaryApp, HttpRequest request)
        {


            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB daDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (daDB.deleteBySequence(diaryApp.Sequence ?? 0))
                        {
                            // delete Natural Forms
                            DiaryAppNaturalFormsDB naturalFormDB = new DiaryAppNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            naturalFormDB.DeleteNaturalFormsByDeSequence(diaryApp.Sequence ?? 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //  User = null;
            }
            return true;
        }

        public List<DiaryApps> GetAppoinmentsByAppDateAndUserId(DateTime? date, HttpRequest request)
        {
            List<DiaryApps> DiaApp = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                int userId = Int32.Parse(request.Headers["UserId"]);
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsDB daDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        DiaApp = daDB.getAppoinmentsByAppDateAndUserId(date, userId);
                    }
                }
            }
            catch (Exception ex)
            {
                DiaApp = null;
            }
            return DiaApp;
        }

        public List<DiaryAppsAssets> GetDiaryAppsAssets(HttpRequest request, long sequence)
        {
            const string METHOD_NAME = "DiaryAppsRepository.GetDiaryAppsAssets()";
            List<DiaryAppsAssets> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (sequence > 0)
                    {
                        DiaryAppsDB diaryAppsDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = diaryAppsDB.SelectDiaryAppsAssets(sequence);
                    }
                    else
                        Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
                else
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Diary Apps Assets.", ex);
            }
            return returnValue;
        }

        public ResponseModel GetUserAppointmentsByAppDate(HttpRequest request, DateTime? date)
        {
            const string METHOD_NAME = "DiaryAppsRepository.GetUserAppointmentsByAppDate()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (date != null)
                    {
                        DiaryAppsDB diaryAppsDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<DiaryAppsSmart> appointments = diaryAppsDB.GetUserDiaryAppsByDateSmart(date, Utilities.GetUserIdFromRequest(request), true);
                        if (appointments != null)
                        {
                            returnValue.TheObject = appointments;
                            returnValue.IsSucessfull = true;
                        }
                        else
                        {
                            returnValue.Message = diaryAppsDB.ErrorMessage;
                        }
                    }
                    else
                    {
                        returnValue.Message = Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                    }
                }
                else
                {
                    returnValue.Message = Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting User Diary Apps By Date.", ex);
                Utilities.WriteLog(METHOD_NAME + "Error Occured while Getting User Diary Apps By Date:" + ex.Message);
            }
            return returnValue;
        }

        public ResponseModel GetUserAppointmentsByAppDateForTimeEntry(HttpRequest request, DateTime? appStartDate)
        {
            const string METHOD_NAME = "DiaryAppsRepository.GetUserAppointmentsByAppDateForTimeEntry()";            
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (appStartDate != null)
                    {
                        DiaryAppsDB diaryAppsDB = new DiaryAppsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<DiaryAppsSmartForTimeSheet> appointments = diaryAppsDB.GetUserDiaryAppsByDateSmartForTimeEntry(appStartDate, Utilities.GetUserIdFromRequest(request));
                        AppointmentTimeEntryResponseModel appointmentResponse = new AppointmentTimeEntryResponseModel();
                        if (appointments != null)
                            appointmentResponse.Appointments = appointments;
                        else
                            returnValue.Message = diaryAppsDB.ErrorMessage;

                        TmpTimesheetRepository tmpTimesheetRepository = new TmpTimesheetRepository();
                        List<AppointmentTimeEntries> timeEntries = tmpTimesheetRepository.GetAllTimeEntriesByDate(request, appStartDate);
                        if (timeEntries != null)
                        {
                            foreach (AppointmentTimeEntries item in timeEntries)
                            {
                                if (appointments != null)
                                    item.Appointment = appointments.Where(x => x.JobSequence == item.JobSequence).FirstOrDefault();
                            }
                            appointmentResponse.TimeEntries = timeEntries;
                        }
                        returnValue.TheObject = appointmentResponse;
                        returnValue.IsSucessfull = true;
                    }
                    else
                        returnValue.Message = Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
                else
                    returnValue.Message = Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting User Diary Apps For Time Entry By Date.", ex);
                Utilities.WriteLog(METHOD_NAME + "Error Occured while Getting User Diary Apps For Time Entry By Date:" + ex.Message);
            }
            return returnValue;
        }
    }
}
