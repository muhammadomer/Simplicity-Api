using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;

namespace SimplicityOnlineWebApi.DAL
{
    public class S4bCheckTimesheetDB : MainDB
    {

        public S4bCheckTimesheetDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insert(out long sequence, S4bCheckTimeSheet obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(S4bCheckTimeSheetQueries.insert(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

		public bool deleteBySequence(S4bCheckTimeSheet obj)
		{
			bool returnValue = false;
			try
			{
				using (OleDbConnection conn = this.getDbConnection())
				{
					using (OleDbCommand objCmd =
						new OleDbCommand(S4bCheckTimeSheetQueries.deleteBySequence(this.DatabaseType, obj), conn))
					{
						objCmd.ExecuteNonQuery();
					}
				}
				returnValue = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return returnValue;
		}

	}
}
