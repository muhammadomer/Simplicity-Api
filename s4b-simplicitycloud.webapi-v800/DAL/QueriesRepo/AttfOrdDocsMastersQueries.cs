using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AttfOrdDocsMastersQueries
    {

        public static string getSelectAllBySequence(long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT * 
                              FROM  un_attf_ord_docs_masters
                              WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectByJobSequenceAndSequence(string databaseType, long jobSequence, long sequence)
        {
            string strSQL = "SELECT doc.* " +
                            "  FROM un_attf_ord_docs_masters AS doc " +
                            " WHERE doc.job_sequence = " + jobSequence +
                            "   AND doc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                            "   AND doc.flg_hide <> " + Utilities.GetBooleanForDML(databaseType, true);
            if (sequence > 0)
            {
                strSQL = strSQL + "   AND doc.sequence = " + sequence;
            }
            return strSQL;
        }

        public static string insert(string databaseType, bool flgDeleted, bool flgHide, long jobSequence, string fileName,
                                   string fileVersionNo, DateTime? dateFileVersionNo, string fileVersionOption, bool flgFileVo, 
                                   string fileNotes, string filePathAndName, long createdBy, DateTime? dateCreated, long lastAmendedBy,
                                   DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO  un_attf_ord_docs_masters( flg_deleted,  flg_hide,  job_sequence,  file_name,  file_version_no,  date_file_version_no,  file_version_option,"+ 
                              "                                       flg_file_vo,  file_notes,  file_path_and_name,  created_by,  date_created,  last_amended_by,  date_last_amended ) " +
                              "VALUES ( " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",   " + Utilities.GetBooleanForDML(databaseType, flgHide) + ",   " + jobSequence + ",   '" + fileName + "',   '" + fileVersionNo + "',   " + Utilities.GetDateTimeForDML(databaseType, dateFileVersionNo,true,true) + ",   '" + 
                                        fileVersionOption + "',   " + Utilities.GetBooleanForDML(databaseType, flgFileVo) + ",   '" + fileNotes + "',   '" + filePathAndName + "',   " + createdBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ",   " + 
                                        lastAmendedBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, long sequence, bool flgDeleted, bool flgHide, long jobSequence, string fileName,
                                   string fileVersionNo, DateTime? dateFileVersionNo, string fileVersionOption, bool flgFileVo,
                                   string fileNotes, string filePathAndName, long createdBy, DateTime? dateCreated, long lastAmendedBy,
                                   DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_attf_ord_docs_masters " +
                              "   SET   flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " +
                              "         flg_hide = " + Utilities.GetBooleanForDML(databaseType, flgHide) + ",  " +
                              "         job_sequence =  " + jobSequence + ",  " +
                              "         file_name =  '" + fileName + "',  " +
                              "         file_version_no =  '" + fileVersionNo + "',  " +
                              "         date_file_version_no =  " + Utilities.GetDateTimeForDML(databaseType, dateFileVersionNo,true,true) + ", " +
                              "         file_version_option =  '" + fileVersionOption + "',  " +
                              "         flg_file_vo = " + Utilities.GetBooleanForDML(databaseType, flgFileVo) + ",  " +
                              "         file_notes =  '" + fileNotes + "',  " +
                              "         file_path_and_name =  '" + filePathAndName + "',  " +
                              "         created_by =  " + createdBy + ",  " +
                              "         date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                              "         last_amended_by =  " + lastAmendedBy + ",  " +
                              "         date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                              "  WHERE sequence = " + sequence;
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
                returnValue = "DELETE FROM un_attf_ord_docs_masters " +
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
                returnValue = "UPDATE un_attf_ord_docs_masters " +
                              "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) + ", " +
                              "WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

