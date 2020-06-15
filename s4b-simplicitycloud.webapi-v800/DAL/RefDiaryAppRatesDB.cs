using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class RefDiaryAppRatesDB : MainDB
    {

        public RefDiaryAppRatesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public RefDiaryAppRates insertRefDiaryAppRates(RefDiaryAppRates obj)
        {
            RefDiaryAppRates returnValue = new RefDiaryAppRates();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    string sql = "select MAX(AppTypeCode) from un_ref_diary_app_rates";
                    using (OleDbCommand cmdObj = new OleDbCommand(sql, conn))
                    {
                        Object result = cmdObj.ExecuteScalar();
                        obj.RateSequence = Convert.ToInt32(result) + 1;
                    }
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(RefDiaryAppRatesQueries.insert(this.DatabaseType, obj), conn))
                    {
                        if (objCmdInsert.ExecuteNonQuery() > 0)
                        {
                            returnValue = obj;
                        }
                        else
                        {
                            returnValue = null;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }


        public List<RefDiaryAppRates> selectAllRefDiaryAppRatesrateSequence(long rateSequence)
        {
            List<RefDiaryAppRates> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefDiaryAppRatesQueries.getSelectAllByrateSequence(this.DatabaseType, rateSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefDiaryAppRates>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefDiaryAppRates(dr));
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

        public List<RefDiaryAppRates> selectAllRefDiaryAppRates()
        {
            List<RefDiaryAppRates> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(RefDiaryAppRatesQueries.getSelectAllDiaryAppRates(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<RefDiaryAppRates>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_RefDiaryAppRates(dr));
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
        public RefDiaryAppRates updateByrateSequence(RefDiaryAppRates obj)
        {
            RefDiaryAppRates returnValue = new RefDiaryAppRates();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefDiaryAppRatesQueries.update(this.DatabaseType, obj), conn))
                    {
                        if (objCmdUpdate.ExecuteNonQuery() > 0)
                        {
                            returnValue = obj;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteByrateSequence(long rateSequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefDiaryAppRatesQueries.delete(this.DatabaseType, rateSequence), conn))
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

        public bool deleteByFlgDeleted(long rateSequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(RefDiaryAppRatesQueries.deleteFlagDeleted(this.DatabaseType, rateSequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
        private RefDiaryAppRates Load_RefDiaryAppRates(OleDbDataReader dr)

        {
            RefDiaryAppRates refDiaryAppRates = null;
            try
            {
                if (dr != null)
                {

                    refDiaryAppRates = new RefDiaryAppRates();
                    refDiaryAppRates.RateSequence = long.Parse(dr["rate_sequence"].ToString());
                    refDiaryAppRates.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    refDiaryAppRates.RowIndex = Utilities.GetDBString(dr["row_index"]);
                    refDiaryAppRates.RateDesc = Utilities.GetDBString(dr["rate_desc"]);

                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return refDiaryAppRates;
        }

    }
}
