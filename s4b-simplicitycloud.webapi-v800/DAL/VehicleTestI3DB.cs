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

	public class VehicleTestI3DB: MainDB
		{
			 
        public VehicleTestI3DB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

        public bool insert(out long sequence,VehicleTestI3 obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(VehicleTestI3Queries.insert(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while inserting into Vehicle Test I_3 " + ex.Message + " " +  ex.InnerException);
            }
            return returnValue;
        }

    }
}