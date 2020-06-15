using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IEntityDetailsCoreRepository : IRepository
    {
        List<EntityDetailsCoreMin> GetSelectAllByTransType(HttpRequest request, string transtype);
        List<EntityDetailsCore> GetAllEmailAddresses(HttpRequest Request, long? jobSequence);

        ResponseModel GetAllClients(HttpRequest request, ClientRequest clientRequest);
        List<EntityDetailsCoreMin> GetAllClientsByName(HttpRequest request, string address);


        ResponseModel GetAllSuppliers(HttpRequest request, ClientRequest requestModel);
        List<EntityDetailsCoreMin> GetAllSuppliers(RequestHeaderModel header,string qSearch);
        ResponseModel GetAllContacts(HttpRequest Request, ClientRequest clientRequest);
        EntityDetailsCore GetEntityByEntityId(HttpRequest Request, long? entityId);
        string GetUniqueShortName(HttpRequest request, string longName);
        ResponseModel GetEntityNotesByEntityId(HttpRequest Request, long entityId);
        bool DeleteEntityByEntityId(HttpRequest Request, long entityId);
        EntityDetailsCore GetEntityByShortName(HttpRequest Request, string shortName);
        EntityDetailsCore GetEntityByLongName(RequestHeaderModel header, string longName);
        ResponseModel InsertUpdateProperty(HttpRequest request, RequestModel reqModel);
        ResponseModel InsertUpdateClient(HttpRequest request, object reqModel);
		ResponseModel InsertUpdateContact(HttpRequest request, object reqModel);
		bool UpdateClientInfo(HttpRequest request, EntityDetailsCore edc, string infoType);

        List<EntityDetailsCoreMin> getFullAddress(HttpRequest request, string address);
        List<EntityDetailsCoreMin> GetPropertyAddress(HttpRequest request, string transtype);
        ResponseModel GetPropertyAddressesByAddress(HttpRequest request, string address);
        ResponseModel GetPropertyAddresses(ClientRequest requestModel, HttpRequest request );
        ResponseModel GetPropertyAddressesByAddressAndClientId(HttpRequest Request, string address, long clientId);
        EntityDetailsCore GetEdcWithCloudFields(long entityId, RequestHeaderModel header);
        ResponseModel SaveEdcCloudFields(EntityDetailsCore edc, RequestHeaderModel header);
    }
}
