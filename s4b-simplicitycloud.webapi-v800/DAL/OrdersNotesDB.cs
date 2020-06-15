using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrdersNotesDB : MainDB
    {

        public OrdersNotesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertOrdersNotes(out long sequence, OrdersNotes obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                           new OleDbCommand(OrdersNotesQueries.insert(this.DatabaseType, obj.JobSequence ?? 0, obj.OrderNotes, 
                                            obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
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

        public List<OrdersNotes> selectBySequence(long sequence)
        {
            List<OrdersNotes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersNotesQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdersNotes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_OrdersNotes(dr));
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

        public List<OrdersNotes> getByJobSequence(long jobSequence)
        {
            List<OrdersNotes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersNotesQueries.getSelectAllByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdersNotes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_OrdersNotes(dr));
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

        public bool updateBySequence(OrdersNotes obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersNotesQueries.update(this.DatabaseType, obj.Sequence ?? 0, obj.JobSequence ?? 0, obj.OrderNotes,
                                         obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended), conn))
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
                        new OleDbCommand(OrdersNotesQueries.delete(this.DatabaseType, sequence), conn))
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
                        new OleDbCommand(OrdersNotesQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
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

        private OrdersNotes Load_OrdersNotes(OleDbDataReader dr)
        {
            OrdersNotes ordersNotes = null;
            try
            {
                if (dr != null)
                {
                    ordersNotes = new OrdersNotes();
                    ordersNotes.Sequence =DBUtil.GetLongValue(dr,"sequence");
                    ordersNotes.JobSequence = DBUtil.GetLongValue(dr,"job_sequence");
                    ordersNotes.OrderNotes = DBUtil.GetStringValue(dr,"order_notes");
                    ordersNotes.CreatedBy = DBUtil.GetLongValue(dr,"created_by");
                    ordersNotes.DateCreated = DBUtil.GetDateTimeValue(dr,"date_created");
                    ordersNotes.LastAmendedBy = DBUtil.GetLongValue(dr,"last_amended_by");
                    ordersNotes.DateLastAmended = DBUtil.GetDateTimeValue(dr,"date_last_amended");
                    ordersNotes.UserName =  DBUtil.GetStringValue(dr, "user_name");
                    ordersNotes.UserLogon = DBUtil.GetStringValue(dr, "user_logon");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ordersNotes;
        }
    }
}
