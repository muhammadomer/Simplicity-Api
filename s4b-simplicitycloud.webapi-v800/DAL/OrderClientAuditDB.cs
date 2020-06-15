using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;


namespace SimplicityOnlineWebApi.DAL
{
    public class OrderClientsAuditDB : MainDB
    {

        public OrderClientsAuditDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertOrderClientAudit(OrderClientAudit orderClientAudit, int createdBy, DateTime? createdDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderClientAuditQueries.insert(this.DatabaseType, orderClientAudit, createdBy, createdDate), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
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


        public List<OrderClientAudit> selectOrderClientAuditByJobSequence(long jobSequence)
        {
            List<OrderClientAudit> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderClientAuditQueries.getSelectAllByJobSequence(jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrderClientAudit>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_OrderClientAudit(dr));
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

        public OrderClientAudit selectActiveOrderClientAuditByJobSequence(long jobSequence)
        {
            OrderClientAudit returnValue = new OrderClientAudit();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderClientAuditQueries.getSelectActiveByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {   
                                while (dr.Read())
                                {
                                    returnValue = Load_OrderClientAudit(dr);
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


        public bool updateBySequence(long sequence, OrderClientAudit orderClientAudit, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderClientAuditQueries.update(this.DatabaseType, orderClientAudit, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public bool updateActiveToFinishByJobSequence(long jobSequence,  long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderClientAuditQueries.updateActiveToFinishByJobSequence(this.DatabaseType, jobSequence, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                        new OleDbCommand(OrderClientAuditQueries.delete(sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        private OrderClientAudit Load_OrderClientAudit(OleDbDataReader dr)

        {
            DateTime dt;
            OrderClientAudit orderClientAudit = null;
            try
            {
                if (dr != null)
                {

                    orderClientAudit = new OrderClientAudit();
                    orderClientAudit.Sequence = long.Parse(dr["sequence"].ToString());
                    orderClientAudit.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    orderClientAudit.JobClientId = long.Parse(dr["job_client_id"].ToString());
                    orderClientAudit.PhaseType = long.Parse(dr["phase_type"].ToString());
                    orderClientAudit.FlgPhaseRef = bool.Parse(dr["flg_phase_ref"].ToString());
                    orderClientAudit.PhaseRef =  Utilities.GetDBString(dr["phase_ref"]);
                    orderClientAudit.FlgPhaseStart = bool.Parse(dr["flg_phase_start"].ToString());
                    orderClientAudit.PhaseStartDesc = Utilities.GetDBString(dr["phase_start_desc"]);
                    orderClientAudit.DatePhaseStart = DateTime.Parse(dr["date_phase_start"].ToString());
                    orderClientAudit.FlgPhaseFinish = bool.Parse(dr["flg_phase_finish"].ToString());
                    orderClientAudit.PhaseFinishDesc = Utilities.GetDBString(dr["phase_finish_desc"]);
                    orderClientAudit.DatePhaseFinish = (dr["date_phase_finish"] != null && dr["date_phase_finish"] != DBNull.Value && DateTime.TryParse(dr["date_phase_finish"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null; 
                    orderClientAudit.CreatedBy = long.Parse(dr["created_by"].ToString());
                    orderClientAudit.DateCreated = DateTime.Parse(dr["date_created"].ToString());
                    orderClientAudit.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    orderClientAudit.DateLastAmended = DateTime.Parse(dr["date_last_amended"].ToString());
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return orderClientAudit;
        }

    }
}
