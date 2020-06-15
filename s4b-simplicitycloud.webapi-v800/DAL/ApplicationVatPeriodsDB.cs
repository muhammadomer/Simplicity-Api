using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace SimplicityOnlineWebApi.DAL
{

	public class ApplicationVatPeriodsDB:MainDB
		{
			 
        public ApplicationVatPeriodsDB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

        public bool insertApplication_Vat_Periods(out long sequence, bool flgDelete, long periodYear, long periodIndex, DateTime? datePeriodStart,
                                                  DateTime? datPeriodEnd)
            {
                bool returnValue = false;
                sequence = -1;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdInsert =
                            new OleDbCommand(ApplicationVatPeriodsQueries.insert(this.DatabaseType, flgDelete, periodYear, periodIndex, datePeriodStart, datPeriodEnd), conn))
                        {
                            objCmdInsert.ExecuteNonQuery();
                            string sql = "select @@IDENTITY";
                            using (OleDbCommand objCommand =
                                new OleDbCommand(sql, conn))
                            {
                                OleDbDataReader dr = objCommand.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    sequence = long.Parse(dr[0].ToString());
                                }
                                else
                                {
                                    //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.'" + METHOD_NAME + "'\n";
                                }
                            }
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

        public List<ApplicationVatPeriods> selectAllApplication_Vat_PeriodsBySequence(long sequence)
            {
                List<ApplicationVatPeriods> returnValue = null;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdSelect =
                            new OleDbCommand(ApplicationVatPeriodsQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                        {
                            using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    returnValue = new List<ApplicationVatPeriods>();
                                    while (dr.Read())
                                    {
                                        returnValue.Add(Load_Application_Vat_Periods(dr));
                                    }
                                }
                            }
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

        public bool updateBySequence(long sequence, bool flgDeleted, long periodYear, long periodIndex, DateTime? datePeriodStart,
                                     DateTime? datPeriodEnd)
             {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(ApplicationVatPeriodsQueries.update(this.DatabaseType, sequence, flgDeleted, periodYear, periodIndex, datePeriodStart, datPeriodEnd), conn))
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
        
       public bool deleteBysSequence(long sequence)
       
           {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(ApplicationVatPeriodsQueries.delete(this.DatabaseType, sequence), conn))
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
       

        private ApplicationVatPeriods Load_Application_Vat_Periods(OleDbDataReader dr)
        
            {
            ApplicationVatPeriods applicationVatPeriods = null;
                try
                { 
                    if(dr!=null)
                    {
                        applicationVatPeriods = new ApplicationVatPeriods();
                        applicationVatPeriods.Sequence = long.Parse(dr["sequence"].ToString());
                        applicationVatPeriods.FlgDelete = bool.Parse(dr["flg_delete"].ToString());
                        applicationVatPeriods.PeriodYear = long.Parse(dr["period_year"].ToString());
                        applicationVatPeriods.PeriodIndex = long.Parse(dr["period_index"].ToString());
                        applicationVatPeriods.DatPeriodStart = Utilities.getSQLDate(DateTime.Parse(dr["period_index"].ToString()));
                        applicationVatPeriods.DatPeriodEnd = Utilities.getSQLDate(DateTime.Parse(dr["period_index"].ToString()));

                }
                }
                catch(Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                    // Requires Logging
                }
                return applicationVatPeriods;
            }	
		}
}