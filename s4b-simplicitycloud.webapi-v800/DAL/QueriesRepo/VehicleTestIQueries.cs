using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class VehicleTestIQueries
    { 

       
        public static string insert(string databaseType, VehicleTestI obj)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_vehicle_test_i(join_sequence, asset_sequence, section_id,row_id
                    ,input_data, created_by, date_created) 
                 Values(" + obj.JoinSequence + " , " + obj.AssetSequence
                + " ," + obj.SectionId + ",'" + obj.RowId +"'"
                + "," + obj.InputData 
                + "," + obj.CreatedBy
                + "," + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true) + ")";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insertMissingRows(string databaseType, long vehicleTestHSequence, long assetSequence, DateTime dateCreated, long userId)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_vehicle_test_i (join_sequence, asset_sequence, section_id ,row_id, input_data, created_by, date_created) 
                SELECT " + vehicleTestHSequence + " AS join_sequence," + assetSequence + " AS asset_sequence, rvttr.test_section_id, rvttr.test_row_id, 0 AS input_data "
                + "," + userId + " AS created_by," + Utilities.GetDateTimeForDML(databaseType,dateCreated,true,true) + @" AS date_created
                FROM un_ref_vehicle_test_type_row AS rvttr
                WHERE test_row_id NOT IN (SELECT row_id FROM un_vehicle_test_i AS ati WHERE ati.join_sequence =" + vehicleTestHSequence + ")"
                + " AND rvttr.test_row_type IN (1, 2) And rvttr.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType,true);
            
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
       

    }
}

