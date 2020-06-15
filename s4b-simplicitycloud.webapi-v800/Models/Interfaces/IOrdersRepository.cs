using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using System.Data;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IOrdersRepository : IRepository
    {
        List<Orders> GetAllOrders(HttpRequest request);
        //bool CancelOrderByJobRef(string jobRef, HttpRequest request);
        List<Orders> GetAllOrdersByJobRef(string jobRef, HttpRequest request);
        List<OrdersMin> GetAllOrdersMinByJobRef(string jobRef, HttpRequest request);
        List<Orders> SearchOrders(string key, string field, string match, HttpRequest request);
        List<Orders> SearchOrders(HttpRequest request);
        List<Orders> GetOrdersByJobRefOrAddressOrClientName(string jobRef, string jobAddress, string jobClientName, HttpRequest request);
        ResponseModel GetOrdersMinByJobRefOrAddressOrClientName(ClientRequest clientRequest,HttpRequest request);
        List<OrdersMinWithJobAddress> GetOrdersMinByJobAddress(long jobAddressId, HttpRequest request);
        Orders GetOrderDetailsBySequence(long sequence, HttpRequest request);
        Orders GetOrderByJobRef(string jobRef, HttpRequest request);
        bool CancelOrderBySequence(Orders order, HttpRequest request);
        bool ReactivateOrderBySequence(Orders order, HttpRequest request);
        bool AddFileByEBSJobSequence(long ebsJobSequence, string fileName, string parentFolderNames, Cld_Ord_Labels_Files oiFireProtectionIImages, HttpRequest request, HttpResponse response);
        bool UpdateJobAddress(OrdersJobAddress jobAddress, HttpRequest request);
        bool UpdateJobAddressDetails(Orders order, HttpRequest request);
        ResponseModel UpdateJobClient(int sequence,long clientId,string clientName, HttpRequest request);
        bool UpdateJobClientName(int sequence, string jobClientName, HttpRequest request);
        bool UpdateJobClientRef(int sequence, string jobClientRef, HttpRequest request);
        bool UpdateOrderInfo(Orders order, string infoType, HttpRequest request);
        string GetNewJobRefNo(HttpRequest request, HttpResponse response);
        bool? CanManualCreateJobRefForCreateOrder(HttpRequest request, HttpResponse response);
        Orders CreateOrderByJobRef(string jobRef, bool autoCreateJobRef, HttpRequest request, HttpResponse response);
        Orders CreateOrderByJobRef(Orders order, bool autoCreateJobRef, HttpRequest request, HttpResponse response);
        Orders UpdateOrder(Orders order, HttpRequest request);
        bool UpdateJobAddressByAddressId(long addressId, string jobAddress, HttpRequest request);
        ResponseModel OrdersList(ClientRequest requestModel, HttpRequest request);
        DataTable OrdersList2(int size, string projectId,HttpRequest request);
        string GetCaptionForActiveProject(HttpRequest request);
        bool GetAPSConfig(HttpRequest request);
    }
}
