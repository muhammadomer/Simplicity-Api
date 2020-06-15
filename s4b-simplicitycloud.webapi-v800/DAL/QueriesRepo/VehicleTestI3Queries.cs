using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class VehicleTestI3Queries
    { 

       
        public static string insert(string databaseType, VehicleTestI3 obj)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_vehicle_test_i3(join_sequence, asset_sequence, brake_test
                    ,flg_tapley_test,flg_roller_brake_test
                    ,laden_type , road_codition_type, break_test_main, break_test_secondary,break_test_parking
                    ,break_test_speed,break_test_speed_type,drawing_reg_no,test_mileage
                    , created_by, date_created) 
                 Values(" + obj.JoinSequence + " , " + obj.AssetSequence +"," + obj.BrakeTest
                + " ," + Utilities.GetBooleanForDML(databaseType, obj.FlgTapleyTest) + "," + Utilities.GetBooleanForDML(databaseType, obj.FlgRollerBrakeTest) 
                + "," + obj.LadenType +","  + obj.RoadCoditionType +"," + obj.BreakTestMain +","+ obj.BreakTestSecondary +"," + obj.BreakTestParking
                +"," + obj.BreakTestSpeed +"," + obj.BreakTestSpeedType +",'" + obj.DrawingRegNo + "'," + obj.TestMileage
                + "," + obj.CreatedBy
                + "," + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true) + ")";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insertMissingTestItem(string databaseType, long assetTestHSequence, long assetId, long testListId,DateTime dateCreated, long userId)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_asset_test_i (join_sequence, asset_sequence, test_item_id, created_by, date_created) 
                SELECT " + assetTestHSequence +" AS join_sequence," + assetId + " AS asset_sequence, rati.test_item_id "
                +"," + userId + " AS created_by," + Utilities.GetDateTimeForDML(databaseType,dateCreated,true,true) + @" AS date_created
                FROM un_ref_asset_test_items AS rati
                WHERE rati.test_list_id = " + testListId 
                +" AND rati.test_item_id NOT IN (SELECT ati.test_item_id FROM un_asset_test_i AS ati WHERE ati.join_sequence = " + assetTestHSequence +")";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
       

    }
}

