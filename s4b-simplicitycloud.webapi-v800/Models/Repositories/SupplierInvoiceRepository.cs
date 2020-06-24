using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq;
using SimplicityOnlineWebApi.Models.ViewModels;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class SupplierInvoiceRepository : ISupplierInvoiceRepository
    {

        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        protected readonly ICldSettingsRepository CldSettingsRepository;
        protected readonly ILogger<IRossumRepository> Logger;
        protected readonly IEntityDetailsCoreRepository EntityDetailsCoreRepository;
        


        public SupplierInvoiceRepository(ICldSettingsRepository cldSettingsRepository,
            ILogger<IRossumRepository> logger,
            IEntityDetailsCoreRepository entityDetailsCoreRepository
            )
        {
            this.CldSettingsRepository = cldSettingsRepository;
            this.EntityDetailsCoreRepository = entityDetailsCoreRepository;
            this.Logger = logger;
        }



        public ResponseModel GetRossumUnfinalizedInvoices(RequestHeaderModel header, ClientRequest clientRequest)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = header.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        SupplierInvoicesDB supplierInvoiceDB = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = supplierInvoiceDB.selectRossumUnfinalizedInvoices(clientRequest, out count, true);
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
                Logger.LogError(ex.Message);
            }
            return returnValue;
        }
        public ResponseModel GetUnfinalizedInvoices(RequestHeaderModel header, ClientRequest clientRequest)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = header.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        SupplierInvoicesDB supplierInvoice = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = supplierInvoice.selectUnfinalizedInvoices(clientRequest, out count, true);
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

        public ResponseModel SaveInvoice(InvoiceItemised invoice, RequestHeaderModel header)
        {
            ResponseModel returnValue = new ResponseModel();
            #region project settings checks block
            if (string.IsNullOrWhiteSpace(header.ProjectId))
            {
                returnValue.IsSucessfull = false; returnValue.Message = "Project Id Not found.";
                return returnValue;
            }

            ProjectSettings settings = Configs.settings[header.ProjectId];
            if (settings == null)
            {
                returnValue.IsSucessfull = false; returnValue.Message = "Project settings not found";
                return returnValue;
            }
            #endregion

            try
            {
                SupplierInvoicesDB invoiceDB = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                long sequence = -1;
                if (!invoiceDB.SaveInvoice(invoice))
                {
                    returnValue.IsSucessfull = true;
                    throw new InvalidExpressionException("Itemised Invoice couldn't be saved- SupplierRepository returns.");
                }
                else
                {
                    returnValue.IsSucessfull = true;
                }
            }
            catch (Exception ex)
            {
                returnValue.IsSucessfull = false;
                returnValue.Message = (ex.Message);
                this.Logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }
        public ResponseModel GetItemisedInvoice(RequestHeaderModel header, long invoiceSequence)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = header.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        SupplierInvoicesDB supplierInvoice = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        SupplierInvoiceVM invoiceItemised = supplierInvoice.selectItemisedInvoice(invoiceSequence);
                        //returnValue.TheObject = invoiceItemised;
                        if (invoiceItemised == null)
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                        else
                        {
                            if (invoiceItemised.ContactId > 0) 
                            {
                                // TODO: Replace the call with GetEntityByEntityId and don't fetch all the suppliers
                                EntityDetailsCoreMin entityDetails = EntityDetailsCoreRepository.GetAllSuppliers(header, null).Where(x => x.EntityJoinId == invoiceItemised.ContactId).FirstOrDefault();
                                if (entityDetails!=null)
                                    invoiceItemised.SupplierName = entityDetails.NameLong;
                            }
                            returnValue.TheObject = invoiceItemised;
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
        public SupplierInvoiceVM GetInvoiceByInvNo(string invoiceNo, RequestHeaderModel header)
        {
            SupplierInvoiceVM result = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings != null)
                {
                    result = new SupplierInvoiceVM();
                    SupplierInvoicesDB invoiceDB = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    result = invoiceDB.GetInvoiceByInvNo(invoiceNo);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "GetInvoiceByInvNo-Repository");
                return result;
            }
            return result;

        }         
        public SageViewModel GetSageDetail(long contactId,RequestHeaderModel header)
        {
            SageViewModel result = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings != null)
                {
                    result = new SageViewModel();
                    SupplierInvoicesDB invoiceDB = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    result = invoiceDB.GetSageDetail(contactId);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "GetInvoiceByInvNo-Repository");
                return result;
            }
            return result;

        }
        public ResponseModel GetVehicle(RequestHeaderModel header)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = header.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        SupplierInvoicesDB supplierInvoice = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = supplierInvoice.selectVehicle();
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
        public ResponseModel GetItemTel(RequestHeaderModel header)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = header.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        SupplierInvoicesDB supplierInvoice = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = supplierInvoice.selectItemsTel();
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
        public ResponseModel GetCostCode(RequestHeaderModel header)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = header.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        SupplierInvoicesDB supplierInvoice = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = supplierInvoice.selectCostCode();
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
        public ResponseModel UpdateInvoiceSupplier(InvoiceItemised invoice, RequestHeaderModel header)
        {
            ResponseModel returnValue = new ResponseModel();
            #region project settings checks block
            if (string.IsNullOrWhiteSpace(header.ProjectId))
            {
                returnValue.IsSucessfull = false; returnValue.Message = "Project Id Not found.";
                return returnValue;
            }

            ProjectSettings settings = Configs.settings[header.ProjectId];
            if (settings == null)
            {
                returnValue.IsSucessfull = false; returnValue.Message = "Project settings not found";
                return returnValue;
            }
            #endregion

            try
            {
                SupplierInvoicesDB invoiceDB = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                if (!invoiceDB.UpdateInvoiceSupplier(invoice))
                {
                    returnValue.IsSucessfull = false;
                    throw new InvalidExpressionException("Supplier couldn't be saved- SupplierRepository returns.");
                }
                else
                {
                    EntityDetailsCoreDB edcDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    EntityDetailsCore edc = edcDB.GetEdcCldCloudFields(invoice.Sequence);
                    if(edc !=null && edc.EdcCloudFields!=null && !string.IsNullOrEmpty(edc.EdcCloudFields.RossumContactName))
                        edcDB.SaveEdcCloudFields(edc);
                    returnValue.IsSucessfull = true;
                }
            }
            catch (Exception ex)
            {
                returnValue.IsSucessfull = false;
                returnValue.Message = (ex.Message);
                this.Logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }
        public SupplierInvoiceVM GetInvoiceBySequenceNo(long sequenceNo, RequestHeaderModel header)
        {
            SupplierInvoiceVM result = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings != null)
                {
                    result = new SupplierInvoiceVM();
                    SupplierInvoicesDB invoiceDB = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    result = invoiceDB.GetInvoiceBySequenceNo(sequenceNo);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "GetInvoiceBySeqNo-Repository");
                return result;
            }
            return result;

        }
        public long GetJobSequenceByPORef(string PONo, RequestHeaderModel header)
        {
            long result=-1;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings != null)
                {
                    SupplierInvoicesDB invoiceDB = new SupplierInvoicesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    result = invoiceDB.GetJobSequenceByPORef(PONo);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "GetJobRefByPO-Repository");
            }
            return result;

        }
    }
}
