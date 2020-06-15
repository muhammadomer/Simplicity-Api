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

	public class VehicleTestHDB: MainDB
		{
			 
        public VehicleTestHDB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

        public bool insert(out long sequence,VehicleTestH obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(VehicleTestHQueries.insert(this.DatabaseType, obj ), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into Vehicle Test H " + ex.Message + " " +  ex.InnerException);
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
                        new OleDbCommand(VehicleTestHQueries.UpdateLocked(this.DatabaseType, assetSequence ), conn))
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
       

    }
}