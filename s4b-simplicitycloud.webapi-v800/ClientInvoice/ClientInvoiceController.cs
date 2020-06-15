using Microsoft.AspNetCore.Mvc;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.ClientInvoice.Entities;
using SimplicityOnlineWebApi.ClientInvoice.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimplicityOnlineWebApi.ClientInvoice
{
    [Route("api/[controller]")]
    public class ClientInvoiceController : Controller
    {
        private readonly IWebThirdPartiesRepository WebThirdPartiesRepository;
        private readonly IClientInvoiceRepository InvoiceRepository;
        public ClientInvoiceController(IWebThirdPartiesRepository webThirdPartiesRepository, IClientInvoiceRepository invoiceRepository)
        {
            this.WebThirdPartiesRepository =webThirdPartiesRepository;
            this.InvoiceRepository = invoiceRepository;
        }


        //public IApplicationEnvironment _appEnvironment { get; set; }

        [HttpGet]
        [ActionName("GetAgeDebtors")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAgeDebtors([FromQuery]ClientPageListRequest listRequest)
        {
            ListResponse<CompanyAgedDabtorResponse> agedDabatorsData = InvoiceRepository.GetAllAgedDabutors(Request, listRequest);
            return new ObjectResult(agedDabatorsData);
        }

        [HttpPost]
        [ActionName("GetStatements")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetStatements([FromBody] Parameters parameters)
        {
            if (parameters != null && parameters.endDate == null)
                parameters.endDate = DateTime.Now;

            string projectId = Request.Headers["ProjectId"];
            StatementListResponse statements = InvoiceRepository.GetAllStatement(projectId, parameters);
            return new ObjectResult(statements);
        }

        [HttpGet]
        [ActionName("GetStatementPdfDownload")]
        [Route("[action]")]
        public IActionResult GetStatementPdfDownload(DateTime? startDate, DateTime? endDate)
        {   
            if (endDate == null)
                endDate = DateTime.Now;

            Parameters parameters = new Parameters();
            parameters.startDate = startDate;
            parameters.endDate = endDate;

            string projectId = Request.Headers["ProjectId"];

            ProjectSettings settings = Configs.settings[projectId];

            string tempFolderPath = settings.TempUploadFolderPath;
            string basePath = InvoiceRepository.GetBasePath();

            StatementListResponse dataResponse = new StatementListResponse();
            try
            {
                dataResponse = InvoiceRepository.GetAllStatement(projectId, parameters);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error In _invoiceRepository.GetAllStatement at : " + DateTime.Now + " And Error Message is :" + ex.Message.ToString());
            }

            StreamReader reader = new StreamReader(basePath + @"\ClientInvoice\Statement-export.html");
            string readFile = reader.ReadToEnd();
                        
            string htmlContent = Utilities.AppendDataInvoiceStatementsPdf(dataResponse, readFile, parameters);
            reader.Close();            

            string fileName = tempFolderPath + "invoice-statement" + ".pdf";
                        
            Utilities.GenerateInvoiceStatementsPdf(htmlContent, basePath, fileName);            

            PdfFileResponse response = new PdfFileResponse();
            response.dataArray = Utilities.ReadAllBytes(fileName);
            response.dataJson = Newtonsoft.Json.JsonConvert.SerializeObject(dataResponse);            
            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("PublicGetStatementPdfDownload")]
        [Route("[action]")]
        public IActionResult GetPublicStatementPdfDownload(DateTime? startDate, DateTime? endDate)
        {
            string projectId = Request.Headers["ProjectId"];
            if (endDate == null)
                endDate = DateTime.Now;

            Parameters parameters = new Parameters();
            parameters.startDate = startDate;
            parameters.endDate = endDate;

            ProjectSettings settings = Configs.settings[projectId];

            string tempFolderPath = settings.TempUploadFolderPath;
            string basePath = InvoiceRepository.GetBasePath();

            StatementListResponse dataResponse = new StatementListResponse();
            try
            {         
                dataResponse = InvoiceRepository.GetAllStatement(projectId, parameters);         
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error In _invoiceRepository.GetAllStatement at : " + DateTime.Now + " And Error Message is :" + ex.Message.ToString());
            }
                        
            StreamReader reader = new StreamReader(basePath + @"\ClientInvoice\Statement-export.html");
            string readFile = reader.ReadToEnd();
                        
            string htmlContent = Utilities.AppendDataInvoiceStatementsPdf(dataResponse, readFile, parameters);
            reader.Close();            

            string fileName = tempFolderPath + "invoice-statement" + ".pdf";
            try
            {
                Utilities.GenerateInvoiceStatementsPdf(htmlContent, basePath, fileName);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error In _invoiceRepository.GetAllStatement at : " + DateTime.Now + " And Error Message is :" + ex.Message.ToString());
            }

            var stream = new FileStream(fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [HttpPost]
        [ActionName("GetInvoiceList")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult Invoices([FromQuery]ClientPageListRequest listRequest, [FromBody] Parameters parameters)
        {
            parameters.status = (parameters.status == "all") ? null : parameters.status;
            InvoiceListResponse<InvoiceResponse> response = InvoiceRepository.GetAllInvoices(Request, listRequest, parameters);
            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("GetInvoiceDetail")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InvoiceDetail([FromQuery] int sequenceId, string invoiceNo)
        {
            string projectId = Request.Headers["ProjectId"];
            InvoiceDetailResponse response = InvoiceRepository.GetInvoiceDetail(projectId, sequenceId, invoiceNo);
            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("GetInvoiceDetailPdfDownload")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetInvoiceDetailPdfDownload(int sequenceId, string invoiceNo,bool? excludeLogo)
        {
            string projectId = Request.Headers["ProjectId"];

            ProjectSettings settings = Configs.settings[projectId];
            string tempFolderPath = settings.TempUploadFolderPath;

            string basePath = InvoiceRepository.GetBasePath();
            InvoiceDetailResponse dataResponse = new InvoiceDetailResponse();
            try
            {
                dataResponse = InvoiceRepository.GetInvoiceDetail(projectId, sequenceId, invoiceNo);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error In _invoiceRepository.GetInvoiceDetail at : " + DateTime.Now + " And Error Message is :" + ex.Message.ToString());
            }

            BankDetailsCompanyNoVatNoResponse bankDetails = new BankDetailsCompanyNoVatNoResponse();
            bankDetails = InvoiceRepository.GetBankDetailForClient(projectId, sequenceId, invoiceNo);

            StreamReader reader = new StreamReader(basePath + @"\ClientInvoice\InvoiceDetail.html");
            string readFile = reader.ReadToEnd();            
            string htmlContent = Utilities.AppendDataInvoiceDetailPdf(dataResponse, bankDetails, readFile);
            reader.Close();

            string fileName = tempFolderPath + dataResponse.invoiceNo.Trim() + ".pdf";
            try
            {
                Utilities.GenerateInvoiceDetailPdf(htmlContent, basePath, dataResponse.paymentStatus, fileName, dataResponse.companyAddressInDetail, excludeLogo);
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error In  Utilities.GenerateInvoiceDetailPdf at : " + DateTime.Now + " And Error Message is :" + ex.Message.ToString());
            }

            PdfFileResponse response = new PdfFileResponse();
            response.dataArray = Utilities.ReadAllBytes(fileName);
            response.dataJson = Newtonsoft.Json.JsonConvert.SerializeObject(dataResponse);
            
            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("GetUsersById2")]
        [Route("[action]")]
        public WebThirdParties GetUsersById2(int UserId)
        {
            WebThirdParties returnedWebUsers = WebThirdPartiesRepository.GetUsersByUserId(UserId, Request, HttpContext.Response);

            if (returnedWebUsers == null)
            {
                return returnedWebUsers;
            }
            return returnedWebUsers;
        }

        [HttpGet]
        [ActionName("PublicGetInvoiceDetailPdfDownload")]
        [Route("[action]")]
        public IActionResult GetPublicInvoiceDetailPdfDownload(int sequenceId, string invoiceNo,bool? excludeLogo)
        {
            string projectId = Request.Headers["ProjectId"];
            ProjectSettings settings = Configs.settings[projectId];
            string tempFolderPath = settings.TempUploadFolderPath;

            string basePath = InvoiceRepository.GetBasePath();
            InvoiceDetailResponse dataResponse = InvoiceRepository.GetInvoiceDetail(projectId, sequenceId, invoiceNo);

            BankDetailsCompanyNoVatNoResponse bankDetails = new BankDetailsCompanyNoVatNoResponse();
            bankDetails = InvoiceRepository.GetBankDetailForClient(projectId, sequenceId, invoiceNo);

            StreamReader reader = new StreamReader(basePath + @"\ClientInvoice\InvoiceDetail.html");
            string readFile = reader.ReadToEnd();

            string htmlContent = Utilities.AppendDataInvoiceDetailPdf(dataResponse, bankDetails, readFile);
            reader.Close();

            string fileName = tempFolderPath + dataResponse.invoiceNo.Trim() + ".pdf";

            Utilities.GenerateInvoiceDetailPdf(htmlContent, basePath, dataResponse.paymentStatus, fileName, dataResponse.companyAddressInDetail, excludeLogo);

            var stream = new FileStream(fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        
        [HttpGet]
        [ActionName("GetInvoiceDetailPdfData")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetInvoiceDetailPdfData([FromQuery] int sequenceId, string invoiceNo)
        {
            string projectId = Request.Headers["ProjectId"];
            InvoiceDetailPdfResponse response = InvoiceRepository.GetInvoiceDetailPdfData(projectId, sequenceId, invoiceNo);
            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("GetJobRefAutoComplete")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetJobRefAutoCompleteData([FromQuery]  string search, [FromQuery] int dataItemCount)
        {
            List<JobRefAutoCompleteResponse> response = InvoiceRepository.GetJobRefAutoComplete(Request, search, dataItemCount);

            if (response == null)
                return new ObjectResult(new List<JobRefAutoCompleteResponse>());

            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("GetCompanyAutoComplete")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetCompanyAutoComplete([FromQuery]  string search, [FromQuery] int dataItemCount)
        {
            List<NamedModel> response = InvoiceRepository.GetCompanyAutoComplete(Request, search, dataItemCount);

            if (response == null)
                return new ObjectResult(new List<NamedModel>());

            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("GetShowTypeAutoComplete")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetShowTypesAutoComplete()
        {
            List<NamedModel> response = InvoiceRepository.GetShowTypesData(Request);

            if (response == null)
                return new ObjectResult(new List<NamedModel>());

            return new ObjectResult(response);
        }
    }
}
