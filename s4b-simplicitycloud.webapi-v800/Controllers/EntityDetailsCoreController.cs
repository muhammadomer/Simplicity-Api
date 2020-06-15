using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class EntityDetailsCoreController : Controller
    {

        private readonly IEntityDetailsCoreRepository EntityDetailsCoreRepository;
        private readonly IEntityDetailsSupplementaryRepository EntityDetailsSupplementaryRepository;
        private readonly IEntityDetailsJoinRepository EntityDetailsJoinRepository;
        private readonly IOrdersRepository OrdersRepository;

        public EntityDetailsCoreController(
            IEntityDetailsCoreRepository entityDetailsCoreRepository,
            IEntityDetailsSupplementaryRepository entityDetailsSupplementaryRepository,
            IEntityDetailsJoinRepository entityDetailsJoinRepository,
            IOrdersRepository ordersRepository
            )
        {
            this.EntityDetailsCoreRepository = entityDetailsCoreRepository;
            this.EntityDetailsSupplementaryRepository =entityDetailsSupplementaryRepository;
            this.EntityDetailsJoinRepository =entityDetailsJoinRepository;
            this.OrdersRepository =ordersRepository;

                }

        [HttpGet]
        [ActionName("GetAllByTransType")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllByTransType(string transType)
        {
            List<EntityDetailsCoreMin> EntityDetailsCore = EntityDetailsCoreRepository.GetSelectAllByTransType(Request, transType);
            if (EntityDetailsCore == null)
            {
                return new ObjectResult(EntityDetailsCore);
            }
            return new ObjectResult(EntityDetailsCore);
        }

        [HttpGet]
        [ActionName("GetAllEmailAddresses")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllEmailAddresses(long jobSequence)
        {
            List<EntityDetailsCore> EntityDetailsCore = EntityDetailsCoreRepository.GetAllEmailAddresses(Request, jobSequence);
            if (EntityDetailsCore == null)
            {
                return new ObjectResult(EntityDetailsCore);
            }
            return new ObjectResult(EntityDetailsCore);
        }

        [HttpGet]
        [ActionName("GetAllClientsByName")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllClientsByName(string name)
        {
            List<EntityDetailsCoreMin> EntityDetailsCore = EntityDetailsCoreRepository.GetAllClientsByName(Request, name);
            if (EntityDetailsCore == null)
            {
                return new ObjectResult(EntityDetailsCore);
            }
            return new ObjectResult(EntityDetailsCore);
        }

        [HttpGet]
        [ActionName("GetFullAddress")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetFullAddress(string address)
        {
            List<EntityDetailsCoreMin> EntityDetailsCore = EntityDetailsCoreRepository.getFullAddress(Request, address);
            if (EntityDetailsCore == null)
            {
                return new ObjectResult(EntityDetailsCore);
            }
            return new ObjectResult(EntityDetailsCore);
        }

      
        [HttpPost]
        [ActionName("GetAddresses")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetPropertyAddressesByAddress([FromBody]ClientRequest requestModel)
        {
            return new ObjectResult(EntityDetailsCoreRepository.GetPropertyAddresses(requestModel, Request));

        }

        [HttpGet]
        [ActionName("GetPropertyAddressesByAddressAndClientId")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetPropertyAddressesByAddressAndClientId(string address, long clientId)
        {
            return new ObjectResult(EntityDetailsCoreRepository.GetPropertyAddressesByAddressAndClientId(Request, address, clientId));
        }

        [HttpGet]
        [ActionName("GetAddressById")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAddressById(long id)
        {
            EntityDetailsCore result = EntityDetailsCoreRepository.GetEntityByEntityId(Request, id);
            if (result != null)
            {
                List<EntityDetailsSupplementary> edcSupplementaryList = EntityDetailsSupplementaryRepository.GetSelectAllByEntityId(result.EntityId ?? 0, Request);
                if (edcSupplementaryList != null)
                {
                    result.SupplementaryEntityDetails = edcSupplementaryList.Find(x => x.DataType == "038") != null ? edcSupplementaryList.Find(x => x.DataType == "038").Data : null;
                    result.PropertyType = edcSupplementaryList.Find(x => x.DataType == "022") != null ? edcSupplementaryList.Find(x => x.DataType == "022").Data : null;
                    result.PropertyStatus = edcSupplementaryList.Find(x => x.DataType == "036") != null ? edcSupplementaryList.Find(x => x.DataType == "036").Data : null;
                }
            }

            if (result == null)
            {
                return new ObjectResult(result);
            }
            return new ObjectResult(result);
        }

        [HttpGet]
        [ActionName("GetEdcById")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetEdcById(long id)
        {
            EntityDetailsCore result = EntityDetailsCoreRepository.GetEntityByEntityId(Request, id);
            if (result != null)
            {
                List<EntityDetailsSupplementary> edcSupplementaryList = EntityDetailsSupplementaryRepository.GetSelectAllByEntityId(result.EntityId ?? 0, Request);
                if (edcSupplementaryList != null)
                {
                    result.SupplementaryEntityDetails = edcSupplementaryList.Find(x => x.DataType == "038") != null ? edcSupplementaryList.Find(x => x.DataType == "038").Data : null;
                    result.WebAddress = edcSupplementaryList.Find(x => x.DataType == "017") != null ? edcSupplementaryList.Find(x => x.DataType == "017").Data : null;
                    result.PropertyType = edcSupplementaryList.Find(x => x.DataType == "022") != null ? edcSupplementaryList.Find(x => x.DataType == "022").Data : null;
                    result.PropertyStatus = edcSupplementaryList.Find(x => x.DataType == "036") != null ? edcSupplementaryList.Find(x => x.DataType == "036").Data : null;
                    result.SageNominalCode = edcSupplementaryList.Find(x => x.DataType == "027") != null ? edcSupplementaryList.Find(x => x.DataType == "027").Data : null;
                    result.SageDefaultTaxCode = edcSupplementaryList.Find(x => x.DataType == "028") != null ? edcSupplementaryList.Find(x => x.DataType == "028").Data : null;
                    result.SageVatNumber = edcSupplementaryList.Find(x => x.DataType == "008") != null ? edcSupplementaryList.Find(x => x.DataType == "008").Data : null;
                }
            }

            if (result == null)
            {
                return new ObjectResult(result);
            }
            return new ObjectResult(result);
        }

        [HttpGet]
        [ActionName("GenerateShortName")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GenerateShortName(string longName)
        {
            string NameShort = EntityDetailsCoreRepository.GetUniqueShortName(Request, longName);
            return new ObjectResult(new { NameShort });
        }

        [HttpGet]
        [ActionName("GetEntityNotesByEntityId")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetEntityNotesByEntityId(long id)
        {
            ResponseModel EntityDetailsCore = EntityDetailsCoreRepository.GetEntityNotesByEntityId(Request,id);
            return new ObjectResult(EntityDetailsCore);
        }

        [HttpGet]
        [ActionName("GetOwnerName")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetOwnerName()
        {
            EntityDetailsCore result = EntityDetailsCoreRepository.GetEntityByEntityId(Request, 1);

            if (result == null)
            {
                return new ObjectResult(result);
            }
            return new ObjectResult(result);
        }

        [HttpGet]
        [ActionName("Delete")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult DeleteClient(long id)
        {
            bool result = EntityDetailsCoreRepository.DeleteEntityByEntityId(Request, id);
            string json = "";
            if (result)
            {
                json = "{\"deleted\": 1}";
            }
            else {
                json = "{\"deleted\": 0}";
            }
            
            return new ObjectResult(json);
        }

        [HttpPost]
        [ActionName("GetAllClients")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllClients([FromBody]ClientRequest requestModel)
        {
            ResponseModel EntityDetailsCore = EntityDetailsCoreRepository.GetAllClients(Request, requestModel);
            return new ObjectResult(EntityDetailsCore);
        }

        [HttpPost]
        [ActionName("GetSuppliers")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetSuppliers([FromBody]ClientRequest requestModel)
        {
            ResponseModel EntityDetailsCore = EntityDetailsCoreRepository.GetAllSuppliers(Request, requestModel);
            return new ObjectResult(EntityDetailsCore);
        }

        [HttpPost]
        [ActionName("GetAllSuppliers")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllSuppliers([FromBody]ClientRequest requestModel)
        {
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            List<EntityDetailsCoreMin> EntityDetailsCore = EntityDetailsCoreRepository.GetAllSuppliers(header, null);
            if (EntityDetailsCore == null)
            {
                return new ObjectResult(EntityDetailsCore);
            }
            return new ObjectResult(EntityDetailsCore);
        }

        //[HttpGet]
        //[ActionName("GetAllSuppliers")]
        //[Route("[action]")]
        //[ValidateRequestState]
        //public IActionResult GetAllSuppliers()
        //{
        //    List<EntityDetailsCoreMin> EntityDetailsCore = EntityDetailsCoreRepository.GetAllSuppliers(Request, null);
        //    if (EntityDetailsCore == null)
        //    {
        //        return new ObjectResult(EntityDetailsCore);
        //    }
        //    return new ObjectResult(EntityDetailsCore);
        //}

        [HttpPost]
        [ActionName("GetAllSuppliersAutoComplete")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllSuppliersAutoComplete([FromBody]ClientRequest requestModel)
        {
            ResponseModel response = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            string query = "";
            if (requestModel != null)
                query = requestModel.query;
            List<EntityDetailsCoreMin> EntityDetailsCore = EntityDetailsCoreRepository.GetAllSuppliers(header, query);
            if (EntityDetailsCore == null)
            {
                response.IsSucessfull = false;
                return new ObjectResult(response);
            }
            response.TheObject = EntityDetailsCore;
            response.IsSucessfull = true;
            return new ObjectResult(response);
        }
        [HttpPost]
        [ActionName("GetAllContacts")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllContacts([FromBody]ClientRequest requestModel)
        {
			ResponseModel EntityDetailsCore = EntityDetailsCoreRepository.GetAllContacts(Request, requestModel);
            return new ObjectResult(EntityDetailsCore);
        }
		[HttpPost]
		[ActionName("InsertUpdateContact")]
		[Route("[action]")]
		[ValidateRequestState]
		public IActionResult InsertUpdateContact([FromBody]object reqModel)
		{
			return new ObjectResult(EntityDetailsCoreRepository.InsertUpdateContact(Request, reqModel));
		}

		[HttpGet]
        [ActionName("GetPropertyAddress")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetPropertyAddress(string transType)
        {
            List<EntityDetailsCoreMin> EntityDetailsCore = EntityDetailsCoreRepository.GetPropertyAddress(Request, transType);
            if (EntityDetailsCore == null)
            {
                return new ObjectResult(EntityDetailsCore);
            }
            return new ObjectResult(EntityDetailsCore);
        }

        [HttpPost]
        [ActionName("InsertUpdateProprty")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InsertUpdateProprty([FromBody]RequestModel reqModel)
        {
            return new ObjectResult(EntityDetailsCoreRepository.InsertUpdateProperty(Request, reqModel));
        }

        [HttpPost]
        [ActionName("InsertUpdateClient")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InsertUpdateClient([FromBody]object reqModel)
        {
            return new ObjectResult(EntityDetailsCoreRepository.InsertUpdateClient(Request, reqModel));
        }

        [HttpPost]
        [ActionName("UpdateClientInfo")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateClientInfo([FromBody]EntityDetailsCore edc, string infoType)
        {
            try
            {
                bool success = EntityDetailsCoreRepository.UpdateClientInfo(Request, edc, infoType);
                return new ObjectResult(success);
            }
            catch (System.Exception ex)
            {
                return new ObjectResult(false);
            }
        }
    }
}
