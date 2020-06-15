using System;
using SimplicityOnlineWebApi.Commons;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class MailMergeQueries
    {

        public static string getSelectAllBymergeCode(string databaseType, string mergeCode)
        {
            string returnValue = "";
            try
            {
                returnValue = @" SELECT * 
                                FROM    un_ref_mail_merge_codes
                                WHERE merge_code = " + mergeCode;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAll(string databaseType)
        {
            return @"SELECT *  FROM  un_ref_mail_merge_codes ";
        }

        public static string insert(string databaseType, string mergeCode, string mergeDesc, string oldTableName, string oldColumnName, string newTableName, string newColumnName,
                                    string mergeFormula)
        {
            string returnValue = "";
            try
            {
                        returnValue = "INSERT INTO   un_ref_mail_merge_codes( merge_code,  merge_desc,  old_table_name,  old_column_name,  new_table_name,  new_column_name,  merge_formula)" +
                                      "VALUES ('" + mergeCode + "',   '" + mergeDesc + "',   '" + oldTableName + "',   '" + oldColumnName + "',   '" + newTableName + "',   '" +
                                               newColumnName + "',   '" + mergeFormula + "')";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, string mergeCode, string mergeDesc, string oldTableName, string oldColumnName, string newTableName, string newColumnName,
                                    string mergeFormula)
        {
            string returnValue = "";
            try
            {
                
                returnValue = " UPDATE   un_ref_mail_merge_codes" +
                                "   SET  merge_desc =  '" + mergeDesc + "',  " +
                                " old_table_name =  '" + oldTableName + "',  " +
                                " old_column_name =  '" + oldColumnName + "',  " +
                                " new_table_name =  '" + newTableName + "',  " +
                                " new_column_name =  '" + newColumnName + "',  " +
                                " merge_formula =  '" + mergeFormula + "',  " +
                                "  WHERE merge_code = " + mergeCode;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, string mergeCode)
        {
            string returnValue = "";
            try
            {
                
                returnValue = " DELETE FROM   un_ref_mail_merge_codes" +
                " WHERE merge_code = " + mergeCode;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, string mergeCode)
        {
            string returnValue = "";
            try
            {
               
                bool flg = true;
                returnValue = " UPDATE   un_ref_mail_merge_codes" +
                                "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) +
                                " WHERE merge_code = " + mergeCode;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

