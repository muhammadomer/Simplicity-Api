using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using SixLabors.ImageSharp.Formats;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrdersKpiDB : MainDB
    {

        public OrdersKpiDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }
        public List<OrdersKpi> selectOutstandingKpiOrderList(ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate, out int count, bool isCountRequired)
        {
            List<OrdersKpi> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersKpiQueries.getOutstandingKpiOrderList(this.DatabaseType, clientRequest, fromDate, toDate), conn))
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
                            returnValue = new List<OrdersKpi>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OutstandingList(row));
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

        public List<OrdersKpi> selectSuccessKpiOrderList(ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate, out int count, bool isCountRequired)
        {
            List<OrdersKpi> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersKpiQueries.getSuccessKpiOrderList(this.DatabaseType, clientRequest, fromDate, toDate), conn))
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
                            returnValue = new List<OrdersKpi>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OutstandingList(row));
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


        private OrdersKpi Load_OutstandingList(DataRow row)
        {
            OrdersKpi record = null;
            if (row != null)
            {
                record = new OrdersKpi();
                record.Sequence = DBUtil.GetLongValue(row, "sequence");
                record.JobRef = DBUtil.GetStringValue(row, "job_ref");
                record.JobClientName = DBUtil.GetStringValue(row, "job_client_name");
                record.JobTradeCode = DBUtil.GetStringValue(row, "job_trade_code");
                record.JobCostCentre = DBUtil.GetStringValue(row, "job_cost_centre");                
                record.JobAddress = DBUtil.GetStringValue(row, "job_address");
                record.JobClientRef = DBUtil.GetStringValue(row, "job_client_ref");
                record.JobDate = DBUtil.GetDateValue(row, "job_date");
                record.JobPriorityCode = DBUtil.GetStringValue(row, "job_priority_code");
                record.JobDateDue = DBUtil.GetDateTimeValue(row, "job_date_due");
                record.FlgJobDateStart = DBUtil.GetBooleanValue(row, "flg_job_date_start");
                record.JobDateStart = DBUtil.GetDateValue(row, "job_date_start");
                record.JobDateFinish = DBUtil.GetDateValue(row, "job_date_finish");
                record.JobManagerName = DBUtil.GetStringValue(row, "job_manager");
                record.OrderType = DBUtil.GetStringValue(row, "order_type_desc_short");
                record.FlgBillPerforma = DBUtil.GetBooleanValue(row, "flg_bill_proforma");

                var JobDateDue = Convert.ToDateTime(record.JobDateDue);
                if (JobDateDue < DateTime.Now)
                    record.KpiStatus = "Expired";
                else if (JobDateDue > DateTime.Now && JobDateDue < (DateTime.Now.AddMinutes(30)))
                    record.KpiStatus = "Expired30";
                else
                    record.KpiStatus = "Active";
            }
            return record;
        }

    }
}
