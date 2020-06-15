using Microsoft.AspNetCore.Http;

using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class AssetRegisterRepository : IAssetRegisterRepository
    {
        //
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public AssetRegisterRepository()
        {
            //
        }

        public List<AssetRegister> getAssetsList(HttpRequest request)
        {
            List<AssetRegister> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    AssetRegisterDB AssetDB = new AssetRegisterDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = AssetDB.getAllAssetsList();
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        public ResponseModel GetSelectAllAssetsList(HttpRequest Request, ClientRequest clientRequest)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        AssetRegisterDB objDB = new AssetRegisterDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = objDB.selectAllAssetsList(clientRequest, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public List<AssetRegister> search(FilterOption filterOption, HttpRequest request)
        {
            List<AssetRegister> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {

                    AssetRegisterDB AssetDB = new AssetRegisterDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = AssetDB.Search(filterOption);
                }
                
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        public List<AssetDetail> getAssetsDetail(long sequence, HttpRequest request)
        {
            List<AssetDetail> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        AssetRegisterDB AssetDB = new AssetRegisterDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = AssetDB.getAssetsDetail(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        public AssetRegister getAssetRegisterByLocationMakeModelTypeSearialNo(HttpRequest request, string location, string make, string model, string type, string serialNo)
        {
            AssetRegister returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    AssetRegisterDB AssetDB = new AssetRegisterDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = AssetDB.getAssetRegisterByLocationMakeModelTypeSearialNo(location, make, model, type, serialNo);
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

		public AssetRegister insert(HttpRequest request, AssetRegister obj)
		{
			AssetRegister returnValue = null;
			long sequence = -1;
			try
			{
				ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
				if (settings != null)
				{
					AssetRegisterDB AssetDB = new AssetRegisterDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
					if (AssetDB.insertAssetRegister(out sequence, false, obj.TransType, obj.EntityId ?? 0, obj.ItemJoinDept ?? 0,
													obj.ItemJoinCategory ?? 0, obj.ItemJoinSupplementary ?? 0, obj.ItemManufacturer,
													obj.ItemModel, obj.ItemSerialRef, obj.ItemExtraInfo, obj.ItemUserField1,
													obj.ItemUserField2, obj.ItemUserField3, obj.ItemQuantity,
													obj.DateInstalled, obj.DateAcquired, obj.DateDisposed, obj.ItemValueBook,
													obj.ItemValueDepreciation, obj.ItemValueDisposal, obj.ItemDesc,
													obj.ItemAddress, obj.FlgUseAddressId, obj.ItemAddressId ?? 0,
													obj.ItemLocationJoinId ?? 0, obj.ItemLocation, obj.FlgItemChargeable, obj.ItemCostMaterialRate,
													obj.ItemCostLabourRate, obj.ItemCostAssetRateWeek, obj.ItemCostLabourRateWeek, obj.FlgService, obj.ServiceStartDay ?? 0, obj.ServiceStartMonth ?? 0,
													obj.ServiceRenewal ?? 0, obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended))
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

		public AssetRegister Update(HttpRequest request, AssetRegister obj)
		{
			AssetRegister returnValue = null;
			long sequence = -1;
			try
			{
				ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
				if (settings != null)
				{
					AssetRegisterDB AssetDB = new AssetRegisterDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

					if (AssetDB.insertAssetRegister(out sequence, false, obj.TransType, obj.EntityId ?? 0, obj.ItemJoinDept ?? 0,
													obj.ItemJoinCategory ?? 0, obj.ItemJoinSupplementary ?? 0, obj.ItemManufacturer,
													obj.ItemModel, obj.ItemSerialRef, obj.ItemExtraInfo, obj.ItemUserField1,
													obj.ItemUserField2, obj.ItemUserField3, obj.ItemQuantity,
													obj.DateInstalled, obj.DateAcquired, obj.DateDisposed, obj.ItemValueBook,
													obj.ItemValueDepreciation, obj.ItemValueDisposal, obj.ItemDesc,
													obj.ItemAddress, obj.FlgUseAddressId, obj.ItemAddressId ?? 0,
													obj.ItemLocationJoinId ?? 0, obj.ItemLocation, obj.FlgItemChargeable, obj.ItemCostMaterialRate,
													obj.ItemCostLabourRate, obj.ItemCostAssetRateWeek, obj.ItemCostLabourRateWeek, obj.FlgService, obj.ServiceStartDay ?? 0, obj.ServiceStartMonth ?? 0,
													obj.ServiceRenewal ?? 0, obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended))
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
	}
}
