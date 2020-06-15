using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AttfOrdDocsFileCounterQueries
    { 

        public static string getSelectAllBySequence(long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM  un_attf_ord_docs_file_counter" +
                              " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByJobSequenceAndFlgMasterFile(string databaseType, long jobSequence, bool flgMasterFile)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT sequence, job_sequence, flg_master_file, last_file_no " +
                              "  FROM un_attf_ord_docs_file_counter " +
                              " WHERE flg_master_file = " + Utilities.GetBooleanForDML(databaseType, flgMasterFile) +
                              "   AND job_sequence = " + jobSequence; ;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long jobSequence, bool flgMasterFile, long lastFileNo)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO  un_attf_ord_docs_file_counter(job_sequence,  flg_master_file,  last_file_no) " +
                              "VALUES ( " + jobSequence + ",  " + Utilities.GetBooleanForDML(databaseType, flgMasterFile) + ", " + lastFileNo + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string update(string databaseType, long sequence, long jobSequence, bool flgMasterFile, long lastFileNo)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_attf_ord_docs_file_counter " +
                              "   SET  job_sequence =  " + jobSequence + ", " +
                              "        flg_master_file = " + Utilities.GetBooleanForDML(databaseType, flgMasterFile) + ", " +
                              "        last_file_no =  " + lastFileNo + "  " +
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
                returnValue = "DELETE FROM un_attf_ord_docs_file_counter " +
                              "WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

