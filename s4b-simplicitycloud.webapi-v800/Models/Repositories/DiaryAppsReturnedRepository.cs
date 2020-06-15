using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class DiaryAppsReturnedRepository : IDiaryAppsReturnedRepository
    {
        //private ILogger<DiaryAppsReturnedRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }


        public DiaryAppsReturnedRepository()
        {
        }

        public ResponseModel AddDiaryAppReturnedWithOrderStatus(HttpRequest request, DiaryAppsReturned diaryAppReturned)
        {
            const string METHOD_NAME = "DiaryAppsReturnedDB.AddDiaryAppReturnedWithOrderStatus()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                bool isFailed = false;
                int userId = Int32.Parse(request.Headers["UserId"]);
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    long sequence = -1;
                    DiaryAppsReturnedDB diaryAppsReturnedDB = new DiaryAppsReturnedDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (diaryAppsReturnedDB.insertDiaryAppsReturnedByDiaryApp(out sequence, diaryAppReturned.DaSequence ?? 0, diaryAppReturned.VisitStatus, diaryAppReturned.ReturnReason,
                                                                              userId, DateTime.Now, userId, DateTime.Now))
                    {
                        RefVisitStatusTypes refVisitStatusType = new RefVisitStatusTypesRepository().GetRefVisitStatusTypeById(request, diaryAppReturned.VisitStatus);
                        bool flgNoAccess = false;
                        if (refVisitStatusType != null)
                        {
                            flgNoAccess = refVisitStatusType.FlgNoAccess;
                        }
                        DiaryAppsRepository diaryAppsRepo = new DiaryAppsRepository();
                        if (!diaryAppsRepo.updateVisitStatusBySequence(request, diaryAppReturned.DaSequence ?? 0, diaryAppReturned.VisitStatus, flgNoAccess, userId, DateTime.Now))
                        {
                            isFailed = true;
                            returnValue.Message += "Unable to Update diary Appointment Visit Status.";
                        }
                        CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
                        if (cldSettingsRepository.GetCldSettingIsOrderStatusUpdateEnabledOnS4BFormVisitStatusUpdate(request))
                        {
                            int jobStatusId = new RefJobStatusTypeRepository().geOrdertStatusByVisitStatus(diaryAppReturned.VisitStatus);
                            OrdersRepository ordersRepository = new OrdersRepository(null);
                            if (!ordersRepository.boolUpdateOrderStatusSucceeded(request, true, diaryAppReturned.JobSequence ?? 0, jobStatusId,
                                                                                DateTime.Now, diaryAppReturned.ReturnReason, userId, DateTime.Now))
                            {
                                isFailed = true;
                                returnValue.Message += "Unable to Update Order Status. Reason: " + ordersRepository.Message + "\n";
                            }
                        }
                        if (cldSettingsRepository.GetCldSettingIsUserControl1UpdateEnabledOnS4BFormVisitStatusUpdate(request))
                        {
                            if (diaryAppReturned.JobSequence > 0)
                            {
                                OrdersRepository ordersRepository = new OrdersRepository(null);
                                if (!ordersRepository.UpdateFlgUser1AndDateUser1ByJobSequence(request, diaryAppReturned.JobSequence ?? 0, true, DateTime.Now))
                                {
                                    isFailed = true;
                                    returnValue.Message += "Unable to Update User Control 1 for Order. " + ordersRepository.Message;
                                }
                            }
                        }
                        if (cldSettingsRepository.GetCldSettingIsKPICompleteUpdateEnabledOnS4BFormVisitStatusUpdate(request))
                        {
                            if (diaryAppReturned.JobSequence > 0)
                            {
                                OrdersRepository ordersRepository = new OrdersRepository(null);
                                if (!ordersRepository.UpdateFlgJobSLATimerStopAndDateJobSLATimerStopByJobSequence(request, diaryAppReturned.JobSequence ?? 0, true, DateTime.Now))
                                {
                                    isFailed = true;
                                    returnValue.Message += "Unable to Update KPI Complete for Order. " + ordersRepository.Message;
                                }
                            }
                        }
                        if (diaryAppReturned.DaGps != null)
                        {
                            if (!diaryAppsRepo.UpdateDiaryAppGPSDetails(request, diaryAppReturned.DaSequence ?? 0, diaryAppReturned.DaGps, userId, DateTime.Now))
                            {
                                isFailed = true;
                                returnValue.Message += diaryAppsRepo.Message;
                            }
                        }
                        returnValue.IsSucessfull = !isFailed;
                    }
                    else
                    {
                        returnValue.Message += "Unable to insert Diary App Returned. Error is " + diaryAppsReturnedDB.ErrorMessage + "\n";
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message += Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Adding Diary App Returned.", ex);
            }
            return returnValue;
        }
    }
}
