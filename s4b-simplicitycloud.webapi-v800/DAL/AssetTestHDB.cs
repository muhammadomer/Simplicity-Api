using Microsoft.VisualBasic;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{

	public class AssetTestHDB: MainDB
		{
			 
        public AssetTestHDB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

        public bool insert(out long sequence,AssetTestH obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(AssetTestHQueries.insert(this.DatabaseType, obj ), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into Asset Test H " + ex.Message + " " +  ex.InnerException);
            }
            return returnValue;
        }

        public long getAssetId(string assetSequence)
        {
            long returnValue = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {

                    using (OleDbCommand objCmd =
                        new OleDbCommand(AssetTestHQueries.getAssetId(this.DatabaseType, assetSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    returnValue = Convert.ToInt64(dr[0].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while getting assetID  " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        public bool updateBySequence(AssetTestH obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(AssetTestHQueries.update(this.DatabaseType,obj), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while updating " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        public bool updateLocked(long assetSequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(AssetTestHQueries.UpdateLocked(this.DatabaseType, assetSequence ), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while updating lock: " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }
        public long getMaxSequence()
        {
            long returnValue = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AssetTestHQueries.getMaxSequence(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            { 
                                while (dr.Read())
                                {
                                    returnValue = Convert.ToInt64(dr[0].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException);
                
            }
            return returnValue;
        }
        public List<AssetTestH> getAllBySequences(long sequence, long assetSequence, long typeSequence)
        {
            List<AssetTestH> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AssetTestHQueries.getSelectAllBySequences(this.DatabaseType, sequence, assetSequence, typeSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AssetTestH>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AssetTestH(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException);
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
                        new OleDbCommand(AssetTestHQueries.deleteBySequence(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException);
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
                        new OleDbCommand(AssetTestHQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

        private AssetTestH Load_AssetTestH(OleDbDataReader dr)
        {
            AssetTestH assetTest = null;
            try
            { 
                if(dr!=null)
                {
                    assetTest = new AssetTestH();
                    assetTest.Sequence = DBUtil.GetLongValue(dr,"sequence");
                    assetTest.FlgDeleted = DBUtil.GetBooleanValue(dr,"flg_deleted");
                    assetTest.AssetSequence = DBUtil.GetLongValue(dr, "asset_sequence");
                    assetTest.TypeSequence = DBUtil.GetLongValue(dr, "type_sequence");
                    assetTest.CheckTypeSequence = DBUtil.GetLongValue(dr, "check_type_sequence");
                    assetTest.EntityId = DBUtil.GetLongValue(dr,"entity_id");
                    assetTest.DateCheck = DBUtil.GetDateTimeValue(dr, "date_check");
                    assetTest.FlgLocked = DBUtil.GetBooleanValue(dr, "flg_locked");
                    assetTest.FlgComplete = DBUtil.GetBooleanValue(dr, "flg_complete");
                    assetTest.TestPassOrFail = DBUtil.GetLongValue(dr, "test_pass_or_fail");
                    assetTest.CheckLocation = DBUtil.GetStringValue(dr, "check_location");
                    assetTest.EngineHours = DBUtil.GetLongValue(dr, "engine_hours");
                    assetTest.NameShort = DBUtil.GetStringValue(dr, "name_short");
                }
            }
            catch(Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException);
            }
            return assetTest;
        }

    }
}