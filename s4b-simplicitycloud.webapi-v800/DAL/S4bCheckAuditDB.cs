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
    public class S4bCheckauditDB : MainDB
    {

        public S4bCheckauditDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }
        public List<S4bCheckAuditList> selectAuditList(ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate,long jobSequence, out int count, bool isCountRequired)
        {
            List<S4bCheckAuditList> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(S4bCheckAuditQueries.getSelectCheckAuditList(this.DatabaseType, clientRequest, fromDate, toDate, jobSequence), conn))
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
                            returnValue = new List<S4bCheckAuditList>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_CheckAudit(row));
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
        public bool insert(out long sequence, S4bCheckAudit obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(S4bCheckAuditQueries.insert(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool updateBySequence(OrdersBills obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersBillsQueries.update(this.DatabaseType, obj), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        private S4bCheckAuditList Load_CheckAudit(DataRow row)
        {
            S4bCheckAuditList record = null;
            if (row != null)
            {
                record = new S4bCheckAuditList();
                record.UserName = DBUtil.GetStringValue(row, "user_name");
                record.Sequence = DBUtil.GetLongValue(row, "sequence");
                int chkType = DBUtil.GetIntValue(row, "check_type");
                record.FlgPassed = DBUtil.GetBooleanValue(row, "flg_passed");
                record.Passed = (record.FlgPassed == true )? "Yes" : "No";
                record.JobRef = DBUtil.GetStringValue(row, "job_ref");
                record.JobAddress = DBUtil.GetStringValue(row, "job_address");
                record.PostCode = DBUtil.GetStringValue(row, "address_post_code");
                record.DateSelfIsolation = DBUtil.GetDateValue(row, "date_self_isolation");
                if (record.DateSelfIsolation != null) { 
                    var date = Convert.ToDateTime( record.DateSelfIsolation);
                    date = date.AddDays(14);
                    record.DateSelfIsolation = date;
                }
                record.DateCreated = DBUtil.GetDateValue(row, "date_created");
                record.SelfIsolationNotes = DBUtil.GetStringValue(row, "self_isolation_notes");
                if (chkType == 0)
                    record.CheckDesc = "Not Set";
                else if(chkType == 2)
                    record.CheckDesc = "Site";
                else if(chkType == 3)
                    record.CheckDesc = "Maintenance";
                else if(chkType == 4)
                    record.CheckDesc = "Office";
                else if(chkType == 5)
                    record.CheckDesc = "Start up questions";
            }
            return record;
        }

    }
}
