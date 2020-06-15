using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefTradeCodeTypeDB : MainDB
    {

        public RefTradeCodeTypeDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertRefTradeCodeType(out long sequence, string subSet, string tradeIdGroup, string tradeId, string tradeDesc, string sageAcc)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(RefTradeCodeTypeQueries.insert(this.DatabaseType, subSet, tradeIdGroup, tradeId, tradeDesc, sageAcc), conn))
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


        public List<RefTradeCodeType> selectAllRefTradeCodeTypeSequence(long sequence)
        {
            List<RefTradeCodeType> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefTradeCodeTypeQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefTradeCodeType>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefTradeCodeType(dr));
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

        public List<RefTradeCodeType> getAllRefTradeCodeType()
        {
            List<RefTradeCodeType> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefTradeCodeTypeQueries.getAllTradeCodeType(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefTradeCodeType>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefTradeCodeType(dr));
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


        public bool updateBySequence(long sequence, string subSet, string tradeIdGroup, string tradeId, string tradeDesc, string sageAcc)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefTradeCodeTypeQueries.update(this.DatabaseType, sequence, subSet, tradeIdGroup, tradeId, tradeDesc, sageAcc), conn))
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
                        new OleDbCommand(RefTradeCodeTypeQueries.delete(this.DatabaseType, sequence), conn))
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
                        new OleDbCommand(RefTradeCodeTypeQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
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
        private RefTradeCodeType Load_RefTradeCodeType(OleDbDataReader dr)

        {
            RefTradeCodeType refTradeCodeType = null;
            try
            {
                if (dr != null)
                {

                    refTradeCodeType = new RefTradeCodeType();
                    refTradeCodeType.Sequence = long.Parse(dr["sequence"].ToString());
                    refTradeCodeType.SubSet = Utilities.GetDBString(dr["sub_set"]);
                    refTradeCodeType.TradeIdGroup = Utilities.GetDBString(dr["trade_id_group"]);
                    refTradeCodeType.TradeId = Utilities.GetDBString(dr["trade_id"]);
                    refTradeCodeType.TradeDesc = Utilities.GetDBString(dr["trade_desc"]);
                    refTradeCodeType.SageAcc = Utilities.GetDBString(dr["sage_acc"]);

                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return refTradeCodeType;
        }

    }
}