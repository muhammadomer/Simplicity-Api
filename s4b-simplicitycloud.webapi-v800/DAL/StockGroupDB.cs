using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineDAL
{
    public class StockGroupDB : MainDB
    {

        public StockGroupDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool InsertStockGroup(long entityId, bool flgHidden, string groupCode, int treeviewLevel, string parentCode, 
                                     string groupDesc, int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "StockGroupDB.InsertStockGroup()";
            bool returnValue = false;
            entityId = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(StockGroupQueries.Insert(this.DatabaseType, entityId, flgHidden, groupCode, treeviewLevel, parentCode, 
                                                                  groupDesc, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Inserting Stock Group.", ex);
            }
            return returnValue;
        }

        public List<StockGroup> SelectAllStockGroupByEntityId(long entityId)
        {
            const string METHOD_NAME = "StockGroupDB.SelectAllStockGroupByEntityId()";
            List<StockGroup> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(StockGroupQueries.getSelectAllByentityId(this.DatabaseType, entityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<StockGroup>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadStockGroup(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Selecting Stock Group By Entity Id.", ex);
            }
            return returnValue;
        }

        public StockGroup GetStockGroupByGroupCodeAndTreeviewLevel(string groupCode, int treeviewLevel)
        {
            const string METHOD_NAME = "StockGroupDB.GetStockGroupByGroupCodeAndTreeviewLevel()";
            StockGroup returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(StockGroupQueries.SelectAllByGroupCodeAndTreeviewLevel(this.DatabaseType, groupCode, treeviewLevel), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadStockGroup(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Stock Group By Group Code And Treeview Level.", ex);
            }
            return returnValue;
        }

        public bool UpdateByEntityId(long entityId, bool flgHidden, string groupCode, int treeviewLevel, string parentCode, 
                                     string groupDesc, int createdBy, DateTime? dateCreated, int lastAmendedBy, 
                                     DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "StockGroupDB.UpdateByEntityId()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockGroupQueries.update(this.DatabaseType, entityId, flgHidden, groupCode, treeviewLevel, parentCode, groupDesc, createdBy,
                                                                  dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating Stock Group.", ex);
            }
            return returnValue;
        }

        public bool DeleteByEntityId(long entityId)
        {
            const string METHOD_NAME = "StockGroupDB.DeleteByEntityId()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockGroupQueries.delete(this.DatabaseType, entityId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Group.", ex);
            }
            return returnValue;
        }

        public bool deleteByFlgDeleted(long entityId)
        {
            const string METHOD_NAME = "StockGroupDB.InsertStockGroup()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockGroupQueries.deleteFlagDeleted(this.DatabaseType, entityId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Group.", ex);
            }
            return returnValue;
        }

        private StockGroup LoadStockGroup(OleDbDataReader dr)
        {
            const string METHOD_NAME = "StockGroupDB.LoadStockGroup()";
            StockGroup stockGroup = null;
            try
            {
                if (dr != null)
                {
                    stockGroup = new StockGroup();
                    stockGroup.EntityId = DBUtil.GetLongValue(dr, "entity_id");
                    stockGroup.FlgHidden = DBUtil.GetBooleanValue(dr, "flg_hidden");
                    stockGroup.GroupCode = DBUtil.GetStringValue(dr, "group_code");
                    stockGroup.TreeviewLevel = DBUtil.GetIntValue(dr, "treeview_level");
                    stockGroup.ParentCode = DBUtil.GetStringValue(dr, "parent_code");
                    stockGroup.GroupDesc = DBUtil.GetStringValue(dr, "group_desc");
                    stockGroup.CreatedBy = DBUtil.GetIntValue(dr, "created_by");
                    stockGroup.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    stockGroup.LastAmendedBy = DBUtil.GetIntValue(dr, "last_amended_by");
                    stockGroup.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Loading Stock Group.", ex);
            }
            return stockGroup;
        }
    }
}