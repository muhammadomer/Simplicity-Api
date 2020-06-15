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
    public class VehicleTestHRepository : IVehicleTestHRepository
    {
        //private ILogger<VehicleTestHRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public VehicleTestHRepository()
        {
            //
        }

        public VehicleTestHRepository(bool _IsSecondaryDatabase, string _SecondaryDatabaseId)
        {
            this.IsSecondaryDatabase = _IsSecondaryDatabase;
            this.SecondaryDatabaseId = _SecondaryDatabaseId;
        }

      
        public ResponseModel Insert(VehicleTestH vehicleTestH, HttpRequest request)
        {   
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    VehicleTestHDB vehicleTestHDB = new VehicleTestHDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                   
                    long sequence = -1;
                    int userId = Utilities.GetUserIdFromRequest(request);
                    vehicleTestH.DateCreated = DateTime.Now;
                    if (vehicleTestHDB.insert(out sequence, vehicleTestH))
                    {
                        vehicleTestH.Sequence = sequence;
                        returnValue.TheObject = vehicleTestH;
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
       
        public bool UpdateLocked(long assetSequence, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        VehicleTestHDB vehicleTestHDB = new VehicleTestHDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = vehicleTestHDB.updateLocked(assetSequence);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
        }



    }
}
