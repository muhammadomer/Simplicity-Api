using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAssetRegisterRepository : IRepository
    {
        List<AssetRegister> getAssetsList(HttpRequest request);
        List<AssetRegister> search(FilterOption filterOption, HttpRequest request);
        List<AssetDetail> getAssetsDetail(long sequence, HttpRequest request);

        ResponseModel GetSelectAllAssetsList(HttpRequest Request, ClientRequest clientRequest);
    }
}
