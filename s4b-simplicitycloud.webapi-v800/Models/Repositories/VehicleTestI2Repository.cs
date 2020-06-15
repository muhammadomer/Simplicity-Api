using Microsoft.AspNetCore.Http;


using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.DAL;
using System.IO;
using System.Data;
using Newtonsoft.Json;
namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class VehicleTestI2Repository : IVehicleTestI2Repository
    {
        
        //private ILogger<VehicleTestI2Repository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public VehicleTestI2Repository()
        {
            
            
        }

        public VehicleTestI2Repository(bool _IsSecondaryDatabase, string _SecondaryDatabaseId)
        {
            this.IsSecondaryDatabase = _IsSecondaryDatabase;
            this.SecondaryDatabaseId = _SecondaryDatabaseId;
        }

        
        
        public ResponseModel Insert(VehicleTestI2 vehicleTestI, HttpRequest request)
        {   
            ResponseModel returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    VehicleTestI2DB vchTestI2DB = new VehicleTestI2DB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                   
                    long sequence = -1;
                    int userId = Utilities.GetUserIdFromRequest(request);
                    vehicleTestI.DateCreated = DateTime.Now;
                    if (vchTestI2DB.insert(out sequence, vehicleTestI))
                    {
                        returnValue.TheObject = vehicleTestI;
                        returnValue.IsSucessfull = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Creating Asset TestH :"+ ex);
            }
            return returnValue;
        }

    }
}
