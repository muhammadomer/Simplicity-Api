using System;
using System.Data.OleDb;
using System.Data;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL
{
    public class MainDB
    {
        public string DatabaseType { get; set; }
        public string ErrorMessage { get; set; }
        public string connectionString = "";

        public MainDB(DatabaseInfo dbInfo)
        {
            this.connectionString = dbInfo.ConnectionString;
            this.DatabaseType = dbInfo.DatabaseType;
            ErrorMessage = "";
        }

        public OleDbConnection getDbConnection()
        {
            OleDbConnection returnValue = null;
            try
            {
                returnValue = new OleDbConnection(connectionString);
                returnValue.Open();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error Occured While Opening Database Connection." + ex.Message + " " + ex.InnerException;
				//LOGGER.LogError(ErrorMessage);
				Utilities.WriteLog(ErrorMessage);
				throw ex;
            }
            return returnValue;
        }

        public bool ColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == columnName)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetDBValueFromQuery(string sql)
        {
            const string METHOD_NAME = "MainDB.GetDBValueFromQuery()";
            string returnValue = "";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(sql, conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = dr[0] != null && !DBNull.Value.Equals(dr[0]) ? dr[0].ToString() : "";
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while getting Value from Query. " + sql, ex);
            }
            return returnValue;
        }
    }
}
