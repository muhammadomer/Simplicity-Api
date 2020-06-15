using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefDiaryAppRatesQueries
    {

        public static string getSelectAllByrateSequence(string databaseType, long rateSequence)
        {
            string returnValue = "";
            try
            {
                      returnValue = "SELECT * " +
                                    "  FROM    un_ref_diary_app_rates" +
                                    " WHERE rate_sequence = " + rateSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllDiaryAppRates(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * FROM    un_ref_diary_app_rates";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string insert(string databaseType, RefDiaryAppRates obj)
        {
            string returnValue = "";
            try
            {

               
                       returnValue = "INSERT INTO   un_ref_diary_app_rates( rate_sequence,  flg_deleted,  row_index,  rate_desc,  rate_amt)" +
                                     "VALUES ( " + obj. RateSequence + ",   " + Utilities.GetBooleanForDML(databaseType, obj.FlgDeleted) + ",   '" + obj.RowIndex + "',   '" + obj.RateDesc + "',  " + obj.RateAmt + ")";
                   
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType,RefDiaryAppRates obj)
        {
            string returnValue = "";
            try
            {
               
                   returnValue = "UPDATE   un_ref_diary_app_rates" +
                                 "   SET   flg_deleted = " + Utilities.GetBooleanForDML(databaseType, obj.FlgDeleted) + ",  " + 
		                         "   row_index =  '" + obj.RowIndex + "',  " + 
		                         "   rate_desc =  '" + obj.RateDesc + "',  " + 
		                         "   rate_amt =  " + obj.RateAmt + ",  " +
                                 "  WHERE rate_sequence = " + obj.RateSequence;
                    
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long rateSequence)
        {
            string returnValue = "";
            try
            {
             
                       returnValue = "DELETE FROM   un_ref_diary_app_rates" +
                                     "WHERE rate_sequence = " + rateSequence;
            } 
       
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long rateSequence)
        {
            string returnValue = "";
            try
            {
               
                    bool flg = true;
                    returnValue = "UPDATE   un_ref_diary_app_rates" +
                                  "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                                  " WHERE rate_sequence = " + rateSequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

