using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrdersNotesRepository : IOrdersNotesRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public OrdersNotesRepository()
        {
            
        }

        public OrdersNotes Insert(HttpRequest request, OrdersNotes obj)
        {
            const string METHOD_NAME = "OrdersNotesRepository.Insert()";
            OrdersNotes returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersNotesDB orderNotesDB = new OrdersNotesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        long sequence = -1;
                        if(orderNotesDB.insertOrdersNotes(out sequence, obj))
                        {
                            returnValue = obj;
                            returnValue.Sequence = sequence;
                        }
                        else
                        {
                            Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Add Order Notes. Reason: " + orderNotesDB.ErrorMessage, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Adding Order Notes. ", ex);
            }
            return returnValue;
        }     

        public List<OrdersNotes> GetOrderNotesBySequence(HttpRequest Request, long sequence)
        {
            const string METHOD_NAME = "OrdersNotesRepository.GetOrderNotesBySequence()";
            List<OrdersNotes> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersNotesDB orderNotesDB = new OrdersNotesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderNotesDB.selectBySequence(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Order Notes by Sequence. ", ex);
            }
            return returnValue;
        }

        public OrdersNotes update(HttpRequest request, OrdersNotes obj)
        {
            OrdersNotes result = new OrdersNotes();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersNotesDB orderNotesDB = new OrdersNotesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if(!orderNotesDB.updateBySequence(obj))
                        {
                            //TODO: Log Error
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }
}
