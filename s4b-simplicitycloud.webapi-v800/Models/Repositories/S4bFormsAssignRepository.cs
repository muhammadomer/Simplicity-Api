using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System.Linq;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class S4bFormsAssignRepository : IS4bFormsAssignRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public S4bFormsAssignRepository()
        {
            
        }

        public ResponseModel GetAllAssignUser(long FormSeq,HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            List<S4bFormsAssign> s4bFormmAsign = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4bFormsAssignDB naturalForm = new S4bFormsAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        s4bFormmAsign = naturalForm.getAllAssignUser(FormSeq);
                        if (s4bFormmAsign != null)
                        {
                            returnValue.TheObject = s4bFormmAsign;
                            returnValue.Count = s4bFormmAsign.Count;
                            returnValue.IsSucessfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception occured in getting assigned user : " + ex.Message;
            }
            return returnValue;
        }

        public ResponseModel GetUnAssignUsers(long FormSeq, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            List<UserDetails> userDetails = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4bFormsAssignDB naturalForm = new S4bFormsAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        userDetails = naturalForm.getUnAssignUser(FormSeq);
                        if (userDetails.Count > 0)
                        {
                            returnValue.IsSucessfull = true;
                            returnValue.TheObject = userDetails;
                            returnValue.Count = userDetails.Count;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception occured in getting un assigned user : " + ex.Message;
            }
            return returnValue;
        }

        public bool UpdateFormUserAssignment(long formSeq, List<long> assignedUserIds, HttpRequest request)
        {
            bool result = false;
            int delRecordsCount = 0;
            int addRecordsCount = 0;
            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                int currentUserId = Convert.ToInt32(request.Headers["UserId"]);
                if (settings != null)
                {
                    S4bFormsAssignDB db = new S4bFormsAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<S4bFormsAssign> formAssigns = db.getAllAssignUser(formSeq);
                    if (formAssigns != null && formAssigns.Any())
                    {
                        foreach (var form in formAssigns)
                        {
                            if (assignedUserIds.Any(x => x == form.UserId) == false)
                            {
                                if(db.deleteFormAssign(form.Sequence ?? 0))
                                {
                                    delRecordsCount++;
                                }
                            }                            
                        }
                    }

                    foreach (var userId in assignedUserIds)
                    {
                        if(formAssigns == null || formAssigns.Any(x => x.UserId == userId) == false)
                        {
                            if(db.addFormAssign(formSeq, userId, currentUserId, DateTime.Now))
                            {
                                addRecordsCount++;
                            }
                        }
                    }

                    if (delRecordsCount > 0 || addRecordsCount > 0)
                    {
                        RefS4bFormsDB refS4bFormsDB = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        refS4bFormsDB.updateLastAmendedDateByformSequence(formSeq, DateTime.Now);                        
                    }
                }
            }

            result = true;
            

            return result;
        }
    }
}
