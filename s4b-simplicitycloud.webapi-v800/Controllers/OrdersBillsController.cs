using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrdersBillsController : Controller
    {
        private readonly IOrdersBillsRepository OrdersBillsRepository;
        private readonly IRefEntityPaymentTypeRepository RefEntityPaymentTypeRepository;
        private readonly ILogger<OrdersController> Logger;

        public OrdersBillsController(
            IOrdersBillsRepository ordersBillsRepository,
            IRefEntityPaymentTypeRepository refEntityPaymentTypeRepository,
            ILogger<OrdersController> logger
            )
        {
            this.RefEntityPaymentTypeRepository = refEntityPaymentTypeRepository;
            this.OrdersBillsRepository = ordersBillsRepository;
            this.Logger= logger;
        }
            

        [HttpPost]
        [ActionName("CreateInvoiceRequest")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult CreateInvoiceRequest([FromBody]OrdersBills orderBill)
        {
            var retOrder = OrdersBillsRepository.CreateInvoiceRequest(Request, orderBill);
            if (retOrder == null)
            {
                return new ObjectResult(HttpContext.Response);
            }

            return new ObjectResult(retOrder);
        }

        [HttpPost]
        [ActionName("CreateRequestForPayment")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult CreateRequestForPayment([FromBody]OrdersBills orderBill)
        {
            var retOrder = OrdersBillsRepository.CreateRequestForPayment(Request, orderBill);
            if (retOrder == null)
            {
                return new ObjectResult(HttpContext.Response);
            }

            return new ObjectResult(retOrder);
        }

        [HttpPost]
        [ActionName("UpdateOrdersBillsBySequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateOrdersBillsBySequence([FromBody]OrdersBills orderBill)
        {
            var retOrder = OrdersBillsRepository.UpdateOrdersBillsBySequence(Request, orderBill);
            if (retOrder == null)
            {
                return new ObjectResult(HttpContext.Response);
            }

            return new ObjectResult(retOrder);
        }

        [HttpPost]
        [ActionName("ConvertToInvoice")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult ConvertToInvoice([FromBody]OrdersBills orderBill)
        {   
            var retOrder = OrdersBillsRepository.ConvertToInvoice(Request, orderBill);
            if (retOrder == null)
            {
                return new ObjectResult(HttpContext.Response);
            }

            return new ObjectResult(retOrder);
        }

        [HttpPost]
        [ActionName("BatchConvertToInvoice")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult BatchConvertToInvoice([FromBody]List<OrdersBills> listOrderBill)
        {
            var retOrder = OrdersBillsRepository.BatchConvertToInvoice(Request, listOrderBill);
            if (retOrder == null)
            {
                return new ObjectResult(HttpContext.Response);
            }

            return new ObjectResult(retOrder);
        }

        [HttpPost]
        [ActionName("SaveInvoice")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult SaveInvoice([FromBody]OrdersBills orderBill)
        {
            var retOrder = OrdersBillsRepository.SaveInvoice(Request, orderBill);
            if (retOrder == null)
            {
                return new ObjectResult(HttpContext.Response);
            }

            return new ObjectResult(retOrder);
        }

        [HttpGet]
        [ActionName("GetOrderBillItemsForInvoicingByJobSequence")]
        [Route("[action]")]
        public IActionResult GetOrderBillItemsForInvoicingByJobSequence(long jobSequence)
        {
            return new ObjectResult(OrdersBillsRepository.GetOrderBillItemsForInvoicingByJobSequence(Request, jobSequence));

        }

        [HttpGet]
        [ActionName("GetOrderBillsForEditingBySequence")]
        [Route("[action]")]
        public IActionResult GetOrderBillsForEditingBySequence(long billSequence,long jobSequence)
        {
            return new ObjectResult(OrdersBillsRepository.GetOrderBillsForEditingBySequence(Request, billSequence,jobSequence));

        }

        [HttpGet]
        [ActionName("GetOrdersBillBySequence")]
        [Route("[action]")]
        public IActionResult GetOrdersBillBySequence(long sequence)
        {
            return new ObjectResult(OrdersBillsRepository.GetOrderBillsBySequence(Request, sequence));

        }

        [HttpGet]
        [ActionName("GetOrdersBillByJobSequenceAndType")]
        [Route("[action]")]
        public IActionResult GetOrdersBillByJobSequenceAndType(long jobSequence,string type)
        {
            return new ObjectResult(OrdersBillsRepository.GetOrdersBillByJobSequenceAndType(Request, jobSequence,type));

        }

        [HttpGet]
        [ActionName("GetOrdersBillInvoiceBySequence")]
        [Route("[action]")]
        public IActionResult GetOrdersBillInvoiceBySequence(long billSequence)
        {
            return new ObjectResult(OrdersBillsRepository.GetOrdersBillInvoiceBySequence(Request, billSequence));

        }

        [HttpGet]
        [ActionName("GetApplicationForPaymentsAndInvoicesByJobSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetApplicationForPaymentsAndInvoicesByJobSequence(long jobSequence)
        {   
            return new ObjectResult(OrdersBillsRepository.GetApplicationForPaymentsAndInvoicesByJobSequence(Request,jobSequence));
        }
        //---Following method used to get data for Report printing
        [HttpGet]
        [ActionName("GetSaleInvoiceBySequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetSaleInvoiceBySequence(long billSequence)
        {
            return new ObjectResult(OrdersBillsRepository.GetSaleInvoiceBySequence(Request, billSequence));
        }

        [HttpPost]
        [ActionName("GetListOfAppForPayments")]
        [Route("[action]")]
        public IActionResult GetListOfAppForPayments([FromBody]ClientRequest clientRequest, string fromDate, string toDate)
        {
            DateTime? FromDate = null; DateTime? ToDate = null;
            if (fromDate != null)
                FromDate = Convert.ToDateTime(fromDate.Substring(0, 24));
            if (toDate != null)
                ToDate = Convert.ToDateTime(toDate.Substring(0, 24));
            return new ObjectResult(OrdersBillsRepository.GetListOfAppForPayments(Request, clientRequest, FromDate, ToDate));

        }

        [HttpGet]
        [ActionName("GetListOfSaleInvoices")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetListOfSaleInvoices(string fromDate,string toDate)
        {
            DateTime? FromDate=null; DateTime? ToDate=null;
            if (fromDate !=null)
               FromDate = Convert.ToDateTime(fromDate.Substring(0, 24));
            if(toDate != null)
                ToDate = Convert.ToDateTime(toDate.Substring(0, 24));
            return new ObjectResult(OrdersBillsRepository.GetListOfSaleInvoices(Request, FromDate,ToDate));
        }

        [HttpPost]
        [ActionName("DownloadPDF")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult DownloadPDF(string htmlString)
        {
            return new ObjectResult(OrdersBillsRepository.DownloadPDF(Request, htmlString));
            //ProjectSettings settings = Configs.settings[Request.Headers["ProjectId"]];
            //htmlString = (string)JsonConvert.DeserializeObject(htmlString);
            //string fileName= Path.Combine(settings.S4BFormsRootFolderPath, "file.pdf");
            //string fileName = "fioe.pdf";
            //Byte[] res = null;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    var pdf = PdfSharp.Pdf.PdfGenerator.GeneratePdf(htmlString, PdfSharp.PageSize.A4);
            //    pdf.Save(ms);
            //    res = ms.ToArray();
            //}

            
            // Render an HTML document or snippet as a string
            //HtmlToPdf htmlToPdf = new IronPdf.HtmlToPdf();
            //IronPdf.PdfDocument PDF = htmlToPdf.RenderHtmlAsPdf(htmlString);
            //PDF.SaveAs(fileName);
            //if (System.IO.File.Exists(fileName))
            //{
            //    Response.ContentType = "application/x-mspowerpoint";
            //    Response.Headers.Add("Content-Disposition", "attachment,fileName = file.pdf");
            //    Response.WriteAsync(fileName);
            //};
        }

    }
}
