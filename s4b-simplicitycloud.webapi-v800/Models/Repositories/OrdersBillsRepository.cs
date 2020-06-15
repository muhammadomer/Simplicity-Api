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
    public class OrdersBillsRepository: IOrdersBillsRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public OrdersBillsRepository()
        {
            
        }

        public OrdersBills insert(HttpRequest request, OrdersBills obj)
        {
            OrdersBills result = new OrdersBills();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        long sequence = -1;
                        if (orderBillsDB.insertOrdersBills(out sequence, obj))
                        {
                            result = obj;
                            result.Sequence = sequence;
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
        public ResponseModel CreateInvoiceRequest(HttpRequest request, OrdersBills obj)
        {
            OrdersBills result = new OrdersBills();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        //----check Order does not has pending request otherwise it can insert another Request for invoice
                        bool isValidForAnotherRequest = orderBillsDB.getOrderValidForRequest(obj.JobSequence ?? 0);
                        if (isValidForAnotherRequest == true)
                        {
                            long sequence = -1;
                            obj.FlgParentOverride = false;
                            obj.InvoiceNo = "Not Set";
                            obj.AmountInitial = 0;
                            obj.AmountDiscount = 0;
                            obj.PcentRetention = 0;
                            obj.AmountRetention = 0;
                            obj.AmountSubTotal = 0;
                            obj.AmountVat = 0;
                            obj.AmountCis = 0;
                            obj.AmountTotal = 0;
                            obj.FlgJobDateStart = false;
                            obj.FlgJobDateFinish = false;
                            obj.MaillingAddress = "Not Set";
                            obj.FlgRequestMade = true;
                            obj.RequestMadeDate = DateTime.Now;
                            obj.FlgSetToProforma = false;
                            obj.SageId = -1;
                            obj.FlgSetToInvoice = false;
                            obj.FlgHasAVatInv = false;
                            obj.FlgIsVatInv = false;
                            obj.JoinBillSequence = -1;
                            obj.RciId = -1;
                            obj.FlgArchive = false;
                            obj.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
                            obj.DateCreated = DateTime.Now;
                            if (orderBillsDB.insertOrdersBills(out sequence, obj))
                            {
                                result = obj;
                                result.Sequence = sequence;
                                returnValue.TheObject = result;
                                returnValue.IsSucessfull = true;
                            }
                        }
                        else
                        {
                            returnValue.Message =  " Order already has pending request. you can not create another request" ;
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

        public ResponseModel CreateRequestForPayment(HttpRequest request, OrdersBills obj)
        {
            OrdersBills result = new OrdersBills();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        obj.InvoiceNo = getNewInvoiceNumber(request);
                        obj.InvoiceDate = DateTime.Now;
                        obj.FlgSetToProforma = true;
                        obj.SetToProformaDate = DateTime.Now;
                        obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                        obj.DateLastAmended = DateTime.Now;
                        if (obj.Sequence > 0) {
                            if (orderBillsDB.updateBySequence(obj))
                            {
                                result = obj;
                                //----Save Bill items
                                if (obj != null && obj.OrderBillItems != null)
                                {
                                    int userId = Utilities.GetUserIdFromRequest(request);
                                    OrdersBillsItemsDB orderBillItemsDB = new OrdersBillsItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                    List<OrderBillItems> itemReturned = new List<OrderBillItems>();
                                    foreach (OrderBillItems item in obj.OrderBillItems)
                                    {
                                        if (item.Sequence <= 0) //case:insert
                                        {
                                            item.BillSequence = obj.Sequence;
                                            OrderBillItems itemNew = orderBillItemsDB.InsertOrderBillItem(item);
                                            item.Sequence = itemNew.Sequence;
                                            itemReturned.Add(item);
                                        }
                                        //---Update Amount_balance in Order_Items Table
                                        OrderItemsDB orderItemsDB = new OrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                        OrderItems orderItem = orderItemsDB.selectOrderItemsBySequence(item.ItemSequence ?? 0);
                                        orderItem.AmountBalance = orderItem.AmountBalance + item.AmountPayment;
                                        orderItemsDB.UpdateOrderItem(orderItem);
                                    }
                                    //---Update flg_bill_proforma in Orders Table
                                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                    bool isSave = orderDB.updateFlgBillProformaByJobSequence(obj.JobSequence ?? 0, true, obj.LastAmendedBy, DateTime.Now);
                                    //---------
                                    result.OrderBillItems = itemReturned;
                                    returnValue.TheObject = result;
                                    returnValue.IsSucessfull = true;
                                }
                                else
                                {
                                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + obj.ToString();
                                }
                            }
                            else
                            {
                                returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + obj.ToString();
                            }
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

        public ResponseModel UpdateOrdersBillsBySequence(HttpRequest request, OrdersBills obj)
        {
            OrdersBills result = new OrdersBills();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                        obj.DateLastAmended = DateTime.Now;
                        if (orderBillsDB.updateBySequence(obj))
                        {
                            result = obj;
                            //----Save Bill items
                            if (obj != null && obj.OrderBillItems != null)
                            {
                                int userId = Utilities.GetUserIdFromRequest(request);
                                OrdersBillsItemsDB orderBillItemsDB = new OrdersBillsItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                List<OrderBillItems> itemReturned = new List<OrderBillItems>();
                                foreach (OrderBillItems item in obj.OrderBillItems)
                                {
                                    if (item.Sequence <= 0) //case:insert
                                    {
                                        item.BillSequence = obj.Sequence;
                                        OrderBillItems itemNew = orderBillItemsDB.InsertOrderBillItem(item);
                                        item.Sequence = itemNew.Sequence;
                                        itemReturned.Add(item);
                                        //---Update Amount_balance in Order_Items Table
                                        OrderItemsDB orderItemsDB = new OrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                        OrderItems orderItem = orderItemsDB.selectOrderItemsBySequence(item.ItemSequence ?? 0);
                                        orderItem.AmountBalance = orderItem.AmountBalance + item.AmountPayment;
                                        orderItemsDB.UpdateOrderItem(orderItem);
                                    }
                                    else if (item.Sequence > 0) //case:update
                                    {
                                        //---Get Old value of AmountPayment of updated row before updation for updating amount balance
                                        OrderBillItems oldItem= orderBillItemsDB.selectOrderBillItemsBySequence(item.Sequence ?? 0);
                                        OrderBillItems itemUpdated = orderBillItemsDB.UpdateOrderBillItem(item);
                                        itemReturned.Add(item);
                                        //---Update Amount_balance in Order_Items Table
                                        OrderItemsDB orderItemsDB = new OrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                        OrderItems orderItem = orderItemsDB.selectOrderItemsBySequence(item.ItemSequence ?? 0);
                                        orderItem.AmountBalance = orderItem.AmountBalance -oldItem.AmountPayment + item.AmountPayment;
                                        orderItemsDB.UpdateOrderItem(orderItem);
                                    }
                                    
                                }
                               
                                //---------
                                result.OrderBillItems = itemReturned;
                                returnValue.TheObject = result;
                                returnValue.IsSucessfull = true;
                            }
                            else
                            {
                                returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + obj.ToString();
                            }
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

        public ResponseModel ConvertToInvoice(HttpRequest request, OrdersBills obj)
        {
            OrdersBills result = new OrdersBills();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        //---Get Job Date start and Finish From Order
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Orders order = orderDB.getOrderByJobRef(obj.JobRef);
                        //
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (obj.FlgSetToProforma == true) { 
                            obj.FlgSetToInvoice = true;
                            obj.FlgJobDateStart = true;
                            obj.JobDateStart =order.JobDateStart;
                            obj.FlgJobDateFinish = order.FlgJobDateFinish;
                            obj.JobDateFinish = order.JobDateFinish;
                            obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                            obj.DateLastAmended = DateTime.Now;
                            if (orderBillsDB.updateBySequence(obj))
                            {
                                //--- Insert Record in un_inovice_entries_new
                                InvoiceEntriesNew objInvoice = new InvoiceEntriesNew();
                                objInvoice.BillSequence = obj.Sequence;
                                objInvoice.EntryDate = Convert.ToDateTime( obj.InvoiceDate!=null ? obj.InvoiceDate : DateTime.Now);
                                objInvoice.EntryType = "SI";
                                objInvoice.JobSequence = obj.JobSequence;
                                objInvoice.TransType = "B";
                                objInvoice.InvoicenoOrItemref = obj.InvoiceNo;
                                objInvoice.ContactId = obj.EntityJoinId;
                                objInvoice.EntryAmtOrMat = obj.AmountInitial - obj.AmountRetention;
                                objInvoice.EntryAmtDiscounted = obj.AmountDiscount;
                                objInvoice.EntryAmtSubtotal = obj.AmountSubTotal;
                                objInvoice.FlgAddVat = (obj.AmountVat > 0);
                                objInvoice.EntryAmtVat = obj.AmountVat;
                                objInvoice.EntryAmtTotalMat = obj.AmountTotal;
                                objInvoice.EntryAmtTotal = obj.AmountTotal;
                                objInvoice.CreatedBy = Convert.ToInt32(request.Headers["userId"]);
                                objInvoice.DateCreated = DateTime.Now;
                                InvoiceEntriesNewDB invoiceNewDB = new InvoiceEntriesNewDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                invoiceNewDB.insertInvoiceEntriesNew(objInvoice);
                                //----
                                result = obj;
                                returnValue.TheObject = result;
                                returnValue.IsSucessfull = true;
                            }
                        }
                        else
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " Bill is not valid to convert to invoice." + obj.ToString();
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

        public ResponseModel BatchConvertToInvoice(HttpRequest request, List<OrdersBills> lstOrderBills)
        {
            OrdersBills result = new OrdersBills();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        foreach (OrdersBills obj in lstOrderBills) {
                            if (obj.FlgSetToProforma == true)
                            {
                                //---Get Job Date start and Finish From Order
                                OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                Orders order = orderDB.getOrderByJobRef(obj.JobRef);
                                //
                                obj.FlgSetToInvoice = true;
                                obj.SetToInvoiceDate = DateTime.Now; 
                                obj.FlgJobDateStart = true;
                                obj.JobDateStart = order.JobDateStart;
                                obj.FlgJobDateFinish = order.FlgJobDateFinish;
                                obj.JobDateFinish = order.JobDateFinish;
                                obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                                obj.DateLastAmended = DateTime.Now;
                                if (orderBillsDB.updateBySequence(obj))
                                {
                                    //--- Insert Record in un_inovice_entries_new
                                    InvoiceEntriesNew objInvoice = new InvoiceEntriesNew();
                                    objInvoice.BillSequence = obj.Sequence;
                                    objInvoice.EntryDate = Convert.ToDateTime(obj.InvoiceDate != null ? obj.InvoiceDate : DateTime.Now);
                                    objInvoice.EntryType = "SI";
                                    objInvoice.JobSequence = obj.JobSequence;
                                    objInvoice.TransType = "B";
                                    objInvoice.InvoicenoOrItemref = obj.InvoiceNo;
                                    objInvoice.ContactId = obj.EntityJoinId;
                                    objInvoice.EntryAmtOrMat = obj.AmountInitial - obj.AmountRetention;
                                    objInvoice.EntryAmtDiscounted = obj.AmountDiscount;
                                    objInvoice.EntryAmtSubtotal = obj.AmountSubTotal;
                                    objInvoice.FlgAddVat = (obj.AmountVat > 0);
                                    objInvoice.EntryAmtVat = obj.AmountVat;
                                    objInvoice.EntryAmtTotalMat = obj.AmountTotal;
                                    objInvoice.EntryAmtTotal = obj.AmountTotal;
                                    objInvoice.CreatedBy = Convert.ToInt32(request.Headers["userId"]);
                                    objInvoice.DateCreated = DateTime.Now;
                                    InvoiceEntriesNewDB invoiceNewDB = new InvoiceEntriesNewDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                    invoiceNewDB.insertInvoiceEntriesNew(objInvoice);
                                    //----
                                    result = obj;
                                    returnValue.TheObject = result;
                                    returnValue.IsSucessfull = true;
                                }
                            }
                            else
                            {
                                returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " Bill is not valid to convert to invoice." + obj.ToString();
                            }
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

        public ResponseModel SaveInvoice(HttpRequest request, OrdersBills obj)
        {
            OrdersBills result = new OrdersBills();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (obj.FlgSetToInvoice == true)
                        {  
                            obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                            obj.DateLastAmended = DateTime.Now;
                            if (orderBillsDB.updateBySequence(obj))
                            {
                                //----
                                result = obj;
                                returnValue.TheObject = result;
                                returnValue.IsSucessfull = true;
                            }
                        }
                        else
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " Bill is not valid to convert to invoice." + obj.ToString();
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

        public ResponseModel GetOrderBillItemsForInvoicingByJobSequence( HttpRequest request, long jobSequence)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {   
                        OrdersBillsItemsDB orderBillsItemsDB = new OrdersBillsItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderBillsItemsDB.selectOrdersItemsForInvoicingByJobSequence(jobSequence);

                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = orderBillsItemsDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting Order items For Invoicing. " + ex.Message + " " + ex.InnerException;
                
            }
            return returnValue;
        }

        public ResponseModel GetOrderBillsForEditingBySequence(HttpRequest request, long billSequence,long jobSequence)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderBillsDB.selectOrderBillForEditingBySequence(billSequence,jobSequence);
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = orderBillsDB.ErrorMessage;
                        }
                        else
                        {  
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting Order items For Invoicing. " + ex.Message + " " + ex.InnerException;

            }
            return returnValue;
        }

        public OrdersBills GetOrderBillsBySequence(HttpRequest Request, long sequence)
        {
            OrdersBills returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderBillsDB.selectOrdersBillsBySequence(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        public ResponseModel GetOrdersBillByJobSequenceAndType(HttpRequest Request, long jobSequence,string type)
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
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderBillsDB.selectOrdersBillsByJobSequenceAndType(jobSequence,type);
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

        public ResponseModel GetOrdersBillInvoiceBySequence(HttpRequest Request, long billSequence)
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
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderBillsDB.selectOrdersBillInvoiceByBillSequence(billSequence);
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

        public ResponseModel GetApplicationForPaymentsAndInvoicesByJobSequence(HttpRequest Request, long jobSequence)
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
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderBillsDB.selectOrdersApplicationForPaymentAndInvoices(jobSequence);
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
       
        public List<OrdersBills> GetOrdersBillByJobSequence(HttpRequest Request, long jobSequence)
        {
            List<OrdersBills> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderBillsDB.selectAllOrdersBillsByJobSequence(jobSequence);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        public ResponseModel GetSaleInvoiceBySequence(HttpRequest Request,long billSequence)
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
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderBillsDB.selectSaleInvoiceBySequence(billSequence);
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

        public ResponseModel GetListOfAppForPayments(HttpRequest Request, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate)
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
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderBillsDB.selectListOfAppForPayments(clientRequest, fromDate, toDate, out count, true);
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

        public ResponseModel GetListOfSaleInvoices(HttpRequest Request, DateTime? fromDate, DateTime? toDate)
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
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderBillsDB.selectListOfSaleInvoices(fromDate,toDate);
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

        public ResponseModel DownloadPDF(HttpRequest Request, string htmlString)
        {
            ResponseModel returnValue = new ResponseModel();
            try {
                ProjectSettings settings = Configs.settings[Request.Headers["ProjectId"]];
                //htmlString = (string)JsonConvert.DeserializeObject(htmlString);
                //---Using PdfSharp
                PdfSharp.Pdf.PdfDocument pdf = PdfGenerator.GeneratePdf(htmlString, PageSize.A4, 20, null, null, OnImageLoadPdfSharp);
                
                pdf.Save(@"C:\Temp\testPDF.pdf");
                returnValue.IsSucessfull = true;
                
                ////---using Iron Pdf. it gives error
                //IronPdf.Installation.TempFolderPath = "C:\\Temp";
                //HtmlToPdf htmlToPdf = new IronPdf.HtmlToPdf();
                //IronPdf.PdfDocument PDF = htmlToPdf.RenderHtmlAsPdf(htmlString);
                //PDF.SaveAs(@"testPDF.pdf");

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static void OnImageLoadPdfSharp(object sender, HtmlImageLoadEventArgs e)
        {
            var imgObj = Image.FromFile(@"C:\Temp\img.png");

            e.Callback(XImage.FromFile(@"C:\Temp\img.png"));
        }

        public List<OrdersBills> getOrderBillsByJobSequence(HttpRequest Request, long jobSequence)
        {
            List<OrdersBills> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderBillsDB.selectAllOrdersBillsByJobSequence(jobSequence);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }
        public OrdersBills update(HttpRequest request, OrdersBills obj)
        {
            OrdersBills result = new OrdersBills();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if(!orderBillsDB.updateBySequence(obj))
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

        public string getNewInvoiceNumber(HttpRequest request)
        {
            string returnValue = SimplicityConstants.NotSet;
            try
            {
                int newInvoiceNo_2ndPart = 1;
                ApplicationSettingsRepository applicationSettingsRepository = new ApplicationSettingsRepository();
                ApplicationSettings applicationSettings = applicationSettingsRepository.GetOrdersBillsLastInvoiceNo(request);
                if(applicationSettings!=null && Information.IsNumeric(applicationSettings.Setting1))
                {
                    newInvoiceNo_2ndPart = Int32.Parse(applicationSettings.Setting1) + 1;
                }
                applicationSettings = applicationSettingsRepository.GetOrdersBillsInvoiceNoPrefix(request);
                string invoiceNoPrefix = "";
                if (applicationSettings != null)
                {
                    invoiceNoPrefix = applicationSettings.Setting1;
                }
                if(!string.IsNullOrEmpty(invoiceNoPrefix))
                {
                    string newInvoiceNo = invoiceNoPrefix + newInvoiceNo_2ndPart.ToString("00000");
                    InvoiceEntriesNewRepository invoiceEntriesNewRepository = new InvoiceEntriesNewRepository();
                    List<InvoiceEntriesNew> invoiceList = invoiceEntriesNewRepository.getByClientInvoiceNo(request, newInvoiceNo);
                    if(invoiceList==null)
                    {
                        if(applicationSettingsRepository.UpdateOrdersBillsLastInvoiceNo(request, newInvoiceNo_2ndPart.ToString()))
                        {
                            returnValue = newInvoiceNo;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                //TODO: Write exception code
            }
            return returnValue;
        }

        public bool CreateApplicationForPayment(HttpRequest request, long jobSequence, DateTime? timeStamp)
        {
            bool returnValue = false;
            try
            {
                int userId = Convert.ToInt32(request.Headers["UserId"]);
                long billClientId = -1;
                long billSequence = -1;
                bool flgSetToJt = false;
                bool flgSetToProforma = false;
                OrdersRepository orderRepository = new OrdersRepository(null);
                Orders order = orderRepository.GetOrderDetailsBySequence(jobSequence, request);
                flgSetToJt = order.FlgJT;
                billClientId = long.Parse(order.JobClientId.ToString());
                if (!flgSetToJt)
                {
                    if (orderRepository.UpdateFlgSetToJTAndDateSetToJTByJobSequence(request, jobSequence, true, timeStamp))
                    {
                        flgSetToJt = true;
                    }
                    else
                    {
                        //Report Error
                    }
                }
                if (!order.FlgBillProforma)
                {
                    if (!orderRepository.updateFlgBillProformaByJobSequence(request, jobSequence, true, timeStamp))
                    {
                        //TODO
                        //Utilities.ReportError("Unable to Update Order Flg Bill Proforma." + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                    }
                }
                if (flgSetToJt)
                {
                    bool orderBillsEntryFound = false;
                    string invoiceNo = "";
                    OrdersBillsRepository ordersBillsRepository = new OrdersBillsRepository();
                    OrdersBills ordersBills = new OrdersBills();
                    List<OrdersBills> ordersBillsList = ordersBillsRepository.GetOrdersBillByJobSequence(request, jobSequence);
                    if (ordersBillsList != null && ordersBillsList.Count > 0)
                    {
                        orderBillsEntryFound = true;
                        long prevBillSequence = -1;
                        foreach (OrdersBills item in ordersBillsList)
                        {
                            if (item.Sequence > prevBillSequence)
                            {
                                prevBillSequence = item.Sequence ?? 0;
                                ordersBills = item;
                            }
                        }
                    }
                    flgSetToProforma = ordersBills.FlgSetToProforma;
                    billSequence = ordersBills.Sequence ?? 0;
                    if (!orderBillsEntryFound)
                    {
                        // Create a new order bills entry
                        invoiceNo = getNewInvoiceNumber(request);
                        if (invoiceNo != SimplicityConstants.NotSet)
                        {
                            OrdersBills ordersBillsNew = new OrdersBills();
                            ordersBillsNew.JobSequence = jobSequence;
                            ordersBillsNew.BillRef = SimplicityConstants.NotSet;
                            ordersBillsNew.ClientId = billClientId;
                            ordersBillsNew.EntityJoinId = billClientId;
                            ordersBillsNew.FlgParentOverride = false;
                            ordersBillsNew.InvoiceNo = invoiceNo;
                            ordersBillsNew.InvoiceDate = DateTime.Now;
                            ordersBillsNew.FlgRequestMade = true;
                            ordersBillsNew.RequestMadeDate = DateTime.Now;
                            ordersBillsNew.FlgSetToProforma = true;
                            ordersBillsNew.SetToProformaDate = DateTime.Now;
                            ordersBillsNew.SageId = 1;
                            ordersBillsNew.CreatedBy = userId;
                            ordersBillsNew.DateCreated = DateTime.Now;
                            ordersBillsNew.LastAmendedBy = userId;
                            ordersBillsNew.DateLastAmended = DateTime.Now;
                            ordersBillsNew.AmountInitial = ordersBillsNew.AmountDiscount = ordersBillsNew.AmountRetention =
                            ordersBillsNew.PcentRetention = ordersBillsNew.AmountSubTotal = ordersBillsNew.AmountVat =
                            ordersBillsNew.AmountCis = ordersBillsNew.AmountTotal = 0;
                            ResponseModel response = ordersBillsRepository.CreateInvoiceRequest(request, ordersBillsNew);
                            OrdersBills ordersBillsUpdated = (OrdersBills) response.TheObject;
                            if (ordersBillsUpdated != null)
                            {
                                orderBillsEntryFound = true;
                                flgSetToProforma = true;
                            }
                            else
                            {
                                //Utilities.ReportError("Unable to Create Order Bills Entry for " + system.Name + ". " + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                            }
                        }
                        else
                        {
                            //Utilities.ReportError("Unable to Generate Invoice No for " + system.Name + ". " + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                        }
                    }
                    if (orderBillsEntryFound && !flgSetToProforma)
                    {
                        if (!flgSetToProforma)
                        {
                            invoiceNo = getNewInvoiceNumber(request);
                            if (invoiceNo != SimplicityConstants.NotSet)
                            {
                                if (!ordersBillsRepository.updateInvoiceNoAndSetToJTDateBySequence(request, billSequence, invoiceNo, DateTime.Now, userId, DateTime.Now))
                                {
                                    //Utilities.ReportError("Unable to convert Order Bills  Entry to App For Payment." + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                                }
                            }
                            else
                            {
                                //Utilities.ReportError("Unable to Generate Invoice No for " + system.Name + ". " + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        //--- this method is copied from v6 called by FormSubmission :ProcessFiveEnvJobTicket
        public bool CreateApplicationForPayment(HttpRequest request, long jobSequence, string customerAddress, DateTime? timeStamp)
        {
            bool returnValue = false;
            try
            {
                int userId = Convert.ToInt32(request.Headers["UserId"]);
                long billClientId = -1;
                long billSequence = -1;
                bool flgSetToJt = false;
                bool flgSetToProforma = false;
                OrdersRepository orderRepository = new OrdersRepository(null);
                Orders order = orderRepository.GetOrderDetailsBySequence(jobSequence, request);
                flgSetToJt = order.FlgJT;
                billClientId = long.Parse(order.JobClientId.ToString());
                if (!flgSetToJt)
                {
                    if (orderRepository.UpdateFlgSetToJTAndDateSetToJTByJobSequence(request, jobSequence, true, timeStamp))
                    {
                        flgSetToJt = true;
                    }
                    else
                    {
                        //Report Error
                    }
                }
                if (!order.FlgBillProforma)
                {
                    if (!orderRepository.updateFlgBillProformaByJobSequence(request, jobSequence, true, timeStamp))
                    {
                        //TODO
                        //Utilities.ReportError("Unable to Update Order Flg Bill Proforma." + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                    }
                }
                if (flgSetToJt)
                {
                    bool orderBillsEntryFound = false;
                    string invoiceNo = "";
                    OrdersBillsRepository ordersBillsRepository = new OrdersBillsRepository();
                    OrdersBills ordersBills = new OrdersBills();
                    List<OrdersBills> ordersBillsList = ordersBillsRepository.getOrderBillsByJobSequence(request, jobSequence);
                    if (ordersBillsList != null && ordersBillsList.Count > 0)
                    {
                        orderBillsEntryFound = true;
                        long prevBillSequence = -1;
                        foreach (OrdersBills item in ordersBillsList)
                        {
                            if (item.Sequence > prevBillSequence)
                            {
                                prevBillSequence = item.Sequence ?? 0;
                                ordersBills = item;
                            }
                        }
                    }
                    flgSetToProforma = ordersBills.FlgSetToProforma;
                    billSequence = ordersBills.Sequence ?? 0;
                    if (!orderBillsEntryFound)
                    {
                        // Create a new order bills entry
                        invoiceNo = getNewInvoiceNumber(request);
                        if (invoiceNo != SimplicityConstants.NotSet)
                        {
                            OrdersBills ordersBillsNew = new OrdersBills();
                            ordersBillsNew.JobSequence = jobSequence;
                            ordersBillsNew.BillRef = SimplicityConstants.NotSet;
                            ordersBillsNew.ClientId = billClientId;
                            ordersBillsNew.EntityJoinId = billClientId;
                            ordersBillsNew.FlgParentOverride = false;
                            ordersBillsNew.InvoiceNo = invoiceNo;
                            ordersBillsNew.InvoiceDate = DateTime.Now;
                            ordersBillsNew.JobDate = order.JobDate;
                            ordersBillsNew.FlgJobDateStart = true;
                            ordersBillsNew.JobDateStart = order.JobDate;
                            ordersBillsNew.FlgRequestMade = true;
                            ordersBillsNew.RequestMadeDate = DateTime.Now;
                            ordersBillsNew.FlgSetToProforma = true;
                            ordersBillsNew.SetToProformaDate = DateTime.Now;
                            ordersBillsNew.SageId = 1;
                            ordersBillsNew.CreatedBy = userId;
                            ordersBillsNew.DateCreated = DateTime.Now;
                            ordersBillsNew.LastAmendedBy = userId;
                            ordersBillsNew.DateLastAmended = DateTime.Now;
                            ordersBillsNew.AmountInitial = ordersBillsNew.AmountDiscount = ordersBillsNew.AmountRetention =
                            ordersBillsNew.PcentRetention = ordersBillsNew.AmountSubTotal = ordersBillsNew.AmountVat =
                            ordersBillsNew.AmountCis = ordersBillsNew.AmountTotal = 0;
                            ordersBillsNew.MaillingAddress = customerAddress;
                            OrdersBills ordersBillsUpdated = ordersBillsRepository.insert(request, ordersBillsNew);
                            if (ordersBillsUpdated != null)
                            {
                                orderBillsEntryFound = true;
                                flgSetToProforma = true;
                            }
                            else
                            {
                                //Utilities.ReportError("Unable to Create Order Bills Entry for " + system.Name + ". " + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                            }
                        }
                        else
                        {
                            //Utilities.ReportError("Unable to Generate Invoice No for " + system.Name + ". " + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                        }
                    }
                    if (orderBillsEntryFound && !flgSetToProforma)
                    {
                        if (!flgSetToProforma)
                        {
                            invoiceNo = getNewInvoiceNumber(request);
                            if (invoiceNo != SimplicityConstants.NotSet)
                            {
                                if (!ordersBillsRepository.updateInvoiceNoAndSetToJTDateBySequence(request, billSequence, invoiceNo, DateTime.Now, userId, DateTime.Now))
                                {
                                    //Utilities.ReportError("Unable to convert Order Bills  Entry to App For Payment." + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                                }
                            }
                            else
                            {
                                //Utilities.ReportError("Unable to Generate Invoice No for " + system.Name + ". " + Utilities.ErrorMessage, METHOD_NAME, true, system, edwFormInstance);
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public bool updateInvoiceNoAndSetToJTDateBySequence(HttpRequest request, long sequence, string invoiceNo, 
                                                            DateTime? setToJTDate, int lastAmendedBy, DateTime? lastAmendedDate)
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
                        OrdersBillsDB orderBillsDB = new OrdersBillsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (!orderBillsDB.updateInvoiceNoAndSetToJTDateBySequence(sequence, invoiceNo, setToJTDate, lastAmendedBy, lastAmendedDate))
                        {
                            //TODO: Log Error
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}
