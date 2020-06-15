using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IOrderCheckListRepository : IRepository
    {
        ResponseModel GetOrderCheckListByJobSequence(HttpRequest request, long jobSequence);
        ResponseModel UpdateOrderCheckList(HttpRequest request, RequestModel reqModel);
    }
}
