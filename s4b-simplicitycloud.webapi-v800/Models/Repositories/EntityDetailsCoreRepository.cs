using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SimplicityOnlineWebApi.Commons;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.IO;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class EntityDetailsCoreRepository : IEntityDetailsCoreRepository
    {
        public string Message { get; set; }
        private ILogger<EntityDetailsCoreRepository> _logger;
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public List<EntityDetailsCoreMin> GetSelectAllByTransType(HttpRequest Request, string transtype)
        {
            List<EntityDetailsCoreMin> returnVal = new List<EntityDetailsCoreMin>();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = DetailsCoreDB.getSelectAllByTransType(transtype,null);
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting Entities By Trans Type. " + ex.Message;
            }
            return returnVal;
        }

        public List<EntityDetailsCore> GetAllEmailAddresses(HttpRequest Request, long? jobSequence)
        {
            List<EntityDetailsCore> returnVal = new List<EntityDetailsCore>();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if(jobSequence>0)
                            returnVal = DetailsCoreDB.GetAllEmailAddresses(jobSequence);
                        else
                            returnVal = DetailsCoreDB.GetAllEmailAddresses(0);

                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting All Email Addresses. " + ex.Message;
            }
            return returnVal;
        }

        public List<EntityDetailsCoreMin> getFullAddress(HttpRequest Request, string address)
        {
            List<EntityDetailsCoreMin> returnVal = new List<EntityDetailsCoreMin>();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = DetailsCoreDB.getFullAddresses(address);

                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting Address. " + ex.Message;

            }
            return returnVal;
        }

        public List<EntityDetailsCoreMin> GetAllClientsByName(HttpRequest request, string name)
        {
            List<EntityDetailsCoreMin> returnVal = new List<EntityDetailsCoreMin>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = DetailsCoreDB.GetEntitiesByName(name, SimplicityConstants.ClientTransType);

                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting Clients By Name. " + ex.Message;
            }
            return returnVal;
        }

        public ResponseModel GetPropertyAddressesByAddressAndClientId(HttpRequest request, string address, long clientId)
        {
            const string METHOD_NAME = "EntityDetailsCoreRepository.GetPropertyAddressesByAddressAndClientId()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (clientId > 0)
                    {
                        EntityDetailsCoreDB entityDetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<EntityDetailsCoreMin> entityDetailsCoreMin = entityDetailsCoreDB.GetEntitiesByTransTypeAndAddressAndEntityJoinId(SimplicityConstants.PropertyTransType, address, clientId);
                        if (entityDetailsCoreMin != null)
                        {
                            returnValue.IsSucessfull = true;
                            returnValue.TheObject = entityDetailsCoreMin;
                        }
                        else
                        {
                            returnValue.Message = entityDetailsCoreDB.ErrorMessage;
                        }
                    }
                    else
                    {
                        returnValue.Message = "Please Select a Valid Client.";
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Property Address By Address and Client Id.", ex);
            }
            return returnValue;
        }

        public ResponseModel GetPropertyAddressesByAddress(HttpRequest request, string address)
        {
            const string METHOD_NAME = "EntityDetailsCoreRepository.GetPropertyAddressesByAddress()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EntityDetailsCoreDB entityDetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<EntityDetailsCoreMin> entityDetailsCoreMin = entityDetailsCoreDB.GetEntitiesByTransTypeAndAddress(SimplicityConstants.PropertyTransType, address);
                    if(entityDetailsCoreMin!=null)
                    {
                        returnValue.IsSucessfull = true;
                        returnValue.TheObject = entityDetailsCoreMin;
                    }
                    else
                    {
                        returnValue.Message = entityDetailsCoreDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Property Address By Address.", ex);
            }
            return returnValue;
        }

        public ResponseModel GetPropertyAddresses(ClientRequest requestModel, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        EntityDetailsCoreDB entityDetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = entityDetailsCoreDB.GetAddressByTransType(SimplicityConstants.PropertyTransType,requestModel, out count,true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = entityDetailsCoreDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting Address List. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }

        public ResponseModel GetAllClients(HttpRequest request, ClientRequest clientRequest)
        {  
            ResponseModel returnValue = new ResponseModel();
            string transType = "B";
            dynamic filter = JsonConvert.DeserializeObject<dynamic>(clientRequest.filters.ToString());
            if (filter.transType == "D")
            {
                transType = "D";
            }
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = DetailsCoreDB.getSelectAllByTransType(clientRequest, transType, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = DetailsCoreDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Gettingall Clients. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public ResponseModel GetAllSuppliers(HttpRequest request, ClientRequest clientRequest)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = DetailsCoreDB.getSelectAllSuppliers(clientRequest, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = DetailsCoreDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting all Suppliers. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public List<EntityDetailsCoreMin> GetPropertyAddress(HttpRequest Request, string transtype)
        {
            List<EntityDetailsCoreMin> returnVal = new List<EntityDetailsCoreMin>();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = DetailsCoreDB.getPropertyAddress(transtype);

                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting Property Addresses. " + ex.Message;
            }
            return returnVal;
        }

        public List<EntityDetailsCoreMin> GetAllSuppliers(RequestHeaderModel header, string qSearch)
        {
            List<EntityDetailsCoreMin> returnVal = new List<EntityDetailsCoreMin>();
            try
            {
                string projectId = header.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = DetailsCoreDB.getSelectAllByTransType(SimplicityConstants.SupplierTransType, qSearch);
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting All Suppliers. " + ex.Message;
            }
            return returnVal;
        }

        public ResponseModel GetAllContacts(HttpRequest Request, ClientRequest clientRequest)
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
						EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
						returnValue.TheObject = DetailsCoreDB.getSelectAllByTransType(SimplicityConstants.ContactTransType,null);
						returnValue.Count = count;
						if (returnValue.TheObject == null)
						{
							returnValue.Message = DetailsCoreDB.ErrorMessage;
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
				returnValue.Message = "Exception Occured While Gettingall Contacts. " + ex.Message + " " + ex.InnerException;
				_logger.LogError(ex.Message, ex);
			}
            return returnValue;
        }

        public EntityDetailsCore GetEntityByEntityId(RequestHeaderModel header, long? entityId)
        {
            EntityDetailsCore returnVal = new EntityDetailsCore();
            try
            {
                string projectId = header.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = DetailsCoreDB.getEntityByEntityid(entityId);
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = null;
            }
            return returnVal;
        }

        public EntityDetailsCore GetEntityByEntityId(HttpRequest Request, long? entityId)
        {
            EntityDetailsCore returnVal = new EntityDetailsCore();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));                       
                        returnVal = DetailsCoreDB.getEntityByEntityid(entityId);
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = null;
            }
            return returnVal;
        }

        public ResponseModel GetEntityNotesByEntityId(HttpRequest Request, long entityId)
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
                        EntityDetailsNotesDB NotesDB = new EntityDetailsNotesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = NotesDB.getByEntityId(entityId);
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = NotesDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }else{
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Gettingall Clients Notes " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
        public bool DeleteEntityByEntityId(HttpRequest Request, long entityId)
        {
            bool returnVal = false;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = DetailsCoreDB.deleteByFlgDeleted(entityId);
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = false;
            }
            return returnVal;
        }

        public EntityDetailsCore GetEntityByShortName(HttpRequest request, string shortName)
        {
            EntityDetailsCore returnVal = new EntityDetailsCore();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnVal = edcDB.getEntityByShortName(shortName);
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting Entity By Short Name " + ex.Message + " " + ex.InnerException;
            }
            return returnVal;
        }

        public EntityDetailsCore GetEntityByLongName(RequestHeaderModel header, string longName)
        {
            EntityDetailsCore returnVal = new EntityDetailsCore();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings != null)
                {
                    EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnVal = edcDB.GetEntityByLongName(longName);
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting Entity By Short Name " + ex.Message + " " + ex.InnerException;
            }
            return returnVal;
        }

        public string GetUniqueShortName(HttpRequest request, string LongName)
        {
            string returnVal = "";
            string shortName = "";
            try
            {
                LongName = LongName.Replace(" ", "_");
                //---Remove Special characters
                StringBuilder sb = new StringBuilder();
                foreach (char c in LongName)
                {
                    if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')  || c == '_')
                    {
                        sb.Append(c);
                    }
                }
                shortName = sb.ToString();
                if (shortName.Length > 32)
                {
                    shortName = shortName.Substring(0, 31);
                }
                EntityDetailsCore edcExisting = GetEntityByShortName(request, shortName);
                if (edcExisting != null) //----Name already exist
                {
                    //----Find unique name by adding numeric character
                    ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                    if (settings != null)
                    {
                        EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        shortName = edcDB.generateShortName(shortName);
                        returnVal = shortName.ToUpper();
                    }
                    else
                    {
                        Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                    }
                }else
                {
                    returnVal = shortName.ToUpper();
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting Entity By Short Name " + ex.Message + " " + ex.InnerException;
            }
            return returnVal;
        }
        
        public bool UpdateEntityDetailsCoreInfo(HttpRequest request, EntityDetailsCore edc, string infoType)
        {
            bool returnValue = false;
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
            if (settings != null)
            {
                EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                returnValue = edcDB.updateEDCInfo(edc, infoType);
            }
            else
            {
                Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
            }
            return returnValue;
        }

        //public ResponseModel InsertUpdateProperty(HttpRequest request, RequestModel reqModel)
        //{
        //    const string METHOD_NAME = "EntityDetailsCoreRepository.InsertUpdateProperty()";
        //    ResponseModel returnValue = new ResponseModel();
        //    try
        //    {
        //        ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
        //        if (settings != null)
        //        {
        //            if (reqModel != null && reqModel.TheObject != null)
        //            {
        //                EntityDetailsCore edc = JsonConvert.DeserializeObject<EntityDetailsCore>(reqModel.TheObject.ToString());
        //                edc.AddressFull = Utilities.GenerateFullAddressFromEntityDetailsCoreObject(edc);
        //                EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                if (edc.EntityId > 0)
        //                {
        //                    if(edcDB.update(edc.EntityId, edc.FlgDeleted, edc.FlgEntityOnHold, edc.FlgContactManager, edc.ClientType, edc.FlgInvoicingClient, edc.FlgEntityJoin,
        //                                    edc.EntityJoinId, edc.EntityApprovedStatus, edc.EntityPymtType, edc.FlgEformsPreferred, SimplicityConstants.NotAvailable, edc.NameLong, edc.SageId,
        //                                    edc.FlgSageTurnOn, edc.NameSage, edc.NameTitle, edc.NameInitilas, edc.NameForename, edc.NameSurname, edc.AddressNo,
        //                                    edc.AddressLine1, edc.AddressLine2, edc.AddressLine3, edc.AddressLine4, edc.AddressLine5, edc.AddressPostCode, edc.AddressFull,
        //                                    edc.Telephone, edc.TelExt, edc.TelFax, edc.TelMobile, edc.TelWork, edc.Email, edc.PropertyEpn, edc.PropertyUpn, edc.EntityDetails,
        //                                    edc.FlgSupAddressHeld, edc.FlgClientCheck, edc.UserListId, edc.UserListId2, edc.UserListId3, "0", edc.FlgUserField1,
        //                                    edc.FlgUserField2, edc.FlgUserField3, edc.FlgUserField4, edc.UserTextField1, edc.UserTextField2, edc.UserTextField3, edc.UserTextField4,
        //                                    edc.FlgUserDateField1, edc.DateUserDateField1, edc.FlgUserDateField2, edc.DateUserDateField2, edc.FlgUserDateField3, 
        //                                    edc.DateUserDateField3, edc.FlgUserDateField4, edc.DateUserDateField4, edc.LastAmendedBy, DateTime.Now))
        //                    {
        //                        EntityDetailsSupplementaryRepository edsRepos = new EntityDetailsSupplementaryRepository();
        //                        if(edsRepos.DeleteUpdateEntityDetailSupplementaryPropertyData(request, edc))
        //                        {
        //                            returnValue.TheObject = edc.EntityId;
        //                            returnValue.IsSucessfull = true;
        //                        }
        //                        else
        //                        {
        //                            returnValue.Message = edsRepos.Message;
        //                        }                                
        //                    }
        //                    else
        //                    {
        //                        returnValue.Message = edcDB.ErrorMessage;
        //                    }
        //                }
        //                else
        //                {
        //                    long entityId = -1;
        //                    if(edcDB.insertEntityDetailsCore(out entityId, edc.FlgDeleted, edc.FlgEntityOnHold, edc.FlgContactManager, edc.ClientType, edc.FlgInvoicingClient, edc.FlgEntityJoin,
        //                                                     edc.EntityJoinId, 0, 0, edc.FlgEformsPreferred, SimplicityConstants.NotAvailable, edc.NameLong, edc.SageId,
        //                                                     edc.FlgSageTurnOn, SimplicityConstants.NotAvailable, edc.NameTitle, edc.NameInitilas, edc.NameForename, edc.NameSurname, edc.AddressNo,
        //                                                     edc.AddressLine1, edc.AddressLine2, edc.AddressLine3, edc.AddressLine4, edc.AddressLine5, edc.AddressPostCode, edc.AddressFull,
        //                                                     edc.Telephone, edc.TelExt, edc.TelFax, edc.TelMobile, edc.TelWork, edc.Email, edc.PropertyEpn, edc.PropertyUpn, edc.EntityDetails,
        //                                                     edc.FlgSupAddressHeld, edc.FlgClientCheck, edc.UserListId, edc.UserListId2, edc.UserListId3, "0", edc.FlgUserField1,
        //                                                     edc.FlgUserField2, edc.FlgUserField3, edc.FlgUserField4, edc.UserTextField1, edc.UserTextField2, edc.UserTextField3, edc.UserTextField4,
        //                                                     edc.FlgUserDateField1, edc.DateUserDateField1, edc.FlgUserDateField2, edc.DateUserDateField2,
        //                                                     edc.FlgUserDateField3, edc.DateUserDateField3, edc.FlgUserDateField4, edc.DateUserDateField4,
        //                                                     edc.CreatedBy, DateTime.Now, edc.LastAmendedBy, edc.DateLastAmended == null ? DateTime.MinValue : edc.DateLastAmended))
        //                    {
        //                        edc.EntityId = entityId;
        //                        EntityDetailsJoinRepository edjRepos = new EntityDetailsJoinRepository();
        //                        if (edjRepos.InsertEntityDetailJoin(edc.EntityId, SimplicityConstants.PropertyTransType, request))
        //                        {
        //                            EntityDetailsSupplementaryRepository edsRepos = new EntityDetailsSupplementaryRepository();
        //                            if (edsRepos.DeleteUpdateEntityDetailSupplementaryPropertyData(request, edc))
        //                            {
        //                                returnValue.TheObject = edc.EntityId;
        //                                returnValue.IsSucessfull = true;
        //                            }
        //                            else
        //                            {
        //                                returnValue.Message = edsRepos.Message;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            returnValue.Message = edjRepos.Message;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        returnValue.Message = edcDB.ErrorMessage;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + reqModel.TheObject.ToString();
        //            }
        //        }
        //        else
        //        {
        //            returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        returnValue.Message = Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Insert/Update Property.", ex);
        //    }
        //    return returnValue;
        //}

        public ResponseModel InsertUpdateProperty(HttpRequest request, RequestModel reqModel)
        {
            const string METHOD_NAME = "EntityDetailsCoreRepository.InsertUpdateProperty()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (reqModel != null && reqModel.TheObject != null)
                    {
                        EntityDetailsCore edc = JsonConvert.DeserializeObject<EntityDetailsCore>(reqModel.TheObject.ToString());
                        edc.AddressFull = Utilities.GenerateFullAddressFromEntityDetailsCoreObject(edc);
                        EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (edc.EntityId > 0)
                        {
                            if (edcDB.updateAddress(edc.EntityId ?? 0, edc.FlgDeleted, edc.FlgEntityJoin,
                                            edc.EntityJoinId ?? 0, edc.AddressNo,
                                            edc.AddressLine1, edc.AddressLine2, edc.AddressLine3, edc.AddressLine4, edc.AddressLine5, edc.AddressPostCode, edc.AddressFull,
                                            edc.Telephone, edc.TelExt, edc.TelFax, edc.TelMobile, edc.TelWork, edc.Email, edc.PropertyEpn, edc.PropertyUpn, edc.EntityDetails,
                                            edc.LastAmendedBy ?? 0, DateTime.Now))
                            {
                                EntityDetailsSupplementaryRepository edsRepos = new EntityDetailsSupplementaryRepository();
                                if (edsRepos.DeleteUpdateEntityDetailSupplementaryPropertyData(request, edc))
                                {
                                    returnValue.TheObject = edc.EntityId;
                                    returnValue.IsSucessfull = true;
                                }
                                else
                                {
                                    returnValue.Message = edsRepos.Message;
                                }
                            }
                            else
                            {
                                returnValue.Message = edcDB.ErrorMessage;
                            }
                        }
                        else
                        {
                            long entityId = -1;
                            if (edcDB.insertAddress(out entityId, edc.FlgDeleted,  edc.FlgEntityJoin,
                                                             edc.EntityJoinId ?? 0,  edc.AddressNo,
                                                             edc.AddressLine1, edc.AddressLine2, edc.AddressLine3, edc.AddressLine4, edc.AddressLine5, edc.AddressPostCode, edc.AddressFull,
                                                             edc.Telephone, edc.TelExt, edc.TelFax, edc.TelMobile, edc.TelWork, edc.Email, edc.PropertyEpn, edc.PropertyUpn, edc.EntityDetails,
                                                             edc.CreatedBy ?? 0, DateTime.Now, edc.LastAmendedBy ?? 0, edc.DateLastAmended == null ? DateTime.MinValue : edc.DateLastAmended))
                            {
                                edc.EntityId = entityId;
                                EntityDetailsJoinRepository edjRepos = new EntityDetailsJoinRepository();
                                if (edjRepos.InsertEntityDetailJoin(edc.EntityId ?? 0, SimplicityConstants.PropertyTransType, request))
                                {
                                    EntityDetailsSupplementaryRepository edsRepos = new EntityDetailsSupplementaryRepository();
                                    if (edsRepos.DeleteUpdateEntityDetailSupplementaryPropertyData(request, edc))
                                    {
                                        returnValue.TheObject = edc.EntityId;
                                        returnValue.IsSucessfull = true;
                                    }
                                    else
                                    {
                                        returnValue.Message = edsRepos.Message;
                                    }
                                }
                                else
                                {
                                    returnValue.Message = edjRepos.Message;
                                }
                            }
                            else
                            {
                                returnValue.Message = edcDB.ErrorMessage;
                            }
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + reqModel.TheObject.ToString();
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Insert/Update Property.", ex);
            }
            return returnValue;
        }

        public ResponseModel InsertUpdateClient(HttpRequest request, object reqModel)

        {
            const string METHOD_NAME = "EntityDetailsCoreRepository.InsertUpdateClient()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (reqModel != null && reqModel != null)
                    {
                        EntityDetailsCore edc = JsonConvert.DeserializeObject<EntityDetailsCore>(reqModel.ToString());
                        edc.AddressFull = Utilities.GenerateFullAddressFromEntityDetailsCoreObject(edc);
                        EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (edc.EntityId > 0)
                        {
                            if(edcDB.update(edc.EntityId ?? 0, edc.FlgDeleted, edc.FlgEntityOnHold, edc.FlgContactManager, edc.ClientType ?? 0, edc.FlgInvoicingClient, edc.FlgEntityJoin,
                                            edc.EntityJoinId ?? 0, edc.EntityApprovedStatus ?? 0, edc.EntityPymtType ?? 0, edc.FlgEformsPreferred,  edc.NameLong, edc.SageId ?? 0,
                                            edc.FlgSageTurnOn, edc.NameSage, edc.NameTitle, edc.NameInitilas, edc.NameForename, edc.NameSurname, edc.AddressNo,
                                            edc.AddressLine1, edc.AddressLine2, edc.AddressLine3, edc.AddressLine4, edc.AddressLine5, edc.AddressPostCode, edc.AddressFull,
                                            edc.Telephone, edc.TelExt, edc.TelFax, edc.TelMobile, edc.TelWork, edc.Email, edc.PropertyEpn, edc.PropertyUpn, edc.EntityDetails,
                                            edc.FlgSupAddressHeld, edc.FlgClientCheck, edc.UserListId ?? 0, edc.UserListId2 ?? 0, edc.UserListId3 ?? 0, "0", edc.FlgUserField1,
                                            edc.FlgUserField2, edc.FlgUserField3, edc.FlgUserField4, edc.UserTextField1, edc.UserTextField2, edc.UserTextField3, edc.UserTextField4,
                                            edc.FlgUserDateField1, edc.DateUserDateField1, 
                                            edc.FlgUserDateField2, edc.DateUserDateField2,
                                            edc.FlgUserDateField3, edc.DateUserDateField3,
                                            edc.FlgUserDateField4, edc.DateUserDateField4,edc.FlgEntityApproved,
                                            Utilities.GetUserIdFromRequest(request), DateTime.Now))
                            {
                                returnValue.TheObject = edc.EntityId;
                                if (edc.EntityJoinId == 0)
                                    edcDB.updateEntityJoinId(edc.EntityId ?? 0, edc.EntityId ?? 0);
                                EntityDetailsSupplementaryRepository edsRepos = new EntityDetailsSupplementaryRepository();
                                if (edsRepos.DeleteUpdateEntityDetailSupplementarySageData(request, edc))
                                {
                                    returnValue.TheObject = edc.EntityId;
                                    returnValue.IsSucessfull = true;
                                }
                                else
                                {
                                    returnValue.Message = edsRepos.Message;
                                }
                                //---save internal notes
                                if (edc.EntityDetailNotes!=null)
                                {
                                    if (edc.EntityDetailNotes.Count > 0)
                                    {
                                        EntityDetailsNotesDB eNoteDB = new EntityDetailsNotesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                        foreach (EntityDetailsNotes entityNote in edc.EntityDetailNotes)
                                        {
                                            if (entityNote.Sequence > 0)
                                            {
                                                entityNote.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                                                entityNote.DateLastAmended = DateTime.Now;
                                                eNoteDB.updateBySequence(entityNote);
                                            }
                                            else
                                            {
                                                entityNote.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
                                                entityNote.DateCreated = DateTime.Now;
                                                long sequence = 0;
                                                eNoteDB.insertEntityNotes(out sequence, entityNote);
                                            }
                                        }
                                    }
                                }
                                //---Save Gdpr
                                EdcGdprDB gdprDB = new EdcGdprDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                EdcGdpr gdpr = gdprDB.selectByEntityId(edc.EntityId ?? 0);
                                if (gdpr == null)//Case: Insert Record does not Exist
                                {
                                    gdpr = new EdcGdpr();
                                    gdpr = edc.EdcGdpr;
                                    gdpr.EntityId = edc.EntityId;
                                    gdpr.CreatedBy = Utilities.GetUserIdFromRequest(request);
                                    gdpr.DateCreated = DateTime.Now;
                                    gdprDB.insert(gdpr);
                                }
                                else
                                {
                                    edc.EdcGdpr.EntityId = edc.EntityId;
                                    edc.EdcGdpr.LastAmendedBy = Utilities.GetUserIdFromRequest(request);
                                    edc.EdcGdpr.DateLastAmended = DateTime.Now;
                                    gdprDB.updateByEntityId(edc.EdcGdpr);
                                }
                                
                                returnValue.IsSucessfull = true;
                            }
                            else
                            {
                                returnValue.Message = edcDB.ErrorMessage;
                            }
                        }
                        else
                        {
                            EntityDetailsCore edcExisting = GetEntityByShortName(request, edc.NameShort);
                            if (edcExisting == null)
                            {
                                long EntityId = -1;
								edc.FlgInvoicingClient = true;
                                if (edcDB.insertEntityDetailsCore(out EntityId, edc.FlgDeleted, edc.FlgEntityOnHold, edc.FlgContactManager, edc.ClientType ?? 0, edc.FlgInvoicingClient, (edc.EntityJoinId>0 ? true:false),
                                                                  edc.EntityJoinId ?? 0, edc.EntityApprovedStatus ?? 0, edc.EntityPymtType ?? 0, edc.FlgEformsPreferred, edc.NameShort, edc.NameLong, edc.SageId ?? 0,
                                                                  edc.FlgSageTurnOn, SimplicityConstants.NotAvailable, edc.NameTitle, edc.NameInitilas, edc.NameForename, edc.NameSurname, edc.AddressNo,
                                                                  edc.AddressLine1, edc.AddressLine2, edc.AddressLine3, edc.AddressLine4, edc.AddressLine5, edc.AddressPostCode, edc.AddressFull,
                                                                  edc.Telephone, edc.TelExt, edc.TelFax, edc.TelMobile, edc.TelWork, edc.Email, edc.PropertyEpn, edc.PropertyUpn, edc.EntityDetails,
                                                                  edc.FlgSupAddressHeld, edc.FlgClientCheck, edc.UserListId ?? 0, edc.UserListId2 ?? 0, edc.UserListId3 ?? 0, "0", edc.FlgUserField1,
                                                                  edc.FlgUserField2, edc.FlgUserField3, edc.FlgUserField4, edc.UserTextField1, edc.UserTextField2, edc.UserTextField3, edc.UserTextField4,
                                                                  edc.FlgUserDateField1, edc.DateUserDateField1,
                                                                  edc.FlgUserDateField2, edc.DateUserDateField2,
                                                                  edc.FlgUserDateField3, edc.DateUserDateField3,
                                                                  edc.FlgUserDateField4, edc.DateUserDateField4,edc.FlgEntityApproved,
                                                                  Utilities.GetUserIdFromRequest(request), DateTime.Now, Utilities.GetUserIdFromRequest(request), DateTime.Now))
                                {
                                    edc.EntityId = EntityId;
                                    EntityDetailsJoinRepository edjRepos = new EntityDetailsJoinRepository();
                                    if (edjRepos.InsertEntityDetailJoin(edc.EntityId ?? 0, SimplicityConstants.ClientTransType, request))
                                    {
                                        returnValue.TheObject = edc.EntityId;
                                        if (edc.EntityJoinId==0)
                                            edcDB.updateEntityJoinId(EntityId,EntityId);

                                        //---Save Gdpr
                                        EdcGdprDB gdprDB = new EdcGdprDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                        EdcGdpr gdpr = new EdcGdpr();
                                        gdpr = edc.EdcGdpr;
                                        gdpr.EntityId = edc.EntityId;
                                        gdpr.CreatedBy = Utilities.GetUserIdFromRequest(request);
                                        gdpr.DateCreated = DateTime.Now;
                                        gdprDB.insert(gdpr);

                                        //---save internal notes
                                        if (edc.EntityDetailNotes != null)
                                        {
                                            if (edc.EntityDetailNotes.Count > 0)
                                            {
                                                EntityDetailsNotesDB eNoteDB = new EntityDetailsNotesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                                foreach (EntityDetailsNotes entityNote in edc.EntityDetailNotes)
                                                {
                                                    if (entityNote.Sequence > 0)
                                                    {
                                                        entityNote.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                                                        entityNote.DateLastAmended = DateTime.Now;
                                                        eNoteDB.updateBySequence(entityNote);
                                                    }
                                                    else
                                                    {
                                                        entityNote.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
                                                        entityNote.DateCreated = DateTime.Now;
                                                        long sequence = 0;
                                                        eNoteDB.insertEntityNotes(out sequence, entityNote);
                                                    }
                                                }
                                            }
                                        }
                                        returnValue.IsSucessfull = true;
                                    }
                                    else
                                    {
                                        returnValue.Message = edjRepos.Message;
                                    }
                                    EntityDetailsSupplementaryRepository edsRepos = new EntityDetailsSupplementaryRepository();
                                    if (edsRepos.DeleteUpdateEntityDetailSupplementarySageData(request, edc))
                                    {
                                        returnValue.TheObject = edc.EntityId;
                                        returnValue.IsSucessfull = true;
                                    }
                                    else
                                    {
                                        returnValue.Message = edsRepos.Message;
                                    }
                                }
                                else
                                {
                                    returnValue.Message = edcDB.ErrorMessage;
                                }
                            }
                            else
                            {
                                returnValue.Message = "Client Name Short must be Unique";
                            }
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + reqModel.ToString();
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Insert/Update Client.", ex);
            }
            return returnValue;
        }

		public ResponseModel InsertUpdateContact(HttpRequest request, object reqModel)

		{
			const string METHOD_NAME = "EntityDetailsCoreRepository.InsertUpdateContact()";
			ResponseModel returnValue = new ResponseModel();
			try
			{
				ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
				if (settings != null)
				{
					if (reqModel != null && reqModel != null)
					{
						EntityDetailsCore edc = JsonConvert.DeserializeObject<EntityDetailsCore>(reqModel.ToString());
						edc.AddressFull = Utilities.GenerateFullAddressFromEntityDetailsCoreObject(edc);
						EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
						if (edc.EntityId > 0)
						{
							if (edcDB.update(edc.EntityId ?? 0, edc.FlgDeleted, edc.FlgEntityOnHold, edc.FlgContactManager, edc.ClientType ?? 0, edc.FlgInvoicingClient, edc.FlgEntityJoin,
											edc.EntityJoinId ?? 0, edc.EntityApprovedStatus ?? 0, edc.EntityPymtType ?? 0, edc.FlgEformsPreferred, edc.NameLong, edc.SageId ?? 0,
											edc.FlgSageTurnOn, edc.NameSage, edc.NameTitle, edc.NameInitilas, edc.NameForename, edc.NameSurname, edc.AddressNo,
											edc.AddressLine1, edc.AddressLine2, edc.AddressLine3, edc.AddressLine4, edc.AddressLine5, edc.AddressPostCode, edc.AddressFull,
											edc.Telephone, edc.TelExt, edc.TelFax, edc.TelMobile, edc.TelWork, edc.Email, edc.PropertyEpn, edc.PropertyUpn, edc.EntityDetails,
											edc.FlgSupAddressHeld, edc.FlgClientCheck, edc.UserListId ?? 0, edc.UserListId2 ?? 0, edc.UserListId3 ?? 0, "0", edc.FlgUserField1,
											edc.FlgUserField2, edc.FlgUserField3, edc.FlgUserField4, edc.UserTextField1, edc.UserTextField2, edc.UserTextField3, edc.UserTextField4,
											edc.FlgUserDateField1, edc.DateUserDateField1,
											edc.FlgUserDateField2, edc.DateUserDateField2,
											edc.FlgUserDateField3, edc.DateUserDateField3,
											edc.FlgUserDateField4, edc.DateUserDateField4, edc.FlgEntityApproved,
											Utilities.GetUserIdFromRequest(request), DateTime.Now))
							{
								returnValue.TheObject = edc.EntityId;
								if (edc.EntityJoinId == 0)
									edcDB.updateEntityJoinId(edc.EntityId ?? 0, edc.EntityId ?? 0);
								EntityDetailsSupplementaryRepository edsRepos = new EntityDetailsSupplementaryRepository();
								if (edsRepos.DeleteUpdateEntityDetailSupplementarySageData(request, edc))
								{
									returnValue.TheObject = edc.EntityId;
									returnValue.IsSucessfull = true;
								}
								else
								{
									returnValue.Message = edsRepos.Message;
								}
								//---save internal notes
								if (edc.EntityDetailNotes != null)
								{
									if (edc.EntityDetailNotes.Count > 0)
									{
										EntityDetailsNotesDB eNoteDB = new EntityDetailsNotesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
										foreach (EntityDetailsNotes entityNote in edc.EntityDetailNotes)
										{
											if (entityNote.Sequence > 0)
											{
												entityNote.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
												entityNote.DateLastAmended = DateTime.Now;
												eNoteDB.updateBySequence(entityNote);
											}
											else
											{
												entityNote.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
												entityNote.DateCreated = DateTime.Now;
												long sequence = 0;
												eNoteDB.insertEntityNotes(out sequence, entityNote);
											}
										}
									}
								}
								//---Save Gdpr
								EdcGdprDB gdprDB = new EdcGdprDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
								EdcGdpr gdpr = gdprDB.selectByEntityId(edc.EntityId ?? 0);
								if (gdpr == null)//Case: Insert Record does not Exist
								{
									gdpr = new EdcGdpr();
									gdpr = edc.EdcGdpr;
									gdpr.EntityId = edc.EntityId;
									gdpr.CreatedBy = Utilities.GetUserIdFromRequest(request);
									gdpr.DateCreated = DateTime.Now;
									gdprDB.insert(gdpr);
								}
								else
								{
									edc.EdcGdpr.EntityId = edc.EntityId;
									edc.EdcGdpr.LastAmendedBy = Utilities.GetUserIdFromRequest(request);
									edc.EdcGdpr.DateLastAmended = DateTime.Now;
									gdprDB.updateByEntityId(edc.EdcGdpr);
								}

								returnValue.IsSucessfull = true;
							}
							else
							{
								returnValue.Message = edcDB.ErrorMessage;
							}
						}
						else
						{
							EntityDetailsCore edcExisting = GetEntityByShortName(request, edc.NameShort);
							if (edcExisting == null)
							{
								long EntityId = -1;
								edc.FlgInvoicingClient = true;
								if (edcDB.insertEntityDetailsCore(out EntityId, edc.FlgDeleted, edc.FlgEntityOnHold, edc.FlgContactManager, edc.ClientType ?? 0, edc.FlgInvoicingClient, (edc.EntityJoinId > 0 ? true : false),
																  edc.EntityJoinId ?? 0, edc.EntityApprovedStatus ?? 0, edc.EntityPymtType ?? 0, edc.FlgEformsPreferred, edc.NameShort, edc.NameLong, edc.SageId ?? 0,
																  edc.FlgSageTurnOn, SimplicityConstants.NotAvailable, edc.NameTitle, edc.NameInitilas, edc.NameForename, edc.NameSurname, edc.AddressNo,
																  edc.AddressLine1, edc.AddressLine2, edc.AddressLine3, edc.AddressLine4, edc.AddressLine5, edc.AddressPostCode, edc.AddressFull,
																  edc.Telephone, edc.TelExt, edc.TelFax, edc.TelMobile, edc.TelWork, edc.Email, edc.PropertyEpn, edc.PropertyUpn, edc.EntityDetails,
																  edc.FlgSupAddressHeld, edc.FlgClientCheck, edc.UserListId ?? 0, edc.UserListId2 ?? 0, edc.UserListId3 ?? 0, "0", edc.FlgUserField1,
																  edc.FlgUserField2, edc.FlgUserField3, edc.FlgUserField4, edc.UserTextField1, edc.UserTextField2, edc.UserTextField3, edc.UserTextField4,
																  edc.FlgUserDateField1, edc.DateUserDateField1,
																  edc.FlgUserDateField2, edc.DateUserDateField2,
																  edc.FlgUserDateField3, edc.DateUserDateField3,
																  edc.FlgUserDateField4, edc.DateUserDateField4, edc.FlgEntityApproved,
																  Utilities.GetUserIdFromRequest(request), DateTime.Now, Utilities.GetUserIdFromRequest(request), DateTime.Now))
								{
									edc.EntityId = EntityId;
									EntityDetailsJoinRepository edjRepos = new EntityDetailsJoinRepository();
									if (edjRepos.InsertEntityDetailJoin(edc.EntityId ?? 0, SimplicityConstants.ContactTransType, request))
									{
										returnValue.TheObject = edc.EntityId;
										if (edc.EntityJoinId == 0)
											edcDB.updateEntityJoinId(EntityId, EntityId);

										//---Save Gdpr
										EdcGdprDB gdprDB = new EdcGdprDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
										EdcGdpr gdpr = new EdcGdpr();
										gdpr = edc.EdcGdpr;
										gdpr.EntityId = edc.EntityId;
										gdpr.CreatedBy = Utilities.GetUserIdFromRequest(request);
										gdpr.DateCreated = DateTime.Now;
                                        gdpr.LastAmendedBy = -1;
										gdprDB.insert(gdpr);

										//---save internal notes
										if (edc.EntityDetailNotes != null)
										{
											if (edc.EntityDetailNotes.Count > 0)
											{
												EntityDetailsNotesDB eNoteDB = new EntityDetailsNotesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
												foreach (EntityDetailsNotes entityNote in edc.EntityDetailNotes)
												{
													if (entityNote.Sequence > 0)
													{
														entityNote.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
														entityNote.DateLastAmended = DateTime.Now;
														eNoteDB.updateBySequence(entityNote);
													}
													else
													{
														entityNote.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
														entityNote.DateCreated = DateTime.Now;
														long sequence = 0;
														eNoteDB.insertEntityNotes(out sequence, entityNote);
													}
												}
											}
										}
										returnValue.IsSucessfull = true;
									}
									else
									{
										returnValue.Message = edjRepos.Message;
									}
									EntityDetailsSupplementaryRepository edsRepos = new EntityDetailsSupplementaryRepository();
									if (edsRepos.DeleteUpdateEntityDetailSupplementarySageData(request, edc))
									{
										returnValue.TheObject = edc.EntityId;
										returnValue.IsSucessfull = true;
									}
									else
									{
										returnValue.Message = edsRepos.Message;
									}
								}
								else
								{
									returnValue.Message = edcDB.ErrorMessage;
								}
							}
							else
							{
								returnValue.Message = "Contact Name Short must be Unique";
							}
						}
					}
					else
					{
						returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + reqModel.ToString();
					}
				}
				else
				{
					returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
				}
			}
			catch (Exception ex)
			{
				returnValue.Message = Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Insert/Update Client.", ex);
			}
			return returnValue;
		}
		public bool UpdateClientInfo(HttpRequest request, EntityDetailsCore edc, string infoType)
        {
            bool returnValue = false;
            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    EntityDetailsCoreDB orderDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = orderDB.updateClientInfo(edc, infoType);
                }
            }
            return returnValue;
        }

        public EntityDetailsCore GetEdcWithCloudFields(long entityId, RequestHeaderModel header)
        {
            EntityDetailsCore returnVal = new EntityDetailsCore();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings != null)
                {
                    EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnVal = edcDB.GetEntityWithCloudFields(entityId);
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting Contact Cab IDs " + ex.Message + " " + ex.InnerException;
            }
            return returnVal;
        }

        public ResponseModel SaveEdcCloudFields(EntityDetailsCore edc, RequestHeaderModel header)
        {
            ResponseModel returnVal = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings == null)
                    throw new InvalidDataException(SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER);
                EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                returnVal.IsSucessfull = edcDB.SaveEdcCloudFields(edc);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception while updating EDC Folder IDs - " +ex.Message, "UpdateEntityFolderIds");
            }
            return returnVal;
        }
    }
}
