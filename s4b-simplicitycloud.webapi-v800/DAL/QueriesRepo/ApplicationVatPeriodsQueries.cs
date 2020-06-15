using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class ApplicationVatPeriodsQueries
    { 

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_application_vat_periods" +
                        " WHERE sequence = " + Sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string insert(string databaseType, bool flgDelete, long periodYear, long periodIndex, DateTime? datePeriodStart, 
                                    DateTime? datPeriodEnd)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_application_vat_periods (flg_delete, period_year, period_index, dat_period_start, dat_period_end) " +
                        "VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDelete) + ", '" + periodYear + ", " + periodIndex + ", " + Utilities.GetDateTimeForDML(databaseType, datePeriodStart,true,true) + ", " + Utilities.GetDateTimeForDML(databaseType, datPeriodEnd,true,true) + ")";
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, long sequence, bool flgDelete, long periodYear, long periodIndex, DateTime? datePeriodStart,
                                    DateTime? datPeriodEnd)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "UPDATE un_application_van_periods " +
                        "   SET flg_delete =  " + Utilities.GetBooleanForDML(databaseType, flgDelete) + "," +
                        "       peripd_year =  " + periodYear + "," +
                        "       peripd_index =  " + periodIndex + "," +
                        "       dat_period_start = " + Utilities.GetDateTimeForDML(databaseType, datePeriodStart,true,true) + "," +
                        "       dat_period_end = " + Utilities.GetDateTimeForDML(databaseType, datPeriodEnd,true,true) + "," +
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
                returnValue = "DELETE FROM un_application_van_periods " +
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
               
                returnValue = "UPDATE un_application_van_periods" +
                        "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                        " WHERE sequence = " + sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }       

    
    }
}

