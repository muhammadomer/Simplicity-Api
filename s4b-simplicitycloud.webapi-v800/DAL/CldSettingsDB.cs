using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class CldSettingsDB:MainDB
	{
			 
        public CldSettingsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertCldSettings(string settingName, string settingValue)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(CldSettingsQueries.insert(this.DatabaseType, settingName, settingValue), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public CldSettings SelectAllCldSettingsBySettingName(string settingName)
        {
            const string METHOD_NAME = "CldSettingsDB.SelectAllCldSettingsBySettingName()";
            CldSettings returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(CldSettingsQueries.getSelectAllBysettingName(this.DatabaseType, settingName), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadCldSettings(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting All Cld Settings By Setting Name.", ex);
            }
            return returnValue;
        }

        public List<CldSettings> SelectAllCldSettings()
        {
            const string METHOD_NAME = "CldSettingsDB.SelectAllCldSettings()";
            List<CldSettings> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(CldSettingsQueries.getSelectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<CldSettings>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadCldSettings(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = "No Cld Settings Found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting All Cld Settings." ,ex);
            }
            return returnValue;
        }

        public List<CldSettings> SelectAllSmartCldSettings()
        {
            const string METHOD_NAME = "CldSettingsDB.SelectAllSmartCldSettings()";
            List<CldSettings> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(CldSettingsQueries.getSelectAllForSmartSetting(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<CldSettings>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadCldSettings(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = "No Cld Settings Found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting All Cld Settings.", ex);
            }
            return returnValue;
        }



        public bool updateBySettingId(string settingName, string settingValue)
        {
            bool returnValue = false;
            string qry = @"UPDATE un_cld_settings SET setting_name = '" + settingName + "'," +
                  "  setting_value = '" + settingValue + "' " + " WHERE setting_name = '" + settingName + "'";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate = new OleDbCommand(qry, conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception while updating Setting by id/name");
            }
            return returnValue;
        }
        
        public bool deleteBySettingId(long settingId)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(CldSettingsQueries.delete(this.DatabaseType, settingId), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }
       
        private CldSettings LoadCldSettings(OleDbDataReader dr)
            {
            CldSettings cldSettings = null;
                try
                { 
                    if(dr!=null)
                    {                    
                        cldSettings = new CldSettings();
                        cldSettings.SettingName = Utilities.GetDBString(dr["setting_name"]);
                        cldSettings.SettingValue = Utilities.GetDBString(dr["setting_value"]);
                    }
                }
                catch(Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return cldSettings;
            }	
		}
}