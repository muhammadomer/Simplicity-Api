using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IVehicleTestI2Repository : IRepository
    {
        
        ResponseModel Insert(VehicleTestI2 vehicleTestI, HttpRequest request);
        
        
    }
}
