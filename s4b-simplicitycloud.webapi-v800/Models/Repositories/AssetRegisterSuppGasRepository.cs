using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class AssetRegisterSuppGasRepository : IAssetRegisterSuppGasRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public AssetRegisterSuppGas insert(HttpRequest request, AssetRegisterSuppGas obj)
        {
            AssetRegisterSuppGas returnValue = null;
            long sequence = -1;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    AssetRegisterSuppGasDB assetRegisterSuppGasDB = new AssetRegisterSuppGasDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (assetRegisterSuppGasDB.insertAssetRegisterSuppGas(out sequence, obj.JoinSequence ?? 0, obj.EntityId ?? 0, obj.AssetGasType,
                                                                            obj.FlgGasFixing, obj.GasFixing, obj.FlgGasType,
                                                                            obj.GasType, obj.FlgGasFuel, obj.GasFuel, obj.FlgGasEfficiency,
                                                                            obj.GasEfficiency, obj.FlgGasFlueType, obj.GasFlueType,
                                                                            obj.FlgGasFlueing, obj.GasFlueing, obj.FlgGasOvUvSs, obj.GasOvUvSs,
                                                                            obj.FlgGasExpansionVessel, obj.FlgGasExpansion, obj.GasExpansion,
                                                                            obj.FlgGasImmersion, obj.GasImmersion, obj.LastAmendedBy,
                                                                            obj.DateLastAmended))
                    {
                        obj.Sequence = sequence;
                        returnValue = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        public AssetRegisterSuppGas Update(HttpRequest request, AssetRegisterSuppGas obj)
        {
            AssetRegisterSuppGas returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    AssetRegisterSuppGasDB AssetDB = new AssetRegisterSuppGasDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (AssetDB.updateBySequence(obj.Sequence ?? 0, obj.JoinSequence ?? 0, obj.EntityId ?? 0, obj.AssetGasType,
                                                    obj.FlgGasFixing, obj.GasFixing, obj.FlgGasType,
                                                    obj.GasType, obj.FlgGasFuel, obj.GasFuel, obj.FlgGasEfficiency,
                                                    obj.GasEfficiency, obj.FlgGasFlueType, obj.GasFlueType,
                                                    obj.FlgGasFlueing, obj.GasFlueing, obj.FlgGasOvUvSs, obj.GasOvUvSs,
                                                    obj.FlgGasExpansionVessel, obj.FlgGasExpansion, obj.GasExpansion,
                                                    obj.FlgGasImmersion, obj.GasImmersion, obj.LastAmendedBy,
                                                    obj.DateLastAmended))
                    {
                        returnValue = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }
    }
}
