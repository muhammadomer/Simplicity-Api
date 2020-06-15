using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IVehicleTestI3Repository : IRepository
    {
        ResponseModel Insert(AssetTestI assetTestI, long testListSequence, HttpRequest request);
        
    }
}
