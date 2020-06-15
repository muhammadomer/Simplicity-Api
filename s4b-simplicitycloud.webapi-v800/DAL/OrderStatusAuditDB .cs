using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;


namespace SimplicityOnlineWebApi.DAL
{
    public class OrderStatusAuditDB : MainDB
    {

        public OrderStatusAuditDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertOrderStatusAudit(out long sequence, long jobSequence, long statusType, bool flgJobClientId, long jobClientId, bool flgStatusRef, string statusRef, DateTime? dateStatusRef,
                                           string statusRef2, string statusDesc, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderStatusAuditQueries.insert(this.DatabaseType, jobSequence, statusType, flgJobClientId, jobClientId, flgStatusRef, statusRef, dateStatusRef, statusRef2, statusDesc, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string OleDb = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(OleDb, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                sequence = long.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.'" + METHOD_NAME + "'\n";
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
            }
            return returnValue;
        }


        public List<OrderStatusAudit> selectAllOrderStatusAuditSequence(long sequence)
        {
            List<OrderStatusAudit> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderStatusAuditQueries.getSelectAllBySequence(sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrderStatusAudit>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_OrderStatusAudit(dr));
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


        public bool updateBySequence(long sequence, long jobSequence, long statusType, bool flgJobClientId, long jobClientId, bool flgStatusRef, string statusRef, DateTime? dateStatusRef,
                                          string statusRef2, string statusDesc, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderStatusAuditQueries.update(this.DatabaseType, sequence, jobSequence, statusType, flgJobClientId, jobClientId, flgStatusRef, statusRef, dateStatusRef, statusRef2,
                                                                      statusDesc, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteBySequence(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderStatusAuditQueries.delete(sequence), conn))
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

        public bool deleteByFlgDeleted(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderStatusAuditQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
        private OrderStatusAudit Load_OrderStatusAudit(OleDbDataReader dr)

        {
            OrderStatusAudit orderStatusAudit = null;
            try
            {
                if (dr != null)
                {

                    orderStatusAudit = new OrderStatusAudit();
                    orderStatusAudit.Sequence = long.Parse(dr["sequence"].ToString());
                    orderStatusAudit.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    orderStatusAudit.StatusType = long.Parse(dr["status_type"].ToString());
                    orderStatusAudit.FlgJobClientId = bool.Parse(dr["flg_job_client_id"].ToString());
                    orderStatusAudit.JobClientId = long.Parse(dr["job_client_id"].ToString());
                    orderStatusAudit.FlgStatusRef = bool.Parse(dr["flg_status_ref"].ToString());
                    orderStatusAudit.StatusRef = Utilities.GetDBString(dr["status_ref"]);
                    orderStatusAudit.DateStatusRef = DateTime.Parse(dr["date_status_ref"].ToString());
                    orderStatusAudit.StatusRef2 = Utilities.GetDBString(dr["status_ref2"]);
                    orderStatusAudit.StatusDesc = Utilities.GetDBString(dr["status_desc"]);
                    orderStatusAudit.CreatedBy = long.Parse(dr["created_by"].ToString());
                    orderStatusAudit.DateCreated = DateTime.Parse(dr["date_created"].ToString());
                    orderStatusAudit.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    orderStatusAudit.DateLastAmended = DateTime.Parse(dr["date_last_amended"].ToString());

                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return orderStatusAudit;
        }
    }
}
