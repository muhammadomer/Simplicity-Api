using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{

    public class AssetRegisterSuppToolsDB : MainDB
    {
        public AssetRegisterSuppToolsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertAssetRegisterSuppTools(out long sequence, long joinSequence, string assetId, long vehicleSequence, bool flgMinimumAge, long minimumAge, bool flgCertificationReq,
                                                long powerById, long gradingId, long acquiredTypeId, long serviceTypeId, long stateTypeId, bool flgOutOfService,
                                                DateTime? dateOutOfService, bool flgDueInService, DateTime? dateDueInService, string assetNotes, long lastAmendedBy,
                                                DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(AssetRegisterSuppToolsQueries.insert(this.DatabaseType, joinSequence, assetId, vehicleSequence, flgMinimumAge, minimumAge, flgCertificationReq,
                                                                             powerById, gradingId, acquiredTypeId, serviceTypeId, stateTypeId, flgOutOfService,
                                                                             dateOutOfService, flgDueInService, dateDueInService, assetNotes, lastAmendedBy,
                                                                             dateLastAmended), conn))
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

        public List<AssetRegisterSuppTools> selectAllAssetRegisterSuppToolsSequence(long sequence)
        {
            List<AssetRegisterSuppTools> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AssetRegisterSuppToolsQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AssetRegisterSuppTools>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AssetRegisterSuppTools(dr));
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

        public bool updateBySequence(long sequence, long joinSequence, string assetId, long vehicleSequence, bool flgMinimumAge, long minimumAge, bool flgCertificationReq,
                                    long powerById, long gradingId, long acquiredTypeId, long serviceTypeId, long stateTypeId, bool flgOutOfService,
                                    DateTime? dateOutOfService, bool flgDueInService, DateTime? dateDueInService, string assetNotes, long lastAmendedBy,
                                    DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(AssetRegisterSuppToolsQueries.update(this.DatabaseType, sequence, joinSequence, assetId, vehicleSequence, flgMinimumAge, minimumAge, flgCertificationReq,
                                                                             powerById, gradingId, acquiredTypeId, serviceTypeId, stateTypeId, flgOutOfService,
                                                                             dateOutOfService, flgDueInService, dateDueInService, assetNotes, lastAmendedBy,
                                                                             dateLastAmended), conn))
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
                        new OleDbCommand(AssetRegisterSuppToolsQueries.delete(this.DatabaseType, sequence), conn))
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

        private AssetRegisterSuppTools Load_AssetRegisterSuppTools(OleDbDataReader dr)

        {
            AssetRegisterSuppTools assetRegisterSuppTools = null;
            try
            {
                if (dr != null)
                {
                    assetRegisterSuppTools = new AssetRegisterSuppTools();
                    assetRegisterSuppTools.Sequence = long.Parse(dr["sequence"].ToString());
                    assetRegisterSuppTools.JoinSequence = long.Parse(dr["join_sequence"].ToString());
                    assetRegisterSuppTools.AssetId = Utilities.GetDBString(dr["asset_id"]);
                    assetRegisterSuppTools.VehicleSequence = long.Parse(dr["vehicle_sequence"].ToString());
                    assetRegisterSuppTools.FlgMinimumAge = bool.Parse(dr["flg_minimum_age"].ToString());
                    assetRegisterSuppTools.MinimumAge = long.Parse(dr["minimum_age"].ToString());
                    assetRegisterSuppTools.FlgCertificationReq = bool.Parse(dr["flg_certification_re"].ToString());
                    assetRegisterSuppTools.PowerById = long.Parse(dr["powered_by_id"].ToString());
                    assetRegisterSuppTools.GradingId = long.Parse(dr["grading_id"].ToString());
                    assetRegisterSuppTools.AcquiredTypeId = long.Parse(dr["acquired_type_id"].ToString()); ;
                    assetRegisterSuppTools.ServiceTypeId = long.Parse(dr["service_type_id"].ToString());
                    assetRegisterSuppTools.StateTypeId = long.Parse(dr["state_type_id"].ToString());
                    assetRegisterSuppTools.FlgOutOfService = bool.Parse(dr["flg_out_of_service"].ToString());
                    assetRegisterSuppTools.DateDueInService = Utilities.getSQLDate(DateTime.Parse(dr["date_out_of_service"].ToString()));
                    assetRegisterSuppTools.FlgDueInService = bool.Parse(dr["flg_due_in_service"].ToString());
                    assetRegisterSuppTools.DateDueInService = Utilities.getSQLDate(DateTime.Parse(dr["date_due_in_service"].ToString()));
                    assetRegisterSuppTools.AssetNotes = Utilities.GetDBString(dr["asset_notes"]);
                    assetRegisterSuppTools.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    assetRegisterSuppTools.DateLastAmended = Utilities.getSQLDate(DateTime.Parse(dr["date_last_amended"].ToString()));
                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return assetRegisterSuppTools;
        }

    }
}