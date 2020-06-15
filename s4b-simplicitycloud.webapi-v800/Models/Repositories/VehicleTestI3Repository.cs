using Microsoft.AspNetCore.Http;


using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using SimplicityOnlineWebApi.DAL;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class VehicleTestI3Repository : IVehicleTestI3Repository
    {
        
        private ILogger<VehicleTestI3Repository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public VehicleTestI3Repository(ILogger<VehicleTestI3Repository> logger)
        {
            
            
        }

        public VehicleTestI3Repository(bool _IsSecondaryDatabase, string _SecondaryDatabaseId)
        {
            this.IsSecondaryDatabase = _IsSecondaryDatabase;
            this.SecondaryDatabaseId = _SecondaryDatabaseId;
        }

        public ResponseModel Insert(AssetTestI assetTestI,long testListSequence, HttpRequest request)
        {   
            ResponseModel returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    AssetTestIDB assetTestIDB = new AssetTestIDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                   
                    long sequence = -1;
                    int userId = Utilities.GetUserIdFromRequest(request);
                    assetTestI.CreatedBy = userId;
                    assetTestI.DateCreated = DateTime.Now;
                    if (assetTestIDB.insert(out sequence, assetTestI, testListSequence))
                    {
                        returnValue.TheObject = assetTestI;
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
