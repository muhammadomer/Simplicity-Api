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
    public class AssetTestIRepository : IAssetTestIRepository
    {
        //private ILogger<AssetTestIRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public AssetTestIRepository()
        {
            //
            //
        }

        public AssetTestIRepository(bool _IsSecondaryDatabase, string _SecondaryDatabaseId)
        {
            this.IsSecondaryDatabase = _IsSecondaryDatabase;
            this.SecondaryDatabaseId = _SecondaryDatabaseId;
        }

        public List<AssetTestI> GetAllBySequences( long sequence, HttpRequest request)
        {
            List<AssetTestI> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        AssetTestIDB assetTestIDB = new AssetTestIDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = assetTestIDB.getAllBySequence(sequence);
                        if (returnValue == null)
                        {
                            //Report back error
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.InnerException + " " + ex.Message);
            }
            return returnValue;
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

        public bool UpdateBySequence(AssetTestI assetTestI, HttpRequest request)
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
                        AssetTestIDB assetTestIDB = new AssetTestIDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Utilities.GetUserIdFromRequest(request);
                        assetTestI.LastAmendedBy = userId;
                        assetTestI.DateLastAmended = DateTime.Now;
                        returnValue = assetTestIDB.updateBySequence(assetTestI);
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
