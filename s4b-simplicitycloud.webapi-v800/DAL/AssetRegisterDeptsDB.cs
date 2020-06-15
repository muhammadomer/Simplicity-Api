using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{

    public class AssetRegisterDeptsDB: MainDB
		{
			 
        public AssetRegisterDeptsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
            }

        public bool insertAsset_Register_Dept(out long assetDeptId, bool flgDeleted, string assetDeptDesc)
            {
                bool returnValue = false;
                assetDeptId = -1;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdInsert =
                            new OleDbCommand(AssetRegisterDeptsQueries.insert(this.DatabaseType, flgDeleted, assetDeptDesc ), conn))
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
                                    assetDeptId = long.Parse(dr[0].ToString());
                                }
                                else
                                {
                                    //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.                                                 '" + METHOD_NAME + "'\n";
                                }
                            }
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

      public List<AssetRegisterDepts> selectAllAssetRegisterDeptsAssetCategoryId(long assetDeptId)
            {
                List<AssetRegisterDepts> returnValue = null;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdSelect =
                            new OleDbCommand(AssetRegisterDeptsQueries.getSelectAllByDeptId(this.DatabaseType, assetDeptId), conn))
                        {
                            using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    returnValue = new List<AssetRegisterDepts>();
                                    while (dr.Read())
                                    {
                                        returnValue.Add(Load_AssetRegisterDepts(dr));
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

        public bool updateByAssetDeptId(long assetDeptId, bool flgDeleted, string assetDeptDesc)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterDeptsQueries.update(this.DatabaseType, assetDeptId,flgDeleted, assetDeptDesc), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }

        public bool deleteByAssetDeptId(long assetDeptId)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterDeptsQueries.delete(this.DatabaseType, assetDeptId), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                                ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }

        public bool deleteByFlgDeleted(long assetDeptId)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterDeptsQueries.deleteFlagDeleted(this.DatabaseType, assetDeptId), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
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

        private AssetRegisterDepts Load_AssetRegisterDepts(OleDbDataReader dr)
            {
            AssetRegisterDepts assetRegisterDepts = null;
                try
                { 
                    if(dr!=null)
                    {
                    assetRegisterDepts = new AssetRegisterDepts();
                    assetRegisterDepts.AssetDeptId = long.Parse(dr["asset_dept_id"].ToString());
                    assetRegisterDepts.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    assetRegisterDepts.AssetDeptDesc = Utilities.GetDBString(dr["asset_dept_desc"]);

                }
                }
                catch(Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                    // Requires Logging
                }
                return assetRegisterDepts;
            }		
		}
}