using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public class S4BSubmissionsDataHQueries
    {
        internal static string SelectAll(string datebaseType)
        {
            string returnValue = "";
            
            returnValue = "Select * from un_s4b_submissions_data_h ";
            return returnValue;
        }

        public static string Insert(string databaseType, S4BSubmissionsDataH obj)
        {
            string returnValue = "";
           
            returnValue = "INSERT INTO un_s4b_submissions_data_h (join_sequence, flg_user_yes_no_1, "+
                            "       flg_user_yes_no_2, date_user_1, " +
                            "       date_user_2, user_amt_1, user_amt_2, " +
                            "       user_qty_1, user_qty_2, " +
                            "       user_text_1, user_text_2, user_memo_1, user_memo_2, " +
                            "       created_by, date_created, last_amended_by, date_last_amended) " +
                            "VALUES (" + obj.JoinSequence + ", " + Utilities.GetBooleanForDML(databaseType, obj.FlgYesOrNo1) + "," + 
                            "        " + Utilities.GetBooleanForDML(databaseType, obj.FlgYesOrNo2) + "," +
                            "        " + Utilities.GetDateTimeForDML(databaseType, obj.DateUser1,true,true) + "," +
                            "        " + Utilities.GetDateTimeForDML(databaseType, obj.DateUser2,true,true) + "," +
                            "        " + obj.UserAmt1 + "," +
                            "        " + obj.UserAmt2 + "," +
                            "        " + obj.UserQty1 + "," +
                            "        " + obj.UserQty2 + "," +
                            "       '" + obj.UserText1 + "'," +
                            "       '" + obj.UserText2 + "'," +
                            "       '" + obj.UserMemo1 + "'," +
                            "       '" + obj.UserMemo2 + "'," +
                            "        " + obj.CreatedBy + "," +
                            "        " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated,true,true) + ", " +
                            "        " + obj.CreatedBy + "," +
                            "        " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated,true,true) + ")";
                   
            return returnValue;
        }
    }
}
