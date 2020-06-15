using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefJobStatusTypeDB : MainDB
    {

        public RefJobStatusTypeDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertRefJobStatusType(out long statusId, bool flgDeleted, long statusIndex, string statusDesc, bool flgAutoUpdate, long orderFieldId, bool flgLinkToDiaryApps,
                                             bool flgCompletedStatus)
        {
            bool returnValue = false;
            statusId = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(RefJobStatusTypeQueries.insert(this.DatabaseType, statusId, flgDeleted, statusIndex, statusDesc, flgAutoUpdate, orderFieldId,
                                                                        flgLinkToDiaryApps, flgCompletedStatus), conn))
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
                                statusId = long.Parse(dr[0].ToString());
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


        public List<RefJobStatusType> selectAllRefJobStatusTypestatusId(long statusId)
        {
            List<RefJobStatusType> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefJobStatusTypeQueries.getSelectAllBystatusId(this.DatabaseType, statusId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefJobStatusType>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefJobStatusType(dr));
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

        public RefJobStatusType getStatusByStatusId(long statusId)
        {
            RefJobStatusType returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefJobStatusTypeQueries.getSelectAllBystatusId(this.DatabaseType, statusId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                RefJobStatusType refJobStatusType = new RefJobStatusType();
                                refJobStatusType.StatusId = int.Parse(dr["status_id"].ToString());
                                refJobStatusType.StatusDesc = dr["status_desc"].ToString();
                                returnValue = refJobStatusType;
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

        public bool updateBystatusId(long statusId, bool flgDeleted, long statusIndex, string statusDesc, bool flgAutoUpdate, long orderFieldId, bool flgLinkToDiaryApps,
                                     bool flgCompletedStatus)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefJobStatusTypeQueries.update(this.DatabaseType, statusId, flgDeleted, statusIndex, statusDesc, flgAutoUpdate, orderFieldId,
                                                                        flgLinkToDiaryApps, flgCompletedStatus), conn))
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

        public bool deleteBystatusId(long statusId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefJobStatusTypeQueries.delete(this.DatabaseType, statusId), conn))
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

        public bool deleteByFlgDeleted(long statusId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefJobStatusTypeQueries.deleteFlagDeleted(this.DatabaseType, statusId), conn))
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

        public List<RefJobStatusType> selectAllRefJobStatusType()
        {
            List<RefJobStatusType> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefJobStatusTypeQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefJobStatusType>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefJobStatusType(dr));
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
        private RefJobStatusType Load_RefJobStatusType(OleDbDataReader dr)

        {
            RefJobStatusType refJobStatusType = null;
            try
            {
                if (dr != null)
                {

                    refJobStatusType = new RefJobStatusType();
                    refJobStatusType.StatusId = int.Parse(dr["status_id"].ToString());
                    refJobStatusType.StatusIndex = int.Parse(dr["status_index"].ToString());
                    refJobStatusType.StatusDesc = Utilities.GetDBString(dr["status_desc"]);
                    refJobStatusType.FlgAutoUpdate = bool.Parse(dr["flg_auto_update"].ToString());
                    refJobStatusType.OrderFieldId = long.Parse(dr["order_field_id"].ToString());
                    refJobStatusType.FlgLinkToDiaryApps = bool.Parse(dr["flg_link_to_diary_apps"].ToString());
                    refJobStatusType.FlgCompletedStatus = bool.Parse(dr["flg_completed_status"].ToString());

                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return refJobStatusType;
        }

    }
}
