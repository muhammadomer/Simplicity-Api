using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IVehicleTestHRepository : IRepository
    {
        
        ResponseModel Insert(VehicleTestH vehicleTestH, HttpRequest request);
        bool UpdateLocked(long assetSequence, HttpRequest request);
        
    }
}
