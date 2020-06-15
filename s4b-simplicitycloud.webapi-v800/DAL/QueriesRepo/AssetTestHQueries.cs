using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetTestHQueries
    { 

        public static string getSelectAllBySequences(string databaseType, long sequence,long assetSequence, long typeSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT ath.sequence, ath.flg_deleted, ath.asset_sequence, ath.type_sequence,
                ath.date_check, ath.entity_id, ath.check_type_sequence, ath.flg_locked,
                ath.flg_complete, ath.test_pass_or_fail, ath.check_location, ath.engine_hours,edc.name_short
                FROM un_asset_test_h AS ath
                    LEFT JOIN un_entity_details_core AS edc ON ath.entity_id = edc.entity_id
                WHERE  1=1 ";
                if (assetSequence > 0)
                {
                    returnValue += " AND ath.asset_sequence = " + assetSequence;
                    if (typeSequence > 0)
                        returnValue += " AND ath.type_sequence = " + typeSequence;
                    returnValue += " And ath.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
                }else
                {
                    returnValue += " AND ath.sequence = " + sequence;
                }
                returnValue += " ORDER BY ath.date_check DESC";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string getMaxSequence(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT MAX(sequence) AS last_test_sequence
                FROM un_asset_test_h AS ath";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, AssetTestH obj)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_asset_test_h(asset_sequence, type_sequence, date_check,flg_locked,flg_complete,entity_id,check_location,engine_hours
                    ,created_by, date_created) 
                VALUES (" + obj.AssetSequence + "," + obj.TypeSequence + "," + Utilities.GetDateTimeForDML(databaseType, obj.DateCheck, true, true)
               + "," + Utilities.GetBooleanForDML(databaseType, obj.FlgLocked) + "," + Utilities.GetBooleanForDML(databaseType, obj.FlgComplete)
               + "," + obj.EntityId + ",'" + obj.CheckLocation + "'," + obj.EngineHours
               + "," + obj.CreatedBy + "," + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true) + ")";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getAssetId(string databaseType, string assetSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT arsp.join_sequence AS asset_id_sequence
                FROM un_asset_register_supp_plant AS arsp
                WHERE arsp.asset_id = '" + assetSequence + "'"
                + @" UNION
                SELECT arst.join_sequence AS asset_id_sequence
                FROM un_asset_register_supp_tools AS arst
                WHERE arst.asset_id = '" + assetSequence + "'"
                + @" UNION
                SELECT arsv.join_sequence  AS asset_id_sequence
                FROM un_asset_register_supp_vehicles AS arsv
                WHERE arsv.vehicle_reg = '" + assetSequence + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string update(string databaseType, AssetTestH obj)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_asset_test_h SET " +
                        "  entity_id =  " + obj.EntityId
                        +"  , flg_complete = " + Utilities.GetBooleanForDML(databaseType,obj.FlgComplete) 
                        + ", test_pass_or_fail =" + obj.TestPassOrFail
                        + ", check_location = '" + obj.CheckLocation +"'"
                        + ", engine_hours = " + obj.EngineHours 
                        + ", last_amended_by = " + obj.LastAmendedBy
                        + ", date_last_amended = " + Utilities.GetDateTimeForDML(databaseType,obj.DateLastAmended,true,true)
                        + " WHERE sequence =  " + obj.Sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string UpdateLocked(string databaseType, long assetSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_asset_test_h SET " 
                        + "   flg_locked =  " + Utilities.GetBooleanForDML(databaseType,true)
                        + " WHERE asset_sequence  =  " + assetSequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string deleteBySequence(string databaseType, long sequence)
       {
           string returnValue = "";
           try
           {
                returnValue = @"DELETE FROM un_asset_test_h 
                        WHERE sequence = " + sequence;
            }
           catch (Exception ex)
           {
           }
           return returnValue;
       }

       public static string deleteFlagDeleted(string databaseType, long sequence)
       {
           string returnValue = "";
           try
           {
                bool flg = true;
                returnValue = @"UPDATE un_asset_test_h SET
                    flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)
                    + " WHERE sequence = " + sequence;
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }       

    }
}

