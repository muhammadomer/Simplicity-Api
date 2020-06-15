using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetRegisterSuppGasQueries
    {

        public static string getSelectAllBySequence(long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM un_asset_register_supp_gas" +
                              " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long joinSequence, long entityId, string assetGasType, bool flgGasFixing, string gasFixing, bool flgGasType,
                                    string gasType, bool flgGasFuel, string gasFuel, bool flgGasEfficiency, string gasEfficiency, bool flgGasFlueType,
                                    string gasFlueType, bool flgGasFlueing, string gasFlueing, bool flgGasOvUvSs, string GasOvUvSs,
                                    bool flgGasExpansionVessel, bool flgGasExpansion, string gasExpansion, bool flgGasImmersion, string gasImmersion,
                                    int lastAmendedBy, DateTime? dateLastAmended)
        {

            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_asset_register_supp_gas(join_sequence, entity_id, asset_gas_type, flg_gas_fixing, gas_fixing, flg_gas_type, gas_type," +
                                                                    "flg_gas_fuel, gas_fuel, flg_gas_efficiency, gas_efficiency, flg_gas_flue_type, gas_flue_type," +
                                                                    "flg_gas_flueing, gas_flueing, flg_gas_ov_uv_ss, gas_ov_uv_ss, flg_gas_expansion_vessel," +
                                                                    "flg_gas_expansion, gas_expansion, flg_gas_immersion, gas_immersion, " +
                                                                    "last_amended_by, date_last_amended) " +
                              "VALUES (" + joinSequence + ", " + entityId + ", '" + assetGasType + "', " + Utilities.GetBooleanForDML(databaseType, flgGasFixing) + ", '" 
                              +  gasFixing + "', " + Utilities.GetBooleanForDML(databaseType, flgGasType) + ",' " 
                              + gasType + "', " + Utilities.GetBooleanForDML(databaseType, flgGasFuel) + ", '" + gasFuel + "', " 
                              + Utilities.GetBooleanForDML(databaseType, flgGasEfficiency) + ", '" + gasEfficiency + "', " + Utilities.GetBooleanForDML(databaseType, flgGasFlueType) + ", '" +
                               gasFlueType + "', " + Utilities.GetBooleanForDML(databaseType, flgGasFlueing) + ", '" + gasFlueing + "', " + Utilities.GetBooleanForDML(databaseType, flgGasOvUvSs) + ", '" + GasOvUvSs + "', " +
                               Utilities.GetBooleanForDML(databaseType, flgGasExpansionVessel) + ", " + Utilities.GetBooleanForDML(databaseType, flgGasExpansion) 
                               + ", '" + gasExpansion + "', " + Utilities.GetBooleanForDML(databaseType, flgGasImmersion) + ", '" + gasImmersion + "', " +
                               lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, long sequence, long joinSequence, long entityId, string assetGasType, bool flgGasFixing, string gasFixing, bool flgGasType,
                                    string gasType, bool flgGasFuel, string gasFuel, bool flgGasEfficiency, string gasEfficiency, bool flgGasFlueType,
                                    string gasFlueType, bool flgGasFlueing, string gasFlueing, bool flgGasOvUvSs, string GasOvUvSs,
                                    bool flgGasExpansionVessel, bool flgGasExpansion, string gasExpansion, bool flgGasImmersion, string gasImmersion,
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {

            string returnValue = "";
            try
            {

                returnValue = "UPDATE un_asset_register_supp_gas" +
                              "   SET join_sequence =  " + joinSequence + ", " +
                              "       entity_id =  " + entityId + ", " +
                              "       asset_gas_type =  '" + assetGasType + "', " +
                              "       flg_gas_fixing =  " + Utilities.GetBooleanForDML(databaseType, flgGasFixing) + ", " +
                              "       gas_fixing =  '" + gasFixing + "', " +
                              "       flg_gas_type =  " + Utilities.GetBooleanForDML(databaseType, flgGasType) + ", " +
                              "       gas_type =  '" + gasType + "', " +
                              "       flg_gas_fuel =  " + Utilities.GetBooleanForDML(databaseType, flgGasFuel) + ", " +
                              "       gas_fuel =  '" + gasFuel + "', " +
                              "       flg_gas_efficiency =  " + Utilities.GetBooleanForDML(databaseType, flgGasEfficiency) + ", " +
                              "       gas_efficiency =  '" + gasEfficiency + "', " +
                              "       flg_gas_flue_type =  " + Utilities.GetBooleanForDML(databaseType, flgGasFlueType) + ", " +
                              "       gas_flue_type =  '" + gasFlueType + "', " +
                              "       flg_gas_flueing =  " + Utilities.GetBooleanForDML(databaseType, flgGasFlueing) + ", " +
                              "       gas_flueing =  '" + gasFlueing + "', " +
                              "       flg_gas_ov_uv_ss =  " + Utilities.GetBooleanForDML(databaseType, flgGasOvUvSs) + ", " +
                              "       gas_ov_uv_ss =  '" + GasOvUvSs + "', " +
                              "       flg_gas_expansion_vessel = " + Utilities.GetBooleanForDML(databaseType, flgGasExpansionVessel) + ", " +
                              "       flg_gas_expansion = " + Utilities.GetBooleanForDML(databaseType, flgGasExpansion) + ", " +
                              "       gas_expansion = '" + gasExpansion + "', " +
                              "       flg_gas_immersion = " + flgGasImmersion + ", " +
                              "       gas_immersion = '" + gasImmersion + "', " +
                              "       last_amended_by = " + lastAmendedBy + ", " +
                              "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true)  +
                              " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string delete(long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM un_asset_register_supp_gas " +
                              " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

