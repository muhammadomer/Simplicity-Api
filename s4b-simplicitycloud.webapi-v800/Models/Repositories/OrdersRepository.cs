using Microsoft.AspNetCore.Http;


using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.DAL;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        
        private ILogger<OrdersRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public OrdersRepository(ILogger<OrdersRepository> logger)
        {
            _logger = logger;
        }

        public OrdersRepository(bool _IsSecondaryDatabase, string _SecondaryDatabaseId)
        {
            this.IsSecondaryDatabase = _IsSecondaryDatabase;
            this.SecondaryDatabaseId = _SecondaryDatabaseId;
        }

        public List<Orders> GetAllOrders(HttpRequest request)
        {
            List<Orders> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderDB.getAllOrders();
                        if (returnValue == null)
                        {
                            //Report back error
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }

        public bool CancelOrderBySequence(Orders order, HttpRequest request)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        order.FlgJobCancelled = true;
                        order.DateCancelled = DateTime.Now;
                        returnValue = orderDB.UpdateCancelFlagBySequence(order);
                         //---Get App Setting For Cancel ref Number
                        long cancelNumber = 0;
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("LOC");
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                           cancelNumber = long.Parse(applicationSettings[0].Setting1);
                           cancelNumber = cancelNumber + 1;
                        }
                        //---insert record in cancel audit
                        OrderCancelAuditDB orderCancelAuditDB = new OrderCancelAuditDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        OrderCancelAudit objCancenAudit = new OrderCancelAudit();
                        objCancenAudit.JobSequence = order.Sequence;
                        objCancenAudit.CancelNotes = order.CancelNotes;
                        objCancenAudit.CancelReference = "0000000"+cancelNumber;
                        objCancenAudit.CancelReference = objCancenAudit.CancelReference.Substring(objCancenAudit.CancelReference.Length - 8, 8);
                        long sequence = 0;
                        orderCancelAuditDB.insert(out sequence,objCancenAudit);
                        //update cancel audit sequence in setting table
                        applicationSettingDB.updateSetting1BySettingId("LOC", sequence.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
               Utilities.WriteLog("Error occured in canceling order:" + ex.Message);
               throw ex;
            }
            return returnValue;
        }

      public bool ReactivateOrderBySequence(Orders order, HttpRequest request)
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
                  OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                  order.FlgJobCancelled = false;
                  order.DateCancelled = null;
                  returnValue = orderDB.UpdateCancelFlagBySequence(order);
               }
            }
         }
         catch (Exception ex)
         {
            Utilities.WriteLog("Error occured in reactivating order:" + ex.Message);
            throw ex;
         }
         return returnValue;
      }
      public List<Orders> GetAllOrdersByJobRef(string jobRef, HttpRequest request)
        {
            List<Orders> returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.GetAllOrdersByJobRef(jobRef);
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }

        public List<OrdersMin> GetAllOrdersMinByJobRef(string jobRef, HttpRequest request)
        {
            List<OrdersMin> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.GetAllOrdersMinByJobRef(jobRef);
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }

        public List<OrdersMin> GetAllOrdersMinByJobClientRef(HttpRequest request, string jobClientRef)
        {
            const string METHOD_NAME = "OrdersRepository.GetAllOrdersMinByJobClientRef()";
            List<OrdersMin> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.GetAllOrdersMinByJobClientRef(jobClientRef);
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Get All Min Orders By Job Client Ref", ex);
            }
            return returnValue;
        }

        public List<Orders> SearchOrders(string key, string field, string match, HttpRequest request)
        {
            List<Orders> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.SearchOrders(key, field, match);
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }

        public List<Orders> SearchOrders(HttpRequest request)
        {
            List<Orders> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.SearchOrders();
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }

        public List<Orders> GetOrdersByClientRef(HttpRequest request, string clientRef)
        {
            List<Orders> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.GetAllOrdersByClientRef(clientRef);
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }

        public Orders GetOrderByJobRef(string jobRef, HttpRequest request)
        {
            Orders returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.getOrderByJobRef(jobRef);
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }

        public Orders GetOrderDetailsBySequence(long sequence, HttpRequest request)
        {
            Orders returnValue = null;
            bool apsConfig = false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");
                    if (applicationSettings != null && applicationSettings.Count > 0)
                    {
                        apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                    }
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.getOrderByJobSequence(sequence,apsConfig);
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }

        public string GetCaptionForActiveProject(HttpRequest request)
        {
            string returnValue = "Convert To JT";
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                    if (applicationSettings != null && applicationSettings.Count > 0)
                    {
                        string value = applicationSettings[0].Setting1.ToString();
                        if (value == "True")
                            returnValue = "Active Project";
                    }
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetAPSConfig(HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                    if (applicationSettings != null && applicationSettings.Count > 0)
                    {
                        return Boolean.Parse(applicationSettings[0].Setting1);
                       
                    }
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool boolUpdateOrderStatusSucceeded(HttpRequest request, bool auditStatus, long jobSequence, int jobStatusId, DateTime? statusDate, 
                                                     string statusDesc, int lastAmendedBy, DateTime? lastAmendedDate)
        {
            const string METHOD_NAME = "OrdersRepository.boolUpdateOrderStatusSucceeded()";
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if(orderDB.UpdateOrderStatusBySequence(jobStatusId, jobSequence, Utilities.GetUserIdFromRequest(request), DateTime.Now))
                    {
                        if(auditStatus)
                        {
                            OrderStatusAuditRepository orderStatusAuditRepos = new OrderStatusAuditRepository();
                            OrderStatusAudit orderStatusAudit = new OrderStatusAudit();
                            orderStatusAudit.CreatedBy = lastAmendedBy;                            
                            orderStatusAudit.DateCreated = lastAmendedDate;
                            orderStatusAudit.DateStatusRef = statusDate;
                            orderStatusAudit.FlgJobClientId = false;
                            orderStatusAudit.FlgStatusRef = false;
                            orderStatusAudit.JobClientId = -1;
                            orderStatusAudit.StatusDesc = statusDesc;
                            orderStatusAudit.StatusRef = "";
                            orderStatusAudit.JobSequence = jobSequence;
                            orderStatusAudit.StatusType = jobStatusId;
                            if (orderStatusAuditRepos.InsertOrderStatusAudit(request, orderStatusAudit))
                            {
                                returnValue = true;
                            }
                            else
                            {
                                Message = orderStatusAuditRepos.Message;
                            }
                        }
                    }
                    else
                    {
                        Message = orderDB.ErrorMessage;
                    }
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating Order Status.", ex);
            }
            return returnValue;
        }

        public List<Orders> GetOrdersByJobRefOrAddressOrClientName(string jobRef, string jobAddress, string jobClientName, HttpRequest request)
        {
            List<Orders> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderDB.GetOrdersByJobRefOrAddressOrClientName(jobRef, jobAddress, jobClientName);
                        if (returnValue == null)
                        {
                            //LOGGER.LogError("Unable to update Order Status " + orderDB.ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        //public List<OrdersMinWithJobAddressClientName> GetOrdersMinByJobRefOrAddressOrClientName(string jobRef, string jobAddress, string jobClientName,string jobClientRef, string ebsJobRef, HttpRequest request)
        //{
        //    const string METHOD_NAME = "OrdersRepository.GetOrdersMinByJobRefOrAddressOrClientName()";
        //    List<OrdersMinWithJobAddressClientName> returnValue = null;
        //    try
        //    {
        //        string projectId = request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                returnValue = orderDB.GetOrdersMinByJobRefOrAddressOrClientName(jobRef, jobAddress, jobClientName,jobClientRef,ebsJobRef);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Loading Orders Min By Job Ref, Address and Client Name", ex);
        //    }
        //    return returnValue;
        //}

        public ResponseModel GetOrdersMinByJobRefOrAddressOrClientName(ClientRequest clientRequest, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderDB.GetOrdersMinByJobRefOrAddressOrClientName(clientRequest, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = orderDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting Order List. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }

        public List<OrdersMinWithJobAddress> GetOrdersMinByJobAddress(long jobAddressId, HttpRequest request)
        {
            const string METHOD_NAME = "OrdersRepository.GetOrdersMinByJobAddress()";
            List<OrdersMinWithJobAddress> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderDB.GetOrdersMinByJobAddress(jobAddressId);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Loading Orders Min By Job Ref, Address and Client Name", ex);
            }
            return returnValue;
        }

        public bool AddFileByEBSJobSequence(long ebsJobSequence, string fileName, string parentFolderNames, Cld_Ord_Labels_Files oiFireProtectionIImages, HttpRequest request, HttpResponse response)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Orders order = orderDB.GetOrdersByEBSJobSequence(ebsJobSequence);
                        if (order != null)
                        {
                            string folderName = order.JobRef;
                            if(!String.IsNullOrEmpty(parentFolderNames))
                            {
                                folderName = folderName + "," + parentFolderNames;
                            }
                            DriveRequest driveRequest = new DriveRequest { Name = fileName, ParentFolderNames = folderName, FireProtectionImages = oiFireProtectionIImages };
                            AttachmentFilesFolderRepository attachmentFileRepos = new AttachmentFilesFolderRepository();
                            AttachmentFiles file = attachmentFileRepos.AddFileInSpecificFolder(driveRequest, request, response);
                            if (file != null)
                            {
                                returnValue = true;
                            }
                            else
                            {
                                //Report Error
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool UpdateJobAddress(OrdersJobAddress jobAddress, HttpRequest request)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderDB.updateJobAddress(jobAddress);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool UpdateJobAddressDetails(Orders order, HttpRequest request)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        order.OccupierEmail = string.IsNullOrEmpty(order.OccupierEmail) ? SimplicityConstants.NotAvailable : order.OccupierEmail;
                        order.OccupierName = string.IsNullOrEmpty(order.OccupierName) ? SimplicityConstants.NotAvailable : order.OccupierName;
                        order.OccupierTelHome = string.IsNullOrEmpty(order.OccupierTelHome) ? SimplicityConstants.NotAvailable : order.OccupierTelHome;
                        order.OccupierTelMobile = string.IsNullOrEmpty(order.OccupierTelMobile) ? SimplicityConstants.NotAvailable : order.OccupierTelMobile;
                        order.OccupierTelWork = string.IsNullOrEmpty(order.OccupierTelWork) ? SimplicityConstants.NotAvailable : order.OccupierTelWork;
                        order.OccupierTelWorkExt = string.IsNullOrEmpty(order.OccupierTelWorkExt) ? SimplicityConstants.NotAvailable : order.OccupierTelWorkExt;
                        order.JobAddressId = order.JobAddressId == null ? -1 : order.JobAddressId;                       
                        returnValue = orderDB.updateJobAddressDetails(order);
                        if(!returnValue)
                        {
                            Message = orderDB.ErrorMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel UpdateJobClient(int sequence,long clientId,string clientName, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        orderDB.updateJobClient(sequence, clientId, clientName);
                        //---Update client audit record
                        #region update job client
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        this.UpdateJobClientAudit(userId,sequence, clientId, request.Headers["ProjectId"]);
                        #endregion update job client
                        returnValue.TheObject = sequence;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While updating job client. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool UpdateJobAddressByAddressId(long addressId, string jobAddress, HttpRequest request)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderDB.updateJobAddressByAddressId(addressId, jobAddress);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool UpdateJobClientName(int sequence, string jobClientName, HttpRequest request)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderDB.updateJobClientName(sequence, jobClientName);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool UpdateJobClientRef(int sequence, string jobClientRef, HttpRequest request)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderDB.updateJobClientRef(sequence, jobClientRef);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool UpdateFlgSetToJTAndDateSetToJTByJobSequence(HttpRequest request, long sequence, bool flgSetToJT, DateTime? datSetToJT)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue = orderDB.UpdateFlgSetToJTAndDateSetToJTByJobSequence(sequence, flgSetToJT, datSetToJT, userId, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return returnValue;
        }

        public bool UpdateFlgJobSLATimerStopAndDateJobSLATimerStopByJobSequence(HttpRequest request, long sequence, bool flgSLATimerStop, DateTime? datSLATimerStop)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue = orderDB.UpdateFlgJobSLATimerStopAndDateJobSLATimerStopByJobSequence(sequence, flgSLATimerStop, datSLATimerStop, userId, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return returnValue;
        }

        public bool UpdateFlgUser1AndDateUser1ByJobSequence(HttpRequest request, long sequence, bool flgUser1, DateTime? datUser1)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue = orderDB.UpdateFlgUser1AndDateUser1ByJobSequence(sequence, flgUser1, datUser1, userId, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return returnValue;
        }

        public bool UpdateUserFlag2AndUserDate2ByJobSequence(HttpRequest request, long sequence, bool flgUser2, DateTime? datUser2)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue = orderDB.updateUserFlag2AndUserDate2ByJobSequence(sequence, flgUser2, datUser2, userId, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool updateFlgJobStartAndJobDateStartByJobSequence(HttpRequest request, long sequence, bool flgJobDateStart, DateTime? jobDateStart)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue = orderDB.updateFlgJobStartAndJobDateStartByJobSequence(sequence, flgJobDateStart, jobDateStart, userId, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool updateFlgJobFinishAndJobDateFinishByJobSequence(HttpRequest request, long sequence, bool flgJobDateFinish, DateTime? jobDateFinish)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue = orderDB.updateFlgJobFinishAndJobDateFinishByJobSequence(sequence, flgJobDateFinish, jobDateFinish, userId, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool updateFlgJobCompletedByJobSequence(HttpRequest request, long sequence, bool flgJobCompleted)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue = orderDB.updateFlgJobCompletedByJobSequence(sequence, flgJobCompleted, userId, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool updateFlgBillProformaByJobSequence(HttpRequest request, long sequence, bool flgBillProforma, DateTime? jobDateFinish)
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
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue = orderDB.updateFlgBillProformaByJobSequence(sequence, flgBillProforma, userId, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool UpdateOrderInfo(Orders order, string infoType, HttpRequest request)
        {
            bool returnValue = false;
            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.updateOrderInfo(order, infoType);
                }
            }

            return returnValue;
        }

        public string GetNewJobRefNo(HttpRequest request, HttpResponse response)
        {
            string returnValue = string.Empty;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    var maxJobRef = orderDB.getMaxJobRef();

                    if (settings.JobRefNumberLength > 0 && maxJobRef.Length > 0)
                    {
                        while (maxJobRef.Length < settings.JobRefNumberLength)
                        {
                            maxJobRef = "0" + maxJobRef;
                        }
                    }

                    returnValue = maxJobRef;
                }
            }catch(Exception ex)
            {
                Utilities.WriteLog("Error occur in getting max job ref:" + ex.Message);
            }
            return returnValue;
        }

        public bool? CanManualCreateJobRefForCreateOrder(HttpRequest request, HttpResponse response)
        {
            bool? returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    returnValue = settings.ManualCreateJobRefForCreateOrder;
                }
            }catch(Exception ex)
            {
                Utilities.WriteLog("Error occur in getting ManualCreateJobRefForCreateOrder:" + ex.Message);
            }
            return returnValue;
        }

        public Orders CreateOrderByJobRef(string jobRef, bool autoCreateJobRef, HttpRequest request, HttpResponse response)
        {
            const string METHOD_NAME = "OrdersRepository.CreateOrderByJobRef()";
            Orders returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    Orders order = orderDB.getOrderByJobRef(jobRef);
                    if (order == null)
                    {
                        long sequence = -1;
                        if (orderDB.insertOrders(out sequence, jobRef, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                        {
                            order = new Orders();
                            order.Sequence = sequence;
                            order.JobRef = jobRef;
                            order.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        if (autoCreateJobRef)
                        {
                            jobRef = GetNewJobRefNo(request, response);
                            long sequence = -1;
                            if (orderDB.insertOrders(out sequence, jobRef, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                            {
                                order = new Orders();
                                order.Sequence = sequence;
                                order.JobRef = jobRef;
                                order.IsSucessfull = true;
                            }
                        }
                        else
                        {
                            order.Message = "Job Ref already exist";
                            order.IsSucessfull = false;
                        }
                    }
                    returnValue = order;
                }
            }
            catch (Exception ex)
            {
                Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Creating Order by Job Ref", ex);
            }
            return returnValue;
        }

        public string GetClientAddressByJobSequence(HttpRequest request, long sequence)
        {
            string returnValue = "";
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderDB.GetClientAddressByJobSequence(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }

        public Orders CreateOrderByJobRef(Orders order, bool autoCreateJobRef, HttpRequest request, HttpResponse response)
        {
            const string METHOD_NAME = "OrdersRepository.CreateOrderByJobRef()";
            Orders returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    Orders oldOrder = orderDB.getOrderByJobRef(order.JobRef);
                    if (oldOrder == null)
                    {
                        long sequence = -1;
                        if (orderDB.insertOrders(out sequence, order, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                        {
                            order.Sequence = sequence;
                            order.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        if (autoCreateJobRef)
                        {
                            string jobRef = GetNewJobRefNo(request, response);
                            long sequence = -1;
                            if (orderDB.insertOrders(out sequence, order, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                            {
                                //order = new Orders();
                                order.Sequence = sequence;
                                order.JobRef = jobRef;
                                order.IsSucessfull = true;
                            }
                        }
                        else
                        {
                            order.Message = "Job Ref already exist";
                            order.IsSucessfull = false;
                            order.StatusCode = -1;
                        }
                    }
                    //--- update Job Manager Audit 
                    if (order.IsSucessfull == true)
                    {
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        if (order.JobManagerId > 0)
                        {
                            this.UpdateJobManagerAudit(userId, order.Sequence ?? 0, Convert.ToInt64( order.JobManagerId), request.Headers["ProjectId"]);
                        }
                    }
                    returnValue = order;
                }
            }
            catch (Exception ex)
            {
                order.Message = "Exception Occured While Creating Order by Job Ref" + ex.Message;
                order.IsSucessfull = false;
                //Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Creating Order by Job Ref", ex);
            }
            return returnValue;
        }

        private bool UpdateJobManagerAudit(int userId, long jobSequence,long jobManagerId,string projectId )
        {
            bool returnValue = false;
            ProjectSettings settings = Configs.settings[projectId];
            try
            {
                //---Update manager audit record
                OrderManagerAuditDB managerAuditDB = new OrderManagerAuditDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                OrderManagerAudit orderManagerAudit = new OrderManagerAudit();
                orderManagerAudit = managerAuditDB.selectActiveOrderManagerAuditByJobSequence(jobSequence);
                //---Make all active flag to finish of selected job
                if (orderManagerAudit.JobManagerId != jobManagerId)
                {
                    managerAuditDB.updateActiveToFinishByJobSequence(jobSequence, userId, DateTime.Now);
                    //---insert new manager audit
                    orderManagerAudit = new OrderManagerAudit();
                    orderManagerAudit.JobManagerId = Convert.ToInt64(jobManagerId);
                    orderManagerAudit.JobSequence = jobSequence;
                    orderManagerAudit.FlgPhaseStart = true;
                    orderManagerAudit.DatePhaseStart = DateTime.Now;
                    orderManagerAudit.PhaseType = -1;
                    managerAuditDB.insertOrderManagerAudit(orderManagerAudit, userId, DateTime.Now);
                    returnValue = true;
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        private bool UpdateJobClientAudit(int userId, long jobSequence, long jobClientId, string projectId)
        {
            bool returnValue = false;
            ProjectSettings settings = Configs.settings[projectId];
            try
            {
                //---Update client audit record
                OrderClientsAuditDB clientAuditDB = new OrderClientsAuditDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                OrderClientAudit orderClientAudit = new OrderClientAudit();
                orderClientAudit = clientAuditDB.selectActiveOrderClientAuditByJobSequence(jobSequence);
                //---Make all active flag to finish of selected job
                if (orderClientAudit.JobClientId != jobClientId)
                {
                    clientAuditDB.updateActiveToFinishByJobSequence(jobSequence, userId, DateTime.Now);
                    //---insert new client audit
                    orderClientAudit = new OrderClientAudit();
                    orderClientAudit.JobClientId = Convert.ToInt64(jobClientId);
                    orderClientAudit.JobSequence = jobSequence;
                    orderClientAudit.FlgPhaseStart = true;
                    orderClientAudit.DatePhaseStart = DateTime.Now;
                    orderClientAudit.PhaseType = -1;
                    clientAuditDB.insertOrderClientAudit(orderClientAudit, userId, DateTime.Now);
                    returnValue = true;
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool UpdateClientNotes(int userId, long jobClientId,string entityDetails, string projectId)
        {
            bool returnValue = false;
            ProjectSettings settings = Configs.settings[projectId];
            try
            {
                //---Update client Note record
                EntityDetailsCoreDB entityDetailtDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                EntityDetailsCore client = new EntityDetailsCore();
                client = entityDetailtDB.getEntityByEntityid(jobClientId);
                client.EntityDetails = entityDetails;
                client.DateLastAmended = DateTime.Now;
                client.LastAmendedBy = userId;

                entityDetailtDB.updateClientInfo(client, "entity_details");
                returnValue = true;
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Orders UpdateOrder(Orders order, HttpRequest request)
        {
            Orders returnValue = order;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        order.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]); 
                        order.LastAmendedDate = DateTime.Now;
                        orderDB.updateOrder(order);
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        //---Create/Update Audit Client
                        #region AuditClient
                        //---Update client audit record
                        if (order.JobClientId>0 )
                        {
                            this.UpdateJobClientAudit(userId, order.Sequence ?? 0, Convert.ToInt64(order.JobClientId), request.Headers["ProjectId"]);
                        }
                        #endregion Audit Client

                        #region Client Notes
                        //---Update client Notes
                        if (order.JobClientId > 0)
                        {
                            this.UpdateClientNotes(userId, Convert.ToInt64(order.JobClientId),order.EntityDetails, request.Headers["ProjectId"]);
                        }
                        #endregion Audit Client

                        #region Update Audit Manager
                        if (order.JobManagerId > 0)
                        {
                            this.UpdateJobManagerAudit(userId, order.Sequence ?? 0, Convert.ToInt64(order.JobManagerId), request.Headers["ProjectId"]);
                        }
                        #endregion Update Audit Manager
                        //---Create/Update Order Notes
                        #region Order Notes
                        if (order.OrderNote !=null)
                        {
                            if (order.OrderNote.Count > 0)
                            {
                                //---Update manager audit record
                                OrdersNotesDB oNoteDB = new OrdersNotesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                                foreach (OrdersNotes orderNote in order.OrderNote)
                                {
                                    if (orderNote.Sequence > 0)
                                    {
                                        orderNote.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                                        orderNote.DateLastAmended = DateTime.Now;
                                        oNoteDB.updateBySequence(orderNote);
                                    }
                                    else
                                    {
                                        orderNote.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
                                        orderNote.DateCreated = DateTime.Now;
                                        long sequence = 0;
                                        oNoteDB.insertOrdersNotes(out sequence, orderNote);
                                    }
                                }
                               order.OrderNote = oNoteDB.getByJobSequence(order.Sequence ?? 0);
                            }
                        }
                        #endregion Order Notes

                        //---Create/Update KPI Notes
                        #region KPI Notes
                        if(order.OrderNoteKPI !=null)
                            if (order.OrderNoteKPI.Count > 0) 
                            {
                                //---Update manager audit record
                                OrderNotesKpiDB oNoteKPIDB = new OrderNotesKpiDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                                foreach (OrderNotesKpi kpiNote in order.OrderNoteKPI)
                                {
                                    if (kpiNote.Sequence > 0)
                                    {
                                        kpiNote.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                                        kpiNote.DateLastAmended = DateTime.Now;
                                        oNoteKPIDB.UpdateNotesKpi(kpiNote);
                                    }
                                    else {
                                        kpiNote.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
                                        kpiNote.DateCreated = DateTime.Now;
                                        long sequence = 0;
                                        oNoteKPIDB.CreateNotesKpi(kpiNote);
                                    }
                                }
                               //---Get KPI Notes
                               order.OrderNoteKPI = oNoteKPIDB.getByJobSequence(order.Sequence ?? 0);
                            }
                        #endregion Order Notes
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }

        public ResponseModel OrdersList(ClientRequest clientRequest, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                //---Write Log file
                Utilities.WriteLog("******************************************");
                Utilities.WriteLog("Method: GetOrdersList:Class:Orders:Date:" + DateTime.Now);
                Utilities.WriteLog("******************************************");
            
                string projectId = request.Headers["ProjectId"];
                bool apsConfig = false;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    Utilities.WriteLog("Get Project Settings On:" + DateTime.Now);
                    if (settings != null)
                    {
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");
                        Utilities.WriteLog("Get application Settings On:" + DateTime.Now);
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                            apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                        }
                        int count = 0;
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                       
                        returnValue.TheObject = orderDB.OrdersList(clientRequest, out count, true, apsConfig);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = orderDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting Order List. " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog(ex.Message);
            }

            return returnValue;
        }

        public DataTable OrdersList2(int size,string projectId, HttpRequest request)
        {
            DataTable returnValue = new DataTable();
            ProjectSettings settings = Configs.settings[projectId];
            if (settings != null)
            {
                ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                DataTable dt = orderDB.OrdersList2(size);
                returnValue = dt;
            }
            return returnValue;
        }

        public void writeTxt(string path, string text)
        {
            try
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(text);
                    tw.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
