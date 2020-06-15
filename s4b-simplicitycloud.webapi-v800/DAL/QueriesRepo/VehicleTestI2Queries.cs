using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class VehicleTestI2Queries
    { 

       
        public static string insert(string databaseType, VehicleTestI2 obj)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_vehicle_test_i2(join_sequence, asset_sequence, row_index,fault_desc
                    ,im_number,fix_fault_desc,done_by, created_by, date_created) 
                 Values(" + obj.JoinSequence + " , " + obj.AssetSequence
                + " ," + obj.RowIndex + ",'" + obj.FaultDesc +"'"
                + "," + obj.IMNumber +",'" + obj.FixFaultDesc +"'" +",'" + obj.DoneBy +"'"
                + "," + obj.CreatedBy
                + "," + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true) + ")";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


    }
}

