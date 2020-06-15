using SimplicityOnlineWebApi.ClientInvoice.Models.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.ClientInvoice.Entities;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.ClientInvoice.Models.RepositoryQuery;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace SimplicityOnlineWebApi.ClientInvoice.Models.Repositories
{
    public class ClientInvoiceRepository : IClientInvoiceRepository
    {
        private ILogger<ClientInvoiceRepository> _logger;
        private readonly IWebHostEnvironment Env;

        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public ClientInvoiceRepository(ILogger<ClientInvoiceRepository> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            this.Env = env;
        }

        public ClientInvoiceRepository(bool _IsSecondaryDatabase, string _SecondaryDatabaseId)
        {
            this.IsSecondaryDatabase = _IsSecondaryDatabase;
            this.SecondaryDatabaseId = _SecondaryDatabaseId;
        }

        public BankDetailsCompanyNoVatNoResponse GetBankDetailForClient(string projectId, int sequnceId, string invoiceNo)
        {
            BankDetailsCompanyNoVatNoResponse responseModel = new BankDetailsCompanyNoVatNoResponse();
            try
            {
                //---Write Log file
                Utilities.WriteLog("******************************************");
                Utilities.WriteLog("Method: Get Bank Detail For Client start : " + DateTime.Now);
                Utilities.WriteLog("******************************************");

                bool apsConfig = false;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];

                    Utilities.WriteLog("Get Project Settings start :" + DateTime.Now);
                    if (settings != null)
                    {
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                        Utilities.WriteLog("Get application Settings start : " + DateTime.Now);
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                            apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                        }
                        Utilities.WriteLog("Get application Settings end : " + DateTime.Now);

                        ClientInvoiceDB invoiceDB = new ClientInvoiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                        Utilities.WriteLog("Call Get Bank Detail For Client DB Method start : " + DateTime.Now);
                        responseModel = invoiceDB.BankDetailForClient(sequnceId, invoiceNo);
                        Utilities.WriteLog("Call Get Bank Detail For Client DB Method end : " + DateTime.Now);
                    }
                    else
                        Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_PROJECT_ID + " " + DateTime.Now);

                    Utilities.WriteLog("Get Project Settings end : " + DateTime.Now);
                }
                else
                    Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting Bank Detail For Client " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }

            Utilities.WriteLog("******************************************");
            Utilities.WriteLog("Method: Get Bank Detail For Client end: " + DateTime.Now);
            Utilities.WriteLog("******************************************");
            return responseModel;
        }
        #region Aged dabator Section

        public ListResponse<CompanyAgedDabtorResponse> GetAllAgedDabutors(HttpRequest request, ClientPageListRequest listRequest)
        {
            ListResponse<CompanyAgedDabtorResponse> responseModel = new ListResponse<CompanyAgedDabtorResponse>();
            try
            {
                Utilities.WriteLog("******************************************");
                Utilities.WriteLog("Method: GetAllAgedDabutors start: " + DateTime.Now);
                Utilities.WriteLog("******************************************");

                string projectId = request.Headers["ProjectId"];
                bool apsConfig = false;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];

                    Utilities.WriteLog("Get Project Settings start : " + DateTime.Now);
                    if (settings != null)
                    {
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                        Utilities.WriteLog("Get application Settings start : " + DateTime.Now);
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                            apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                        }
                        Utilities.WriteLog("Get application Settings end : " + DateTime.Now);
                        int count = 0;
                        ClientInvoiceDB invoiceDB = new ClientInvoiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                        Utilities.WriteLog("Call GetAllAgedDabutors Method start:" + DateTime.Now);
                        List<CompanyAgedDabtorResponse> data = invoiceDB.AgedDabatorList(listRequest, out count, true, apsConfig);

                        responseModel.data = (data != null) ? data.ToArray() : new List<CompanyAgedDabtorResponse>().ToArray();
                        responseModel.totalRecords = count;
                        responseModel.currenPage = listRequest.page;
                        responseModel.pageSize = listRequest.size;
                        responseModel.currenPageRecords = (data != null) ? data.Count : 0;
                        Utilities.WriteLog("Call GetAllAgedDabutors Method end:" + DateTime.Now);
                    }
                    else
                        Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_PROJECT_ID + " " + DateTime.Now);
                    Utilities.WriteLog("Get Project Settings end : " + DateTime.Now);
                }
                else
                    Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting GetAllAgedDabutors List. " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            Utilities.WriteLog("******************************************");
            Utilities.WriteLog("Method: GetAllAgedDabutors end: " + DateTime.Now);
            Utilities.WriteLog("******************************************");
            return responseModel;
        }

        #endregion

        #region Statement Region

        public StatementListResponse GetAllStatement(string projectId, Parameters parameters)
        {
            StatementListResponse responseModel = new StatementListResponse();
            try
            {
                //---Write Log file
                Utilities.WriteLog("******************************************");
                Utilities.WriteLog("Method: GetAllStatement List start : " + DateTime.Now);
                Utilities.WriteLog("******************************************");

                bool apsConfig = false;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];

                    Utilities.WriteLog("Get Project Settings start :" + DateTime.Now);
                    if (settings != null)
                    {
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                        Utilities.WriteLog("Get application Settings start : " + DateTime.Now);
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                            apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                        }
                        Utilities.WriteLog("Get application Settings end : " + DateTime.Now);

                        ClientInvoiceDB invoiceDB = new ClientInvoiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                        Utilities.WriteLog("Call GetAllStatement List Method start : " + DateTime.Now);
                        responseModel = invoiceDB.StatementList(parameters, true);
                        Utilities.WriteLog("Call GetAllStatement List Method end : " + DateTime.Now);
                    }
                    else
                        Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_PROJECT_ID + " " + DateTime.Now);

                    Utilities.WriteLog("Get Project Settings end : " + DateTime.Now);
                }
                else
                    Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting GetAllStatement List. " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }

            Utilities.WriteLog("******************************************");
            Utilities.WriteLog("Method: GetAllStatement List end: " + DateTime.Now);
            Utilities.WriteLog("******************************************");
            return responseModel;
        }

        #endregion

        #region All Items & Unpaid Items Region

        public InvoiceListResponse<InvoiceResponse> GetAllInvoices(HttpRequest request, ClientPageListRequest listRequest, Parameters parameters)
        {
            string methodCallFor = (parameters.status == "open" ? "Unpaid " : " All ");
            Utilities.WriteLog("******************************************");
            Utilities.WriteLog("Method: GetAllInvoices of " + methodCallFor + "Items Start : " + DateTime.Now);
            Utilities.WriteLog("******************************************");

            InvoiceListResponse<InvoiceResponse> responseModel = new InvoiceListResponse<InvoiceResponse>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                bool apsConfig = false;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    Utilities.WriteLog("Get Project Settings of GetAllInvoices for " + methodCallFor + "Items start :" + DateTime.Now);
                    if (settings != null)
                    {
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                        Utilities.WriteLog("Get application Settings of GetAllInvoices for " + methodCallFor + "Items start : " + DateTime.Now);
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                            apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                        }
                        Utilities.WriteLog("Get application Settings of GetAllInvoices for " + methodCallFor + "Items  end : " + DateTime.Now);

                        Utilities.WriteLog("Get ClientInvoiceDB of GetAllInvoices for " + methodCallFor + "Items  start : " + DateTime.Now);
                        ClientInvoiceDB invoiceDB = new ClientInvoiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Utilities.WriteLog("Get ClientInvoiceDB of GetAllInvoices for " + methodCallFor + "Items end : " + DateTime.Now);

                        Utilities.WriteLog("Call AllAndUnPaidItemsList Method of ClientInvoiceDB  for " + methodCallFor + "Items start : " + DateTime.Now);
                        int count = 0;
                        responseModel = invoiceDB.AllAndUnPaidItemsList(Env.ContentRootPath, listRequest, out count, true, apsConfig, parameters, projectId);

                        Utilities.WriteLog("Call AllAndUnPaidItemsList Method of ClientInvoiceDB  for " + methodCallFor + "Items end : " + DateTime.Now);
                    }
                    else
                        Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_PROJECT_ID + " " + DateTime.Now);

                    Utilities.WriteLog("Get Project Settings of GetAllInvoices  for " + methodCallFor + "Items end :" + DateTime.Now);
                }
                else
                    Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting GetAllInvoices  for " + methodCallFor + "Items . " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }

            Utilities.WriteLog("******************************************");
            Utilities.WriteLog("Method: GetAllInvoices of " + methodCallFor + "Items  End : " + DateTime.Now);
            Utilities.WriteLog("******************************************");
            return responseModel;
        }

        #endregion

        #region Invoice Details Section

        public InvoiceDetailResponse GetInvoiceDetail(string projectId, int sequenceId, string invoiceNo)
        {
            InvoiceDetailResponse responseModel = new InvoiceDetailResponse();
            try
            {
                //---Write Log file
                Utilities.WriteLog("******************************************");
                Utilities.WriteLog("Method: Get Invoice Detail of " + sequenceId + "  : " + DateTime.Now);
                Utilities.WriteLog("******************************************");

                bool apsConfig = false;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    Utilities.WriteLog("Get Project Settings On:" + DateTime.Now);
                    if (settings != null)
                    {
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                        Utilities.WriteLog("Get application Settings On : " + DateTime.Now);
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                            apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                        }

                        ClientInvoiceDB invoiceDB = new ClientInvoiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                        Utilities.WriteLog("Call Get Invoice Detail Method On:" + DateTime.Now);

                        responseModel = invoiceDB.GetInvoiceDetail(projectId, Env.ContentRootPath, sequenceId, invoiceNo);

                        Utilities.WriteLog("Return Object Result On:" + DateTime.Now);
                    }
                    else
                        Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_PROJECT_ID + " " + DateTime.Now);
                }
                else
                    Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting Invoice Detail . " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return responseModel;
        }

        public InvoiceDetailPdfResponse GetInvoiceDetailPdfData(string projectId, int sequenceId,string invoiceNo)
        {
            InvoiceDetailResponse invoice = GetInvoiceDetail(projectId, sequenceId,invoiceNo);
            InvoiceDetailPdfResponse pdfJsonData = new InvoiceDetailPdfResponse();
            pdfJsonData.invoice_detail_title = invoice.invoiceDetailMainTitlePretext + " " + invoice.invoiceNo;
            pdfJsonData.company_address = invoice.invaddr.Replace("<p>", "").Replace("</p>", "").Replace("<br/>", "\r\n");
            pdfJsonData.invoice_date = invoice.invoiceDate;
            pdfJsonData.client = invoice.nameLong;
            pdfJsonData.job_ref = invoice.jobRef;
            pdfJsonData.job_address = invoice.jobAddress.Replace("\r\n", " ");
            pdfJsonData.start_date = invoice.jobDateStart;
            pdfJsonData.end_date = invoice.jobDateFinish;
            pdfJsonData.your_ref = invoice.jobClientRef;
            pdfJsonData.trade_code = invoice.jobTradeCode;
            pdfJsonData.foot_note = (invoice.footNote != null && invoice.footNote != "") ? ("Invoice Notes: " + " " + invoice.footNote) : "";
            //pdfJsonData.amount_sub_total = invoice.amountSubTotal;
            //pdfJsonData.orders_bills_amount_vat = invoice.ordersBillsAmountVat;
            //pdfJsonData.amount_total = invoice.amountTotal;
            pdfJsonData.paid_to_date = invoice.paidToDate;

            pdfJsonData.paymement_logo = (invoice.paymentStatus != null && invoice.paymentStatus.Equals("PAID")) ? "display:block:" : "display:none;";

            pdfJsonData.invoice_items = new List<InvoiceDetailItemsPdfResponse>();
            if (invoice.invoiceItems != null && invoice.invoiceItems.Count > 0)
            {
                foreach (InvoiceDetailItemsResponse item in invoice.invoiceItems)
                {
                    InvoiceDetailItemsPdfResponse model = new InvoiceDetailItemsPdfResponse();
                    model.item_code = item.flgRowIsText == true ? "" : item.itemCode;
                    model.item_desc = item.itemDesc;
                    model.item_units = item.flgRowIsText == true ? "" : item.itemUnits;
                    //model.item_quantity = item.flgRowIsText == true ? 0 : item.itemQuantity;
                    model.amount_payment = item.flgRowIsText == true ? 0 : item.amountPayment;
                    pdfJsonData.invoice_items.Add(model);
                }
            }
            pdfJsonData.invoice_payments = new List<InvoiceDetailPaymentPdfResponse>();
            if (invoice.invoicePayments != null && invoice.invoicePayments.Count > 0)
            {
                pdfJsonData.paymement_item_display = "display:block:";
                foreach (InvoiceDetailPaymentResponse item in invoice.invoicePayments)
                {
                    InvoiceDetailPaymentPdfResponse model = new InvoiceDetailPaymentPdfResponse();
                    model.entry_type = item.entryType;
                    model.entry_date = item.entryDate;
                    model.invoice_no_or_item_ref = item.invoicenoOrItemRef;
                    model.entry_amt_allocated = item.entryAmtAllocated;
                    pdfJsonData.invoice_payments.Add(model);
                }
            }
            else
                pdfJsonData.paymement_item_display = "display:none;";
            return pdfJsonData;
        }

        #endregion

        #region Get AutoComplete Data

        public List<JobRefAutoCompleteResponse> GetJobRefAutoComplete(HttpRequest request, string searchText, int itemCount)
        {
            List<JobRefAutoCompleteResponse> responseModel = new List<JobRefAutoCompleteResponse>();
            try
            {
                string projectId = request.Headers["ProjectId"];

                //---Write Log file
                Utilities.WriteLog("******************************************");
                Utilities.WriteLog("Method: Get Job Ref AutoComplete List: " + DateTime.Now);
                Utilities.WriteLog("******************************************");

                bool apsConfig = false;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    Utilities.WriteLog("Get Project Settings On:" + DateTime.Now);
                    if (settings != null)
                    {
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                        Utilities.WriteLog("Get application Settings On : " + DateTime.Now);
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                            apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                        }

                        ClientInvoiceDB invoiceDB = new ClientInvoiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                        Utilities.WriteLog("Call Get Job Ref Autocomplete data Method On:" + DateTime.Now);

                        responseModel = invoiceDB.GetJobRefAutoComplete(projectId, searchText, itemCount);

                        Utilities.WriteLog("Return Object Result On:" + DateTime.Now);
                    }
                    else
                        Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_PROJECT_ID + " " + DateTime.Now);
                }
                else
                    Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting Job Ref Autocomplete . " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return responseModel;
        }

        public List<NamedModel> GetShowTypesData(HttpRequest request)
        {
            List<NamedModel> responseModel = new List<NamedModel>();
            try
            {
                //---Write Log file
                Utilities.WriteLog("******************************************");
                Utilities.WriteLog("Method: Get Show Types Data : " + DateTime.Now);
                Utilities.WriteLog("******************************************");

                bool apsConfig = false;
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    Utilities.WriteLog("Get Project Settings On:" + DateTime.Now);
                    if (settings != null)
                    {
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                        Utilities.WriteLog("Get application Settings On : " + DateTime.Now);
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                            apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                        }

                        ClientInvoiceDB invoiceDB = new ClientInvoiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                        Utilities.WriteLog("Call Get Job Ref Autocomplete data Method On:" + DateTime.Now);

                        if (projectId != null && projectId.Length > 0)
                        {
                            // ConfigModel config = invoiceDB.GetCompanyConfiguration(projectId, _appEnvironment.ApplicationBasePath);
                            responseModel = new List<NamedModel>();
                            responseModel.Add(new NamedModel("Invoices", 0));
                            //responseModel.Add(new NamedModel(config.appnpl, 1));
                            responseModel.Add(new NamedModel("Applications for Payment", 1));
                        }
                        Utilities.WriteLog("Return Object Result On:" + DateTime.Now);
                    }
                    else
                        Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_PROJECT_ID + " " + DateTime.Now);
                }
                else
                    Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting Show Types Auto complete . " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return responseModel;
        }

        public List<NamedModel> GetCompanyAutoComplete(HttpRequest request, string searchText, int itemCount)
        {
            List<NamedModel> responseModel = new List<NamedModel>();
            try
            {
                string projectId = request.Headers["ProjectId"];

                //---Write Log file
                Utilities.WriteLog("******************************************");
                Utilities.WriteLog("Method: Get Company AutoComplete List: " + DateTime.Now);
                Utilities.WriteLog("******************************************");

                bool apsConfig = false;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    Utilities.WriteLog("Get Project Settings On:" + DateTime.Now);
                    if (settings != null)
                    {
                        ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("APS");

                        Utilities.WriteLog("Get application Settings On : " + DateTime.Now);
                        if (applicationSettings != null && applicationSettings.Count > 0)
                        {
                            apsConfig = Boolean.Parse(applicationSettings[0].Setting1);
                        }

                        ClientInvoiceDB invoiceDB = new ClientInvoiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                        Utilities.WriteLog("Call Get Company Autocomplete data Method On:" + DateTime.Now);

                        responseModel = invoiceDB.GetCompanyAutoComplete(projectId, searchText, itemCount);

                        Utilities.WriteLog("Return Object Result On:" + DateTime.Now);
                    }
                    else
                        Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_PROJECT_ID + " " + DateTime.Now);
                }
                else
                    Utilities.WriteLog(SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting Company Autocomplete . " + ex.Message + " " + ex.InnerException + " " + DateTime.Now);
            }
            return responseModel;
        }

        #endregion

        private string DecryptData(string EncryptedText)
        {
            string Encryptionkey = "Si1abmp2yzli3cdci4wxty5ef46uvBu7ghsi8stne9ijss01qr";
            RijndaelManaged objrij = new RijndaelManaged();
            objrij.Mode = CipherMode.CBC;
            objrij.Padding = PaddingMode.PKCS7;

            objrij.KeySize = 0x80;
            objrij.BlockSize = 0x80;
            byte[] encryptedTextByte = Convert.FromBase64String(EncryptedText);
            byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);
            byte[] EncryptionkeyBytes = new byte[0x10];
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
            return Encoding.UTF8.GetString(TextByte);
        }

        private string UppercaseFirst(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            return char.ToUpper(name[0]) + name.Substring(1).ToLower();
        }

        public LicenseInformation GetLicenseInfo(string projectId)
        {
            string path = Env.ContentRootPath + @"\Configs\" + UppercaseFirst(projectId).Trim() + "_License.txt";

            LicenseInformation licenseInformation = null;
            if (File.Exists(path))
            {
                string licenseFileData;
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream))
                {
                    licenseFileData = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                if (licenseFileData == null)
                    return null;
                else if (licenseFileData.Length < 1)
                    return null;
                else
                {
                    string jsonStrData = DecryptData(licenseFileData);

                    licenseInformation = new LicenseInformation();
                    licenseInformation = Newtonsoft.Json.JsonConvert.DeserializeObject<LicenseInformation>(jsonStrData);
                    if (projectId != licenseInformation.projectId)
                        return null;
                }
                return licenseInformation;
            }
            return null;
        }

        public string GetBasePath()
        {
            return Env.ContentRootPath;
        }

    }
}
