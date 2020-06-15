using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrderCheckListDB : MainDB
    {

        public OrderCheckListDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertRefOrderCheckList(out long sequence, bool flgDeleted, long listSequence, string checkDesc, bool flgCompulsory, bool flgOrdEnqDataCapture, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderCheckListQueries.insertRefOrderCheckList(this.DatabaseType, flgDeleted, listSequence, checkDesc, flgCompulsory, flgOrdEnqDataCapture, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public List<OrderCheckList> selectAllOrderCheckListByJobSequence(long jobSequence)
        {
            List<OrderCheckList> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderCheckListQueries.getSelectAllByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrderCheckList>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadOrderCheckList(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<RefOrderCheckList> selectAllRefOrderCheckListSequence(long sequence)
        {
            List<RefOrderCheckList> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderCheckListQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefOrderCheckList>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefOrderCheckList(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public OrderCheckListMain insertOrderCheckListMain(OrderCheckListMain orderCheckListMain)
        {
            OrderCheckListMain returnValue = orderCheckListMain;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderCheckListQueries.insertOrderCheckListMain(this.DatabaseType, orderCheckListMain), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue.Sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public OrderCheckListItems insertOrderCheckListItem(OrderCheckListItems orderCheckListItem)
        {
            OrderCheckListItems returnValue = orderCheckListItem;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderCheckListQueries.insertOrderCheckListItem(this.DatabaseType, orderCheckListItem), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue.Sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public OrderCheckListItems updateOrderCheckListItem(OrderCheckListItems orderCheckListItem)
        {
            OrderCheckListItems returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderCheckListQueries.updateOrderCheckListItem(this.DatabaseType, orderCheckListItem), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = orderCheckListItem;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool updateBySequence(long sequence, bool flgDeleted, long listSequence, string checkDesc, bool flgCompulsory, bool flgOrdEnqDataCapture, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderCheckListQueries.updateRefOrderCheckList(this.DatabaseType, sequence, flgDeleted, listSequence, checkDesc, flgCompulsory, flgOrdEnqDataCapture, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
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
                        new OleDbCommand(OrderCheckListQueries.deleteRefOrderCheckList(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
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
                        new OleDbCommand(OrderCheckListQueries.updateRefOrderCheckListFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        private RefOrderCheckList Load_RefOrderCheckList(OleDbDataReader dr)
        {
            RefOrderCheckList refOrderCheckList = null;
            try
            {
                if (dr != null)
                {
                    refOrderCheckList = new RefOrderCheckList();
                    refOrderCheckList.Sequence = long.Parse(dr["sequence"].ToString());
                    refOrderCheckList.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    refOrderCheckList.ListSequence = long.Parse(dr["list_sequence"].ToString());
                    refOrderCheckList.CheckDesc = Utilities.GetDBString(dr["check_desc"]);
                    refOrderCheckList.FlgCompulsory = bool.Parse(dr["flg_compulsory"].ToString());
                    refOrderCheckList.FlgOrdEnqDataCapture = bool.Parse(dr["flg_ord_enq_data_capture"].ToString());
                    refOrderCheckList.CreatedBy = long.Parse(dr["created_by"].ToString());
                    refOrderCheckList.DateCreated = Utilities.getSQLDate(DateTime.Parse(dr["date_created"].ToString()));
                    refOrderCheckList.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    refOrderCheckList.DateLastAmended = Utilities.getSQLDate(DateTime.Parse(dr["date_last_amended"].ToString()));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return refOrderCheckList;
        }

        private OrderCheckList LoadOrderCheckList(OleDbDataReader dr)
        {
            OrderCheckList orderCheckList = null;
            try
            {
                if (dr != null)
                {
                    orderCheckList = new OrderCheckList();
                    orderCheckList.RefSequence = DBUtil.IsColumnExists(dr, "refsequence") ? long.Parse(dr["refsequence"].ToString()) : -1;
                    orderCheckList.ListSequence = DBUtil.IsColumnExists(dr, "list_sequence") ? long.Parse(dr["list_sequence"].ToString()) : -1;
                    orderCheckList.JobSequence = DBUtil.IsColumnExists(dr, "job_sequence") ? long.Parse(dr["job_sequence"].ToString()) : -1;
                    orderCheckList.CheckDesc = DBUtil.IsColumnExists(dr, "check_desc") ? dr["check_desc"].ToString() : null;
                    orderCheckList.FlgCompulsory = DBUtil.IsColumnExists(dr, "flg_compulsory") ? bool.Parse(dr["flg_compulsory"].ToString()) : false;
                    orderCheckList.FlgOrdEnqDataCapture = DBUtil.IsColumnExists(dr, "flg_ord_enq_data_capture") ? bool.Parse(dr["flg_ord_enq_data_capture"].ToString()) : false;
                    orderCheckList.JoinSequence = DBUtil.IsColumnExists(dr, "join_sequence") ? long.Parse(dr["join_sequence"].ToString()) : -1;
                    orderCheckList.ItemSequence = DBUtil.IsColumnExists(dr, "item_sequence") ? long.Parse(dr["item_sequence"].ToString()) : -1;
                    orderCheckList.FlgChecked = DBUtil.IsColumnExists(dr, "flg_checked") ? Convert.ToBoolean(Convert.ToInt32( dr["flg_checked"].ToString())) : false;
                    orderCheckList.FlgCheckedYes = DBUtil.IsColumnExists(dr, "flg_checked_yes") ? Convert.ToBoolean(Convert.ToInt32(dr["flg_checked_yes"].ToString())) : false;
                    orderCheckList.FlgCheckedNo = DBUtil.IsColumnExists(dr, "flg_checked_no") ? Convert.ToBoolean(Convert.ToInt32(dr["flg_checked_no"].ToString())) : false;
                    orderCheckList.FlgCheckedDate = DBUtil.IsColumnExists(dr, "flg_checked_date") ? Convert.ToBoolean(Convert.ToInt32(dr["flg_checked_date"].ToString())) : false;
                    orderCheckList.CheckedDate = DBUtil.IsColumnExists(dr, "date_check") ? Utilities.getDBDate(dr["date_check"]) : null;
                    orderCheckList.CheckedDetails = DBUtil.IsColumnExists(dr, "check_details") ? dr["check_details"].ToString() : null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return orderCheckList;
        }
    }
}