using System;
using SimplicityOnlineWebApi.Commons;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class Cld_Ord_Labels_Queries
    { 
        public static string getSelectAllByJobSequence(string databaseType, long jobSequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = @"SELECT * 
                        FROM un_cld_ord_labels
                        WHERE job_sequence = " + jobSequence;
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByJobSequenceAndTagNo(string databaseType, long jobSequence, string tagNo)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_cld_ord_labels" +
                        " WHERE job_sequence = " + jobSequence +"   AND tag_no = '" + tagNo + "'";
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByJobSequenceAndTagNoSearch(string databaseType, long jobSequence, string tagNo)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                             "  FROM un_cld_ord_labels" +
                             " WHERE job_sequence = " + jobSequence + "   AND tag_no LIKE '%" + tagNo + "%'";
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string GetAllOtherTagNoByJobSequenceAndTagNo(string databaseType, long jobSequence,long sequence, string tagNo)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                              "  FROM un_cld_ord_labels" +
                              " WHERE job_sequence = " + jobSequence +
                              "   AND tag_no ='" + tagNo + "' and sequence<>" + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long sequence, long jobSequence, string tagNo, int createdBy, DateTime? dateCreated, 
                                    int modifiedBy, DateTime? datModified)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "INSERT INTO un_cld_ord_labels (job_sequence, tag_no, item_quantity, flg_complete, created_by, date_created, last_amended_by, date_last_amended) " +
                            "VALUES (" + jobSequence + ", '" + tagNo + "', 0, 0, " + createdBy + ", " 
                                     +  Utilities.GetDateTimeForDML(databaseType,dateCreated,true,true) + "," + modifiedBy + ", "
                                    + Utilities.GetDateTimeForDML(databaseType, datModified,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string updateTagNo(string databaseType, long sequence, string tagNo, int modifiedBy, DateTime? datModified)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "UPDATE un_cld_ord_labels " +
                    "   SET tag_No =  '" + tagNo + "'," +
                    "       last_amended_by =  " + modifiedBy + "," +
                    "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, datModified,true,true) +
                    " WHERE sequence = " + sequence;
              
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string getSelectAllBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                             "  FROM un_cld_ord_labels" +
                             " WHERE sequence = " + sequence;
               
            }
            catch (Exception ex)
            {
            }
            
            return returnValue;
        }
    }
}

