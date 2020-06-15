using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefOrderTypeDB : MainDB
    {

        public RefOrderTypeDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertRefOrderType(out long orderTypeId, string orderTypeDescShort, string orderTypeDescLong)
        {
            bool returnValue = false;
            orderTypeId = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(RefOrderTypeQueries.insert(this.DatabaseType, orderTypeId, orderTypeDescShort, orderTypeDescLong), conn))
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
                                orderTypeId = long.Parse(dr[0].ToString());
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

        public RefOrderType getOrderTypeById(long orderTypeId)
        {
            RefOrderType returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefOrderTypeQueries.getSelectAllByorderTypeId(this.DatabaseType, orderTypeId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                RefOrderType refType = new RefOrderType();
                                refType.OrderTypeId = int.Parse(dr["order_type_id"].ToString());
                                refType.OrderTypeDescShort = dr["order_type_desc_short"].ToString();
                                refType.OrderTypeDescLong = dr["order_type_desc_long"].ToString();
                                returnValue = refType;
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

        public List<RefOrderType> selectAllRefOrderTypeorderTypeId(long orderTypeId)
        {
            List<RefOrderType> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefOrderTypeQueries.getSelectAllByorderTypeId(this.DatabaseType, orderTypeId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefOrderType>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefOrderType(dr));
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

        public List<RefOrderType> selectAllRefOrderTypes()
        {
            List<RefOrderType> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefOrderTypeQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefOrderType>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefOrderType(dr));
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

        public bool updateByorderTypeId(long orderTypeId, string orderTypeDescShort, string orderTypeDescLong)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefOrderTypeQueries.update(this.DatabaseType, orderTypeId, orderTypeDescShort, orderTypeDescLong), conn))
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

        public bool deleteByorderTypeId(long orderTypeId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefOrderTypeQueries.delete(this.DatabaseType, orderTypeId), conn))
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

        private RefOrderType Load_RefOrderType(OleDbDataReader dr)
        {
            RefOrderType refOrderType = null;
            try
            {
                if (dr != null)
                {
                    refOrderType = new RefOrderType();
                    refOrderType.OrderTypeId = long.Parse(dr["order_type_id"].ToString());
                    refOrderType.OrderTypeDescShort = Utilities.GetDBString(dr["order_type_desc_short"]);
                    refOrderType.OrderTypeDescLong = Utilities.GetDBString(dr["order_type_desc_long"]);
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return refOrderType;
        }
    }
}