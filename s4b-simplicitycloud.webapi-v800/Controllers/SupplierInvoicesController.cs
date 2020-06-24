using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplicityOnlineWebApi.Models;
using SimplicityOnlineWebApi.Models.ViewModels;
using System.Linq;
using System.Data;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class SupplierInvoicesController : Controller
    {
        private readonly ISupplierInvoiceRepository SupplierInvoiceRepository;
        private readonly ILogger<OrdersController> Logger;
        private readonly IRossumRepository RossumRepository;

        public SupplierInvoicesController(
            ISupplierInvoiceRepository supplierInvoiceRepository,
            IRossumRepository rossumRepository,
            ILogger<OrdersController> logger
            )
        {
            this.SupplierInvoiceRepository = supplierInvoiceRepository;
            this.RossumRepository = rossumRepository;
            this.Logger= logger;
        }
        [HttpPost]
        [ActionName("GetRossumUnfinalizedInvoices")]
        [Route("[action]")]
        public IActionResult GetRossumUnfinalizedInvoices([FromBody]ClientRequest clientRequest)
        {
            ResponseModel returnValue = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            return new ObjectResult(SupplierInvoiceRepository.GetRossumUnfinalizedInvoices(header, clientRequest));
        }   
        [HttpPost]
        [ActionName("GetUnfinalizedInvoices")]
        [Route("[action]")]
        public IActionResult GetUnfinalizedInvoices([FromBody]ClientRequest clientRequest)
        {
            ResponseModel returnValue = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            return new ObjectResult(SupplierInvoiceRepository.GetUnfinalizedInvoices(header, clientRequest));
        }
        [HttpPost]
        [ActionName("CreateInvoiceItemised")]
        [Route("[action]")]
        public IActionResult CreateInvoiceItemised([FromBody]SupplierInvoiceVM invoiceItemesed)
        {
            ResponseModel invItemised = null;
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            if (invoiceItemesed.IsConvertToInvoice)
            {
                if (invoiceItemesed.InvoiceLines.Count == 0)
                {
                    invItemised = new ResponseModel();
                    invItemised.IsSucessfull = false;
                    invItemised.Message = ("Add atleast one Item to proceed!");
                    return new ObjectResult(invItemised);
                }
                double footerSumAmtMain = Math.Round(invoiceItemesed.InvoiceLines.Sum(x => x.ItemAmtSubTotal),2);
                double footerSumAmtVAT = Math.Round(invoiceItemesed.InvoiceLines.Sum(x => x.ItemAmtVAT),2);
                double footerSumAmtTotal = Math.Round(invoiceItemesed.InvoiceLines.Sum(x => x.ItemAmtTotal),2);
                if(footerSumAmtMain !=invoiceItemesed.SumAmtMain || footerSumAmtVAT != invoiceItemesed.SumAmtVAT || footerSumAmtTotal != invoiceItemesed.SumAmtTotal)
                {
                    invItemised = new ResponseModel();
                    invItemised.IsSucessfull = false;
                    invItemised.Message = ("Header summary mismatch with the footer summary!");
                    return new ObjectResult(invItemised);
                }
            }
            InvoiceItemised itemised = new InvoiceItemised();
            itemised.CopyPropertyValues(invoiceItemesed);
            itemised.InvoiceLines = new List<InvoiceItemisedItems>();
            InvoiceItemisedItems invoiceItem = null;
            foreach (var item in invoiceItemesed.InvoiceLines)
            {
                invoiceItem = new InvoiceItemisedItems();
                invoiceItem.CopyPropertyValues(item);
                itemised.InvoiceLines.Add(invoiceItem);
            }
            invItemised = SupplierInvoiceRepository.SaveInvoice(itemised, header);
            if (invItemised == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(invItemised);
        } 
        [HttpPost]
        [ActionName("GetItemisedInvoice")]
        [Route("[action]")]
        public IActionResult GetItemisedInvoice([FromBody] string invoiceSequence)
        {
            ResponseModel returnValue = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            return new ObjectResult(SupplierInvoiceRepository.GetItemisedInvoice(header, long.Parse(invoiceSequence)));
        }      
        [HttpPost]
        [ActionName("GetVehicle")]
        [Route("[action]")]
        public IActionResult GetVehicle()
        {
            ResponseModel returnValue = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            return new ObjectResult(SupplierInvoiceRepository.GetVehicle(header));
        }        
        [HttpPost]
        [ActionName("GetItemTels")]
        [Route("[action]")]
        public IActionResult GetItemTels()
        {
            ResponseModel returnValue = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            return new ObjectResult(SupplierInvoiceRepository.GetItemTel(header));
        }      
        [HttpPost]
        [ActionName("GetCostCode")]
        [Route("[action]")]
        public IActionResult GetCostCode()
        {
            ResponseModel returnValue = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            return new ObjectResult(SupplierInvoiceRepository.GetCostCode(header));
        }
        [HttpPost]
        [ActionName("GetSageDetail")]
        [Route("[action]")]
        public IActionResult GetSageDetail(long contactId)
        {
            ResponseModel returnValue = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            return new ObjectResult(SupplierInvoiceRepository.GetSageDetail(contactId, header));
        }
        [HttpPost]
        [ActionName("UpdateInvoiceSupplier")]
        [Route("[action]")]
        public IActionResult UpdateInvoiceSupplier([FromBody]SupplierInvoiceVM invoiceItemesed)
        {
            ResponseModel invItemised = null;
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            InvoiceItemised itemised = new InvoiceItemised();
            itemised.CopyPropertyValues(invoiceItemesed);
            invItemised = SupplierInvoiceRepository.UpdateInvoiceSupplier(itemised, header);
            if (invItemised == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            SupplierInvoiceVM invoiceData = SupplierInvoiceRepository.GetInvoiceBySequenceNo(itemised.Sequence,header);
            itemised.CopyPropertyValues(invoiceData);
            invItemised = RossumRepository.MoveInvoiceFileToSuccessFolder(itemised, header);
            return new ObjectResult(invItemised);
        }
    }
}
