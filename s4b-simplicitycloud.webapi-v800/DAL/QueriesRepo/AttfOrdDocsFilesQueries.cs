using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AttfOrdDocsFilesQueries
    {

        public static string getSelectAllBySequence(long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM  un_attf_ord_docs_files" +
                              " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, bool flgDeleted, long jobSequence, long fileMasterId, string fileSubmissonId,
                                    string fileDescription, string fileNotes, string filePathAndName, long createdBy,
                                    DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO  un_attf_ord_docs_files(flg_deleted,  job_sequence,  file_master_id,  file_submisson_id,  file_description," +
                              "                                    file_notes,  file_path_and_name,  created_by,  date_created,  last_amended_by,  date_last_amended) " +
                              "VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " + jobSequence + ", " + 
                              "        " + fileMasterId + ",  " + fileSubmissonId + ",  '" + fileDescription + "', " +
                              "       '" + fileNotes + "',  '" + filePathAndName + "', " + createdBy + ",  " + 
                              "        " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ",  " + 
                              "        " + lastAmendedBy + ",  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, long sequence, bool flgDeleted, long jobSequence, long fileMasterId, string fileSubmissonId,
                                    string fileDescription, string fileNotes, string filePathAndName, long createdBy,
                                    DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_attf_ord_docs_files " +
                              "   SET  flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " +
                              "        job_sequence =  " + jobSequence + ",  " +
                              "        file_master_id =  " + fileMasterId + ",  " +
                              "        file_submisson_id =  " + fileSubmissonId + ",  " +
                              "        file_description =  " + fileDescription + ",  " +
                              "        file_notes =  " + fileNotes + ",  " +
                              "        file_path_and_name =  " + filePathAndName + ",  " +
                              "        created_by =  " + createdBy + ",  " +
                              "        date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +
                              "        last_amended_by =  " + lastAmendedBy + ",  " +
                              "        date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ", " +
                              " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string delete(long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM un_attf_ord_docs_files " +
                              "WHERE sequence = " + sequence;
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
                returnValue = "UPDATE un_attf_ord_docs_files " +
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

