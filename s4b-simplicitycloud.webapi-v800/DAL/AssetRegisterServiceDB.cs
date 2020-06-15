using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{

    public class AssetRegisterServiceDB : MainDB
		{

        public AssetRegisterServiceDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
            }

        public bool insertAssetRegisterService(out long sequence, bool flgDeleted, bool flgNotActive, long assetSequence, long jobSequence, long daSequence, long daAppType, DateTime? dateDaStart,
                                                DateTime? dateService, string serviceInitial, string serviceNotes, long conditionSequence, long serviceBy, bool flgNewJobCreated,
                                                bool flgNewApp, bool flgValidation, long validatedBy, DateTime? dateValidated, long createdBy,
                                                DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
            {
                bool returnValue = false;
                sequence = -1;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdInsert =
                            new OleDbCommand(AssetRegisterServiceQueries.insert(this.DatabaseType, flgDeleted, flgNotActive, assetSequence, jobSequence, daSequence, daAppType, dateDaStart,
                                                                                dateService, serviceInitial, serviceNotes, conditionSequence, serviceBy,  flgNewJobCreated,
                                                                                flgNewApp, flgValidation, validatedBy, dateValidated, createdBy,
                                                                                dateCreated, lastAmendedBy, dateLastAmended), conn))
                        {
                            objCmdInsert.ExecuteNonQuery();
                            sequence = Utilities.GetDBAutoNumber(conn);
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

      public List<AssetRegisterService> selectAllAssetRegisterServiceSequence(long sequence)
            {
                List<AssetRegisterService> returnValue = null;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdSelect =
                            new OleDbCommand(AssetRegisterServiceQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                        {
                            using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    returnValue = new List<AssetRegisterService>();
                                    while (dr.Read())
                                    {
                                        returnValue.Add(Load_AssetRegisterService(dr));
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

        public bool updateBySequence(long sequence, bool flgDeleted, bool flgNotActive, long assetSequence, long jobSequence, long daSequence, long daAppType, DateTime? dateDaStart,
                                    DateTime? dateService, string serviceInitial, string serviceNotes, long conditionSequence, long serviceBy, bool flgNewJobCreated,
                                    bool flgNewApp, bool flgValidation, long validatedBy, DateTime? dateValidated, long createdBy,
                                    DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterServiceQueries.update(this.DatabaseType, sequence, flgDeleted, flgNotActive, assetSequence, jobSequence, daSequence, daAppType, dateDaStart,
                                                                            dateService, serviceInitial, serviceNotes, conditionSequence, serviceBy, flgNewJobCreated,
                                                                            flgNewApp, flgValidation, validatedBy, dateValidated, createdBy,
                                                                            dateCreated, lastAmendedBy, dateLastAmended), conn))
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
        
        public bool deleteBySequence(long sequence)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterServiceQueries.delete(this.DatabaseType, sequence), conn))
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

        public bool deleteByFlgDeleted(long sequence)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(AssetRegisterServiceQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                             ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }

        private AssetRegisterService Load_AssetRegisterService(OleDbDataReader dr)
            {
            AssetRegisterService assetRegisterService = null;
                try
                { 
                    if(dr!=null)
                    {
                    assetRegisterService = new AssetRegisterService();
                    assetRegisterService.Sequence = long.Parse(dr["sequence"].ToString());
                    assetRegisterService.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    assetRegisterService.FlgNotActive = bool.Parse(dr["flg_not_active"].ToString());
                    assetRegisterService.AssetSequence = long.Parse(dr["asset_sequence"].ToString());
                    assetRegisterService.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    assetRegisterService.DaSequence = long.Parse(dr["de_sequence"].ToString());
                    assetRegisterService.DaAppType = long.Parse(dr["da_app_type"].ToString());
                    assetRegisterService.DateDaStart = DateTime.Parse(dr["data_da_start"].ToString());
                    assetRegisterService.DateService = DateTime.Parse(dr["date_service"].ToString());
                    assetRegisterService.ServiceInitials = Utilities.GetDBString(dr["service_initials"]);
                    assetRegisterService.ServiceNotes = Utilities.GetDBString(dr["service_notes"]);
                    assetRegisterService.ConditionSequence = long.Parse(dr["condition_sequence"].ToString());
                    assetRegisterService.ServiceBy = long.Parse(dr["service_by"].ToString());
                    assetRegisterService.FlgNewJobCreated = bool.Parse(dr["flg_new_job_created"].ToString());
                    assetRegisterService.FlgNewApp = bool.Parse(dr["flg_new_app"].ToString());
                    assetRegisterService.FlgValidated = bool.Parse(dr["flg_validated"].ToString());
                    assetRegisterService.ValidatedBy = long.Parse(dr["validated_by"].ToString());
                    assetRegisterService.DateValidated = DateTime.Parse(dr["date_validated"].ToString());
                    assetRegisterService.CreatedBy = long.Parse(dr["created_by"].ToString());
                    assetRegisterService.DateCreated = DateTime.Parse(dr["date_created"].ToString());
                    assetRegisterService.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    assetRegisterService.DateLastAmended = DateTime.Parse(dr["date_last_amended"].ToString());
                }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return assetRegisterService;
            }
	
		}
}

                                                                          