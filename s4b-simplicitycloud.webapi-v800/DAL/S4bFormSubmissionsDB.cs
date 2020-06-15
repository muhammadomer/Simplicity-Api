using System;
using System.Collections.Generic;
using System.Linq;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Data.OleDb;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Data;

namespace SimplicityOnlineWebApi.DAL
{
    public class S4bFormSubmissionsDB : MainDB
    {
        public S4bFormSubmissionsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {

        }

        public List<S4bFormSubmissions> getFormSubmissionList()
        {
            List<S4bFormSubmissions> returnList = new List<S4bFormSubmissions>();
            try
            {
                using (OleDbConnection connection = this.getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(S4bFormSubmissionsQueries.GetFormSubmissionsList(DatabaseType), connection))
                    {
                        OleDbDataReader dr = cmdObj.ExecuteReader();
                        while (dr.Read())
                        {
                            returnList.Add(LoadSubmissions(dr));
                        }
                        returnList = returnList.OrderByDescending(x => x.Sequence).ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                returnList = null;
            }
            return returnList;
        }

        public List<S4bFormSubmissions> getFormSubmissionListByJobSequence(long jobSequence)
        {
            List<S4bFormSubmissions> returnList = new List<S4bFormSubmissions>();
            try
            {
                using (OleDbConnection connection = this.getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(S4bFormSubmissionsQueries.GetFormSubmissionsListByJobSequence(DatabaseType,jobSequence), connection))
                    {
                        OleDbDataReader dr = cmdObj.ExecuteReader();
                        while (dr.Read())
                        {
                            returnList.Add(LoadSubmissions(dr));
                        }
                        returnList = returnList.OrderByDescending(x => x.Sequence).ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                returnList = null;
            }
            return returnList;
        }

        public List<S4bFormSubmissions> S4BeFormsList(ClientRequest clientRequest, out int count, bool isCountRequired)
        {
            List<S4bFormSubmissions> returnList = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(S4bFormSubmissionsQueries.GetFormSubmissionsList(DatabaseType, clientRequest), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        if (isCountRequired)
                        {
                            count = da.Fill(new DataSet("temp"));
                        }
                        DataTable dt = new DataTable();
						Utilities.WriteLog("First Record number is:" + clientRequest.first);
						if (clientRequest.first >= 0)
						{
							da.Fill(clientRequest.first, clientRequest.rows, dt);
							if (dt.Rows != null && dt.Rows.Count > 0)
							{
								returnList = new List<S4bFormSubmissions>();
								foreach (DataRow row in dt.Rows)
								{
									returnList.Add(LoadSubmissionsFromDataRow(row));
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
                ErrorMessage = "Exception Occured While Getting Submission List. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnList;
        }
        public S4bFormSubmissions getFormSubmissionBySubmitNo(string s4bSubmitNo)
        {
            S4bFormSubmissions returnValue = null;
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(S4bFormSubmissionsQueries.GetFormSubmissionsByS4BSubmitNo(DatabaseType, s4bSubmitNo), connection))
                    {
                        OleDbDataReader dr = cmdObj.ExecuteReader();
                        if (dr.HasRows)
                        {
                            returnValue = new S4bFormSubmissions();
                            dr.Read();
                            returnValue = LoadSubmissions(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public S4bFormSubmissions getFormSubmissionBySequenceNo(long sequenceNo)
        {
            S4bFormSubmissions returnValue = null;
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(S4bFormSubmissionsQueries.GetFormSubmissionsBySequence(DatabaseType, sequenceNo.ToString()), connection))
                    {
                        OleDbDataReader dr = cmdObj.ExecuteReader();
                        if (dr.HasRows)
                        {
                            returnValue = new S4bFormSubmissions();
                            dr.Read();
                            returnValue = LoadSubmissions(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public S4bFormSubmissions getFormSubmissionInfoBySequenceNo(long sequenceNo)
        {
            S4bFormSubmissions returnValue = null;
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(S4bFormSubmissionsQueries.GetFormSubmissionsInfoBySequence(DatabaseType, sequenceNo.ToString()), connection))
                    {
                        OleDbDataReader dr = cmdObj.ExecuteReader();
                        if (dr.HasRows)
                        {
                            returnValue = new S4bFormSubmissions();
                            dr.Read();
                            returnValue = LoadSubmissions(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public S4bFormSubmissions InsertFormSubmission(S4bFormSubmissions s4bFormSubmission)
        {
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(S4bFormSubmissionsQueries.InsertFormSubmissions(DatabaseType, s4bFormSubmission), connection))
                    {
                        if (cmdObj.ExecuteNonQuery() > 0)
                        {
                            s4bFormSubmission.Sequence = Utilities.GetDBAutoNumber(connection);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return s4bFormSubmission;
        }

        public bool UpdateFormSubmission(S4bFormSubmissions s4bFormSubmission)
        {
            bool returnVal = false;
            try
            {
                using (OleDbConnection connection = getDbConnection())
                {
                    using (OleDbCommand cmdObj = new OleDbCommand(S4bFormSubmissionsQueries.UpdateFormSubmissions(DatabaseType, s4bFormSubmission), connection))
                    {
                        if (cmdObj.ExecuteNonQuery() > 0)
                        {
                            returnVal = true;
                        }
                        else
                        {
                            returnVal = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
            }
            return returnVal;
        }

        internal bool UpdateFileCabIdAndPDFCount(S4bFormSubmissions submissionData)
        {
            bool returnValue = false;
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(S4bFormSubmissionsQueries.UpdateFileCabIdAndPdfCountForSubmissionsData(this.DatabaseType, submissionData), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                }
                returnValue = true;
            }
            return returnValue;
        }

        internal bool UpdateFlgCompleted(S4bFormSubmissions submissionData)
        {
            bool returnValue = false;
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(S4bFormSubmissionsQueries.UpdateFlgCompletedForSubmissionsData(this.DatabaseType, submissionData), conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                }
                returnValue = true;
            }
            return returnValue;
        }
        S4bFormSubmissions LoadSubmissions(OleDbDataReader dr)
        {
            S4bFormSubmissions submission = new S4bFormSubmissions();
            try
            {
                submission.RefNatForms = new RefS4bForms();
                submission.Orders = new Orders();
                submission.Sequence = long.Parse(dr["sequence"].ToString());
                submission.S4bSubmitNo = dr["s4b_submit_no"].ToString();
                submission.DateSubmit = Convert.ToDateTime(dr["date_submit"].ToString());
                submission.TemplateName = dr["template_name"].ToString();
                submission.SubmitDetails = dr["submit_details"].ToString();
                submission.FileCabId = dr["file_cab_id"].ToString();
                submission.RefNatForms.FormId = ColumnExists(dr, "form_id") ? dr["form_id"].ToString() : "";
                submission.Submitter = ColumnExists(dr, "submitter") ? dr["submitter"].ToString() : "";
                submission.JobSequence = long.Parse(ColumnExists(dr, "job_sequence") ? dr["job_sequence"].ToString() : "");
                submission.CreatedBy = long.Parse(ColumnExists(dr, "created_by") ? dr["created_by"].ToString() : "");
                submission.DateCreated = Convert.ToDateTime(ColumnExists(dr, "date_created") ? dr["date_created"].ToString() : DateTime.Now.ToString());
                submission.Orders.JobRef = ColumnExists(dr, "job_ref") ? dr["Job_Ref"].ToString() : "";
                submission.Orders.Sequence = long.Parse(ColumnExists(dr, "job_sequence") ? dr["Job_sequence"].ToString() : "-1");
                submission.JobAddressShort = ColumnExists(dr, "job_address_short") ? dr["job_address_short"].ToString() : "";
                submission.Orders.JobAddress = ColumnExists(dr, "job_Address") ? dr["job_Address"].ToString() : "";
            }
            catch (Exception ex)
            {

            }
            return submission;
        }
        private S4bFormSubmissions LoadSubmissionsFromDataRow(DataRow row)
        {
            S4bFormSubmissions submission = null;
            if (row != null)
            {
                submission = new S4bFormSubmissions();
                submission.RefNatForms = new RefS4bForms();
                submission.Orders = new Orders();
                submission.Sequence = DBUtil.GetLongValue(row, "sequence");
                submission.JobSequence = DBUtil.GetLongValue(row, "jobSequence");
                submission.TemplateId = DBUtil.GetStringValue(row, "form_id");
                submission.S4bSubmitNo = DBUtil.GetStringValue(row, "s4b_submit_no");
                submission.DateSubmit = DBUtil.GetDateValue(row, "date_submit");
                submission.TemplateName = DBUtil.GetStringValue(row, "template_name");
                submission.SubmitDetails = DBUtil.GetStringValue(row, "submit_details");
                submission.FileCabId = DBUtil.GetStringValue(row, "file_cab_id");
                submission.RefNatForms.FormId = DBUtil.GetStringValue(row, "form_id") != null ? row["form_id"].ToString() : "";
                submission.Submitter = DBUtil.GetStringValue(row, "submitter") != null ? row["submitter"].ToString() :"";
                submission.Orders.JobRef = DBUtil.GetStringValue(row, "job_ref") != null ? row["job_ref"].ToString() : ""; ;
                submission.JobAddressShort = DBUtil.GetStringValue(row, "job_address_short") != null ? row["job_address_short"].ToString() : ""; ;
                submission.Orders.JobAddress = DBUtil.GetStringValue(row, "job_Address") != null ? row["job_Address"].ToString() : ""; 
                submission.Orders.JobClientName = DBUtil.GetStringValue(row, "job_client_name") != null ? row["job_client_name"].ToString() : ""; 
                submission.Orders.JobClientRef = DBUtil.GetStringValue(row, "job_client_ref") != null ? row["job_client_ref"].ToString() : ""; 
                submission.Orders.JobCostCentre = DBUtil.GetStringValue(row, "job_cost_centre") != null ? row["job_cost_centre"].ToString() : ""; 
                submission.Orders.StatusDescription = DBUtil.GetStringValue(row, "status_desc") != null ? row["status_desc"].ToString() : "";
                submission.Orders.OrderType = DBUtil.GetIntValue(row, "order_type");
                if (submission.Orders.OrderType >= 0)
                {
                    DatabaseInfo dbInfo = new DatabaseInfo();
                    dbInfo.DatabaseType = this.DatabaseType;
                    dbInfo.ConnectionString = this.connectionString;
                    submission.Orders.OrderTypeDesc = new RefOrderTypeDB(dbInfo).getOrderTypeById((long)submission.Orders.OrderType);
                }
               

            }
            return submission;
        }

        internal List<DashboardViewOrdersCountByQuarterAndType> GetDashboardViewForSubmissionByTemplateName(DateTime fromDate, DateTime toDate)
        {
            List<DashboardViewOrdersCountByQuarterAndType> returnValue = null;
            const string METHOD_NAME = "SubmissionDB.GetDashboardViewForSubmissionByTemplateName()";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(S4bFormSubmissionsQueries.SelectSubmissionCountByTemplateName(this.DatabaseType, fromDate, toDate), conn))
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
                                    item.RecordCount = DBUtil.GetIntValue(dr, "SubmissionCount");
                                    item.RecordType = DBUtil.GetStringValue(dr, "TemplateName");
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
    }
}