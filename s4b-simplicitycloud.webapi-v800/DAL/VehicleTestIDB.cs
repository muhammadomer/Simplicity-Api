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

	public class VehicleTestIDB: MainDB
		{
			 
        public VehicleTestIDB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

        public bool insert(out long sequence,VehicleTestI obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(VehicleTestIQueries.insert(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into Vehicle Test I " + ex.Message + " " +  ex.InnerException);
            }
            return returnValue;
        }

        public bool insertMissingRows(long vehicleTestHSequence, long assetSequence, long uerId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {

                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(VehicleTestIQueries.insertMissingRows(this.DatabaseType, vehicleTestHSequence, assetSequence, DateTime.Now, uerId), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting Missing Rows into Vehicle Test I " + ex.Message + " " + ex.InnerException);
            }
            return returnValue;
        }

    }
}