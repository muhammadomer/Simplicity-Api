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
    public class VehicleTestIRepository : IVehicleTestIRepository
    {
        
        //private ILogger<VehicleTestIRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public VehicleTestIRepository()
        {
            
            
        }

        public VehicleTestIRepository(bool _IsSecondaryDatabase, string _SecondaryDatabaseId)
        {
            this.IsSecondaryDatabase = _IsSecondaryDatabase;
            this.SecondaryDatabaseId = _SecondaryDatabaseId;
        }

        public ResponseModel Insert(VehicleTestI vehicleTestI, HttpRequest request)
        {   
            ResponseModel returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    VehicleTestIDB vehicleTestIDB = new VehicleTestIDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                   
                    long sequence = -1;
                    int userId = Utilities.GetUserIdFromRequest(request);
                    vehicleTestI.DateCreated = DateTime.Now;
                    if (vehicleTestIDB.insert(out sequence, vehicleTestI))
                    {
                        returnValue.TheObject = vehicleTestI;
                        returnValue.IsSucessfull = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Creating Vehicle TestI :"+ ex);
            }
            return returnValue;
        }

    }
}
