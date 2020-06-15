using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public DashboardRepository()
        {
        }

        public ResponseModel GetDashboardView(HttpRequest request, DateTime date)
        {
            ResponseModel returnValue = new ResponseModel();

            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    Dictionary<string, object> returnList = new Dictionary<string, object>();
                    OrdersDB ordersDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<DashboardViewOrderByClientAndYear>  orderClientCountByYear = ordersDB.GetDashboardViewForOrdersByClientByYear(date.Year);
                    returnList.Add("OrdersByClientByYear", orderClientCountByYear);
                    List<DashboardViewInvoiceTotalByClient> invoiceTotalByClient = ordersDB.GetDashboardViewForInvoiceTotalByClient();
                    returnList.Add("InvoiceTotalByClient", invoiceTotalByClient);
                   
                    returnValue.TheObject = returnList;
                    returnValue.IsSucessfull = true;
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting DashboardView. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetDashboardViewForOrdersByOrderType(HttpRequest request, DateTime fromDate, DateTime toDate)
        {
            ResponseModel returnValue = new ResponseModel();

            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    Dictionary<string, object> returnList = new Dictionary<string, object>();
                    OrdersDB ordersDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<DashboardViewOrdersCountByQuarterAndType> OrderCountByOrderType = ordersDB.GetDashboardViewForOrdersByOrderType(fromDate, toDate);
                    returnList.Add("OrderCountByOrderType", OrderCountByOrderType);
                    returnValue.TheObject = returnList;
                    returnValue.IsSucessfull = true;
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting DashboardView. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetDashboardViewForOrdersByOrderStatus(HttpRequest request, DateTime fromDate, DateTime toDate)
        {
            ResponseModel returnValue = new ResponseModel();

            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    Dictionary<string, object> returnList = new Dictionary<string, object>();
                    OrdersDB ordersDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<DashboardViewOrdersCountByQuarterAndType> OrderCountByOrderStatus = ordersDB.GetDashboardViewForOrdersByOrderStatus(fromDate, toDate);
                    returnList.Add("OrderCountByOrderStatus", OrderCountByOrderStatus);
                    returnValue.TheObject = returnList;
                    returnValue.IsSucessfull = true;
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting DashboardView. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetDashboardViewForOrdersByJobStatus(HttpRequest request, DateTime fromDate, DateTime toDate)
        {
            ResponseModel returnValue = new ResponseModel();

            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    Dictionary<string, object> returnList = new Dictionary<string, object>();
                    OrdersDB ordersDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<DashboardViewOrdersCountByQuarterAndType> orderCountByJobStatus = ordersDB.GetDashboardViewForOrdersByJobStatus(fromDate, toDate);
                    returnList.Add("OrderCountByJobStatus", orderCountByJobStatus);
                    returnValue.TheObject = returnList;
                    returnValue.IsSucessfull = true;
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting DashboardView. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetDashboardViewForSubmissionByTemplate(HttpRequest request, DateTime fromDate, DateTime toDate)
        {
            ResponseModel returnValue = new ResponseModel();

            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    Dictionary<string, object> returnList = new Dictionary<string, object>();
                    S4bFormSubmissionsDB s4bFormsDB = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<DashboardViewOrdersCountByQuarterAndType> submissionCountByTemplate = s4bFormsDB.GetDashboardViewForSubmissionByTemplateName(fromDate, toDate);
                    returnList.Add("SubmissionCountByTemplate", submissionCountByTemplate);
                    returnValue.TheObject = returnList;
                    returnValue.IsSucessfull = true;
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting DashboardView. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
    }
}