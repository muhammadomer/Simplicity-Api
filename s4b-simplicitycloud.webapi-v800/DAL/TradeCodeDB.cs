using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class TradeCodeDB : MainDB
    {
        public TradeCodeDB(DatabaseInfo dbInfo) : base(dbInfo)
        {

        }

        public List<TradeCode> getAllTradeCodes()
        {
            List<TradeCode> returnValue = null;
            
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(TradeCodeQueries.SelectTradeIdAndCodeFields(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<TradeCode>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadDetails(dr));
                                }
                            }
                        }
                    }
                }
           
            return returnValue;
        }

        TradeCode LoadDetails(OleDbDataReader dr)
        {
            TradeCode tradeCode = null;
            try
            {
                if (dr != null)
                {

                    tradeCode = new TradeCode();
                    tradeCode.TradeId = (dr["trade_id"] == null || dr["trade_id"] == DBNull.Value) ? "" : dr["trade_id"].ToString();
                    tradeCode.TradeDesc = (dr["trade_desc"] == null || dr["trade_desc"] == DBNull.Value) ? "" : dr["trade_desc"].ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return tradeCode;
        }

    }
}
