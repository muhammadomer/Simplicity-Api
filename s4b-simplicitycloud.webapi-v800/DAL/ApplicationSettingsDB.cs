using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class ApplicationSettingsDB : MainDB
	{
			 
        public ApplicationSettingsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertApplication_Settings(string settingId, string setting1, string setting2, string setting3,
                                               string setting4)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(ApplicationSettingsQueries.insert(this.DatabaseType, settingId, setting1, setting2, setting3, setting4), conn))
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

        public List<ApplicationSettings> selectAll()
        {
            List<ApplicationSettings> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(ApplicationSettingsQueries.selectAll(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<ApplicationSettings>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Application_Settings(dr));
                                }
                            }
                        }
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

        public List<ApplicationSettings> selectAllApplication_SettingsSettingId(string settingId)
        {
            List<ApplicationSettings> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(ApplicationSettingsQueries.getSelectAllBySettingId(this.DatabaseType, settingId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<ApplicationSettings>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_Application_Settings(dr));
                                }
                            }
                        }
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

        internal bool updateSetting1BySettingId(string settingId, string setting1)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(ApplicationSettingsQueries.updateSetting1BySettingId(this.DatabaseType, settingId, setting1), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public ApplicationSettings selectBySettingId(string settingId)
        {
            ApplicationSettings returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(ApplicationSettingsQueries.getSelectAllBySettingId(this.DatabaseType, settingId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_Application_Settings(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // TODO:Requires Logging
            }
            return returnValue;
        }

        public bool updateBySettingId(string settingId, string setting1, string setting2, string setting3,
                                      string setting4)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(ApplicationSettingsQueries.update(this.DatabaseType, settingId, setting1, setting2, setting3, setting4), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
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
                            new OleDbCommand(ApplicationSettingsQueries.delete(this.DatabaseType, settingId), conn))
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
       
        private ApplicationSettings Load_Application_Settings(OleDbDataReader dr)
            {
            ApplicationSettings applicationSettings = null;
                try
                { 
                    if(dr!=null)
                    {
                        applicationSettings = new ApplicationSettings();
                        applicationSettings.SettingId = Utilities.GetDBString(dr["setting_id"]);
                        applicationSettings.Setting1 = Utilities.GetDBString(dr["setting_1"]);
                        applicationSettings.Setting2 = Utilities.GetDBString(dr["setting_2"]);
                        applicationSettings.Setting3 = Utilities.GetDBString(dr["setting_3"]);
                        applicationSettings.Setting4 = Utilities.GetDBString(dr["setting_4"]);
                    }
                }
                catch(Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return applicationSettings;
            }
	
		}
}