using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class VehicleTestHQueries
    { 
        public static string insert(string databaseType, VehicleTestH obj)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_vehicle_test_h(asset_sequence, type_sequence, date_test,flg_locked,flg_complete
                    ,test_pass_or_fail,created_by, date_created) 
                VALUES (" + obj.AssetSequence +"," + obj.TypeSequence +"," + Utilities.GetDateTimeForDML(databaseType,obj.DateTest,true,true)
                + "," + Utilities.GetBooleanForDML(databaseType, obj.FlgLocked) +"," + Utilities.GetBooleanForDML(databaseType,obj.FlgComplete) 
                + "," + obj.TestPassOrFail +"," + obj.CreatedBy +"," + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated,true,true) + ")";
                
            }
            catch (Exception ex)
            {
            }
            Utilities.WriteLog(returnValue);
            return returnValue;
        }

        public static string UpdateLocked(string databaseType, long assetSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_vehicle_test_h SET " 
                        + "   flg_locked =  " + Utilities.GetBooleanForDML(databaseType,true)
                        + " WHERE asset_sequence  =  " + assetSequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
      


    }
}

