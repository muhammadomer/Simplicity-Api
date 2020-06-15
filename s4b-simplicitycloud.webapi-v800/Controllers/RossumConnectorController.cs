using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models;
using SimplicityOnlineWebApi.Models.Interfaces;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RossumConnectorController : Controller
    {
        private readonly IRossumRepository RossumRepository;
        protected readonly ILogger<RossumController> Logger;

        public RossumConnectorController(IRossumRepository rossumRepository, ILogger<RossumController> logger)
        {
            this.RossumRepository = rossumRepository;
            this.Logger = logger;
        }
       
        [HttpPost]
        [ActionName("RossumWebHook")]
        [Route("[action]")]
        public string RossumWebHook(string action, string annotation)
        {
            //RossumWebHook resp = JsonConvert.DeserializeObject<RossumWebHook>(para);
            string lines = "--Webhook called--";
            //if (para != null) lines += para.ToString();

            //System.IO.File.WriteAllText(@"WriteLines.txt", lines);
            //this.Logger.LogError("WebHook Called by Postman - " + para.ToString()) ;
            return "Successful";
        }
    }


}
