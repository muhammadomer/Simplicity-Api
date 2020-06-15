using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class PayrollJoinDB : MainDB
    {

        public PayrollJoinDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool Insert(out long sequence, long prReference, string prFullName, long entityId, int userId, long webViewerId,
                           double lunchBreak, double hoursPerWeek, string hoursDesc, double pcentJobCostUplift,
                           int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "PayrollJoinDB.Insert()";
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(PayrollJoinQueries.Insert(this.DatabaseType, prReference, prFullName, entityId, userId, webViewerId, lunchBreak,
                                                                   hoursPerWeek, hoursDesc, pcentJobCostUplift, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Inserting Payroll Join.", ex);
            }
            return returnValue;
        }

        public List<PayrollJoin> Select(long sequence)
        {
            const string METHOD_NAME = "PayrollJoinDB.Select()";
            List<PayrollJoin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(PayrollJoinQueries.Select(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<PayrollJoin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadPayrollJoin(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Selecting Payroll Join.", ex);
            }
            return returnValue;
        }

        public bool Update(long sequence, long prReference, string prFullName, long entityId, int userId, long webViewerId, double lunchBreak,
                           double hoursPerWeek, string hoursDesc, double pcentJobCostUplift, int createdBy, DateTime? dateCreated,
                           int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "PayrollJoinDB.Update()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(PayrollJoinQueries.Update(this.DatabaseType, sequence, prReference, prFullName, entityId, userId,
                                                                   webViewerId, lunchBreak, hoursPerWeek, hoursDesc, pcentJobCostUplift, createdBy,
                                                                   dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating Payroll Join.", ex);
            }
            return returnValue;
        }

        public bool Delete(long sequence)
        {
            const string METHOD_NAME = "PayrollJoinDB.Delete()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(PayrollJoinQueries.Delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Payroll Join.", ex);
            }
            return returnValue;
        }

        private PayrollJoin LoadPayrollJoin(OleDbDataReader dr)
        {
            const string METHOD_NAME = "PayrollJoinDB.LoadPayrollJoin()";
            PayrollJoin payrollJoin = null;
            try
            {
                if (dr != null)
                {
                    payrollJoin = new PayrollJoin();
                    payrollJoin.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    payrollJoin.PrReference = DBUtil.GetLongValue(dr, "pr_reference");
                    payrollJoin.PrFullName = DBUtil.GetStringValue(dr, "pr_full_name");
                    payrollJoin.EntityId = DBUtil.GetLongValue(dr, "entity_id");
                    payrollJoin.UserId = DBUtil.GetIntValue(dr, "user_id");
                    payrollJoin.WebViewerId = DBUtil.GetLongValue(dr, "web_viewer_id");
                    payrollJoin.LunchBreak = DBUtil.GetDoubleValue(dr, "lunch_break");
                    payrollJoin.HoursPerWeek = DBUtil.GetDoubleValue(dr, "hours_per_week");
                    payrollJoin.HoursDesc = DBUtil.GetStringValue(dr, "hours_desc");
                    payrollJoin.PcentJobCostUplift = DBUtil.GetDoubleValue(dr, "pcent_job_cost_uplift");
                    payrollJoin.CreatedBy = DBUtil.GetIntValue(dr, "created_by");
                    payrollJoin.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    payrollJoin.LastAmendedBy = DBUtil.GetIntValue(dr, "last_amended_by");
                    payrollJoin.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While loading Payroll Join.", ex);
            }
            return payrollJoin;
        }
    }
}
