using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.IO;
namespace SimplicityOnlineWebApi.DAL
{
    public class OrdersDB : MainDB
    {
        public OrdersDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertOrders(out long sequence, string jobRef, int createdBy, DateTime? createdDate)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrdersQueries.insert(this.DatabaseType, jobRef, createdBy, createdDate), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(sql, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                sequence = long.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record. '" + METHOD_NAME + "'\n";
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog("Insert Orders:" + ex.Message);
            }
            return returnValue;
        }

        public bool insertOrders(out long sequence, Orders order, int createdBy, DateTime? createdDate)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrdersQueries.insert(this.DatabaseType, order, createdBy, createdDate), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(sql, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                sequence = long.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record. '" + METHOD_NAME + "'\n";
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
                throw ex;
            }
            return returnValue;
        }

        internal List<DashboardViewOrderByClientAndYear> GetDashboardViewForOrdersByClientByYear(int year)
        {
            List<DashboardViewOrderByClientAndYear> returnValue = null;
            const string METHOD_NAME = "OrdersDB.GetDashboardViewForOrdersByClientByYear()";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectNoOrdersClientByYear(this.DatabaseType, year), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                returnValue = new List<DashboardViewOrderByClientAndYear>();
                                while (dr.Read() && count <= SimplicityConstants.DashboardViewMaxNumberOfRecords)
                                {
                                    count++;
                                    DashboardViewOrderByClientAndYear item = new DashboardViewOrderByClientAndYear();
                                    item.Client = DBUtil.GetStringValue(dr, "Client");
                                    item.NoOfOrders = DBUtil.GetIntValue(dr, "NoOfOrders");
                                    returnValue.Add(item);
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Orders By Client For Dashboard View.", ex);
            }
            return returnValue;
        }

        internal List<DashboardViewInvoiceTotalByClient> GetDashboardViewForInvoiceTotalByClient()
        {
            List<DashboardViewInvoiceTotalByClient> returnValue = null;
            const string METHOD_NAME = "OrdersDB.GetDashboardViewForInvoiceTotalByClient()";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectInvoiceTotalByClient(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                returnValue = new List<DashboardViewInvoiceTotalByClient>();
                                while (dr.Read() && count <= SimplicityConstants.DashboardViewMaxNumberOfRecords)
                                {
                                    count++;
                                    DashboardViewInvoiceTotalByClient item = new DashboardViewInvoiceTotalByClient();
                                    item.Client = DBUtil.GetStringValue(dr, "Client");
                                    item.InvoiceTotal = DBUtil.GetIntValue(dr, "InvoiceTotal");
                                    returnValue.Add(item);
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Invoice Total By Client For Dashboard View.", ex);
            }
            return returnValue;
        }

        internal List<DashboardViewOrdersCountByQuarterAndType> GetDashboardViewForOrdersByOrderStatus(DateTime fromDate, DateTime toDate)
        {
            List<DashboardViewOrdersCountByQuarterAndType> returnValue = null;
            const string METHOD_NAME = "OrdersDB.GetDashboardViewForOrdersCountByType()";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectOrdersByOrderStatus(this.DatabaseType, fromDate, toDate), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                returnValue = new List<DashboardViewOrdersCountByQuarterAndType>();
                                while (dr.Read() && count <= SimplicityConstants.DashboardViewMaxNumberOfRecords)
                                {
                                    count++;
                                    DashboardViewOrdersCountByQuarterAndType item = new DashboardViewOrdersCountByQuarterAndType();
                                    item.RecordCount = DBUtil.GetIntValue(dr, "OrderCount");
                                    item.RecordType = DBUtil.GetStringValue(dr, "Status");
                                    returnValue.Add(item);
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Order Total By Type For Dashboard View.", ex);
            }
            return returnValue;
        }

        internal List<DashboardViewOrdersCountByQuarterAndType> GetDashboardViewForOrdersByOrderType(DateTime fromDate, DateTime toDate)
        {
            List<DashboardViewOrdersCountByQuarterAndType> returnValue = null;
            const string METHOD_NAME = "OrdersDB.GetDashboardViewForOrdersCountByType()";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectOrdersCountByOrderType(this.DatabaseType, fromDate, toDate), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                returnValue = new List<DashboardViewOrdersCountByQuarterAndType>();
                                while (dr.Read() && count <= SimplicityConstants.DashboardViewMaxNumberOfRecords)
                                {
                                    count++;
                                    DashboardViewOrdersCountByQuarterAndType item = new DashboardViewOrdersCountByQuarterAndType();
                                    item.RecordCount = DBUtil.GetIntValue(dr, "OrderCount");
                                    item.RecordType = DBUtil.GetStringValue(dr, "OrderType");
                                    returnValue.Add(item);
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Order Total By Type For Dashboard View.", ex);
            }
            return returnValue;
        }

        internal List<DashboardViewOrdersCountByQuarterAndType> GetDashboardViewForOrdersByJobStatus(DateTime fromDate, DateTime toDate)
        {
            List<DashboardViewOrdersCountByQuarterAndType> returnValue = null;
            const string METHOD_NAME = "OrdersDB.GetDashboardViewForOrdersCountByType()";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectOrdersCountByJobStatus(this.DatabaseType, fromDate, toDate), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                int count = 0;
                                returnValue = new List<DashboardViewOrdersCountByQuarterAndType>();
                                while (dr.Read() && count <= SimplicityConstants.DashboardViewMaxNumberOfRecords)
                                {
                                    count++;
                                    DashboardViewOrdersCountByQuarterAndType item = new DashboardViewOrdersCountByQuarterAndType();
                                    item.RecordCount = DBUtil.GetIntValue(dr, "OrderCount");
                                    item.RecordType = DBUtil.GetStringValue(dr, "JobStatus");
                                    returnValue.Add(item);
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Order Total By Type For Dashboard View.", ex);
            }
            return returnValue;
        }

        public Orders getOrderByJobRef(string jobRef)
        {
            Orders returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectAllFieldsByJobRef(this.DatabaseType, jobRef), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadOrderDetails(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<Orders> GetAllOrdersByJobRef(string jobRef)
        {
            List<Orders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetAllOrdersByJobRef(this.DatabaseType, jobRef), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Orders>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrderDetails(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting All Orders by Job Ref " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<Orders> GetAllOrdersByClientRef(string clientRef)
        {
            List<Orders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetAllOrdersByClientRef(this.DatabaseType, clientRef), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Orders>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrderDetails(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting All Orders by Client Ref " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<OrdersMin> GetAllOrdersMinByJobRef(string jobRef)
        {
            List<OrdersMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetAllOrdersMinByJobRef(this.DatabaseType, jobRef), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdersMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrderMinDetails(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting All Orders by Job Ref " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<OrdersMin> GetAllOrdersMinByJobClientRef(string jobClientRef)
        {
            List<OrdersMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetAllOrdersMinByJobClientRef(this.DatabaseType, jobClientRef), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdersMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrderMinDetails(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting All Orders by Job Client Ref " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public List<Orders> SearchOrders(string key, string field, string match)
        {
            List<Orders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SearchOrders(this.DatabaseType, key, field, match), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Orders>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrders(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting All Orders by Job Ref " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<Orders> SearchOrders()
        {
            List<Orders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SearchOrders(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Orders>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrders(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting All Orders by Job Ref " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        internal Orders cancelOrderByJobRef(string jobRef)
        {
            Orders returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersQueries.UpdateCancelFlagByJobRef(this.DatabaseType, jobRef), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        internal bool UpdateCancelFlagBySequence(Orders order)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersQueries.UpdateCancelFlagBySequence(this.DatabaseType, order), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured in updating flag cancel:" + ex.Message);
                throw ex;
            }
            return returnValue;
        }

        internal bool UpdateOrderStatusBySequence(int jobStatus, long sequence, int userId, DateTime? lastModifiedDate)
        {
            const string METHOD_NAME = "OrdersDB.UpdateOrderStatusBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersQueries.UpdateOrderStatusBySequence(this.DatabaseType, sequence, jobStatus, userId, lastModifiedDate), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Updating Order Status By Sequence.", ex);
            }
            return returnValue;
        }

        internal Orders GetOrdersByEBSJobSequence(long ebsJobSequence)
        {
            Orders returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectAllFieldsByJobSequence(this.DatabaseType, ebsJobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadOrderDetails(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        internal Orders getOrderByJobSequence(long jobSequence)
        {
            Orders returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectAllFieldsByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadOrderDetails(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        internal Orders getOrderByJobSequence(long jobSequence, bool apsConfig)
        {
            Orders returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectAllFieldsByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadOrderDetails(dr, apsConfig);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<Orders> getOrdersByJobRef(string jobRef)
        {
            List<Orders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectAllFieldsByJobRefSearch(this.DatabaseType, jobRef), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Orders>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrderDetails(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<Orders> getAllOrders()
        {
            List<Orders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectAllFieldsByActiveOrders(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Orders>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrderDetails(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<Orders> searchOrders()
        {
            List<Orders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectAllFieldsByActiveOrders(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Orders>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrders(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        Orders LoadOrderDetails(OleDbDataReader dr)
        {
            Orders order = null;
            try
            {
                if (dr != null)
                {
                    DateTime? dt;
                    bool chk;
                    double dbl;
                    int i;
                    long longParam;
                    order = new Orders();
                    order.Sequence = long.Parse(dr["sequence"].ToString());
                    order.JobRef = (dr["job_ref"] == null || dr["job_ref"] == DBNull.Value) ? "" : dr["job_ref"].ToString();
                    order.JobManagerId = (dr["job_manager"] == null || dr["job_manager"] == DBNull.Value) ? -1 : long.TryParse(dr["job_manager"].ToString(), out longParam) ? (long)longParam : -1;
                    order.JobStatusId = (dr["job_status"] == null || dr["job_status"] == DBNull.Value) ? -1 : int.TryParse(dr["job_status"].ToString(), out i) ? (int)i : -1;
                    order.JobClientId = (dr["job_client_id"] == null || dr["job_client_id"] == DBNull.Value) ? null : int.TryParse(dr["job_client_id"].ToString(), out i) ? (int?)i : null;
                    order.JobClientName = (dr["job_client_name"] == null || dr["job_client_name"] == DBNull.Value) ? "" : dr["job_client_name"].ToString();
                    order.JobAddress = (dr["job_address"] == null || dr["job_address"] == DBNull.Value) ? "" : dr["job_address"].ToString();
                    order.JobAddressId = (dr["job_address_id"] == null || dr["job_address_id"] == DBNull.Value) ? null : int.TryParse(dr["job_address_id"].ToString(), out i) ? (int?)i : null;
                    order.JobClientRef = (dr["job_client_ref"] == null || dr["job_client_ref"] == DBNull.Value) ? "" : dr["job_client_ref"].ToString();
                    order.FlgUser1 = (dr["flg_user1"] != null && dr["flg_user1"] != DBNull.Value && Boolean.TryParse(dr["flg_user1"].ToString(), out chk)) ? chk : false;
                    order.JobDesc = (dr["job_desc"] == null || dr["job_desc"] == DBNull.Value) ? "" : dr["job_desc"].ToString();
                    order.JobCostCentre = (dr["job_cost_centre"] == null || dr["job_cost_centre"] == DBNull.Value) ? "" : dr["job_cost_centre"].ToString();
                    order.OccupierName = (dr["occupier_name"] == null || dr["occupier_name"] == DBNull.Value) ? "" : dr["occupier_name"].ToString();
                    order.OccupierTelHome = (dr["occupier_tel_home"] == null || dr["occupier_tel_home"] == DBNull.Value) ? "" : dr["occupier_tel_home"].ToString();
                    order.OccupierTelWork = (dr["occupier_tel_work"] == null || dr["occupier_tel_work"] == DBNull.Value) ? "" : dr["occupier_tel_work"].ToString();
                    order.OccupierTelWorkExt = (dr["occupier_tel_work_ext"] == null || dr["occupier_tel_work_ext"] == DBNull.Value) ? "" : dr["occupier_tel_work_ext"].ToString();
                    order.OccupierTelMobile = (dr["occupier_tel_mobile"] == null || dr["occupier_tel_mobile"] == DBNull.Value) ? "" : dr["occupier_tel_mobile"].ToString();
                    order.OccupierEmail = (dr["occupier_email"] == null || dr["occupier_email"] == DBNull.Value) ? "" : dr["occupier_email"].ToString();
                    order.JobOriginator = (dr["job_originator"] == null || dr["job_originator"] == DBNull.Value) ? "" : dr["job_originator"].ToString();
                    order.JobResolution = (dr["job_resolution"] == null || dr["job_resolution"] == DBNull.Value) ? "" : dr["job_resolution"].ToString();
                    order.JobShortDesc = (dr["job_short_desc"] == null || dr["job_short_desc"] == DBNull.Value) ? "" : dr["job_short_desc"].ToString();
                    order.JobDate = Utilities.getDBDate(dr["job_date"]);
                    order.FlgClient = (dr["flg_to_client"] != null && dr["flg_to_client"] != DBNull.Value && Boolean.TryParse(dr["flg_to_client"].ToString(), out chk)) ? chk : false;
                    order.DateClient = Utilities.getDBDate(dr["date_to_client"]);
                    order.FlgUser2 = (dr["flg_user2"] != null && dr["flg_user2"] != DBNull.Value && Boolean.TryParse(dr["flg_user2"].ToString(), out chk)) ? chk : false;
                    order.DateUser2 = Utilities.getDBDate(dr["date_user2"]);
                    order.FlgJT = (dr["flg_set_to_jt"] != null && dr["flg_set_to_jt"] != DBNull.Value && Boolean.TryParse(dr["flg_set_to_jt"].ToString(), out chk)) ? chk : false;
                    order.DateJT = Utilities.getDBDate(dr["date_set_to_jt"]);
                    order.JobDueDate = Utilities.getDBDate(dr["job_date_due"]);
                    order.FlgJobSlaTimerStop = (dr["flg_job_sla_timer_stop"] != null && dr["flg_job_sla_timer_stop"] != DBNull.Value && Boolean.TryParse(dr["flg_job_sla_timer_stop"].ToString(), out chk)) ? chk : false;
                    order.DateJobSlaTimerStop = Utilities.getDBDate(dr["date_job_sla_timer_stop"]);
                    order.FlgJobDateStart = (dr["flg_job_date_start"] != null && dr["flg_job_date_start"] != DBNull.Value && Boolean.TryParse(dr["flg_job_date_start"].ToString(), out chk)) ? chk : false;
                    order.JobDateStart = Utilities.getDBDate(dr["job_date_start"]);
                    order.FlgJobDateFinish = (dr["flg_job_date_finish"] != null && dr["flg_job_date_finish"] != DBNull.Value && Boolean.TryParse(dr["flg_job_date_finish"].ToString(), out chk)) ? chk : false;
                    order.JobDateFinish = Utilities.getDBDate(dr["job_date_finish"]);
                    order.JobPriorityCode = (dr["job_priority_code"] == null || dr["job_priority_code"] == DBNull.Value) ? "" : dr["job_priority_code"].ToString();
                    order.FlgJobCompleted = (dr["flg_job_completed"] != null && dr["flg_job_completed"] != DBNull.Value && Boolean.TryParse(dr["flg_job_completed"].ToString(), out chk)) ? chk : false;
                    order.RetentionPcent = (dr["retention_pcent"] != null && dr["retention_pcent"] != DBNull.Value && Double.TryParse(dr["retention_pcent"].ToString(), out dbl)) ? dbl : 0;
                    order.SalesDiscountPcent = (dr["sales_discount_pcent"] != null && dr["sales_discount_pcent"] != DBNull.Value && Double.TryParse(dr["sales_discount_pcent"].ToString(), out dbl)) ? dbl : 0;
                    order.JobTradeCode = (dr["job_trade_code"] == null || dr["job_trade_code"] == DBNull.Value) ? "" : dr["job_trade_code"].ToString();
                    order.JobTradeCodeDesc = (dr["trade_desc"] == null || dr["trade_desc"] == DBNull.Value) ? "" : dr["trade_desc"].ToString();
                    order.OrderType = (dr["order_type"] == null || dr["order_type"] == DBNull.Value) ? null : int.TryParse(dr["order_type"].ToString(), out i) ? (int?)i : null;
                    order.FlgBillProforma = (dr["flg_bill_proforma"] == null || dr["flg_bill_proforma"] == DBNull.Value) ? false : bool.Parse(dr["flg_bill_proforma"].ToString());
                    order.JobEstDetails = (dr["job_desc"] == null || dr["job_desc"] == DBNull.Value) ? "" : dr["job_desc"].ToString();
                    order.JobVODetails = (dr["job_desc"] == null || dr["job_desc"] == DBNull.Value) ? "" : dr["job_desc"].ToString();

                    DatabaseInfo dbInfo = new DatabaseInfo();
                    dbInfo.DatabaseType = this.DatabaseType;
                    dbInfo.ConnectionString = this.connectionString;
                    if (order.JobManagerId > 0)
                    {
                        order.JobManagerName = new EntityDetailsCoreDB(dbInfo).getEntityByEntityid(order.JobManagerId).NameLong;
                    }
                    if (order.JobStatusId > 0)
                    {
                        order.JobStatus = new RefJobStatusTypeDB(dbInfo).getStatusByStatusId((long)order.JobStatusId);
                    }
                    if (order.OrderType > 0)
                    {
                        order.OrderTypeDesc = new RefOrderTypeDB(dbInfo).getOrderTypeById((long)order.OrderType);
                    }
                    //--- Get Order Notes
                    order.OrderNote = new OrdersNotesDB(dbInfo).getByJobSequence(order.Sequence ?? 0);
                    //---Get KPI Notes
                    order.OrderNoteKPI = new OrderNotesKpiDB(dbInfo).getByJobSequence(order.Sequence ?? 0);
                }
            }
            catch (Exception ex)
            {
            }
            return order;
        }

        JobRefALL LoadAllJobRefAll(OleDbDataReader dr)
        {
            JobRefALL order = null;
            try
            {
                if (dr != null)
                {
                    order = new JobRefALL();
                    order.JobRef = (dr["job_ref"] == null || dr["job_ref"] == DBNull.Value) ? "" : dr["job_ref"].ToString();
                    order.JobAddress = (dr["job_address"] == null || dr["job_address"] == DBNull.Value) ? "" : dr["job_address"].ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return order;
        }

        Orders LoadOrderDetails(OleDbDataReader dr, bool apsConfig)
        {
            Orders order = null;
            try
            {
                if (dr != null)
                {
                    int i;
                    order = new Orders();
                    order.Sequence = DBUtil.GetLongValue(dr, "sequence");// long.Parse(dr["sequence"].ToString());
                    order.JobRef = DBUtil.GetStringValue(dr, "job_ref");// (dr["job_ref"] == null || dr["job_ref"] == DBNull.Value) ? "" : dr["job_ref"].ToString();
                    order.JobManagerId = DBUtil.GetLongValue(dr, "job_manager"); // (dr["job_manager"] == null || dr["job_manager"] == DBNull.Value) ? -1 : long.TryParse(dr["job_manager"].ToString(), out longParam) ? (long)longParam : -1;
                    order.JobStatusId = DBUtil.GetIntValue(dr, "job_status");// (dr["job_status"] == null || dr["job_status"] == DBNull.Value) ? -1 : int.TryParse(dr["job_status"].ToString(), out i) ? (int)i : -1;
                    order.JobClientId = DBUtil.GetLongValue(dr, "job_client_id"); //(dr["job_client_id"] == null || dr["job_client_id"] == DBNull.Value) ? null : int.TryParse(dr["job_client_id"].ToString(), out i) ? (int?)i : null;
                    order.JobClientName = DBUtil.GetStringValue(dr, "job_client_name");// (dr["job_client_name"] == null || dr["job_client_name"] == DBNull.Value) ? "" : dr["job_client_name"].ToString();
                    order.EntityDetails = DBUtil.GetStringValue(dr, "entity_details");
                    long? parentClientId = (dr["parent_client_id"] == null || dr["parent_client_id"] == DBNull.Value) ? null : int.TryParse(dr["parent_client_id"].ToString(), out i) ? (int?)i : null;
                    if (order.JobClientId == parentClientId)
                    {
                        order.ParentClientName = "N/A";
                    }
                    else
                    {
                        order.ParentClientName = DBUtil.GetStringValue(dr, "parent_name_short");// (dr["parent_name_short"] == null || dr["parent_name_short"] == DBNull.Value) ? "" : dr["parent_name_short"].ToString();
                    }
                    order.JobAddress = DBUtil.GetStringValue(dr, "job_address"); // (dr["job_address"] == null || dr["job_address"] == DBNull.Value) ? "" : dr["job_address"].ToString();
                    order.JobAddressId = DBUtil.GetIntValue(dr, "job_address_id"); // (dr["job_address_id"] == null || dr["job_address_id"] == DBNull.Value) ? null : int.TryParse(dr["job_address_id"].ToString(), out i) ? (int?)i : null;
                    order.JobClientRef = DBUtil.GetStringValue(dr, "job_client_ref"); // (dr["job_client_ref"] == null || dr["job_client_ref"] == DBNull.Value) ? "" : dr["job_client_ref"].ToString();
                    order.FlgUser1 = DBUtil.GetBooleanValue(dr, "flg_user1"); //(dr["flg_user1"] != null && dr["flg_user1"] != DBNull.Value && Boolean.TryParse(dr["flg_user1"].ToString(), out chk)) ? chk : false;
                    order.JobDesc = DBUtil.GetStringValue(dr, "job_desc"); // (dr["job_desc"] == null || dr["job_desc"] == DBNull.Value) ? "" : dr["job_desc"].ToString();
                    order.JobCostCentre = DBUtil.GetStringValue(dr, "job_cost_centre");// (dr["job_cost_centre"] == null || dr["job_cost_centre"] == DBNull.Value) ? "" : dr["job_cost_centre"].ToString();
                    order.OccupierName = DBUtil.GetStringValue(dr, "occupier_name");// (dr["occupier_name"] == null || dr["occupier_name"] == DBNull.Value) ? "" : dr["occupier_name"].ToString();
                    order.OccupierTelHome = DBUtil.GetStringValue(dr, "occupier_tel_home");// (dr["occupier_tel_home"] == null || dr["occupier_tel_home"] == DBNull.Value) ? "" : dr["occupier_tel_home"].ToString();
                    order.OccupierTelWork = DBUtil.GetStringValue(dr, "occupier_tel_work");// (dr["occupier_tel_work"] == null || dr["occupier_tel_work"] == DBNull.Value) ? "" : dr["occupier_tel_work"].ToString();
                    order.OccupierTelWorkExt = DBUtil.GetStringValue(dr, "occupier_tel_work_ext");// (dr["occupier_tel_work_ext"] == null || dr["occupier_tel_work_ext"] == DBNull.Value) ? "" : dr["occupier_tel_work_ext"].ToString();
                    order.OccupierTelMobile = DBUtil.GetStringValue(dr, "occupier_tel_mobile");// (dr["occupier_tel_mobile"] == null || dr["occupier_tel_mobile"] == DBNull.Value) ? "" : dr["occupier_tel_mobile"].ToString();
                    order.OccupierEmail = DBUtil.GetStringValue(dr, "occupier_email");// (dr["occupier_email"] == null || dr["occupier_email"] == DBNull.Value) ? "" : dr["occupier_email"].ToString();
                    order.JobOriginator = DBUtil.GetStringValue(dr, "job_originator");// (dr["job_originator"] == null || dr["job_originator"] == DBNull.Value) ? "" : dr["job_originator"].ToString();
                    order.JobResolution = DBUtil.GetStringValue(dr, "job_resolution");// (dr["job_resolution"] == null || dr["job_resolution"] == DBNull.Value) ? "" : dr["job_resolution"].ToString();
                    order.JobShortDesc = DBUtil.GetStringValue(dr, "job_short_desc");// (dr["job_short_desc"] == null || dr["job_short_desc"] == DBNull.Value) ? "" : dr["job_short_desc"].ToString();
                    order.JobDate = Utilities.getDBDate(dr["job_date"]);
                    order.FlgClient = DBUtil.GetBooleanValue(dr, "flg_to_client"); //(dr["flg_to_client"] != null && dr["flg_to_client"] != DBNull.Value && Boolean.TryParse(dr["flg_to_client"].ToString(), out chk)) ? chk : false;
                    order.DateClient = Utilities.getDBDate(dr["date_to_client"]);
                    order.FlgUser2 = DBUtil.GetBooleanValue(dr, "flg_user2"); //(dr["flg_user2"] != null && dr["flg_user2"] != DBNull.Value && Boolean.TryParse(dr["flg_user2"].ToString(), out chk)) ? chk : false;
                    order.DateUser2 = Utilities.getDBDate(dr["date_user2"]);
                    order.FlgJT = DBUtil.GetBooleanValue(dr, "flg_set_to_jt"); //(dr["flg_set_to_jt"] != null && dr["flg_set_to_jt"] != DBNull.Value && Boolean.TryParse(dr["flg_set_to_jt"].ToString(), out chk)) ? chk : false;
                    order.DateJT = Utilities.getDBDate(dr["date_set_to_jt"]);
                    order.JobDueDate = Utilities.getDBDate(dr["job_date_due"]);
                    order.FlgJobSlaTimerStop = DBUtil.GetBooleanValue(dr, "flg_job_sla_timer_stop"); //(dr["flg_job_sla_timer_stop"] != null && dr["flg_job_sla_timer_stop"] != DBNull.Value && Boolean.TryParse(dr["flg_job_sla_timer_stop"].ToString(), out chk)) ? chk : false;
                    order.DateJobSlaTimerStop = Utilities.getDBDate(dr["date_job_sla_timer_stop"]);
                    order.FlgJobDateStart = DBUtil.GetBooleanValue(dr, "flg_job_date_start"); //(dr["flg_job_date_start"] != null && dr["flg_job_date_start"] != DBNull.Value && Boolean.TryParse(dr["flg_job_date_start"].ToString(), out chk)) ? chk : false;
                    order.JobDateStart = Utilities.getDBDate(dr["job_date_start"]);
                    order.FlgJobDateFinish = DBUtil.GetBooleanValue(dr, "flg_job_date_finish"); //(dr["flg_job_date_finish"] != null && dr["flg_job_date_finish"] != DBNull.Value && Boolean.TryParse(dr["flg_job_date_finish"].ToString(), out chk)) ? chk : false;
                    order.JobDateFinish = Utilities.getDBDate(dr["job_date_finish"]);
                    order.JobPriorityCode = DBUtil.GetStringValue(dr, "job_priority_code");// (dr["job_priority_code"] == null || dr["job_priority_code"] == DBNull.Value) ? "" : dr["job_priority_code"].ToString();
                    order.FlgJobCompleted = DBUtil.GetBooleanValue(dr, "flg_job_completed"); //(dr["flg_job_completed"] != null && dr["flg_job_completed"] != DBNull.Value && Boolean.TryParse(dr["flg_job_completed"].ToString(), out chk)) ? chk : false;
                    order.FlgJobCancelled = DBUtil.GetBooleanValue(dr, "flg_cancelled");
                    order.RetentionPcent = DBUtil.GetDoubleValue(dr, "retention_pcent"); //(dr["retention_pcent"] != null && dr["retention_pcent"] != DBNull.Value && Double.TryParse(dr["retention_pcent"].ToString(), out dbl)) ? dbl : 0;
                    order.SalesDiscountPcent = DBUtil.GetDoubleValue(dr, "sales_discount_pcent"); //(dr["sales_discount_pcent"] != null && dr["sales_discount_pcent"] != DBNull.Value && Double.TryParse(dr["sales_discount_pcent"].ToString(), out dbl)) ? dbl : 0;
                    order.JobTradeCode = DBUtil.GetStringValue(dr, "job_trade_code");// (dr["job_trade_code"] == null || dr["job_trade_code"] == DBNull.Value) ? "" : dr["job_trade_code"].ToString();
                    order.JobTradeCodeDesc = DBUtil.GetStringValue(dr, "trade_desc");// (dr["trade_desc"] == null || dr["trade_desc"] == DBNull.Value) ? "" : dr["trade_desc"].ToString();
                    order.OrderType = (dr["order_type"] == null || dr["order_type"] == DBNull.Value) ? null : int.TryParse(dr["order_type"].ToString(), out i) ? (int?)i : null;
                    order.FlgBillProforma = DBUtil.GetBooleanValue(dr, "flg_bill_proforma"); //(dr["flg_bill_proforma"] == null || dr["flg_bill_proforma"] == DBNull.Value) ? false : bool.Parse(dr["flg_bill_proforma"].ToString());
                    order.JobEstDetails = DBUtil.GetStringValue(dr, "job_est_details");// (dr["job_est_details"] == null || dr["job_est_details"] == DBNull.Value) ? "" : dr["job_est_details"].ToString();
                    order.JobVODetails = DBUtil.GetStringValue(dr, "job_v_o_details");// (dr["job_v_o_details"] == null || dr["job_v_o_details"] == DBNull.Value) ? "" : dr["job_v_o_details"].ToString();
                    order.RecordType = GetOrderRecordType(order.FlgJT, order.FlgClient, apsConfig);
                    DatabaseInfo dbInfo = new DatabaseInfo();
                    dbInfo.DatabaseType = this.DatabaseType;
                    dbInfo.ConnectionString = this.connectionString;
                    if (order.JobManagerId > 0)
                    {
                        order.JobManagerName = new EntityDetailsCoreDB(dbInfo).getEntityByEntityid(order.JobManagerId).NameLong;
                    }
                    if (order.JobStatusId > 0)
                    {
                        order.JobStatus = new RefJobStatusTypeDB(dbInfo).getStatusByStatusId((long)order.JobStatusId);
                    }
                    if (order.OrderType > 0)
                    {
                        order.OrderTypeDesc = new RefOrderTypeDB(dbInfo).getOrderTypeById((long)order.OrderType);
                    }
                    //--- Get Order Notes
                    order.OrderNote = new OrdersNotesDB(dbInfo).getByJobSequence(order.Sequence ?? 0);
                    //---Get KPI Notes
                    order.OrderNoteKPI = new OrderNotesKpiDB(dbInfo).getByJobSequence(order.Sequence ?? 0);
                }
            }
            catch (Exception ex)
            {
            }
            return order;
        }

        OrdersMin LoadOrderMinDetails(OleDbDataReader dr)
        {
            OrdersMin order = null;
            try
            {
                if (dr != null)
                {
                    order = new OrdersMin();
                    order.Sequence = long.Parse(dr["sequence"].ToString());
                    order.JobRef = (dr["job_ref"] == null || dr["job_ref"] == DBNull.Value) ? "" : dr["job_ref"].ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return order;
        }

        OrdersMinWithJobAddressClientName LoadOrderMinDetailsWithJobAddressAndJobClientName(DataRow row)
        {
            OrdersMinWithJobAddressClientName order = null;
            try
            {
                if (row != null)
                {
                    order = new OrdersMinWithJobAddressClientName();
                    order.Sequence = DBUtil.GetLongValue(row, "sequence");
                    order.JobRef = DBUtil.GetStringValue(row, "job_ref");
                    order.JobAddress = DBUtil.GetStringValue(row, "job_address");
                    order.JobClientName = DBUtil.GetStringValue(row, "job_client_name");
					order.InvoiceNo = DBUtil.GetStringValue(row, "invoice_no");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return order;
        }

        OrdersMinWithJobAddress LoadOrderMinDetailsWithJobAddress(OleDbDataReader dr)
        {
            OrdersMinWithJobAddress order = null;
            try
            {
                DateTime dt;
                if (dr != null)
                {
                    order = new OrdersMinWithJobAddress();
                    order.Sequence = long.Parse(dr["sequence"].ToString());
                    order.JobRef = (dr["job_ref"] == null || dr["job_ref"] == DBNull.Value) ? "" : dr["job_ref"].ToString();
                    order.JobAddress = this.ColumnExists(dr, "job_address") ? ((dr["job_address"] == null || dr["job_address"] == DBNull.Value) ? "" : dr["job_address"].ToString()) : "";
                    order.JobClientName = (dr["job_client_name"] == null || dr["job_client_name"] == DBNull.Value) ? "" : dr["job_client_name"].ToString();
                    order.ContactName = (dr["name_long"] == null || dr["name_long"] == DBNull.Value) ? "" : dr["name_long"].ToString();
                    order.jobDate = (dr["job_date"] != null && dr["job_date"] != DBNull.Value && DateTime.TryParse(dr["job_date"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null;
                }
            }
            catch (Exception ex)
            {
            }
            return order;
        }

        Orders LoadOrders(OleDbDataReader dr)
        {
            Orders order = null;
            try
            {
                if (dr != null)
                {
                    DateTime dt;
                    bool chk;
                    double dbl;
                    int i;
                    long longParam;
                    order = new Orders();
                    order.Sequence = long.Parse(dr["sequence"].ToString());
                    order.JobRef = (dr["job_ref"] == null || dr["job_ref"] == DBNull.Value) ? "" : dr["job_ref"].ToString();
                    order.JobManagerId = (dr["job_manager"] == null || dr["job_manager"] == DBNull.Value) ? -1 : long.TryParse(dr["job_manager"].ToString(), out longParam) ? (long)longParam : -1;
                    order.JobClientId = (dr["job_client_id"] == null || dr["job_client_id"] == DBNull.Value) ? null : int.TryParse(dr["job_client_id"].ToString(), out i) ? (int?)i : null;
                    order.JobClientName = (dr["job_client_name"] == null || dr["job_client_name"] == DBNull.Value) ? "" : dr["job_client_name"].ToString();
                    order.JobAddress = (dr["job_address"] == null || dr["job_address"] == DBNull.Value) ? "" : dr["job_address"].ToString();
                    order.JobAddressId = (dr["job_address_id"] == null || dr["job_address_id"] == DBNull.Value) ? null : int.TryParse(dr["job_address_id"].ToString(), out i) ? (int?)i : null;
                    order.JobClientRef = (dr["job_client_ref"] == null || dr["job_client_ref"] == DBNull.Value) ? "" : dr["job_client_ref"].ToString();
                    order.FlgUser1 = (dr["flg_user1"] != null && dr["flg_user1"] != DBNull.Value && Boolean.TryParse(dr["flg_user1"].ToString(), out chk)) ? chk : false;
                    order.JobDesc = (dr["job_desc"] == null || dr["job_desc"] == DBNull.Value) ? "" : dr["job_desc"].ToString();
                    order.JobCostCentre = (dr["job_cost_centre"] == null || dr["job_cost_centre"] == DBNull.Value) ? "" : dr["job_cost_centre"].ToString();
                    order.OccupierName = (dr["occupier_name"] == null || dr["occupier_name"] == DBNull.Value) ? "" : dr["occupier_name"].ToString();
                    order.OccupierTelHome = (dr["occupier_tel_home"] == null || dr["occupier_tel_home"] == DBNull.Value) ? "" : dr["occupier_tel_home"].ToString();
                    order.OccupierTelWork = (dr["occupier_tel_work"] == null || dr["occupier_tel_work"] == DBNull.Value) ? "" : dr["occupier_tel_work"].ToString();
                    order.OccupierTelWorkExt = (dr["occupier_tel_work_ext"] == null || dr["occupier_tel_work_ext"] == DBNull.Value) ? "" : dr["occupier_tel_work_ext"].ToString();
                    order.OccupierTelMobile = (dr["occupier_tel_mobile"] == null || dr["occupier_tel_mobile"] == DBNull.Value) ? "" : dr["occupier_tel_mobile"].ToString();
                    order.OccupierEmail = (dr["occupier_email"] == null || dr["occupier_email"] == DBNull.Value) ? "" : dr["occupier_email"].ToString();
                    order.JobOriginator = (dr["job_originator"] == null || dr["job_originator"] == DBNull.Value) ? "" : dr["job_originator"].ToString();
                    order.JobResolution = (dr["job_resolution"] == null || dr["job_resolution"] == DBNull.Value) ? "" : dr["job_resolution"].ToString();
                    order.JobShortDesc = (dr["job_short_desc"] == null || dr["job_short_desc"] == DBNull.Value) ? "" : dr["job_short_desc"].ToString();
                    order.JobDate = (dr["job_date"] != null && dr["job_date"] != DBNull.Value && DateTime.TryParse(dr["job_date"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null;
                    order.FlgClient = (dr["flg_to_client"] != null && dr["flg_to_client"] != DBNull.Value && Boolean.TryParse(dr["flg_to_client"].ToString(), out chk)) ? chk : false;
                    order.DateClient = (dr["date_to_client"] != null && dr["date_to_client"] != DBNull.Value && DateTime.TryParse(dr["date_to_client"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null;
                    order.FlgUser2 = (dr["flg_user2"] != null && dr["flg_user2"] != DBNull.Value && Boolean.TryParse(dr["flg_user2"].ToString(), out chk)) ? chk : false;
                    order.DateUser2 = (dr["date_user2"] != null && dr["date_user2"] != DBNull.Value && DateTime.TryParse(dr["date_user2"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null;
                    order.FlgJT = (dr["flg_set_to_jt"] != null && dr["flg_set_to_jt"] != DBNull.Value && Boolean.TryParse(dr["flg_set_to_jt"].ToString(), out chk)) ? chk : false;
                    order.DateJT = (dr["date_set_to_jt"] != null && dr["date_set_to_jt"] != DBNull.Value && DateTime.TryParse(dr["date_set_to_jt"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null;
                    order.JobDueDate = (dr["job_date_due"] != null && dr["job_date_due"] != DBNull.Value && DateTime.TryParse(dr["job_date_due"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null;
                    order.FlgJobSlaTimerStop = (dr["flg_job_sla_timer_stop"] != null && dr["flg_job_sla_timer_stop"] != DBNull.Value && Boolean.TryParse(dr["flg_job_sla_timer_stop"].ToString(), out chk)) ? chk : false;
                    order.DateJobSlaTimerStop = (dr["date_job_sla_timer_stop"] != null && dr["date_job_sla_timer_stop"] != DBNull.Value && DateTime.TryParse(dr["date_job_sla_timer_stop"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null;
                    order.FlgJobDateStart = (dr["flg_job_date_start"] != null && dr["flg_job_date_start"] != DBNull.Value && Boolean.TryParse(dr["flg_job_date_start"].ToString(), out chk)) ? chk : false;
                    order.JobDateStart = (dr["job_date_start"] != null && dr["job_date_start"] != DBNull.Value && DateTime.TryParse(dr["job_date_start"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null;
                    order.FlgJobDateFinish = (dr["flg_job_date_finish"] != null && dr["flg_job_date_finish"] != DBNull.Value && Boolean.TryParse(dr["flg_job_date_finish"].ToString(), out chk)) ? chk : false;
                    order.JobDateFinish = (dr["job_date_finish"] != null && dr["job_date_finish"] != DBNull.Value && DateTime.TryParse(dr["job_date_finish"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null;
                    order.JobPriorityCode = (dr["job_priority_code"] == null || dr["job_priority_code"] == DBNull.Value) ? "" : dr["job_priority_code"].ToString();
                    order.FlgJobCompleted = (dr["flg_job_completed"] != null && dr["flg_job_completed"] != DBNull.Value && Boolean.TryParse(dr["flg_job_completed"].ToString(), out chk)) ? chk : false;
                    order.RetentionPcent = (dr["retention_pcent"] != null && dr["retention_pcent"] != DBNull.Value && Double.TryParse(dr["retention_pcent"].ToString(), out dbl)) ? dbl : 0;
                    order.SalesDiscountPcent = (dr["sales_discount_pcent"] != null && dr["sales_discount_pcent"] != DBNull.Value && Double.TryParse(dr["sales_discount_pcent"].ToString(), out dbl)) ? dbl : 0;
                    order.JobPriorityCode = (dr["job_trade_code"] == null || dr["job_trade_code"] == DBNull.Value) ? "" : dr["job_trade_code"].ToString();
                    order.JobTradeCode = (dr["job_trade_code"] == null || dr["job_trade_code"] == DBNull.Value) ? "" : dr["job_trade_code"].ToString();
                    order.FlgBillProforma = (dr["flg_bill_proforma"] == null || dr["flg_bill_proforma"] == DBNull.Value) ? false : bool.Parse(dr["flg_bill_proforma"].ToString());

                }
            }
            catch (Exception ex)
            {
            }
            return order;
        }

        internal List<OrdersList> OrdersList(ClientRequest clientRequest, out int count, bool isCountRequired, bool apsConfig)
        {
            List<OrdersList> returnValue = null;
            count = 0;
            Utilities.WriteLog("Request for DB Connection On:" + DateTime.Now);
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    if (isCountRequired)
                    {

                        using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetCountOfOrdersList(this.DatabaseType, clientRequest), conn))
                        {
                            OleDbDataReader dr = objCmdSelect.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    count = Convert.ToInt32(dr[0].ToString());
                                }
                            }
                            Utilities.WriteLog("Fill count on :" + DateTime.Now);
                        }
                    }
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetOrdersList(this.DatabaseType, clientRequest), conn))
                    {
                        Utilities.WriteLog("Set Data Adapter For Selecting records :" + DateTime.Now);
                        da.SelectCommand = objCmdSelect;
                        DataTable dt = new DataTable();
                        da.Fill(clientRequest.first, clientRequest.rows, dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrdersList>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadOrderFromDataRow(row, apsConfig));
                            }
                        }
                        else
                        {
                            ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting Order List. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
                Utilities.WriteLog("Exception Occured While Getting Order List. " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        //---Entered For testing purpose to test performance (It does not use loop to populate records
        internal DataTable OrdersList2(int size)
        {
            DataTable returnValue = null;
            Utilities.WriteLog("Request for DB Connection On:" + DateTime.Now);
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    Utilities.WriteLog("Get DB Connection On :" + DateTime.Now);
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetOrdersList2(this.DatabaseType, size), conn))
                    {
                        Utilities.WriteLog("Create Data Adapter :" + DateTime.Now);
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        Utilities.WriteLog("Data Adapter Created on :" + DateTime.Now);

                        DataTable dt = new DataTable();
                        Utilities.WriteLog("Request to Fill records on :" + DateTime.Now);
                        da.Fill(1, size, dt);
                        Utilities.WriteLog("Fill records on:" + DateTime.Now);
                        returnValue = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Getting Order List. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
                Utilities.WriteLog("Exception Occured While Getting Order List. " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        private OrdersList LoadOrderFromDataRow(DataRow row, bool apsConfig)
        {
            OrdersList returnValue = null;
            if (row != null)
            {
                returnValue = new OrdersList();
                returnValue.Sequence = DBUtil.GetLongValue(row, "sequence");
                returnValue.JobRef = DBUtil.GetStringValue(row, "job_ref");
                returnValue.JobClientName = DBUtil.GetStringValue(row, "job_client_name");
                returnValue.JobClientRef = DBUtil.GetStringValue(row, "job_client_ref");
                returnValue.JobTradeCode = DBUtil.GetStringValue(row, "job_trade_code");
                returnValue.StatusDescription = DBUtil.GetStringValue(row, "status_desc");
                returnValue.JobDate = DBUtil.GetDateValue(row, "job_date");
                returnValue.FlgJT = DBUtil.GetBooleanValue(row, "flg_set_to_jt");
                returnValue.DateJT = DBUtil.GetDateValue(row, "date_set_to_jt");
                returnValue.FlgClient = DBUtil.GetBooleanValue(row, "flg_to_client");
                returnValue.DateClient = DBUtil.GetDateValue(row, "date_to_client");
                returnValue.FlgJobCompleted = DBUtil.GetBooleanValue(row, "flg_job_completed");
                returnValue.FlgJobCancelled = DBUtil.GetBooleanValue(row, "flg_cancelled");
                //returnValue.FlgNoAccess = DBUtil.GetBooleanValue(row, "flg_no_access_ref_set");
                returnValue.AddressPostCode = DBUtil.GetStringValue(row, "address_post_code");
                returnValue.JobAddress = DBUtil.GetStringValue(row, "job_address");
                returnValue.RecordType = GetOrderRecordType(returnValue.FlgJT, returnValue.FlgClient, apsConfig);
                returnValue.OrderType = DBUtil.GetIntValue(row, "order_type");
                returnValue.OrderTypeDesc = new RefOrderType();
                if (returnValue.OrderType >= 0)
                {
                    DatabaseInfo dbInfo = new DatabaseInfo();
                    dbInfo.DatabaseType = this.DatabaseType;
                    dbInfo.ConnectionString = this.connectionString;
                    returnValue.OrderTypeDesc = new RefOrderTypeDB(dbInfo).getOrderTypeById((long)returnValue.OrderType);
                }

            }
            return returnValue;
        }

        public string GetOrderRecordType(bool flgJT, bool flgClient, bool apsConfig)
        {
            string returnValue = SimplicityConstants.ORDER_RECORD_TYPE_ENQ;
            if (flgJT)
            {
                if (apsConfig)
                {
                    returnValue = SimplicityConstants.ORDER_RECORD_TYPE_AP;
                }
                else
                {
                    returnValue = SimplicityConstants.ORDER_RECORD_TYPE_ORD;
                }
            }
            else if (flgClient)
            {
                if (apsConfig)
                {
                    returnValue = SimplicityConstants.ORDER_RECORD_TYPE_TEN;
                }
                else
                {
                    returnValue = SimplicityConstants.ORDER_RECORD_TYPE_EST;
                }
            }
            return returnValue;
        }

        public List<Orders> GetOrdersByJobRefOrAddressOrClientName(string jobRef, string jobAddress, string jobClientName)
        {
            List<Orders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetOrdersByJobRefOrAddressOrClientName(this.DatabaseType, jobRef, jobAddress, jobClientName), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Orders>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrderDetails(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<OrdersMinWithJobAddressClientName> GetOrdersMinByJobRefOrAddressOrClientName(ClientRequest clientRequest, out int count, bool isCountRequired)
        {
            List<OrdersMinWithJobAddressClientName> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                      new OleDbCommand(OrdersQueries.GetOrdersMinByJobRefOrAddressOrClientName(this.DatabaseType, clientRequest), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        if (isCountRequired)
                        {
                            count = da.Fill(new DataSet("temp"));
                        }
                        DataTable dt = new DataTable();
                        da.Fill(clientRequest.first, clientRequest.rows, dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrdersMinWithJobAddressClientName>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadOrderMinDetailsWithJobAddressAndJobClientName(row));
                            }
                        }
                        else
                        {
                            ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public string GetClientAddressByJobSequence(long sequence)
        {
            string returnValue = "";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetClientAddressByJobSequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    returnValue = dr["address_full"].ToString();
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while Getting address by Job Ref " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<OrdersMinWithJobAddress> GetOrdersMinByJobAddress(long jobAddressId)
        {
            List<OrdersMinWithJobAddress> returnValue = new List<OrdersMinWithJobAddress>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.GetOrdersMinByJobAddress(this.DatabaseType, jobAddressId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrderMinDetailsWithJobAddress(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        internal bool updateJobAddress(OrdersJobAddress jobAddress)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersQueries.UpdateJobAddress(this.DatabaseType, jobAddress), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateJobAddressDetails(Orders order)
        {
            bool returnValue = false;
            try
            {

                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersQueries.UpdateJobAddress(this.DatabaseType, order.Sequence ?? 0, order.JobAddress, (int)order.JobAddressId, order.OccupierName, order.OccupierTelHome, order.OccupierTelWork, order.OccupierTelMobile, order.OccupierEmail), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        internal bool updateJobClient(int Sequence, long JobClientId, string JobClientName)
        {
            bool returnValue = false;
            try
            {

                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersQueries.UpdateJobClient(this.DatabaseType, Sequence, JobClientId, JobClientName), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateJobAddressByAddressId(long addressId, string jobAddress)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersQueries.UpdateJobAddressByAddressId(this.DatabaseType, addressId, jobAddress), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateJobClientName(int sequence, string jobClientName)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersQueries.UpdateJobClientName(this.DatabaseType, sequence, jobClientName), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateJobClientRef(int sequence, string jobClientRef)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersQueries.UpdateJobClientRef(this.DatabaseType, sequence, jobClientRef), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool UpdateFlgSetToJTAndDateSetToJTByJobSequence(long sequence, bool flgSetToJT, DateTime? datSetToJT,
                                                                  int userId, DateTime? lastModifiedDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                       new OleDbCommand(OrdersQueries.UpdateFlgSetToJTAndDateSetToJTByJobSequence(this.DatabaseType, sequence, flgSetToJT,
                                                                                                  datSetToJT, userId, lastModifiedDate), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool UpdateFlgJobSLATimerStopAndDateJobSLATimerStopByJobSequence(long sequence, bool flgSLATimerStop, DateTime? slaTimerStopDate,
                                                                                  int userId, DateTime? lastModifiedDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                       new OleDbCommand(OrdersQueries.UpdateFlgJobSLATimerStopAndDateJobSLATimerStopByJobSequence(this.DatabaseType, sequence, flgSLATimerStop,
                                                                                                                  slaTimerStopDate, userId, lastModifiedDate), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool UpdateFlgUser1AndDateUser1ByJobSequence(long sequence, bool flgUser1, DateTime? datUser1,
                                                   int userId, DateTime? lastModifiedDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                       new OleDbCommand(OrdersQueries.UpdateFlgUser1AndDateUser1ByJobSequence(this.DatabaseType, sequence, flgUser1,
                                                                                              datUser1, userId, lastModifiedDate), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateUserFlag2AndUserDate2ByJobSequence(long sequence, bool flgUser2, DateTime? datUser2,
                                                               int userId, DateTime? lastModifiedDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                       new OleDbCommand(OrdersQueries.UpdateUserFlag2AndDateUser2ByJobSequence(this.DatabaseType, sequence, flgUser2,
                                                                                               datUser2, userId, lastModifiedDate), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateFlgJobStartAndJobDateStartByJobSequence(long sequence, bool flgJobDateStart, DateTime? jobDateStart,
                                                                    int userId, DateTime? lastModifiedDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                       new OleDbCommand(OrdersQueries.UpdateFlgJobStartAndJobDateStartByJobSequence(this.DatabaseType, sequence, flgJobDateStart,
                                                                                                    jobDateStart, userId, lastModifiedDate), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateFlgJobFinishAndJobDateFinishByJobSequence(long sequence, bool flgJobDateFinish, DateTime? jobDateFinish,
                                                                      int userId, DateTime? lastModifiedDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                       new OleDbCommand(OrdersQueries.UpdateFlgJobFinishAndJobDateFinishByJobSequence(this.DatabaseType, sequence, flgJobDateFinish,
                                                                                                      jobDateFinish, userId, lastModifiedDate), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateFlgJobCompletedByJobSequence(long sequence, bool flgJobCompleted,
                                                         int userId, DateTime? lastModifiedDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                       new OleDbCommand(OrdersQueries.updateFlgJobCompletedByJobSequence(this.DatabaseType, sequence, flgJobCompleted,
                                                                                         userId, lastModifiedDate), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        internal bool updateFlgBillProformaByJobSequence(long sequence, bool flgBillProforma,
                                                         int userId, DateTime? lastModifiedDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                       new OleDbCommand(OrdersQueries.UpdateFlgBillProformaBySequence(this.DatabaseType, sequence, flgBillProforma,
                                                                                      userId, lastModifiedDate), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal bool updateOrderInfo(Orders order, string infoType)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(OrdersQueries.UpdateOrderInfo(this.DatabaseType, order, infoType), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                }
                returnValue = true;
            }

            return returnValue;
        }

        internal List<Orders> selectAllOI_FireProtection_IByJobSequenceAndTagNumberSearch(bool isJobRef, string jobRef, bool isTag, string tagNo, bool isTagCreatedDate, DateTime? tagCreatedDate, bool isTagUser, int tagUser)
        {
            List<Orders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {

                    using (OleDbCommand objCmdSelect = new OleDbCommand(OrdersQueries.SelectAllFieldsBySearchCriteria(this.DatabaseType, isJobRef, jobRef, isTag, tagNo, isTagCreatedDate, tagCreatedDate, isTagUser, tagUser), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<Orders>();
                                Orders order = null;
                                Cld_Ord_Labels oiFireProtectionI = null;
                                Cld_Ord_Labels_Files oiFireProtectionIImages = null;
                                while (dr.Read())
                                {
                                    order = returnValue.FirstOrDefault(x => x.Sequence == long.Parse(dr["sequence"].ToString()));
                                    if (order == null)
                                    {
                                        order = LoadOrderDetails(dr);
                                        returnValue.Add(order);
                                    }

                                    if (order.OI_FireProtection_I == null)
                                    {
                                        order.OI_FireProtection_I = new List<Cld_Ord_Labels>();
                                    }

                                    if (dr["un_cld_ord_labels_sequence"] != null && String.IsNullOrWhiteSpace(dr["un_cld_ord_labels_sequence"].ToString()) == false)
                                    {
                                        oiFireProtectionI = order.OI_FireProtection_I.FirstOrDefault(x => x.Sequence == long.Parse(dr["un_cld_ord_labels_sequence"].ToString()));
                                        if (oiFireProtectionI == null)
                                        {
                                            oiFireProtectionI = new Cld_Ord_Labels();
                                            oiFireProtectionI.Sequence = long.Parse(dr["un_cld_ord_labels_sequence"].ToString());
                                            oiFireProtectionI.JobSequence = long.Parse(dr["un_cld_ord_labels_job_sequence"].ToString());
                                            oiFireProtectionI.TagNo = (string)dr["un_cld_ord_labels_tag_no"];

                                            order.OI_FireProtection_I.Add(oiFireProtectionI);
                                        }
                                    }

                                    if (oiFireProtectionI == null)
                                    {
                                        oiFireProtectionI = new Cld_Ord_Labels();
                                    }
                                    if (oiFireProtectionI.OI_FireProtection_I_Images == null)
                                    {
                                        oiFireProtectionI.OI_FireProtection_I_Images = new List<Cld_Ord_Labels_Files>();
                                    }

                                    if (dr["un_cld_ord_labels_files_sequence"] != null && String.IsNullOrWhiteSpace(dr["un_cld_ord_labels_files_sequence"].ToString()) == false)
                                    {
                                        oiFireProtectionIImages = new Cld_Ord_Labels_Files();
                                        oiFireProtectionIImages.Sequence = long.Parse(dr["un_cld_ord_labels_files_sequence"].ToString());
                                        oiFireProtectionIImages.JobSequence = long.Parse(dr["un_cld_ord_labels_files_job_sequence"].ToString());
                                        oiFireProtectionIImages.OiSequence = (int)dr["un_cld_ord_labels_files_oi_sequence"];
                                        oiFireProtectionIImages.HeaderSequence = (int)dr["un_cld_ord_labels_files_header_sequence"];
                                        oiFireProtectionIImages.JoinSequence = (int)dr["un_cld_ord_labels_files_join_sequence"];
                                        oiFireProtectionIImages.FlgDeleted = (bool)dr["un_cld_ord_labels_files_flg_deleted"];
                                        oiFireProtectionIImages.FileDate = Utilities.getDBDate(dr["un_cld_ord_labels_files_date_file_date"]);
                                        oiFireProtectionIImages.FileDesc = Utilities.GetDBString(dr["un_cld_ord_labels_files_file_desc"]);
                                        oiFireProtectionIImages.ImageURL = Utilities.GetDBString(dr["un_cld_ord_labels_files_image_url"]);
                                        if (dr["un_cld_ord_labels_files_logo_url"] == DBNull.Value || dr["un_cld_ord_labels_files_logo_url"] == null)
                                        {
                                            oiFireProtectionIImages.LogoURL = Utilities.GetDBString(dr["un_cld_ord_labels_files_image_url"]);
                                        }
                                        else
                                        {
                                            oiFireProtectionIImages.LogoURL = Utilities.GetDBString(dr["un_cld_ord_labels_files_logo_url"]);
                                        }
                                        oiFireProtectionIImages.FileNameAndPath = Utilities.GetDBString(dr["un_cld_ord_labels_files_file_name_and_path"]);
                                        oiFireProtectionIImages.CreatedBy = (int)dr["un_cld_ord_labels_files_created_by"];
                                        oiFireProtectionIImages.DateCreated = (DateTime)dr["un_cld_ord_labels_files_date_created"];
                                        oiFireProtectionIImages.LastAmendedBy = (int)dr["un_cld_ord_labels_files_last_amended_by"];
                                        oiFireProtectionIImages.DateLastAmended = (DateTime)dr["un_cld_ord_labels_files_date_last_amended"];
                                        oiFireProtectionIImages.ImageName = Utilities.GetDBString(dr["user_name"]);
                                        //oiFireProtectionIImages.ImageUser = Utilities.getDBString(dr["un_cld_ord_labels_files_image_user_name"]);

                                        //oiFireProtectionIImages.AddInfoSequence = dr["un_cld_ord_labels_files_add_info_sequence"].ToString().Equals("") ? 0 : long.Parse(dr["un_cld_ord_labels_files_add_info_sequence"].ToString());
                                        //oiFireProtectionIImages.AddInfo = ColumnExists(dr, "un_cld_ord_labels_files_add_info") ? Utilities.getDBString(dr["un_cld_ord_labels_files_add_info"]) : "";
                                        oiFireProtectionIImages.DriveFileId = ColumnExists(dr, "un_cld_ord_labels_files_drive_file_id") ? Utilities.GetDBString(dr["un_cld_ord_labels_files_drive_file_id"]) : "";

                                        oiFireProtectionI.OI_FireProtection_I_Images.Add(oiFireProtectionIImages);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        internal List<JobRefALL> GetJobRefListForTimeSheet(bool isJobRef, string jobRef, bool isTag, string tagNo, bool isTagCreatedDate, DateTime? tagCreatedDate, bool isTagUser, int tagUser)
        {
            List<JobRefALL> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(OrdersQueries.GetJobRefListForTimeSheet(this.DatabaseType, isJobRef, jobRef, isTag, tagNo, isTagCreatedDate, tagCreatedDate, isTagUser, tagUser), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<JobRefALL>();

                                while (dr.Read())
                                {
                                    JobRefALL item = LoadAllJobRefAll(dr);
                                    returnValue.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public string getMaxJobRef()
        {
            String returnValue = "1";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersQueries.SelectMaxJobRef(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = dr[0].ToString();
                                long lngValue = long.Parse(returnValue) + 1;
                                returnValue = lngValue.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occur in getting max job ref:" + ex.Message + ":" + this.DatabaseType);
            }
            return returnValue;
        }

        internal bool updateOrder(Orders order)
        {
            bool returnValue = false;

            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(OrdersQueries.UpdateOrder(this.DatabaseType, order), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                }
                returnValue = true;
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
