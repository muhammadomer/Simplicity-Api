using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefTradeCodeTypeQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = " SELECT * " +
                                      "  FROM    un_ref_trade_code_type" +
                                      " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getAllTradeCodeType(string databaseType)
        {
            string returnValue = "";
            try
            { 
                returnValue = " SELECT * FROM    un_ref_trade_code_type";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, string subSet, string tradeIdGroup, string tradeId, string tradeDesc, string sageAcc)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO   un_ref_trade_code_type(sub_set,  trade_id_group,  trade_id,  trade_desc,  sage_acc)" +
                    "VALUES ('" + subSet + "',   '" + tradeIdGroup + "',   '" + tradeId + "',   '" + tradeDesc + "',   '" + sageAcc + "')";
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, long sequence, string subSet, string tradeIdGroup, string tradeId, string tradeDesc, string sageAcc)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = " UPDATE   un_ref_trade_code_type" +
                                      "   SET   sub_set =  '" + subSet + "',  " +
                                      " trade_id_group =  '" + tradeIdGroup + "',  " +
                                      " trade_id =  '" + tradeId + "',  " +
                                      " trade_desc =  '" + tradeDesc + "',  " +
                                      " sage_acc =  '" + sageAcc + "',  " +
                                      " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = " DELETE FROM   un_ref_trade_code_type" +
                                      " WHERE sequence = " + sequence;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                
                bool flg = true;
                returnValue = " UPDATE   un_ref_trade_code_type" +
                                "  SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                                "  WHERE sequence = " + sequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

