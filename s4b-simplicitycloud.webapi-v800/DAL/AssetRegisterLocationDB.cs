using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{

	public class AssetRegisterLocationDB:MainDB
		{
			 
        public AssetRegisterLocationDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
            }

        public bool insertAssetRegisterLocation(out long sequence, long entityId, bool flgDeleted, bool flgUseBuilding, string assetLocationBuilding, bool flgUseFloor,
                                                string assetLocationFloor, bool flgUseRoom, string assetLocationRoom, long assetLocationRoomType, string assetLocation)
            {
                bool returnValue = false;
                sequence = -1;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdInsert =
                            new OleDbCommand(AssetRegisterLocationQueries.insert(this.DatabaseType, entityId, flgDeleted, flgUseBuilding, assetLocationBuilding, flgUseFloor, assetLocationFloor,
                                                                                flgUseRoom, assetLocationRoom, assetLocationRoomType, assetLocation), conn))
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
                                    sequence = long.Parse(dr[0].ToString());
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

      public List<AssetRegisterLocation> selectAllAssetRegisterLocationSequence(long sequence)
            {
                List<AssetRegisterLocation> returnValue = null;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdSelect =
                            new OleDbCommand(AssetRegisterLocationQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                        {
                            using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    returnValue = new List<AssetRegisterLocation>();
                                    while (dr.Read())
                                    {
                                        returnValue.Add(Load_AssetRegisterLocation(dr));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }

        public bool updateBySequence(long sequence, long entityId, bool flgDeleted, bool flgUseBuilding, string assetLocationBuilding, bool flgUseFloor,
                                     string assetLocationFloor, bool flgUseRoom, string assetLocationRoom, long assetLocationRoomType, string assetLocation)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterLocationQueries.update(this.DatabaseType, sequence, entityId, flgDeleted, flgUseBuilding, assetLocationBuilding, flgUseFloor, assetLocationFloor,
                                                                               flgUseRoom, assetLocationRoom, assetLocationRoomType, assetLocation), conn))
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
        
        public bool deleteBySequence(long sequence)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterLocationQueries.delete(this.DatabaseType, sequence), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }

        public bool deleteByFlgDeleted(long sequence)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterLocationQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
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

        private AssetRegisterLocation Load_AssetRegisterLocation(OleDbDataReader dr)
            {
            AssetRegisterLocation assetRegisterLocation = null;
                try
                { 
                    if(dr!=null)
                    {
                    assetRegisterLocation = new AssetRegisterLocation();
                    assetRegisterLocation.Sequence = long.Parse(dr["sequence"].ToString());
                    assetRegisterLocation.EntityId = long.Parse(dr["entity_id"].ToString());
                    assetRegisterLocation.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    assetRegisterLocation.FlgUseBuilding = bool.Parse(dr["flg_use_building"].ToString());
                    assetRegisterLocation.AssetLocationBuilding = Utilities.GetDBString(dr["asset_location_building"]);
                    assetRegisterLocation.FlgUseFloor = bool.Parse(dr["flg_use_floor"].ToString());
                    assetRegisterLocation.AssetLocationFloor = Utilities.GetDBString(dr["asset_location_floor"]);
                    assetRegisterLocation.FlgUseRoom = bool.Parse(dr["flg_use_room"].ToString());
                    assetRegisterLocation.AssetLocationRoom = Utilities.GetDBString(dr["asset_location_room"]);
                    assetRegisterLocation.AssetLocationRoomType = long.Parse(dr["asset_location_room_type"].ToString());
                    assetRegisterLocation.AssetLocation = Utilities.GetDBString(dr["asset_location"]); 
                    }
                }
                catch(Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return assetRegisterLocation;
            }
	
		}
}