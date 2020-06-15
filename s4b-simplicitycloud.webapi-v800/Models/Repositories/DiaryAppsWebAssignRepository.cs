using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class DiaryAppsWebAssignRepository : IDiaryAppsWebAssignRepository
    {
        private readonly IDiaryAppsWebAssignRepository _AppsWebAssign;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public object Response { get; private set; }

        public DiaryAppsWebAssignRepository()
        {
        }

        public List<DiaryAppsWebAssign> GetAllWebAssignApp(HttpRequest Request, HttpResponse Response)
        {
            List<DiaryAppsWebAssign> returnObj = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsWebAssignDB WebAssignDB = new DiaryAppsWebAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnObj = WebAssignDB.GetAllWebAssignApp();
                        if (returnObj == null)
                        {
                            Response.Headers["message"] = "No Record Found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Headers["message"] = "Exception occured while getting Record. " + ex.Message;
            }
            return returnObj;
        }
        public DiaryAppsWebAssign IsWebAssignAppExists(DiaryAppsWebAssign webAssign, HttpRequest Request)
        {
            DiaryAppsWebAssign returnObj = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsWebAssignDB WebAssignDB = new DiaryAppsWebAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnObj = WebAssignDB.IsWebAssignAppExists(webAssign);
                        //if (!returnObj)
                        //{
                        //    Response.Headers["message"] = "No Record Found.";
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Record. " + ex.Message;
            }
            return returnObj;
        }
        public DiaryAppsWebAssign AddWebAssignObject(DiaryAppsWebAssign WebAssignObj, HttpRequest request)
        {
            DiaryAppsWebAssign Object = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsWebAssignDB WebAssignDB = new DiaryAppsWebAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        WebAssignObj.CreatedBy = WebAssignObj.LastAmendedBy = Convert.ToInt32(request.Headers["WebId"]);
                        WebAssignObj.EntityId = Convert.ToInt32(request.Headers["EntityId"]);
                        WebAssignObj.DateCreated = WebAssignObj.DateLastAmended = DateTime.Now;
                        //var webAssign = WebAssignDB.IsWebAssignAppExists(WebAssignObj, WebAssignObj.LastAmendedBy);
                        if (WebAssignObj.SequenceId == 0)
                        {
                            Object = WebAssignDB.CreateWebAssignApp(WebAssignObj);
                        }
                        else
                        {
                            //WebAssignObj.SequenceId = webAssign.SequenceId;
                            Object = WebAssignDB.UpdateWebAssignApp(WebAssignObj);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Object = null;
            }
            return Object;
        }
        public DiaryAppsWebAssign UpdateWebAssignObject(DiaryAppsWebAssign WebAssignObj, HttpRequest request)
        {
            DiaryAppsWebAssign Obj = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        WebAssignObj.LastAmendedBy = Convert.ToInt32(request.Headers["WebId"]);
                        WebAssignObj.EntityId = Convert.ToInt32(request.Headers["EntityId"]);
                        WebAssignObj.DateLastAmended = DateTime.Now;
                        DiaryAppsWebAssignDB WebAssignDB = new DiaryAppsWebAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Obj = WebAssignDB.UpdateWebAssignApp(WebAssignObj);
                    }
                }
            }
            catch (Exception ex)
            {
                Obj = null;
            }
            return Obj;
        }
        public DiaryAppsWebAssign UpdateWebAssignByCriteria(DiaryAppsWebAssign WebAssignObj, HttpRequest request)
        {
            DiaryAppsWebAssign Obj = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        WebAssignObj.LastAmendedBy = Convert.ToInt32(request.Headers["WebId"]);
                        WebAssignObj.EntityId = Convert.ToInt32(request.Headers["EntityId"]);
                        WebAssignObj.DateLastAmended = DateTime.Now;
                        DiaryAppsWebAssignDB WebAssignDB = new DiaryAppsWebAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        //var webAssign = WebAssignDB.IsWebAssignAppExists(WebAssignObj);
                        if (WebAssignObj.SequenceId == 0)
                        {
                            WebAssignObj.DateAppCompleted = DateTime.MinValue;
                            WebAssignObj.DateAppStart=WebAssignObj.DateCreated = DateTime.Now;
                            WebAssignObj.CreatedBy = Convert.ToInt32(request.Headers["WebId"]);
                            if (((DateTime)WebAssignObj.DateAppCompleted).Date == new DateTime())
                            {
                                WebAssignObj.DateAppCompleted = DateTime.Now;
                            }
                            Obj = WebAssignDB.CreateWebAssignApp(WebAssignObj);
                        }
                        else
                        {
                            //WebAssignObj.SequenceId = webAssign.SequenceId;
                            Obj = WebAssignDB.UpdateWebAssignByCriteria(WebAssignObj);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Obj = null;
            }
            return Obj;
        }

        public List<DiaryAppsWebAssign> GetThirdPartyApp(DiaryAppsWebAssign webAssign, HttpRequest Request)
        {
            List<DiaryAppsWebAssign> returnObj = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppsWebAssignDB WebAssignDB = new DiaryAppsWebAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<DiaryAppsWebAssign> JobsDetails = WebAssignDB.GetThirdPartyApp(webAssign);
                        if (JobsDetails != null && JobsDetails.Count > 0)
                        {
                            returnObj = new List<DiaryAppsWebAssign>();
                            foreach (var Item in JobsDetails)
                            {
                                Item.Forms = settings.Forms;
                                returnObj.Add(Item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnObj;
        }
    }
}
