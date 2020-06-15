using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrdersDistributionListDB : MainDB
    {
        public OrdersDistributionListDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertOrdersDistributionList(out long sequence, bool flgDeleted, long jobSequence, string emailName, string emailAddress, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrdersDistributionListQueries.insert(this.DatabaseType, flgDeleted, 
                                                                              jobSequence, emailName, emailAddress, 
                                                                              createdBy, dateCreated, lastAmendedBy, 
                                                                              dateLastAmended), conn))
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


        public List<OrdersDistributionList> getOrdersDistributionListByJobSequence(long jobSequence)
        {
            List<OrdersDistributionList> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersDistributionListQueries.getSelectAllByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OrdersDistributionList>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_OrdersDistributionList(dr));
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
        
        public bool updateBySequence(long sequence, bool flgDeleted, long jobSequence, string emailName, string emailAddress, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersDistributionListQueries.update(this.DatabaseType, sequence, flgDeleted, jobSequence, emailName, emailAddress, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
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
                        new OleDbCommand(OrdersDistributionListQueries.delete(this.DatabaseType, sequence), conn))
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
                        new OleDbCommand(OrdersDistributionListQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
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

        private OrdersDistributionList Load_OrdersDistributionList(OleDbDataReader dr)
        {
            OrdersDistributionList ordersDistributionList = null;
            try
            {
                if (dr != null)
                {
                    ordersDistributionList = new OrdersDistributionList();
                    ordersDistributionList.Sequence = long.Parse(dr["sequence"].ToString());
                    ordersDistributionList.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    ordersDistributionList.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    ordersDistributionList.EmailName = Utilities.GetDBString(dr["email_name"]);
                    ordersDistributionList.EmailAddress = Utilities.GetDBString(dr["email_address"]);
                    ordersDistributionList.CreatedBy = long.Parse(dr["created_by"].ToString());
                    ordersDistributionList.DateCreated = Utilities.getDBDate(dr["date_created"]);
                    ordersDistributionList.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    ordersDistributionList.DateLastAmended = Utilities.getDBDate(DateTime.Parse(dr["date_last_amended"].ToString()));
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return ordersDistributionList;
        }
    }
}
