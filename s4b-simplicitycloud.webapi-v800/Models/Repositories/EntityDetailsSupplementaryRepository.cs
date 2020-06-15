using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class EntityDetailsSupplementaryRepository:IEntityDetailsSupplementaryRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public  List<EntityDetailsSupplementary> GetSelectAllByEntityId(long entityId, HttpRequest request)
        {
            List<EntityDetailsSupplementary> returnVal = new List<EntityDetailsSupplementary>();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EntityDetailsSupplementaryDB edsDB = new EntityDetailsSupplementaryDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnVal = edsDB.GetSelectAllByEntityId(entityId);
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = "Error Occured While Getting Entity Details Supplementary Data for Entity Id '" + entityId + "'";
            }
            return returnVal;           
        }

        public bool UpdateEntityDetailSupplementary(long entityId, string dataType, string data, HttpRequest request)
        {            
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EntityDetailsSupplementaryDB orderDB = new EntityDetailsSupplementaryDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));                        
                    returnValue = orderDB.updateByentityId(entityId,dataType,data);
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception)
            {
                Message = "Error Occured While Updating Entity Details Supplementary Data '" + data + "' for data type '" + dataType + "'";
            }
            return returnValue;
        }

        public bool InsertEntityDetailSupplementary(HttpRequest request, long entityId, string dataType, string data)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EntityDetailsSupplementaryDB edsDB = new EntityDetailsSupplementaryDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if(edsDB.insertEntityDetailsSupplementary(entityId, dataType, data))
                    {
                        returnValue = true;
                    }
                    else
                    {
                        Message = edsDB.ErrorMessage;
                    }
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = "Error Occured While Inserting Entity Details Supplementary Data '" + data + "' for data type '" + dataType + "'. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool DeleteAllByEntityId(HttpRequest request, long entityId)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EntityDetailsSupplementaryDB edsDB = new EntityDetailsSupplementaryDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (edsDB.deleteByEntityId(entityId))
                    {
                        returnValue = true;
                    }
                    else
                    {
                        Message = edsDB.ErrorMessage;
                    }
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = "Error Occured While Deleting Entity Details Supplementary Data for Entity Id '" + entityId + "'. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        internal bool DeleteUpdateEntityDetailSupplementaryPropertyData(HttpRequest request, EntityDetailsCore edc)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if(DeleteAllByEntityId(request, edc.EntityId ?? 0))
                    {
                        if(InsertEntityDetailSupplementary(request, edc.EntityId ?? 0, SimplicityConstants.EntityDetailsSupplementaryDataTypePropertyDetails, edc.EntityDetails))
                        {
                            if (InsertEntityDetailSupplementary(request, edc.EntityId ?? 0, SimplicityConstants.EntityDetailsSupplementaryDataTypePropertyType, edc.PropertyType))
                            {
                                if (InsertEntityDetailSupplementary(request, edc.EntityId ?? 0, SimplicityConstants.EntityDetailsSupplementaryDataTypePropertyStatus, edc.PropertyStatus))
                                {
                                    if (InsertEntityDetailSupplementary(request, edc.EntityId ?? 0, SimplicityConstants.EntityDetailsSupplementaryDataTypeContactName, edc.NameTitle))
                                    {
                                        returnValue = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = "Error Occured While Deleting & Updating Entity Details Supplementary Data for Entity Id '" + edc.EntityId + "'. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        internal bool DeleteUpdateEntityDetailSupplementarySageData(HttpRequest request, EntityDetailsCore edc)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (DeleteAllByEntityId(request, edc.EntityId ?? 0))
                    {
                        if (InsertEntityDetailSupplementary(request, edc.EntityId ?? 0, SimplicityConstants.EntityDetailsSupplementaryDataTypeVATNumber, edc.SageVatNumber))
                        {
                            if (InsertEntityDetailSupplementary(request, edc.EntityId ?? 0, SimplicityConstants.EntityDetailsSupplementaryDataTypeNominalCode, edc.SageNominalCode))
                            {
                                if (InsertEntityDetailSupplementary(request, edc.EntityId ?? 0, SimplicityConstants.EntityDetailsSupplementaryDataTypeWebAddress, edc.WebAddress))
                                {
                                    if (InsertEntityDetailSupplementary(request, edc.EntityId ?? 0, SimplicityConstants.EntityDetailsSupplementaryDataTypeTaxCode, edc.SageDefaultTaxCode))
                                    {
                                        returnValue = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = "Error Occured While Deleting & Updating Entity Details Supplementary Data for Entity Id '" + edc.EntityId + "'. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
    }
}