using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class MailMergeDB : MainDB
    {

        public MailMergeDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertRefMailMergeCodes(out string mergeCode, string mergeDesc, string oldTableName, string oldColumnName, string newTableName, string newColumnName, string mergeFormula)
        {
            bool returnValue = false;
            mergeCode = "";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(MailMergeQueries.insert(this.DatabaseType, mergeCode, mergeDesc, oldTableName, oldColumnName, newTableName, newColumnName, mergeFormula), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(sql, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                mergeCode = Utilities.GetDBString(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.'" + METHOD_NAME + "'\n";
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<RefMailMergeCodes> SelectAllByMergeCode(string mergeCode)
        {
            const string METHOD_NAME = "MailMergeDB.SelectAllByMergeCode()";
            List<RefMailMergeCodes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(MailMergeQueries.getSelectAllBymergeCode(this.DatabaseType, mergeCode), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefMailMergeCodes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadRefMailMergeCodes(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting All Ref Mail Merge Codes.", ex);
            }
            return returnValue;
        }

        public List<RefMailMergeCodes> SelectAll()
        {
            const string METHOD_NAME = "MailMergeDB.SelectAll()";
            List<RefMailMergeCodes> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(MailMergeQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefMailMergeCodes>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadRefMailMergeCodes(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = "No Mail Merge Code Found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting All Ref Mail Merge Codes.", ex);
            }
            return returnValue;
        }

        public List<RefMailMergeCodesMin> SelectAllMin()
        {
            const string METHOD_NAME = "MailMergeDB.SelectAllMin()";
            List<RefMailMergeCodesMin> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(MailMergeQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefMailMergeCodesMin>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadRefMailMergeCodesMin(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = "No Mail Merge Code Found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while getting All Ref Mail Merge Codes.", ex);
            }
            return returnValue;
        }

        public bool UpdateBymergeCode(string mergeCode, string mergeDesc, string oldTableName, string oldColumnName, string newTableName, string newColumnName, string mergeFormula)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(MailMergeQueries.update(this.DatabaseType, mergeCode, mergeDesc, oldTableName, oldColumnName, newTableName, newColumnName, mergeFormula), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting All Ref Mail Merge Codes. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool DeleteBymergeCode(string mergeCode)
        {
            const string METHOD_NAME = "MailMergeDB.DeleteBymergeCode()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(MailMergeQueries.delete(this.DatabaseType, mergeCode), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Deleting Mail Merge Codes By Merge Code.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(string mergeCode)
        {
            const string METHOD_NAME = "MailMergeDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(MailMergeQueries.deleteFlagDeleted(this.DatabaseType, mergeCode), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Deleting Mail Merge Codes By Flag Deleted.", ex);
            }
            return returnValue;
        }

        private RefMailMergeCodes LoadRefMailMergeCodes(OleDbDataReader dr)
        {
            const string METHOD_NAME = "MailMergeDB.LoadRefMailMergeCodes()";
            RefMailMergeCodes refMailMergeCodes = null;
            try
            {
                if (dr != null)
                {
                    refMailMergeCodes = new RefMailMergeCodes();
                    refMailMergeCodes.MergeCode = DBUtil.GetStringValue(dr, "merge_code");
                    refMailMergeCodes.MergeDesc = DBUtil.GetStringValue(dr, "merge_desc");
                    refMailMergeCodes.OldTableName = DBUtil.GetStringValue(dr, "old_table_name");
                    refMailMergeCodes.OldColumnName = DBUtil.GetStringValue(dr, "old_column_name");
                    refMailMergeCodes.NewTableName = DBUtil.GetStringValue(dr, "new_table_name");
                    refMailMergeCodes.NewColumnName = DBUtil.GetStringValue(dr, "new_column_name");
                    refMailMergeCodes.MergeFormula = DBUtil.GetStringValue(dr, "merge_formula");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Ref Mail Merge Codes.", ex);
            }
            return refMailMergeCodes;
        }

        private RefMailMergeCodesMin LoadRefMailMergeCodesMin(OleDbDataReader dr)
        {
            const string METHOD_NAME = "MailMergeDB.LoadRefMailMergeCodesMin()";
            RefMailMergeCodesMin refMailMergeCodes = null;
            try
            {
                if (dr != null)
                {
                    refMailMergeCodes = new RefMailMergeCodesMin();
                    refMailMergeCodes.MergeCode = DBUtil.GetStringValue(dr, "merge_code");
                    refMailMergeCodes.MergeDesc = DBUtil.GetStringValue(dr, "merge_desc");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Ref Mail Merge Codes.", ex);
            }
            return refMailMergeCodes;
        }
    }
}