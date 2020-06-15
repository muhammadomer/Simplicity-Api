using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL
{
    public class S4bFormsAssignDB : MainDB
    {

        public S4bFormsAssignDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public List<S4bFormsAssign> getAllAssignUser(long FormSeq)
        {
            List<S4bFormsAssign> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(S4bFormsAssignQueries.getAllAssignUser(this.DatabaseType, FormSeq), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<S4bFormsAssign>();
                                while(dr.Read())
                                {
                                    returnValue.Add(Load_S4bFormsAssign(dr));
                                }
                              
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<UserDetails> getUnAssignUser(long FormSequence)
        {
            List<UserDetails> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(S4bFormsAssignQueries.getUnAssignUsers(this.DatabaseType, FormSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<UserDetails>();
                                while(dr.Read())
                                {
                                    returnValue.Add(Load_UserDetails(dr));
                                }
                              
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool addFormAssign(long formSeq, long userId, int createdBy, DateTime? createdDate)
        {
            bool returnValue = false;
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdAdd = new OleDbCommand(S4bFormsAssignQueries.insert(this.DatabaseType, formSeq, userId, createdBy, createdDate), conn))
                {
                    int affectedRows = objCmdAdd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        returnValue = true;
                    }
                }
            }

            return returnValue;
        }

        public bool deleteFormAssign(long seq)
        {
            bool returnValue = false;
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdAdd = new OleDbCommand(S4bFormsAssignQueries.delete(this.DatabaseType, seq), conn))
                {
                    int affectedRows = objCmdAdd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        returnValue = true;
                    }

                }
            }

            return returnValue;
        }

        private S4bFormsAssign Load_S4bFormsAssign(OleDbDataReader dr)
        {
            S4bFormsAssign S4bFormsAssign = null;
            try
            {
                if (dr != null)
                {
                    S4bFormsAssign = new S4bFormsAssign();
                    S4bFormsAssign.Sequence = DBUtil.GetLongValue(dr,"sequence");
                    S4bFormsAssign.FormSequence = DBUtil.GetLongValue(dr,"form_sequence");
                    S4bFormsAssign.UserId = DBUtil.GetLongValue(dr,"user_id");
                    S4bFormsAssign.UserName = DBUtil.GetStringValue( dr,"user_name");
                    S4bFormsAssign.CreatedBy = DBUtil.GetLongValue(dr,"created_by");
                    S4bFormsAssign.DateCreated = DBUtil.GetDateTimeValue(dr,"date_created");
                    S4bFormsAssign.LastAmendedBy = DBUtil.GetLongValue(dr,"last_amended_by");
                    S4bFormsAssign.DateLastAmended = DBUtil.GetDateTimeValue(dr,"date_last_amended");
                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return S4bFormsAssign;
        }

        private UserDetails Load_UserDetails(OleDbDataReader dr)
        {
            UserDetails UserDetails = null;
            try
            {
                if (dr != null)
                {
                    UserDetails = new UserDetails();
                    UserDetails.UserId = Convert.ToInt32(dr["user_id"].ToString());
                    UserDetails.UserName = dr["user_name"].ToString();
                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return UserDetails;
        }

    }
}