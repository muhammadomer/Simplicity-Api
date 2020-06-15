using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IVehicleTestIRepository : IRepository
    {   
        ResponseModel Insert(VehicleTestI vehicleTestI, HttpRequest request);
        
        
    }
}
