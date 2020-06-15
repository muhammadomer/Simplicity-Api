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
    public class AssetTestHRepository : IAssetTestHRepository
    {
        //private ILogger<AssetTestHRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public AssetTestHRepository()
        {
            //
        }

        public AssetTestHRepository(bool _IsSecondaryDatabase, string _SecondaryDatabaseId)
        {
            this.IsSecondaryDatabase = _IsSecondaryDatabase;
            this.SecondaryDatabaseId = _SecondaryDatabaseId;
        }

        public List<AssetTestH> GetAllBySequences(long sequence, long assetSequence, long typeSequence, HttpRequest request)
        {
            List<AssetTestH> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        AssetTestHDB assetTestHDB = new AssetTestHDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = assetTestHDB.getAllBySequences(sequence,assetSequence,typeSequence);
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
      
        
        public ResponseModel Insert(AssetTestH assetTestH, HttpRequest request)
        {   
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    AssetTestHDB assetTestHDB = new AssetTestHDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                   
                    long sequence = -1;
                    int userId = Utilities.GetUserIdFromRequest(request);
                    assetTestH.CreatedBy = userId;
                    assetTestH.DateCreated = DateTime.Now;
                    if (assetTestHDB.insert(out sequence, assetTestH))
                    {
                        assetTestH.Sequence = sequence;
                        returnValue.TheObject = assetTestH;
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

        public long GetAssetId(string assetSequence, HttpRequest request)
        {
            long returnValue = -1;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    AssetTestHDB assetTestHDB = new AssetTestHDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = assetTestHDB.getAssetId(assetSequence);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception Occured While Getting Asset ID :" + ex);
            }
            return returnValue;
        }

        public bool UpdateBySequence(AssetTestH assetTestH, HttpRequest request)
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
                        AssetTestHDB assetTestHDB = new AssetTestHDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Utilities.GetUserIdFromRequest(request);
                        assetTestH.LastAmendedBy = userId;
                        assetTestH.DateLastAmended = DateTime.Now;
                        returnValue = assetTestHDB.updateBySequence(assetTestH);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.InnerException + " " + ex.Message);
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
                        AssetTestHDB assetTestHDB = new AssetTestHDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = assetTestHDB.updateLocked(assetSequence);
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
