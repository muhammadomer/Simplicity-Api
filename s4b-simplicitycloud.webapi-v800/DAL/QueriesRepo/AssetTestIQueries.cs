using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetTestIQueries
    { 

        public static string getSelectAllBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT ati.sequence, ati.join_sequence, ati.asset_sequence, ati.test_item_id,
                    ati.input_done_by, ati.input_comments, ati.date_input_done, ati.input_checked_by,
                    ratat.action_type_desc, rati.input_type_id, rati.flg_label, rati.test_item_code, rati.test_item_description,
                    rati.test_item_instruction, rati.test_item_location, rati.test_item_criteria
                FROM un_ref_asset_test_items AS rati 
                    INNER JOIN ( un_asset_test_i AS ati ON ati.test_item_id = rati.test_item_id
                        INNER JOIN un_ref_asset_test_action_types AS ratat ON rati.action_type_id = ratat.action_type_id)
                WHERE ati.join_sequence = " + sequence;
                returnValue += " ORDER BY rati.row_index";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
       
        public static string insert(string databaseType, AssetTestI obj,long testListId)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_asset_test_i(join_sequence, asset_sequence, test_item_id,input_comments
                    ,input_done_by,date_input_done,input_checked_by, created_by, date_created) 
                 Values(" + obj.JoinSequence + " , " + obj.AssetSequence
                + " ," + obj.TestItemId + ",'" + obj.InputComments + "'"
                + ",'" + Utilities.GetDBString(obj.InputDoneBy) + "'," + Utilities.GetDateTimeForDML(databaseType, obj.DateInputDone, true, true)
                + ",'" + Utilities.GetDBString(obj.InputCheckedBy) + "'"
                + "," + obj.CreatedBy
                + "," + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true) + ")";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insertMissingTestItem(string databaseType, long assetTestHSequence, long assetId, long testListId, DateTime dateCreated, long userId)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_asset_test_i (join_sequence, asset_sequence, test_item_id, created_by, date_created) 
                SELECT " + assetTestHSequence + " AS join_sequence," + assetId + " AS asset_sequence, rati.test_item_id "
                + "," + userId + " AS created_by," + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + @" AS date_created
                FROM un_ref_asset_test_items AS rati
                WHERE rati.test_list_id = " + testListId
                + " AND rati.test_item_id NOT IN (SELECT ati.test_item_id FROM un_asset_test_i AS ati WHERE ati.join_sequence = " + assetTestHSequence + ")";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string update(string databaseType, AssetTestI obj)
        {
            string returnValue = "";
            try
            {
                returnValue = @"UPDATE un_asset_test_i SET 
                    SET input_done_by = '" + obj.InputDoneBy + "'"
            + " , input_comments = '" + obj. InputComments +"',"
            +",  date_input_done = " + Utilities.GetDateValueForDML(databaseType,obj.DateInputDone)
            +",       input_checked_by = '"+ obj.InputCheckedBy + "'"
            + ",       last_amended_by = " + obj.LastAmendedBy 
            +",       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType,obj.DateLastAmended,true,true)
            + " WHERE sequence = " + obj.Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       

    }
}

