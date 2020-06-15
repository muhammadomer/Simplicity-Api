using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;


namespace SimplicityOnlineWebApi.DAL
{
    public class OrderManagerAuditDB : MainDB
    {

        public OrderManagerAuditDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertOrderManagerAudit(OrderManagerAudit orderManagerAudit, int createdBy, DateTime? createdDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderManagerAuditQueries.insert(this.DatabaseType, orderManagerAudit, createdBy, createdDate), conn))
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


        public List<OrderManagerAudit> selectOrderManagerAuditByJobSequence(long jobSequence)
        {
            List<OrderManagerAudit> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderManagerAuditQueries.getSelectAllByJobSequence(jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrderManagerAudit>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_OrderManagerAudit(dr));
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

        public OrderManagerAudit selectActiveOrderManagerAuditByJobSequence(long jobSequence)
        {
            OrderManagerAudit returnValue = new OrderManagerAudit();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderManagerAuditQueries.getSelectActiveByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {   
                                while (dr.Read())
                                {
                                    returnValue = Load_OrderManagerAudit(dr);
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


        public bool updateBySequence(long sequence, OrderManagerAudit orderManagerAudit, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderManagerAuditQueries.update(this.DatabaseType, orderManagerAudit, lastAmendedBy, dateLastAmended), conn))
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
                        new OleDbCommand(OrderManagerAuditQueries.updateActiveToFinishByJobSequence(this.DatabaseType, jobSequence, lastAmendedBy, dateLastAmended), conn))
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
                        new OleDbCommand(OrderManagerAuditQueries.delete(sequence), conn))
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

        private OrderManagerAudit Load_OrderManagerAudit(OleDbDataReader dr)

        {
            DateTime dt;
            OrderManagerAudit orderManagerAudit = null;
            try
            {
                if (dr != null)
                {

                    orderManagerAudit = new OrderManagerAudit();
                    orderManagerAudit.Sequence = long.Parse(dr["sequence"].ToString());
                    orderManagerAudit.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    orderManagerAudit.JobManagerId = long.Parse(dr["job_manager_id"].ToString());
                    orderManagerAudit.PhaseType = long.Parse(dr["phase_type"].ToString());
                    orderManagerAudit.FlgPhaseRef = bool.Parse(dr["flg_phase_ref"].ToString());
                    orderManagerAudit.PhaseRef =  Utilities.GetDBString(dr["phase_ref"]);
                    orderManagerAudit.FlgPhaseStart = bool.Parse(dr["flg_phase_start"].ToString());
                    orderManagerAudit.PhaseStartDesc = Utilities.GetDBString(dr["phase_start_desc"]);
                    orderManagerAudit.DatePhaseStart = DateTime.Parse(dr["date_phase_start"].ToString());
                    orderManagerAudit.FlgPhaseFinish = bool.Parse(dr["flg_phase_finish"].ToString());
                    orderManagerAudit.PhaseFinishDesc = Utilities.GetDBString(dr["phase_finish_desc"]);
                    orderManagerAudit.DatePhaseFinish = (dr["date_phase_finish"] != null && dr["date_phase_finish"] != DBNull.Value && DateTime.TryParse(dr["date_phase_finish"].ToString(), out dt)) ? (Nullable<DateTime>)dt : null; 
                    orderManagerAudit.CreatedBy = long.Parse(dr["created_by"].ToString());
                    orderManagerAudit.DateCreated = DateTime.Parse(dr["date_created"].ToString());
                    orderManagerAudit.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    orderManagerAudit.DateLastAmended = DateTime.Parse(dr["date_last_amended"].ToString());
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return orderManagerAudit;
        }

    }
}
