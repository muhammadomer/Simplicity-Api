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

	public class AssetTestIDB: MainDB
		{
			 
        public AssetTestIDB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

        public bool insert(out long sequence,AssetTestI obj, long testListId)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(AssetTestIQueries.insert(this.DatabaseType, obj, testListId), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into Asset Test I " + ex.Message + " " +  ex.InnerException);
            }
            return returnValue;
        }

        public bool insertMissingTestItem(long assetTestHSequence, long assetId, long testListId, long uerId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {

                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(AssetTestIQueries.insertMissingTestItem(this.DatabaseType, assetTestHSequence, assetId, testListId, DateTime.Now, uerId), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting Missing Test Items into Asset Test I " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }
        public bool updateBySequence(AssetTestI obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(AssetTestIQueries.update(this.DatabaseType,obj), conn))
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

      
        public List<AssetTestI> getAllBySequence(long sequence)
        {
            List<AssetTestI> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AssetTestIQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AssetTestI>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AssetTestI(dr));
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
        
        private AssetTestI Load_AssetTestI(OleDbDataReader dr)
        {
            AssetTestI assetTest = null;
            try
            { 
                if(dr!=null)
                {
                    assetTest = new AssetTestI();
                    assetTest.Sequence = DBUtil.GetLongValue(dr,"sequence");
                    assetTest.JoinSequence = DBUtil.GetLongValue(dr, "join_sequence");
                    assetTest.AssetSequence = DBUtil.GetLongValue(dr, "asset_sequence");
                    assetTest.TestItemId = DBUtil.GetLongValue(dr, "test_item_id");
                    assetTest.InputDoneBy = DBUtil.GetStringValue(dr, "input_done_by");
                    assetTest.InputComments = DBUtil.GetStringValue(dr, "input_comments");
                    assetTest.DateInputDone = DBUtil.GetDateTimeValue(dr, "date_input_done");
                    assetTest.InputCheckedBy = DBUtil.GetStringValue(dr, "input_checked_by");
                    assetTest.assetTestActionTypes.ActionTypeDescription = DBUtil.GetStringValue(dr, "action_type_desc");
                    assetTest.assetTestItems.InputTypeId = DBUtil.GetLongValue(dr, "input_type_id");
                    assetTest.assetTestItems.FlgLabel = DBUtil.GetBooleanValue(dr, "flg_label");
                    assetTest.assetTestItems.TestItemCode = DBUtil.GetStringValue(dr, "test_item_code");
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