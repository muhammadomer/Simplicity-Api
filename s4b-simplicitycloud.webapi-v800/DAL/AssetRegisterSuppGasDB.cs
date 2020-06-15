using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{

    public class AssetRegisterSuppGasDB : MainDB
    {

        public AssetRegisterSuppGasDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertAssetRegisterSuppGas(out long sequence, long joinSequence, long entityId, string assetGasType, bool flgGasFixing, string gasFixing, bool flgGasType,
                                               string gasType, bool flgGasFuel, string gasFuel, bool flgGasEfficiency, string gasEfficiency, bool flgGasFlueType,
                                               string gasFlueType, bool flgGasFlueing, string gasFlueing, bool flgGasOvUvSs, string GasOvUvSs,
                                               bool flgGasExpansionVessel, bool flgGasExpansion, string gasExpansion, bool flgGasImmersion, string gasImmersion,
                                               int lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(AssetRegisterSuppGasQueries.insert(this.DatabaseType, joinSequence, entityId, assetGasType, flgGasFixing, gasFixing, flgGasType,
                                                                          gasType, flgGasFuel, gasFuel, flgGasEfficiency, gasEfficiency, flgGasFlueType,
                                                                          gasFlueType, flgGasFlueing, gasFlueing, flgGasOvUvSs, GasOvUvSs,
                                                                          flgGasExpansionVessel, flgGasExpansion, gasExpansion, flgGasImmersion, gasImmersion,
                                                                          lastAmendedBy, dateLastAmended), conn))
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

        public List<AssetRegisterSuppGas> selectAllAssetRegisterSuppGasSequence(long sequence)
        {
            List<AssetRegisterSuppGas> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AssetRegisterSuppGasQueries.getSelectAllBySequence(sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AssetRegisterSuppGas>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AssetRegisterSuppGas(dr));
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

        public bool updateBySequence(long sequence, long joinSequence, long entityId, string assetGasType, bool flgGasFixing, string gasFixing, bool flgGasType,
                                     string gasType, bool flgGasFuel, string gasFuel, bool flgGasEfficiency, string gasEfficiency, bool flgGasFlueType,
                                     string gasFlueType, bool flgGasFlueing, string gasFlueing, bool flgGasOvUvSs, string GasOvUvSs,
                                     bool flgGasExpansionVessel, bool flgGasExpansion, string gasExpansion, bool flgGasImmersion, string gasImmersion,
                                     long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(AssetRegisterSuppGasQueries.update(this.DatabaseType, sequence, joinSequence, entityId, assetGasType, flgGasFixing, gasFixing, flgGasType,
                                                                        gasType, flgGasFuel, gasFuel, flgGasEfficiency, gasEfficiency, flgGasFlueType,
                                                                        gasFlueType, flgGasFlueing, gasFlueing, flgGasOvUvSs, GasOvUvSs,
                                                                        flgGasExpansionVessel, flgGasExpansion, gasExpansion, flgGasImmersion, gasImmersion,
                                                                        lastAmendedBy, dateLastAmended), conn))
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
                        new OleDbCommand(AssetRegisterSuppGasQueries.delete(sequence), conn))
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

        private AssetRegisterSuppGas Load_AssetRegisterSuppGas(OleDbDataReader dr)
        {
            AssetRegisterSuppGas assetRegisterSuppGas = null;
            try
            {
                if (dr != null)
                {
                    assetRegisterSuppGas = new AssetRegisterSuppGas();
                    assetRegisterSuppGas.Sequence = long.Parse(dr["sequence"].ToString());
                    assetRegisterSuppGas.JoinSequence = long.Parse(dr["join_sequence"].ToString());
                    assetRegisterSuppGas.EntityId = long.Parse(dr["entity_id"].ToString());
                    assetRegisterSuppGas.AssetGasType = Utilities.GetDBString(dr["asset_gas_type"]);
                    assetRegisterSuppGas.FlgGasFixing = bool.Parse(dr["flg_gas_fixing"].ToString());
                    assetRegisterSuppGas.GasFixing = Utilities.GetDBString(dr["gas_fixing"]);
                    assetRegisterSuppGas.FlgGasType = bool.Parse(dr["flg_gas_type"].ToString());
                    assetRegisterSuppGas.GasType = Utilities.GetDBString(dr["gas_type"]);
                    assetRegisterSuppGas.FlgGasFuel = bool.Parse(dr["flg_gas_fuel"].ToString());
                    assetRegisterSuppGas.GasFuel = Utilities.GetDBString(dr["gas_fuel"]);
                    assetRegisterSuppGas.FlgGasEfficiency = bool.Parse(dr["flg_gas_efficiency"].ToString());
                    assetRegisterSuppGas.GasEfficiency = Utilities.GetDBString(dr["gas_efficiency"]);
                    assetRegisterSuppGas.FlgGasFlueType = bool.Parse(dr["flg_gas_flue_type"].ToString());
                    assetRegisterSuppGas.GasFlueType = Utilities.GetDBString(dr["gas_flue_type"]);
                    assetRegisterSuppGas.FlgGasFlueing = bool.Parse(dr["flg_gas_flueing"].ToString());
                    assetRegisterSuppGas.GasFlueing = Utilities.GetDBString(dr["gas_flueing"]);
                    assetRegisterSuppGas.FlgGasOvUvSs = bool.Parse(dr["flg_gas_ov_uv_ss"].ToString());
                    assetRegisterSuppGas.GasOvUvSs = Utilities.GetDBString(dr["gas_ov_uv_ss"]);
                    assetRegisterSuppGas.FlgGasExpansionVessel = bool.Parse(dr["flg_gas_expansion_vessel"].ToString());
                    assetRegisterSuppGas.FlgGasExpansion = bool.Parse(dr["flg_gas_expansion"].ToString());
                    assetRegisterSuppGas.GasExpansion = Utilities.GetDBString(dr["gas_expansion"]);
                    assetRegisterSuppGas.FlgGasImmersion = bool.Parse(dr["flg_gas_immersion"].ToString());
                    assetRegisterSuppGas.GasImmersion = Utilities.GetDBString(dr["gas_immersion"]);
                    assetRegisterSuppGas.LastAmendedBy = Int32.Parse(dr["last_amended_by"].ToString());
                    assetRegisterSuppGas.DateLastAmended = DateTime.Parse(dr["date_last_amended"].ToString());
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return assetRegisterSuppGas;
        }
    }
}