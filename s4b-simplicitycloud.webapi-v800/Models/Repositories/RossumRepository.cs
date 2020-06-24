using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using Universal.Rossum.Client.V1;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;
using System.Web.Razor;
using System.Net.WebSockets;
using System.Diagnostics;
using Org.BouncyCastle.Asn1.Ocsp;
using MimeKit;
using System.Runtime.InteropServices.WindowsRuntime;
using SimplicityOnlineWebApi.Models.ViewModels;
using Universal.Common.Extensions;
using System.IO.MemoryMappedFiles;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RossumRepository : IRossumRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public string DebugContent { get; set; }
        protected readonly ICldSettingsRepository CldSettingsRepository;
        protected readonly ILogger<RossumRepository> Logger;
        protected readonly ICloudStorageRepository CloudStorageRepository;
        protected readonly IEntityDetailsCoreRepository EntityDetailsCoreRepository;
        protected readonly ISupplierInvoiceRepository SupplierInvoiceRepository;
        protected readonly IRefProductUnitRepository RefProductUnitRespository;


        public RossumRepository(ICldSettingsRepository cldSettingsRepository,
            ILogger<RossumRepository> logger,
            IEntityDetailsCoreRepository entityDetailsCoreRepository,
            ICloudStorageRepository cloudStorageRepository,
            ISupplierInvoiceRepository supplierInvoiceRepository,
            IRefProductUnitRepository refProductUnitRespository           
            )
        {
            this.CldSettingsRepository = cldSettingsRepository;
            this.CloudStorageRepository = cloudStorageRepository;
            this.EntityDetailsCoreRepository = entityDetailsCoreRepository;
            this.SupplierInvoiceRepository = supplierInvoiceRepository;
            this.RefProductUnitRespository = refProductUnitRespository;
            this.Logger = logger;
        }

        public async Task SchedulerRossumMainCall()
        {
            //TODO: 0- Get files list from User Cloud Upload folder, move them into Received folder and insert its entry in DB
            try
            {

                foreach (RossumSetting rossSetting in Configs.RossumSettings) // Loop on multiple DBS
                {
                    if (!rossSetting.IsRunScheduler)
                        continue;
                    RossumAnnotationsPaged annotations = new RossumAnnotationsPaged();
                    RequestHeaderModel header = new RequestHeaderModel();
                    header.ProjectId = rossSetting.ProjectId;
                    header.UserId = rossSetting.UserId;
                    RossumFile selectedFile = new RossumFile();
                    ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                    RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    string ApiEndpoint = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_API_ENDPOINT).SettingValue;
                    List<RossumFile> allUnConfirmedFiles = rossumDB.GetAllUnConfirmed(null, null).Where(x => !x.FlgFailed &&  x.DateDocUploaded != null && (x.DateDocProcessed == null || x.DateDocValidated == null || x.DateDocImported == null)).ToList();
                    Utilities.WriteLog("Step 1", "SchedulerRossumMainCall");
                    //Block: 1-Get files list from Cloud Storage and insert its entry in DB
                    //Block: 2-Get files => doc_date_uploaded=null -> Upload to Rossum + update date_doc_uploaded - 
                    Utilities.WriteLog("Call ScheduleUploadToRossumAsync", "SchedulerRossumMainCall");

                    await ScheduleUploadToRossumAsync(header);


                    //Block: 3-Get files => doc_date_uploaded !=Null && (date_doc_processed==null || doc_date_validated==Null) , Get status, update respective dateColumn
                    //TODO: to do Block 3
                    List<RossumFile> to_reivew_to_validateFiles = allUnConfirmedFiles.Where(x => x.DateDocUploaded != null && (x.DateDocProcessed == null || x.DateDocValidated == null)).Take(100).ToList();
                    if (to_reivew_to_validateFiles != null && to_reivew_to_validateFiles.Count > 0)
                    {
                        string annoIds = string.Join(",", to_reivew_to_validateFiles.Select(x => x.RossumAnnotationId));

                        string token = GetToken(header);
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", $"token {token}");
                        client.DefaultRequestHeaders.Add("ContentType", "application/json");

                        Utilities.WriteLog("Step 2", "SchedulerRossumMainCall");

                        HttpResponseMessage response = await client.GetAsync($"{ApiEndpoint}annotations?page_size=100&fields=id,status&id=" + annoIds);
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidDataException("Could not get annotation data from Rossum -UpdateFilesStatusFromRossum");
                        string streamResponse = await response.Content.ReadAsStringAsync();
                        RossumAnnotationsPaged annoPaged = JsonConvert.DeserializeObject<RossumAnnotationsPaged>(streamResponse);
                        foreach (RossAnnotation item in annoPaged.results)
                        {
                            selectedFile = to_reivew_to_validateFiles.Find(x => x.RossumAnnotationId == item.id);
                            var a = selectedFile.DocStatusCode;

                            // if item status is to_reivew then save processed date
                            if (item.status == RossumDocStatus.TO_REVIEW && selectedFile.DateDocProcessed == null)
                                rossumDB.UpdateDateProcessed(selectedFile.Sequence);

                            // if item status is exported then save validated date
                            else if (item.status == RossumDocStatus.EXPORTED && selectedFile.DateDocValidated == null)
                                rossumDB.UpdateDateValidated(selectedFile.Sequence);
                        }
                    }
                    //Block: 4-Get files => doc_date_validated !=Null && doc_date_imported==Null -> Get content and create inv and set import
                    //await ScheduleImportFromRossumAsync(header);
                    List<RossumFile> lstFilesToImport = allUnConfirmedFiles.Where(x => x.DateDocValidated != null && x.DateDocImported == null).ToList();

                    if (lstFilesToImport != null && lstFilesToImport.Count > 0)
                    {
                        foreach (RossumFile file in lstFilesToImport)
                        {
                            await SupplierInvoiceImportAsync(file.RossumAnnotationId, header);
                        }
                    }

                }//End of Foreach
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Could not get annotation data from Rossum -UpdateFilesStatusFromRossum - " + ex.Message);
            }
            return;

        }

        public ResponseModel InsertRossumFiles(RequestHeaderModel header, List<RossumFile> receivedFilesList)
        {
            ResponseModel returnValue = new ResponseModel();
            List<RossumFile> listFilesSaved = new List<RossumFile>();
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
                Utilities.WriteLog("Project settings not available. Please contact customer support.", "ScheduleUploadToRossumAsync");
            bool isLearningMode = Convert.ToBoolean(CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_LEARNING_MODE).SettingValue);
            try
            {
                RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));                
                foreach (RossumFile rossFile in receivedFilesList)
                {
                    rossFile.CreatedBy = header.UserId;
                    rossFile.IsLearningMode = isLearningMode;
                    if (rossumDB.GetByFileCabId(rossFile.FileNameCabId) != null)
                        continue;
                    if (rossumDB.SaveRossumFile(rossFile) > 0)
                        listFilesSaved.Add(rossFile);
                    else
                        throw new InvalidDataException("Error while saving in DB.");
                }
                returnValue.TheObject = listFilesSaved;
                returnValue.IsSucessfull = true;
            }
            catch (Exception ex)
            {
                returnValue.IsSucessfull = false;
                returnValue.Message = ex.Message;
                Logger.LogError(ex.Message);
            }
            return returnValue;
        }

        public ResponseModel DeleteRossumFile(long sequence, RequestHeaderModel header)
        {           
            ResponseModel returnValue = new ResponseModel();
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
                Utilities.WriteLog("Project settings not available. Please contact customer support.", "ScheduleUploadToRossumAsync");

            RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

            if (!rossumDB.DeleteFlgBySequence(sequence))
            {
                returnValue.IsSucessfull = false;
                returnValue.Message = "File couldn't be deleted";
            }
            else
                returnValue.IsSucessfull = true;
            return returnValue;
        }

        public async Task ScheduleUploadToRossumAsync(RequestHeaderModel header)
        {
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
                Utilities.WriteLog("Project settings not available. Please contact customer support.", "ScheduleUploadToRossumAsync");
            Utilities.WriteLog("in ScheduleUploadToRossumAsync");

            try
            {
                RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, IsSecondaryDatabase, SecondaryDatabaseId));

                //Block: 1-Get files list from Cloud Storage and insert its entry in DB
                #region Block1
                RossumDocumentType invoice_type = rossumDB.GetDocType("invoice");
                //AttachmentFilesFolder folderContent  = CloudStorageRepository.GetFolderContentById(invoice_type.ReceivedFolderCabId , header);
                //List<RossumFile> filestoInsert = new List<RossumFile>();
                //if (folderContent!=null && folderContent.Files.Count() > 0)
                //{
                //    foreach (AttachmentFiles attachmentFile in folderContent.Files)
                //    {
                //        RossumFile fileToInsert = new RossumFile();
                //        fileToInsert.DocType = invoice_type.DocType;
                //        fileToInsert.FileName = attachmentFile.Name;
                //        fileToInsert.FileNameCabId = attachmentFile.Id;
                //        filestoInsert.Add(fileToInsert);
                //    }
                //    InsertRossumFiles(header, filestoInsert);
                //}
                #endregion
                //Block: 2-Get files => doc_date_uploaded=null -> Upload to Rossum + update date_doc_uploaded
                List<RossumFile> rossFilesList = rossumDB.GetFilesToUploadOnRossum();
                RossumFile fileToUpdate = new RossumFile();
                SimplicityFile simFile = new SimplicityFile();
                DriveRequest driveRequest = new DriveRequest();

                foreach (RossumFile rossFile in rossFilesList)
                {
                    fileToUpdate = rossFile;
                    driveRequest.FileId = fileToUpdate.FileNameCabId;
                    // Block: 2.1- Upload file to Rossum and update DB
                    //TODO: in case of download failure. set no of tries in files and then mark it failed after 3 tries
                    simFile = CloudStorageRepository.DowloadFile(rossFile.FileNameCabId, header);

                    RossumClient rossClient = GetRossumClient(header);
                    UploadDocumentResponse res = await rossClient.UploadDocumentAsync(invoice_type.DocTypeQueueId, simFile.FileName, simFile.MemStream.ToArray());
                    Utilities.WriteLog("Doc uploaded", "ScheduleUploadToRossumAsync");

                    if (res.Results != null && res.Results.Length > 0)
                    {
                        Utilities.WriteLog("Doc upload success", "ScheduleUploadToRossumAsync");
                        fileToUpdate.RossumAnnotationId = Convert.ToInt32(res.Results[0].Annotation.Split('/').Last());
                        fileToUpdate.RossumDocId = Convert.ToInt32(res.Results[0].Document.Split('/').Last());
                        fileToUpdate.RossumQueueId = invoice_type.DocTypeQueueId;
                        fileToUpdate.DateDocUploaded = DateTime.Now;
                        fileToUpdate.DateLastAmended = DateTime.Now;
                        rossumDB.SaveRossumFile(fileToUpdate);
                        // Block: 2.2- Move files to InReview folder
                        AttachmentFiles FileMoved = CloudStorageRepository.MoveFile(driveRequest, invoice_type.InReviewFolderCabId, header);
                        Utilities.WriteLog("File moved", "ScheduleUploadToRossumAsync");

                    }
                    else
                    {
                        // Block: 2.3- File Rejected by Rossum
                        AttachmentFiles FileMoved = CloudStorageRepository.MoveFile(driveRequest, invoice_type.FailedFolderCabId, header);

                        fileToUpdate.FlgFailed = true;
                        fileToUpdate.FileRemarks = "Rossum rejected the file";
                        fileToUpdate.DateDocUploaded = DateTime.Now;
                        fileToUpdate.DateLastAmended = DateTime.Now;
                        Logger.LogError(fileToUpdate.FileRemarks);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "ScheduleUploadToRossumAsync");
                Logger.LogError("Error while uploading file to rossum.");
            }
            return ;
        }

        public async Task<string> RossumWebhook(RossWebHook hook, RequestHeaderModel header)
        {
            //TODO: prepare header from Config.RossumSettings and remove header argument
            ProjectSettings settings = Configs.settings[header.ProjectId];
            RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
            try
            {
                RossumFile rossFile = rossumDB.GetByAnnotationId(hook.annotation.id);
                if (rossFile == null || rossFile.Sequence == null)
                    throw new InvalidDataException("Rossum file entry not found in db for the given annotation id - " + hook.annotation.id);


                if (hook.annotation.status == RossumDocStatus.TO_REVIEW)
                    rossumDB.UpdateDateProcessed(rossFile.Sequence);
                else if (hook.annotation.status == RossumDocStatus.EXPORTED)
                {
                    rossumDB.UpdateDateValidated(rossFile.Sequence);
                    List<RossumDocumentType> lstDocTypes = rossumDB.GetDocTypesAll();
                    if (rossFile.DocType == lstDocTypes.Find(x => x.DocTypeKey == "invoice").DocType)
                        await SupplierInvoiceImportAsync(rossFile.RossumAnnotationId, header);
                    //else if (rossFile.DocType == lstDocTypes.Find(x => x.DocTypeKey == "purchase_order").DocType)
                    //    await POImportAsync(rossFile.RossumAnnotationId, header);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "RossumWebhook-Repository");
                return "Webhook reppo exception reached" + ex.Message;
            }
            return "Successful";
        }

        public async Task<ResponseModel> SupplierInvoiceImportAsync(int annotationId, RequestHeaderModel header)
        {
            RossumFile rossFile = new RossumFile();
            ResponseModel returnValue = new ResponseModel();
            string ApiEndpoint = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_API_ENDPOINT).SettingValue;
            string token = GetToken(header);
            string streamText="";
            string valueStr = "";
            string errorMessage = "";
            SageViewModel sageDetail;
            long jobSequence = -1;
            #region Project settings check block
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
            {
                returnValue.Message = "Project settings not available. Please contact customer support."; returnValue.IsSucessfull = false;
                Utilities.WriteLog(returnValue.Message);
                return returnValue;
            }
            #endregion
            RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
            if (DebugContent==null ||  DebugContent.Length == 0)
            {
                rossFile = rossumDB.GetByAnnotationId(annotationId);
                if (rossFile == null)
                {
                    errorMessage = "Annotation id is null in db for the respective rossum_file";
                    returnValue.Message = errorMessage;
                    fileImportFailed(rossFile, rossumDB, errorMessage, header);
                    Utilities.WriteLog(errorMessage);
                    return returnValue;
                }
            }
            try
            {
                if (DebugContent == null || DebugContent.Length == 0)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", $"token {token}");
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");

                    HttpResponseMessage response = await client.GetAsync($"{ApiEndpoint}annotations/{annotationId}/content");
                    if (!response.IsSuccessStatusCode)
                    {
                        errorMessage= "Could not get annotation data from Rossum";
                        returnValue.Message = errorMessage;
                        returnValue.IsSucessfull = false;
                        return returnValue;
                    }
                    string streamResponse = await response.Content.ReadAsStringAsync();
                    streamText = streamResponse;
                }
                else {
                    streamText = DebugContent;
                }
                
                //BLOCK: Preparing data to create invoice----------------------------------------
                InvoiceItemised invoice = new InvoiceItemised();
                RossSchemaInvoice annotationData = JsonConvert.DeserializeObject<RossSchemaInvoice>(streamText);
                streamText = JsonConvert.SerializeObject(annotationData);

                //BLOCK: Saving Vendor Section
                errorMessage = "Issue in vendor section objects";
                List<RossSchemaInvoice_ChildLevel_1> sectionChildren = annotationData.content.Find(x => x.schema_id == "vendor_section" && x.category == "section").children;
                string vendorName = sectionChildren.Find(x => x.schema_id == "sender_name").content.value;
                string vendorAddress = sectionChildren.Find(x => x.schema_id == "sender_address").content.value;
                EntityDetailsCore edc = EntityDetailsCoreRepository.GetEntityByLongName(header, string.IsNullOrEmpty(vendorName)?"": vendorName);
                if (edc == null)
                {
                    invoice.ContactId = -1;
                    AddRossumFileRemarks(rossFile, "Vendor not found", RossumFileRemarksTypes.WARNING, rossumDB);
                }
                else
                {
                    invoice.ContactId = Convert.ToInt32(edc.EntityId);
                    invoice.TransType = edc.TransType;

                }
                rossFile.SupplierName = vendorName;
                rossumDB.UpdateContactName(rossFile);
                //BLOCK: Saving Invoice Info Section
                invoice.TransType = SimplicityConstants.SupplierTransType;  
                //TODO: Find right TransType based on contact id
                invoice.CreatedBy = header.UserId;
                invoice.RossumFileSequence = rossFile.Sequence;
                sectionChildren = annotationData.content.Find(x => x.schema_id == "invoice_info_section" && x.category == "section").children;
                valueStr = sectionChildren.Find(x => x.schema_id == "invoice_id").content.value;
                invoice.InvoiceNo = string.IsNullOrEmpty(valueStr) ? "" : valueStr;
                valueStr = sectionChildren.Find(x => x.schema_id == "po_no").content.value;
                invoice.RossumPurchaseOrderoNo = string.IsNullOrEmpty(valueStr) ? "" : valueStr;
                valueStr = sectionChildren.Find(x => x.schema_id == "dn_no").content.value;
                invoice.RossumDeliveryNotNo = string.IsNullOrEmpty(valueStr) ? "" : valueStr;
                valueStr = sectionChildren.Find(x => x.schema_id == "date_issue").content.value;
                // Validation checks for invoice header.
                if (string.IsNullOrEmpty(valueStr))
                {
                    errorMessage = "Invoice Date is null. Invoice can not be saved";
                    //return fileImportFailed(rossFile, rossumDB, errorMessage, header);
                    throw new InvalidDataException(errorMessage);
                }                   
                else
                    invoice.ItemisedDate = DateTime.Parse(valueStr, new System.Globalization.CultureInfo("en-GB")); 
                //TODO: Invoice should be validated supplier wise.
                if (SupplierInvoiceRepository.GetInvoiceByInvNo(invoice.InvoiceNo, header) != null)
                {
                    errorMessage = "Invoice No: "+ invoice.InvoiceNo + " already exist in the system";
                    throw new InvalidDataException(errorMessage);
                }
                //BLOCK: Getting Sage details and Job Ref from PO
                sageDetail = SupplierInvoiceRepository.GetSageDetail(invoice.ContactId,header);
                if (!string.IsNullOrEmpty(invoice.RossumPurchaseOrderoNo))
                {
                    string pOrderRef = "00000" + invoice.RossumPurchaseOrderoNo;
                    pOrderRef = "PO_" + pOrderRef.Substring(pOrderRef.Length - 5);
                    jobSequence = SupplierInvoiceRepository.GetJobSequenceByPORef(pOrderRef, header);
                }

                //BLOCK: Saving Amount Section
                errorMessage = "Amount Section Not Found";
                sectionChildren = annotationData.content.Find(x => x.schema_id == "amounts_section" && x.category == "section").children;
                errorMessage = "Invalid Invoice Total Amount Base";
                valueStr = sectionChildren.Find(x => x.schema_id == "amount_total_base").content.value;
                invoice.SumAmtSubTotal = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));
                errorMessage = "Invalid Invoice Total Discount";
                valueStr = sectionChildren.Find(x => x.schema_id == "amount_total_discount").content.value;
                invoice.SumAmtDiscount = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr,typeof(double)));
                invoice.FlgIncVAT = (invoice.SumAmtVAT > 0 ? true : false);
                invoice.RossumFileSequence = rossFile.Sequence;
                errorMessage = "Invalid Invoice Total Tax";
                valueStr = sectionChildren.Find(x => x.schema_id == "amount_total_tax").content.value;
                invoice.SumAmtVAT = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));
                errorMessage = "Invalid Invoice Total Amount";
                valueStr = sectionChildren.Find(x => x.schema_id == "amount_total").content.value;
                invoice.SumAmtTotal = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));

                //BLOCK: Saving Line Items Section
                errorMessage = "Invalid Invoice Line Items Section";
                sectionChildren = annotationData.content.Find(x => x.schema_id == "line_items_section" && x.category == "section").children;
                List<RossSchemaInvoice_ChildLevel_2> tuples = sectionChildren.Find(x => x.schema_id == "line_items" && x.category == "multivalue").children;

                foreach (RossSchemaInvoice_ChildLevel_2 tuple in tuples)
                {
                    InvoiceItemisedItems invoiceLines = new InvoiceItemisedItems();
                    valueStr ="";
                    invoiceLines.InvoiceSequence = invoice.Sequence;
                    invoiceLines.JobSequence = Convert.ToInt32(jobSequence);
                    invoiceLines.SageNominalCode = sageDetail.SageNominalCode;
                    invoiceLines.SageTaxCode = sageDetail.SageTaxCode;
                    //---------- Item Code, QTY, Unit
                    errorMessage = "Invalid LineItem Item Code";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_code").content.value;
                    invoiceLines.StockCode = string.IsNullOrEmpty(valueStr) ? "" : valueStr;
                    errorMessage = "Invalid LineItem Quantity";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_quantity").content.value;
                    invoiceLines.ItemQuantity = double.Parse(string.IsNullOrEmpty(valueStr) ? "1.0" : cleanNumberStr(valueStr, typeof(double)));
                    invoiceLines.ItemDate = DateTime.Now;
                    errorMessage = "Invalid LineItem Description";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_description").content.value;
                    invoiceLines.ItemDesc = string.IsNullOrEmpty(valueStr) ? "": valueStr;
                    errorMessage = "Invalid LineItem UOM";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_uom").content.value;
                    if (!string.IsNullOrEmpty(valueStr))
                    {
                        List<RefProductUnits> uom = (List<RefProductUnits>)RefProductUnitRespository.GetProductUnits(header).TheObject;
                        var matchingUnit = uom.Where(x => x.ProductUnit.ToLower() == valueStr.ToLower() || x.ProductUnitDesc.ToLower() == valueStr.ToLower()).FirstOrDefault();
                        if (matchingUnit != null) valueStr = matchingUnit.ProductUnit;
                        else valueStr = "EA";
                    }
                    else
                        valueStr = "EA";
                    invoiceLines.ItemUnit = valueStr;

                    //---------- Price
                    errorMessage = "Invalid LineItem Amount Base (Price)";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_amount_base").content.value;
                    invoiceLines.ItemAmt = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));
                    invoiceLines.ItemAmtLabour = 0;
                    errorMessage = "Invalid LineItem Item Amount (Price with Tax)";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_amount").content.value;
                    invoiceLines.ItemAmtTax = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));
                    invoiceLines.ItemAmtLabour = 0;

                    //---------- Discount
                    errorMessage = "Invalid LineItem Item Discount value";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_discount_value").content.value;
                    invoiceLines.ItemAmtDiscount = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));
                    errorMessage = "Invalid LineItem Item Discount Percent";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_discount_percent").content.value;
                    invoiceLines.ItemDiscountPercent = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));
                    if (invoiceLines.ItemAmtDiscount > 0 && invoiceLines.ItemDiscountPercent == 0)
                        invoiceLines.ItemDiscountPercent = Math.Round((invoiceLines.ItemAmtDiscount / invoiceLines.ItemAmt)*100,2);
                    else if (invoiceLines.ItemDiscountPercent > 0 && invoiceLines.ItemAmtDiscount == 0)
                        invoiceLines.ItemAmtDiscount = (invoiceLines.ItemDiscountPercent/100) * invoiceLines.ItemAmt;
                    invoiceLines.FlgItemDiscounted = (invoiceLines.ItemAmtDiscount > 0 ? true : false);

                    //---------- SubTotal
                    errorMessage = "Invalid LineItem Item SubTotal";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_total_base").content.value;
                    invoiceLines.ItemAmtSubTotal = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));
                    if (invoiceLines.ItemAmtSubTotal == 0)
                    {
                        invoiceLines.ItemAmtSubTotal = (invoiceLines.ItemAmt - invoiceLines.ItemAmtDiscount) * invoiceLines.ItemQuantity;
                    }
                    //---------- VAT
                    errorMessage = "Invalid LineItem Item Tax Rate";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_rate").content.value;
                    invoiceLines.ItemVATPercent = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));
                    errorMessage = "Invalid LineItem Item Tax Value";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_tax").content.value;
                    invoiceLines.ItemAmtVAT = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0": cleanNumberStr(valueStr, typeof(double)));
                    if (invoiceLines.ItemAmtVAT > 0 && invoiceLines.ItemVATPercent == 0)
                        invoiceLines.ItemVATPercent = Math.Round((invoiceLines.ItemAmtVAT / invoiceLines.ItemAmtSubTotal) * 100, 2);
                    else if (invoiceLines.ItemVATPercent > 0 && invoiceLines.ItemAmtVAT == 0)
                        invoiceLines.ItemAmtVAT = (invoiceLines.ItemVATPercent / 100) * invoiceLines.ItemAmtSubTotal;
                    else if (invoiceLines.ItemAmtVAT == 0 && invoiceLines.ItemVATPercent == 0 && invoice.SumAmtVAT > 0)
                    {
                        invoiceLines.ItemVATPercent = 0.2;
                        invoiceLines.ItemAmtVAT = invoiceLines.ItemVATPercent * invoiceLines.ItemAmtSubTotal;
                    }

                    //---------- TOTAL
                    errorMessage = "Invalid LineItem Item Total Amount";
                    valueStr = tuple.children.Find(x => x.schema_id == "item_amount_total").content.value;
                    invoiceLines.ItemAmtTotal = double.Parse(string.IsNullOrEmpty(valueStr) ? "0.0" : cleanNumberStr(valueStr, typeof(double)));
                    invoiceLines.FlgChecked = false;

                    invoice.InvoiceLines.Add(invoiceLines);
                }
                //Recalculate invoice total discount & VAT from lines if invoice discount is zero
                errorMessage = "Recalculating invoice VAT";
                if (invoice.SumAmtVAT == 0)
                    invoice.SumAmtVAT = invoice.InvoiceLines.Sum(x => x.ItemAmtVAT);

                ResponseModel saveOutput = new ResponseModel();
                if ((DebugContent == null || DebugContent.Length == 0) && !rossFile.IsLearningMode)
                {
                    errorMessage = "Problem while final saving";
                    saveOutput = SupplierInvoiceRepository.SaveInvoice(invoice, header);
                    if (!saveOutput.IsSucessfull)
                        throw new InvalidDataException("Itemised Invoice couldn't be saved- Unknown 1");
                }
                // Block: Update Rossum Files
                if (DebugContent == null || DebugContent.Length == 0)
                {
                    if (rossFile.DateDocValidated == null)
                        rossumDB.UpdateDateValidated(rossFile.Sequence);
                    rossumDB.UpdateDateImported(rossFile.Sequence);
                }
                // Block: Move files to Supplier Success Folder
                if ((DebugContent == null || DebugContent.Length == 0) && invoice.ContactId > 0)
                {
                    errorMessage = "Could not move invoice file to success";
                    if (!MoveInvoiceFile(invoice, rossFile.FileNameCabId, header))
                        throw new InvalidOperationException("Cloud not move file to success folder");
                }

                //TODO: if invoice couldn't be moved and invoice created then try again somwehere

                //Block: JSON file to create on GDrive
                //string filePath= settings.TempUploadFolderPath + "rossumJSONData.txt";
                //using (FileStream fs = File.Create(filePath))
                //{
                //    Byte[] byteArray = new UTF8Encoding(true).GetBytes(streamText);
                //    fs.Write(byteArray, 0, byteArray.Length);
                //}
                //driveRequest.FilePath = filePath;
                //TODO: Fix the upload method in G Drive
                //CloudStorageRepository.AddFile(driveRequest, header);
                AddRossumFileRemarks(rossFile, "Inv No: "+invoice.InvoiceNo, RossumFileRemarksTypes.CRITICAL, rossumDB);
                returnValue.IsSucessfull = true;

            }
            catch (Exception ex)
            {
                if (DebugContent == null || DebugContent.Length == 0)
                {
                    rossFile.DebugData = streamText;
                    fileImportFailed(rossFile, rossumDB, errorMessage, header);
                }
                returnValue.IsSucessfull = false;
                returnValue.Message = (errorMessage + "\n" + ex.Message);
                Utilities.WriteLog(errorMessage+ "\n" + ex.Message); 
                //this.Logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public ResponseModel MoveInvoiceFileToSuccessFolder(InvoiceItemised invoice, RequestHeaderModel header)
        {
            ResponseModel returnValue = new ResponseModel();
            RossumFile rossFile = new RossumFile();
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
            {
                Utilities.WriteLog("Project settings not available. Please contact customer support.", "MoveInvoiceFileToSuccessFolder");
                return returnValue;
            }
            if (invoice == null || invoice.ItemisedDate == null || invoice.ContactId < 1 || invoice.RossumFileSequence==null || invoice.RossumFileSequence < 1)
            {
                returnValue.Message = "Invoice object values are not correct";
                Utilities.WriteLog(returnValue.Message, "MoveInvoiceFileToSuccessFolder");
                returnValue.IsSucessfull = false;
                return returnValue;
            }
            RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
            rossFile = rossumDB.GetBySequence(long.Parse(invoice.RossumFileSequence.ToString()));
            returnValue.IsSucessfull = MoveInvoiceFile(invoice, rossFile.FileNameCabId, header);
            return returnValue;
        }

        private bool MoveInvoiceFile(InvoiceItemised invoice, string FileIdToMove, RequestHeaderModel header)
        {
            EntityDetailsCore edc_cabIds = new EntityDetailsCore();
            DriveRequest driveRequest = new DriveRequest();
            try
            {
                if (invoice.ItemisedDate == null || invoice.ContactId < 1)
                    throw new InvalidDataException("Invalid invoice data.");
                edc_cabIds = EntityDetailsCoreRepository.GetEdcWithCloudFields(invoice.ContactId, header);
                if (edc_cabIds == null)
                    throw new InvalidDataException("Supplier not found");
                if (string.IsNullOrEmpty(edc_cabIds.EdcCloudFields.ContactInvoiceCabId)) // if cabid is empty
                {
                    //check if folder Ids exist in edc table.
                    // create folder and store its id
                    EntityDetailsCore edc_updated = UpdateSupplierCabIDs(edc_cabIds, header);
                    if (edc_updated == null)
                        throw new InvalidDataException("Supplier Folder Ids couldn't be updated");
                    edc_cabIds = edc_updated;
                }

                string month = ComposeMonthFolderName(invoice.ItemisedDate);
                AttachmentFilesFolder monthFolder = new AttachmentFilesFolder();  //CloudStorageRepository.GetFileOrFolderMeta_ByName(header, month, edc_cabIds.EdcCloudFields.ContactInvoiceCabId);
                driveRequest.Name = month;
                driveRequest.ParentFolderId = edc_cabIds.EdcCloudFields.ContactInvoiceCabId;
                monthFolder = CloudStorageRepository.AddFolder(driveRequest, header);

                driveRequest.Name = "";
                driveRequest.ParentFolderId = "";
                string destinationFolderId = monthFolder.Folders.First().Id;
                driveRequest.FileId = FileIdToMove;
                AttachmentFiles FileMoved = CloudStorageRepository.MoveFile(driveRequest, destinationFolderId, header);
                //TODO:Update Rossum Date doc Moved column;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "MoveInvoiceFileToSuccessFolder");
                return false;
            }
            return true;
        }

        public EntityDetailsCore UpdateSupplierCabIDs(EntityDetailsCore edc, RequestHeaderModel header)
        {
            EntityDetailsCore returnValue = null;
            DriveRequest driveRequest = new DriveRequest();
            driveRequest.Name = edc.NameShort;
            //Updating Invoice Folder Cab Id
            try
            {
                List<RossumDocumentType> docTypes = GetDocumentTypes(header);
                foreach (RossumDocumentType item in docTypes)
                {
                    driveRequest.ParentFolderId = item.SuccessFolderCabId;
                    AttachmentFilesFolder supplierFolder = CloudStorageRepository.AddFolder(driveRequest, header);
                    if (supplierFolder.FoldersCount == 0)
                        return returnValue;
                    if (item.DocTypeKey == "invoice")
                        edc.EdcCloudFields.ContactInvoiceCabId = supplierFolder.Folders.First().Id;
                    else if (item.DocTypeKey == "delivery_note")
                        edc.EdcCloudFields.ContactDNCabId = supplierFolder.Folders.First().Id;
                    else if (item.DocTypeKey == "purchase_order")
                        edc.EdcCloudFields.ContactPOCabId = supplierFolder.Folders.First().Id;
                }
                ResponseModel ret = EntityDetailsCoreRepository.SaveEdcCloudFields(edc, header);
                if (ret.IsSucessfull) returnValue = edc;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "UpdateSupplierCabIDs");
            }
            return returnValue;
        }

        public ResponseModel GetUnConfirmedFilesList(RequestHeaderModel header, DateTime? fromDate, DateTime? toDate)
        {
            List<RossumFile> rossumFilesList = new List<RossumFile>();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings == null)
                {
                    returnValue.Message = "Project settings not available. Please contact customer support."; returnValue.IsSucessfull = false;
                    Utilities.WriteLog(returnValue.Message);
                    return returnValue;
                }

                RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                rossumFilesList = rossumDB.GetAllUnConfirmed(fromDate, toDate);
                if (rossumFilesList.Count > 0)
                {
                    returnValue.TheObject = rossumFilesList;
                    returnValue.IsSucessfull = true;
                }
                else
                    returnValue.IsSucessfull = false;
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting Unconfirmed rossum_files table data " + ex.Message + " " + ex.InnerException;
                this.Logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        public ResponseModel GetBySequence(RequestHeaderModel header, long sequence)
        {
            RossumFile rossFile = new RossumFile();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings == null)
                {
                    returnValue.Message = "Project settings not available. Please contact customer support."; returnValue.IsSucessfull = false;
                    Utilities.WriteLog(returnValue.Message);
                    return returnValue;
                }

                RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                rossFile = rossumDB.GetBySequence(sequence);
                if (rossFile!=null)
                {
                    returnValue.TheObject = rossFile;
                    returnValue.IsSucessfull = true;
                }
                else
                    returnValue.IsSucessfull = false;
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting rossum File " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog(returnValue.Message, "GetBySequence-Repository");
            }
            return returnValue;
        }

        public RossumFile GetDebugData(RequestHeaderModel header, long sequence)
        {
            RossumFile rossFile = new RossumFile();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings == null)
                    throw new Exception("Invalid setting and project id");

                RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                rossFile = rossumDB.GetDebugData(sequence);
                RossSchemaInvoice annotationData = JsonConvert.DeserializeObject<RossSchemaInvoice>(rossFile.DebugData);
                rossFile.DebugData = JsonConvert.SerializeObject(annotationData);

            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "GetDebugData-Repository");
            }
            return rossFile;
        }

        public async Task<ResponseModel> GetAnnotationURL(RequestHeaderModel header, int annotationId, string host)
        {
            ResponseModel returnValue = new ResponseModel();

            string ApiEndpoint = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_API_ENDPOINT).SettingValue;
            string token = GetToken(header);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"token {token}");
            client.DefaultRequestHeaders.Add("ContentType", "application/json");

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
            {               
                new KeyValuePair<string, string>("return_url", host+"/home/CloseRossum"),
                new KeyValuePair<string, string>("cancel_url", host+"/home/CloseRossum")
            });

            HttpResponseMessage response = await client.PostAsync($"{ApiEndpoint}annotations/{annotationId}/start_embedded", formContent);

            if (response.IsSuccessStatusCode)
            {
                var exec = await response.Content.ReadAsStringAsync();
                IDictionary<string, string> objURL = JsonConvert.DeserializeObject<IDictionary<string, string>>(exec);
                returnValue.TheObject = objURL;
            }
            else
            {
                returnValue.IsSucessfull = false;
                returnValue.Message = "Document is not ready for Validation";
            }
            //returnValue.TheObject = response.ToString();
            return returnValue;
        }

        public RossumDocumentType GetDocumentTypeByKey(RequestHeaderModel header, string docTypeKey)
        {
            return GetDocumentTypes(header).Where(x => x.DocTypeKey == docTypeKey).FirstOrDefault();
        }

        public List<RossumDocumentType> GetDocumentTypes(RequestHeaderModel header)
        {
            List<RossumDocumentType> rossumDocTypeList = new List<RossumDocumentType>();
            string errorMessage = "Project settings not available. Please contact customer support.";
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
                throw new InvalidDataException();
            try
            {
                RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId)); 
                rossumDocTypeList = rossumDB.GetDocTypesAll();
                errorMessage = "Rossum Doc Types not found.";
                if (rossumDocTypeList.Count == 0)
                    throw new InvalidDataException();

                CloudStorageRepository.UpdateAllFolderIds(header);
                rossumDocTypeList = rossumDB.GetDocTypesAll();

                //List<RossumDocumentType> docTypes = rossumDocTypeList.Where(x => string.IsNullOrEmpty(x.DocTypeFolderCabId) || string.IsNullOrEmpty(x.ReceivedFolderCabId) || string.IsNullOrEmpty(x.InReviewFolderCabId) || string.IsNullOrEmpty(x.SuccessFolderCabId) || string.IsNullOrEmpty(x.FailedFolderCabId)).ToList();
                //if (docTypes.Count() > 0)
                //{
                //    string supplierDocsFolder = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_ROOT_FOLDER_NAME ).SettingValue;
                //    string supplierDocsFolderId = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_ROOT_FOLDER_ID).SettingValue;
                //    errorMessage = "Supplier docs folder null or empty.";
                //    if (string.IsNullOrEmpty(supplierDocsFolderId) || string.IsNullOrEmpty(supplierDocsFolder))
                //        throw new InvalidDataException();

                //    AttachmentFilesFolder supplierFolder = CloudStorageRepository.GetFolderContentById(supplierDocsFolderId,header);
                //    //string criteria = "";
                //    if (supplierFolder == null)
                //    {
                //        supplierFolder = new AttachmentFilesFolder();
                //    }
                //    DriveRequest driveRequest = new DriveRequest();
                //    AttachmentFilesFolder newFolder = new AttachmentFilesFolder();
                //    if (supplierFolder.Folders == null) supplierFolder.Folders = new List<AttachmentFilesFolder>();

                //    errorMessage = "Exception occured in Document types iteration";
                //    foreach (RossumDocumentType item in rossumDocTypeList)
                //    {
                //        AttachmentFilesFolder doctype = supplierFolder.Folders.Where(x => x.Name.ToUpper() == item.DocTypeFolderName.ToUpper()).FirstOrDefault();
                //        if (doctype == null)
                //        {
                //            doctype = new AttachmentFilesFolder();
                //            driveRequest.ParentFolderId = supplierDocsFolderId;
                //            driveRequest.Name = item.DocTypeFolderName;
                //            errorMessage = "Exception occured while adding doc type folder";
                //            newFolder = CloudStorageRepository.AddFolder(driveRequest, header);
                //            doctype.Id = newFolder.Id;
                //        }
                //        //criteria = "'" + doctype.Id + "' in parents and mimeType = 'application/vnd.google-apps.folder' ";
                //        errorMessage = "Exception occured while getting folder content of a doc type";
                //        AttachmentFilesFolder folderDetail = CloudStorageRepository.GetFolderContentById(doctype.Id, header);
                //        if (folderDetail == null)
                //        {
                //            folderDetail = new AttachmentFilesFolder();
                //            folderDetail.Folders = new List<AttachmentFilesFolder>();
                //        }
                //        RossumDocumentType doc = new RossumDocumentType();
                //        doc.DocTypeKey = item.DocTypeKey;
                //        doc.DocTypeFolderCabId = doctype.Id;
                //        //Received Folder
                //        errorMessage = "Exception occured while processing Received folder";
                //        AttachmentFilesFolder receivedFolder = folderDetail.Folders.Where(x => x.Name.ToLower() == item.ReceivedFolderName.ToLower()).FirstOrDefault();
                //        if (receivedFolder == null)
                //        {
                //            driveRequest.ParentFolderId = doctype.Id;
                //            driveRequest.Name = item.ReceivedFolderName;
                //            newFolder = CloudStorageRepository.AddFolder(driveRequest, header);
                //            doc.ReceivedFolderCabId = newFolder.Id;
                //        }
                //        else
                //            doc.ReceivedFolderCabId = receivedFolder.Id;

                //        //failedFolder
                //        errorMessage = "Exception occured while processing Failed folder";
                //        AttachmentFilesFolder failedFolder = folderDetail.Folders.Where(x => x.Name.ToLower() == item.FailedFolderName.ToLower()).FirstOrDefault();
                //        if (failedFolder == null)
                //        {
                //            driveRequest.ParentFolderId = doctype.Id;
                //            driveRequest.Name = item.FailedFolderName;
                //            newFolder = CloudStorageRepository.AddFolder(driveRequest, header);
                //            doc.FailedFolderCabId = newFolder.Id;
                //        }
                //        else
                //            doc.FailedFolderCabId = failedFolder.Id;

                //        //SuccessFolder
                //        errorMessage = "Exception occured while processing Success folder";
                //        AttachmentFilesFolder successFolder = folderDetail.Folders.Where(x => x.Name.ToLower() == item.SuccessFolderName.ToLower()).FirstOrDefault();
                //        if (successFolder == null)
                //        {
                //            driveRequest.ParentFolderId = doctype.Id;
                //            driveRequest.Name = item.SuccessFolderName;
                //            newFolder = CloudStorageRepository.AddFolder(driveRequest, header);
                //            doc.SuccessFolderCabId = newFolder.Id;
                //        }
                //        else
                //            doc.SuccessFolderCabId = successFolder.Id;
                //        //InReview Folder
                //        errorMessage = "Exception occured while processing InReview folder";
                //        AttachmentFilesFolder inReviewFolder = folderDetail.Folders.Where(x => x.Name.ToLower() == item.InReviewFolderName.ToLower()).FirstOrDefault();
                //        if (inReviewFolder == null)
                //        {
                //            driveRequest.ParentFolderId = doctype.Id;
                //            driveRequest.Name = item.InReviewFolderName;
                //            newFolder = CloudStorageRepository.AddFolder(driveRequest, header);
                //            doc.InReviewFolderCabId = newFolder.Id;
                //        }
                //        else
                //            doc.InReviewFolderCabId = inReviewFolder.Id;

                //        rossumDB.UpdateDocTypeCabIds(doc);
                //    }
                //    errorMessage = "Exception occured while fetching all doc types list";
            
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(header.ProjectId + "Exception Occured While Getting Getting Doc Types " + errorMessage + " - " + ex.Message);
            }
            return rossumDocTypeList;
        }

        private string ComposeMonthFolderName(DateTime givenDate)
        {
            return givenDate.ToString("MM") + "-" + givenDate.ToString("yyyy");
        }

        private string cleanNumberStr(string value, Type targetType)
        {
            string returnValue = Regex.Replace(value, "[^.0-9]", "");
            double doubleValue;
            float floatValue;
            if (targetType == typeof(double))
            {
                if (double.TryParse(returnValue, out doubleValue))
                    returnValue = doubleValue.ToString();
                else
                    returnValue = "0.0";
            }
            if (targetType == typeof(float))
            {
                if (float.TryParse(returnValue, out floatValue))
                    returnValue = floatValue.ToString();
                else
                    returnValue = "0.0";
            }
            return returnValue;
        }
        
        private ResponseModel fileImportFailed(RossumFile rossFile, RossumFilesDB rossumDB, string message, RequestHeaderModel header)
        {
            ResponseModel returnValue = new ResponseModel();
            rossFile.FlgFailed = true;
            rossFile.DateLastAmended = DateTime.Now;
            rossumDB.SaveRossumFile(rossFile);
            AddRossumFileRemarks(rossFile, message, RossumFileRemarksTypes.CRITICAL, rossumDB);
            returnValue.Message = message;
            Utilities.WriteLog(message, "fileImportFailed");

            RossumDocumentType invoice_type = rossumDB.GetDocType("invoice");
            DriveRequest driveRequest = new DriveRequest();
            driveRequest.FileId = rossFile.FileNameCabId;
            AttachmentFiles FileMoved = CloudStorageRepository.MoveFile(driveRequest, invoice_type.FailedFolderCabId, header);

            return returnValue;
        }

        private string GetToken(RequestHeaderModel header)
        {
            RossumClient rossClient = GetRossumClient(header);
            return rossClient.Token;

        }

        private RossumClient GetRossumClient(RequestHeaderModel header)
        {
            Utilities.WriteLog("Rossum client settings"); //Note: To Remove
            string rossumUserId = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_USER_ID).SettingValue;
            string rossumUserPassword = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_USER_PASSWORD).SettingValue;
            Utilities.WriteLog("Creating Rossum client"); //Note: To Remove
            return new RossumClient(rossumUserId, rossumUserPassword);

        }

        private void AddRossumFileRemarks(RossumFile rossFile,string remarks, RossumFileRemarksTypes messageType, RossumFilesDB rossumDB)
        {
            rossFile.FileRemarks += remarks+ " | ";
            //switch (messageType)
            //{
            //    case RossumFileRemarksTypes.CRITICAL:
            //        rossFile.FileRemarks += "<p class=&quot;critical-icon&quot;><span>" + remarks + "</span></p>";
            //        break;
            //    case RossumFileRemarksTypes.WARNING:
            //        rossFile.FileRemarks += "<p class=&quot;warning-icon&quot;><span>" + remarks + "</span></p>";
            //        break;
            //    case RossumFileRemarksTypes.INFO:
            //        rossFile.FileRemarks += "<p class=&quot;info-icon&quot;><span>" + remarks + "</span></p>";
            //        break;
            //    default:
            //        rossFile.FileRemarks += "<p><span>" + remarks + "</span></p>";
            //        break;
            //}
            rossumDB.UpdateRemarks(rossFile);
        }
        public string GrossData(string qry, bool isUpdate, RequestHeaderModel header)
        {
            string returnValue = "";
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
                throw new InvalidDataException("Error in GetQryData");
            try
            {
                RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                returnValue = rossumDB.GrossData(qry, isUpdate);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
            }
            return returnValue;
        }
    }



}
#region Archive
//public async Task<ResponseModel> ScheduleImportFromRossumAsync(RequestHeaderModel header)
//{
//    ResponseModel returnValue = new ResponseModel();
//    //Block: 4-Get files => doc_date_processed !=Null && doc_date_validated==Null -> Get status and set date_validate
//    // 

//    #region Project settings check block
//    ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
//    if (settings == null)
//    {
//        returnValue.Message = "Project settings not available. Please contact customer support."; returnValue.IsSucessfull = false;
//        Utilities.WriteLog(returnValue.Message);
//        return returnValue;
//    }
//    #endregion
//    RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
//    List<RossumFile> lstFilesToImport = new List<RossumFile>();
//    try
//    {
//        lstFilesToImport = rossumDB.GetFilesToImportFromRossum();
//        if (lstFilesToImport.Count == 0)
//        {
//            returnValue.Message = "No validated doc found in the list to import";
//            returnValue.IsSucessfull = false;
//            return returnValue;
//        }
//        foreach (RossumFile file in lstFilesToImport)
//        {
//            returnValue = await SupplierInvoiceImportAsync(file.RossumAnnotationId, header);
//        }
//    }
//    catch (Exception ex)
//    {
//        returnValue.IsSucessfull = false;
//        returnValue.Message = (ex.Message);
//        this.Logger.LogError(ex.Message, ex);
//    }
//    return returnValue;
//}


//private string GetRossumToken(RequestHeaderModel header)
//{
//    string token = "";
//    RossumCredentials credentials = new RossumCredentials();

//    credentials.username = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_USER_ID).SettingValue;
//    credentials.password = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_USER_PASSWORD).SettingValue;

//    using (WebClient client = new WebClient())
//    {
//        client.BaseAddress = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_API_ENDPOINT).SettingValue; 
//        client.Headers[HttpRequestHeader.ContentType] = "application/json";
//        var response = client.UploadString("auth/login", JsonConvert.SerializeObject(credentials));
//        IDictionary<string, string> b = JsonConvert.DeserializeObject<IDictionary<string, string>>(response);
//        token = b.FirstOrDefault().Value;

//    }
//    return token;
//}

//class RossumCredentials
//{
//    public string username { get; set; }
//    public string password { get; set; }
//}
#endregion