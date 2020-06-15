using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAssetRegisterSuppGasRepository : IRepository
    {
        AssetRegisterSuppGas insert(HttpRequest request, AssetRegisterSuppGas obj);
    }
}
