using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class S4BSubmissionsDataHDB : MainDB
	{
        public S4BSubmissionsDataHDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insert(out long sequence, S4BSubmissionsDataH obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(S4BSubmissionsDataHQueries.Insert(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                   ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
                
        public List<S4BSubmissionsDataH> selectAll()
        {
            List<S4BSubmissionsDataH> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(S4BSubmissionsDataHQueries.SelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<S4BSubmissionsDataH>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_S4BSubmissionsDataH(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                             ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        private S4BSubmissionsDataH Load_S4BSubmissionsDataH(OleDbDataReader dr)
        {
            S4BSubmissionsDataH S4BSubmissionsDataH = null;
            try
            { 
                if(dr!=null)
                {
                    S4BSubmissionsDataH = new S4BSubmissionsDataH();
                    S4BSubmissionsDataH.Sequence = long.Parse(dr["sequence"].ToString());
                    S4BSubmissionsDataH.JoinSequence = long.Parse(dr["join_sequence"].ToString());
                    S4BSubmissionsDataH.FlgYesOrNo1 = bool.Parse(dr["flg_user_yes_no_1"].ToString());
                    S4BSubmissionsDataH.FlgYesOrNo2 = bool.Parse(dr["flg_user_yes_no_1"].ToString());
                    S4BSubmissionsDataH.DateUser1 = Utilities.getDBDate(dr["date_user_1"]);
                    S4BSubmissionsDataH.DateUser2 = Utilities.getDBDate(dr["date_user_2"]);
                    S4BSubmissionsDataH.UserAmt1 = double.Parse(dr["user_amt_1"].ToString());
                    S4BSubmissionsDataH.UserAmt2 = double.Parse(dr["user_amt_2"].ToString());
                    S4BSubmissionsDataH.UserQty1 = double.Parse(dr["user_qty_1"].ToString());
                    S4BSubmissionsDataH.UserQty2 = double.Parse(dr["user_qty_2"].ToString());
                    S4BSubmissionsDataH.UserText1 = Utilities.GetDBString(dr["user_text_1"]);
                    S4BSubmissionsDataH.UserText2 = Utilities.GetDBString(dr["user_text_2"]);
                    S4BSubmissionsDataH.UserMemo1 = Utilities.GetDBString(dr["user_memo_1"]);
                    S4BSubmissionsDataH.UserMemo2 = Utilities.GetDBString(dr["user_memo_2"]);
                }
            }
            catch(Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return S4BSubmissionsDataH;
        }
    }
}