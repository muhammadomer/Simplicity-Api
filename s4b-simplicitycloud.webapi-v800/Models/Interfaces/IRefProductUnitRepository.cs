using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefProductUnitRepository : IRepository
    {
        ResponseModel GetProductUnits(RequestHeaderModel header);
    }
}
