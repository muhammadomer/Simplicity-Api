
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;

using System;
using Newtonsoft.Json;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseOrderItemsController : Controller
    {
        private readonly IPurchaseOrderItemsRepository POItemsRepository;
        private readonly ILogger<PurchaseOrderItemsController> Logger;

        public PurchaseOrderItemsController(IPurchaseOrderItemsRepository poItemsRepository, ILogger<PurchaseOrderItemsController> logger)
        {
            this.POItemsRepository = poItemsRepository;
            this.Logger = logger;
                }

        [HttpPost]
        [ActionName("GetAllPurchaseOrderItems")]
        [Route("[action]")]
        public IActionResult GetAllPurchaseOrderItems([FromBody]ClientRequest clientRequest)
        {
            return new ObjectResult(POItemsRepository.GetAllPurchaseOrderItems(Request, clientRequest));

        }

       

       

    }
}
