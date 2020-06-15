using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class StockJobReqHeaderQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = " SELECT * " +
                "  FROM    un_stock_job_req_header" +
                " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string Insert(string databaseType, bool flgDeleted, long jobSequence, bool flgAuthorised, long authorisedBy, DateTime? dateAuthorised,
                                    int poType, bool flgPoPlaced, long poSequence, string userField01, string userField02, string userField03, string userField04,
                                    string userField05, string userField06, string userField07, string userField08, string userField09, string userField10, int createdBy,
                                    DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_stock_job_req_header(flg_deleted, job_sequence, flg_authorised, authorised_by, date_authorised, po_type,
                            flg_po_placed, po_sequence, user_field_01, user_field_02, user_field_03, user_field_04, user_field_05,
                            user_field_06, user_field_07, user_field_08, user_field_09, user_field_10, created_by, date_created,
                            last_amended_by,  date_last_amended)
                    VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " + jobSequence + ", " +
                           Utilities.GetBooleanForDML(databaseType, flgAuthorised) + ", " + authorisedBy + ", " +
                           Utilities.GetDateTimeForDML(databaseType, dateAuthorised, true, true) + ", " + poType + ", " + 
                           Utilities.GetBooleanForDML(databaseType, flgPoPlaced) + ", " + poSequence + ", '" + userField01 + "', '" +
                           userField02 + "', '" + userField03 + "', '" + userField04 + "', '" + userField05 + "', '" + userField06 + "', '" +
                           userField07 + "', '" + userField08 + "', '" + userField09 + "', '" + userField10 + "', " + createdBy + ", " +
                           Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, long sequence, bool flgDeleted, long jobSequence, bool flgAuthorised, long authorisedBy, DateTime dateAuthorised,
                                    string poType, bool flgPoPlaced, long poSequence, string userField01, string userField02, string userField03, string userField04,
                                    string userField05, string userField06, string userField07, string userField08, string userField09, string userField10, long createdBy,
                                    DateTime dateCreated, long lastAmendedBy, DateTime dateLastAmended)
        {
            string returnValue = "";
            try
            {
               
                returnValue = " UPDATE   un_stock_job_req_header" +
                    "   SET  flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " +
                    " job_sequence =  " + jobSequence + ",  " +
                    " flg_authorised = " + Utilities.GetBooleanForDML(databaseType, flgAuthorised) + ",  " +
                    " authorised_by =  " + authorisedBy + ",  " +
                    " date_authorised =  " + Utilities.GetDateTimeForDML(databaseType, dateAuthorised,true,true) + ", " +
                    " po_type =  '" + poType + "',  " +
                    " flg_po_placed = " + Utilities.GetBooleanForDML(databaseType, flgPoPlaced) + ",  " +
                    " po_sequence =  " + poSequence + ",  " +
                    " user_field_01 =  '" + userField01 + "',  " +
                    " user_field_02 =  '" + userField02 + "',  " +
                    " user_field_03 =  '" + userField03 + "',  " +
                    " user_field_04 =  '" + userField04 + "',  " +
                    " user_field_05 =  '" + userField05 + "',  " +
                    " user_field_06 =  '" + userField06 + "',  " +
                    " user_field_07 =  '" + userField07 + "',  " +
                    " user_field_08 =  '" + userField08 + "',  " +
                    " user_field_09 =  '" + userField09 + "',  " +
                    " user_field_10 =  '" + userField10 + "',  " +
                    " created_by =  " + createdBy + ",  " +
                    " date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                    " last_amended_by =  " + lastAmendedBy + ",  " +
                    " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                    "  WHERE sequence = " + sequence;
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
               
                returnValue = " DELETE FROM   un_stock_job_req_header" +
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
                returnValue = " UPDATE   un_stock_job_req_header" +
                                "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) +
                                " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

