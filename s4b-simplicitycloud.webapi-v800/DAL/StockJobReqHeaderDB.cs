using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineDAL
{
    public class StockJobReqHeaderDB : MainDB
    {

        public StockJobReqHeaderDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool InsertStockJobReqHeader(out long sequence, bool flgDeleted, long jobSequence, bool flgAuthorised, long authorisedBy, DateTime? dateAuthorised, 
                                            int poType, bool flgPoPlaced, long poSequence, string userField01, string userField02, string userField03, 
                                            string userField04, string userField05, string userField06, string userField07, string userField08, 
                                            string userField09, string userField10, int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "StockJobReqHeaderDB.InsertStockJobReqHeader()";
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(StockJobReqHeaderQueries.Insert(this.DatabaseType, flgDeleted, jobSequence, flgAuthorised, authorisedBy, dateAuthorised, poType,
                                                                         flgPoPlaced, poSequence, userField01, userField02, userField03, userField04, userField05, userField06, 
                                                                         userField07, userField08, userField09, userField10, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Inserting Stock Job Req Header.", ex);
            }
            return returnValue;
        }

        public List<StockJobReqHeader> SelectAllStockJobReqHeaderSequence(long sequence)
        {
            const string METHOD_NAME = "StockJobReqHeaderDB.SelectAllStockJobReqHeaderSequence()";
            List<StockJobReqHeader> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(StockJobReqHeaderQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<StockJobReqHeader>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_StockJobReqHeader(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Selecting All Stock Job Req Header By Sequence.", ex);
            }
            return returnValue;
        }

        public bool UpdateBySequence(long sequence, bool flgDeleted, long jobSequence, bool flgAuthorised, long authorisedBy, DateTime dateAuthorised, string poType,
                                    bool flgPoPlaced, long poSequence, string userField01, string userField02, string userField03, string userField04, string userField05,
                                    string userField06, string userField07, string userField08, string userField09, string userField10, long createdBy, DateTime dateCreated,
                                    long lastAmendedBy, DateTime dateLastAmended)
        {
            const string METHOD_NAME = "StockJobReqHeaderDB.UpdateBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockJobReqHeaderQueries.update(this.DatabaseType, sequence, flgDeleted, jobSequence, flgAuthorised, authorisedBy, dateAuthorised, poType,
                                                                         flgPoPlaced, poSequence, userField01, userField02, userField03, userField04, userField05, userField06,
                                                                         userField07, userField08, userField09, userField10, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating Stock Job Req Header By Sequence.", ex);
            }
            return returnValue;
        }

        public bool DeleteBySequence(long sequence)
        {
            const string METHOD_NAME = "StockJobReqHeaderDB.DeleteBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockJobReqHeaderQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Job Req Header By Sequence.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(long sequence)
        {
            const string METHOD_NAME = "StockJobReqHeaderDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockJobReqHeaderQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Job Req Header By Flag Deleted.", ex);
            }
            return returnValue;
        }

        private StockJobReqHeader Load_StockJobReqHeader(OleDbDataReader dr)
        {
            const string METHOD_NAME = "StockJobReqHeaderDB.Load_StockJobReqHeader()";
            StockJobReqHeader stockJobReqHeader = null;
            try
            {
                if (dr != null)
                {
                    stockJobReqHeader = new StockJobReqHeader();
                    stockJobReqHeader.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    stockJobReqHeader.FlgDeleted = DBUtil.GetBooleanValue(dr, "flg_deleted");
                    stockJobReqHeader.JobSequence = DBUtil.GetLongValue(dr, "job_sequence");
                    stockJobReqHeader.FlgAuthorised = DBUtil.GetBooleanValue(dr, "flg_authorised");
                    stockJobReqHeader.AuthorisedBy = DBUtil.GetLongValue(dr, "authorised_by");
                    stockJobReqHeader.DateAuthorised = DBUtil.GetDateTimeValue(dr, "date_authorised");
                    stockJobReqHeader.PoType = DBUtil.GetIntValue(dr, "po_type");
                    stockJobReqHeader.FlgPoPlaced = DBUtil.GetBooleanValue(dr, "flg_po_placed");
                    stockJobReqHeader.PoSequence = DBUtil.GetLongValue(dr, "po_sequence");
                    stockJobReqHeader.UserField01 = DBUtil.GetStringValue(dr, "user_field_01");
                    stockJobReqHeader.UserField02 = DBUtil.GetStringValue(dr, "user_field_02");
                    stockJobReqHeader.UserField03 = DBUtil.GetStringValue(dr, "user_field_03");
                    stockJobReqHeader.UserField04 = DBUtil.GetStringValue(dr, "user_field_04");
                    stockJobReqHeader.UserField05 = DBUtil.GetStringValue(dr, "user_field_05");
                    stockJobReqHeader.UserField06 = DBUtil.GetStringValue(dr, "user_field_06");
                    stockJobReqHeader.UserField07 = DBUtil.GetStringValue(dr, "user_field_07");
                    stockJobReqHeader.UserField08 = DBUtil.GetStringValue(dr, "user_field_08");
                    stockJobReqHeader.UserField09 = DBUtil.GetStringValue(dr, "user_field_09");
                    stockJobReqHeader.UserField10 = DBUtil.GetStringValue(dr, "user_field_10");
                    stockJobReqHeader.CreatedBy = Int32.Parse(dr["created_by"].ToString());
                    stockJobReqHeader.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    stockJobReqHeader.LastAmendedBy = Int32.Parse(dr["last_amended_by"].ToString());
                    stockJobReqHeader.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Loading Stock Job Req Header.", ex);
            }
            return stockJobReqHeader;
        }
    }
}