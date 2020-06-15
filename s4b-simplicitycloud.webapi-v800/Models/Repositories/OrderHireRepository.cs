using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using Microsoft.VisualBasic;
using SimplicityOnlineWebApi.DAL;
using Newtonsoft.Json;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing;


namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrderHireRepository: IOrderHireRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public OrderHireRepository()
        {
            
        }

        public OrderHire Insert(HttpRequest request, OrderHire obj)
        {
            OrderHire result = new OrderHire();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrderHireDB objDB = new OrderHireDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        long sequence = -1;

                        if (obj.POISequence==0) obj.POISequence = -1;
                        if (obj.AssetSequence==0 || obj.AssetSequence==null) obj.AssetSequence = -1;
                        obj.DamageType = -1;
                        obj.LastAmendedBy = -1;
                        obj.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
                        obj.DateCreated = DateTime.Now;
                        if (objDB.insertOrderHire(out sequence, obj))
                        {
                            result = obj;
                            result.Sequence = sequence;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occur in insertion:" + ex.Message);
                throw ex;
            }
            return result;
        }

        public ResponseModel UpdateBySequence(HttpRequest request, OrderHire obj)
        {
            ResponseModel result = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrderHireDB orderHireDB = new OrderHireDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                        obj.DateLastAmended = DateTime.Now;
                        if (!orderHireDB.updateBySequence(obj))
                        {
                            Utilities.WriteLog("Error occur while updating Order Hire");
                            result.Message = "Error occur while updating Order Hire";
                            result.IsSucessfull = false;
                        }
                        else
                        {
                            result.TheObject = obj;
                            result.IsSucessfull = true;
                            result.Message = "Order hire saved successfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occur in updating Order Hire:" + ex.Message);
                result.Message = "Error occur while updating Order Hire" + ex.Message;
            }
            return result;
        }
        public ResponseModel GetOrderHireBySequence(HttpRequest Request, int sequence)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrderHireDB orderHireDB = new OrderHireDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderHireDB.selectOrdersHireBySequence(sequence);
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public ResponseModel GetListOfOrderHire(HttpRequest Request, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate,int hireType)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        OrderHireDB orderHireDB = new OrderHireDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderHireDB.selectListOfOrdersHire(clientRequest,fromDate, toDate,hireType, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public ResponseModel GetAssetSelectedForDateRange(HttpRequest Request, long assetsequence,  DateTime? fromDate, DateTime? toDate)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrderHireDB orderHireDB = new OrderHireDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderHireDB.selectAssetSelectedForDateRange(assetsequence, fromDate, toDate);
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public List<OrderHire> GetOrdHireForReportByDate(HttpRequest Request, DateTime? fromDate, DateTime? toDate)
        {
            List<OrderHire> returnValue = new List<OrderHire>();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrderHireDB objDB = new OrderHireDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = objDB.selectOrdersHireForReportByDate(fromDate,toDate);
                    }
                }
            }catch(Exception ex)
            {
                Utilities.WriteLog("Error occured in getting Ord Hire For Report:" + ex.Message);
                throw ex;
            }
            return returnValue;
        }


    }
}
