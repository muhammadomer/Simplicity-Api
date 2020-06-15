using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{

    public class AssetRegisterCatsDB:MainDB
		{
			 
        public AssetRegisterCatsDB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

        public bool insertAssetRegisterCats(out long assetCategoryId, bool flgDeleted, string assetCategoryDesc)
            {
                bool returnValue = false;
                assetCategoryId = -1;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdInsert =
                            new OleDbCommand(AssetRegisterCatsQueries.insert(this.DatabaseType, flgDeleted, assetCategoryDesc ), conn))
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
                                    assetCategoryId = long.Parse(dr[0].ToString());
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

      public List<AssetRegisterCats> selectAllAsset_Register_CatsAssetCategoryId(long assetCategoryId)
            {
                List<AssetRegisterCats> returnValue = null;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdSelect =
                            new OleDbCommand(AssetRegisterCatsQueries.getSelectAllByAssetCategoryId(this.DatabaseType, assetCategoryId), conn))
                        {
                            using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    returnValue = new List<AssetRegisterCats>();
                                    while (dr.Read())
                                    {
                                        returnValue.Add(Load_Asset_Register_Cats(dr));
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

        public bool updateByAssetCategoryId(long assetCategoryId, bool flgDeleted, string assetCategoryDesc)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterCatsQueries.update(this.DatabaseType, assetCategoryId,flgDeleted, assetCategoryDesc), conn))
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

        public bool deleteByAssetCategoryId(long assetCategoryId)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterCatsQueries.delete(this.DatabaseType, assetCategoryId), conn))
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

        public bool deleteByFlgDeleted(long assetCategoryId)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterCatsQueries.deleteFlagDeleted(this.DatabaseType, assetCategoryId), conn))
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

        private AssetRegisterCats Load_Asset_Register_Cats(OleDbDataReader dr)
            {
            AssetRegisterCats assetRegisterCats = null;
                try
                { 
                    if(dr!=null)
                    {
                        assetRegisterCats = new AssetRegisterCats();
                        assetRegisterCats.AssetCategoryId = long.Parse(dr["asset_category_id"].ToString());
                        assetRegisterCats.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                        assetRegisterCats.AssetCategoryDesc = Utilities.GetDBString(dr["asset_category_desc"]);
                    
                    }
                }
                catch(Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                    // Requires Logging
                }
                return assetRegisterCats;
            }		
		}
}