using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrdersTagsRepository : IOrdersTagsRepository
    {
        
        private readonly IPassthroughRepository _passthroughRepository;
        private ILogger<OrdersTagsRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public OrdersTagsRepository(ILogger<OrdersTagsRepository> logger, IPassthroughRepository passthroughRepository)
        {
            
            _passthroughRepository = passthroughRepository;
            
        }

        public Cld_Ord_Labels FindCreateTagByTagNoAndJobSequence(long jobSequence, string tagNo, HttpRequest Request, HttpResponse Response)
        {
            Cld_Ord_Labels returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[Request.Headers["ProjectId"]];
                if (settings != null)
                {
                    Cld_Ord_Labels_DB cldOrdLabelsDB = new Cld_Ord_Labels_DB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    Cld_Ord_Labels orderTags = cldOrdLabelsDB.selectCld_Ord_LabelsByJobSequenceAndTag(jobSequence, tagNo);
                    if (orderTags == null)
                    {
                        long sequence = -1;
                        if (cldOrdLabelsDB.insertCld_Ord_Labels(out sequence, jobSequence, tagNo, Convert.ToInt32(Request.Headers["UserId"]), DateTime.Now))
                        {
                            orderTags = new Cld_Ord_Labels();
                            orderTags.Sequence = sequence;
                            orderTags.JobSequence = jobSequence;
                            orderTags.TagNo = tagNo;
                        }
                    }
                    else
                    {
                        orderTags.OI_FireProtection_I_Images = new OrdersTagsImagesRepository().GetOrderTagImagesByJoinSequence(orderTags.Sequence ?? 0, Request);
                    }
                    returnValue = orderTags;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public Cld_Ord_Labels FindTagByOrderSequenceAndTagNo(long jobSequence, string tagNo, HttpRequest Request, HttpResponse Response)
        {
            Cld_Ord_Labels returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[Request.Headers["ProjectId"]];
                if (settings != null)
                {
                    Cld_Ord_Labels_DB cldOrdLabelsDB = new Cld_Ord_Labels_DB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = cldOrdLabelsDB.selectCld_Ord_LabelsByJobSequenceAndTag(jobSequence, tagNo);
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public List<Cld_Ord_Labels> SearchTagByOrderSequenceAndTagNo(long jobSequence, string tagNo, HttpRequest Request, HttpResponse Response)
        {
            List<Cld_Ord_Labels> returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[Request.Headers["ProjectId"]];
                if (settings != null)
                {
                    Cld_Ord_Labels_DB cldOrdLabelsDB = new Cld_Ord_Labels_DB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = cldOrdLabelsDB.selectAllCld_Ord_LabelsByJobSequenceAndTagNumberSearch(jobSequence, tagNo);
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public List<Cld_Ord_Labels> FindOtherTagByOrderSequenceAndTagNo(long jobSequence, long sequence, string tagNo, HttpRequest request, HttpResponse response)
        {
            List<Cld_Ord_Labels> returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    Cld_Ord_Labels_DB cldOrdLabelsDB = new Cld_Ord_Labels_DB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = cldOrdLabelsDB.selectAllOtherCld_Ord_LabelsByJobSequenceAndTagNumberSearch(jobSequence, sequence, tagNo);
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public Orders FindCreateOrderByJobRefWithTag(string jobRef, bool getTagsDetails, HttpRequest request, HttpResponse response)
        {
            Orders returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    Orders order = orderDB.getOrderByJobRef(jobRef);
                    if (order == null)
                    {
                        long sequence = -1;
                        if (orderDB.insertOrders(out sequence, jobRef, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                        {
                            order = new Orders();
                            order.Sequence = sequence;
                            order.JobRef = order.JobRef;
                        }
                    }
                    else if (getTagsDetails)
                    {
                        order = GetOrderWithTagsAndImages(order, request, response);
                    }
                    returnValue = order;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public Orders CreateUpdateOrderWithTagsAndImages(Orders order, HttpRequest request, HttpResponse Response)
        {
            Orders returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (order != null)
                    {
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Orders newOrder = null;
                        if (order.Sequence > 0 || !string.IsNullOrEmpty(order.JobRef))
                        {
                            if (order.Sequence > 0)
                            {
                                newOrder = orderDB.getOrderByJobSequence(order.Sequence ?? 0);
                            }
                            else if (!string.IsNullOrEmpty(order.JobRef))
                            {
                                newOrder = orderDB.getOrderByJobRef(order.JobRef);
                            }
                            if (newOrder == null)
                            {
                                long sequence = -1;
                                if (orderDB.insertOrders(out sequence, order.JobRef, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                                {
                                    newOrder = new Orders();
                                    newOrder.Sequence = sequence;
                                    newOrder.JobRef = order.JobRef;
                                }
                            }
                        }
                        if (newOrder != null)
                        {
                            newOrder.OI_FireProtection_I = new List<Cld_Ord_Labels>();
                            if (order.OI_FireProtection_I != null)
                            {
                                foreach (Cld_Ord_Labels tag in order.OI_FireProtection_I)
                                {
                                    Cld_Ord_Labels_DB cldOrdLabelsDB = new Cld_Ord_Labels_DB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                    Cld_Ord_Labels newOrderTag = cldOrdLabelsDB.selectCld_Ord_LabelsByJobSequenceAndTag(newOrder.Sequence ?? 0, tag.TagNo);
                                    if (newOrderTag == null)
                                    {
                                        long tagSequence = -1;
                                        if (cldOrdLabelsDB.insertCld_Ord_Labels(out tagSequence, newOrder.Sequence ?? 0, tag.TagNo, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                                        {
                                            newOrderTag = new Cld_Ord_Labels();
                                            newOrderTag.Sequence = tagSequence;
                                            newOrderTag.JobSequence = newOrder.Sequence;
                                            newOrderTag.TagNo = tag.TagNo;
                                        }
                                    }
                                    if (newOrderTag != null && tag.OI_FireProtection_I_Images != null)
                                    {
                                        newOrderTag.OI_FireProtection_I_Images = new List<Cld_Ord_Labels_Files>();

                                        foreach (Cld_Ord_Labels_Files tagImages in tag.OI_FireProtection_I_Images)
                                        {
                                            if (tagImages.FlgIsBase64Img == false)
                                            {
                                                IFormFile bfile = request.Form.Files.GetFile(tagImages.ImageName);
                                                if (bfile != null)
                                                {
                                                    using (var fileStream = bfile.OpenReadStream())
                                                    using (var ms = new MemoryStream())
                                                    {
                                                        fileStream.CopyTo(ms);
                                                        var fileBytes = ms.ToArray();
                                                        string base64String = Convert.ToBase64String(fileBytes);
                                                        tagImages.Base64Img = base64String;
                                                    }
                                                }
                                            }


                                            Cld_Ord_Labels_Files attachmentImage = new Cld_Ord_Labels_Files { FolderNames = tagImages.FolderNames, ImageName = tagImages.ImageName, Base64Img = tagImages.Base64Img, FlgIsBase64Img = true };
                                            string parentFolderNames = newOrder.JobRef + "," + SimplicityConstants.FilingCabinetCustomFolderName + "," + newOrderTag.TagNo;
                                            DriveRequest driveRequest = new DriveRequest { Name = tagImages.ImageName, ParentFolderNames = parentFolderNames, FireProtectionImages = attachmentImage };
                                            AttachmentFiles file = new AttachmentFilesFolderRepository().AddFileInSpecificFolder(driveRequest, request, Response);
                                            //save drive file Id
                                            if (file != null)
                                            {
                                                tagImages.DriveFileId = file.Id;
                                            }
                                            tagImages.JobSequence = newOrderTag.JobSequence;
                                            tagImages.JoinSequence = newOrderTag.Sequence;
                                            Cld_Ord_Labels_Files updatedImage = new OrdersTagsImagesRepository().AddUpdateImage(tagImages, request);
                                            newOrderTag.OI_FireProtection_I_Images.Add(updatedImage);
                                        }
                                    }
                                    newOrder.OI_FireProtection_I.Add(newOrderTag);
                                }
                            }
                        }
                        returnValue = newOrder;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("CreateUpdateOrderWithTagsAndImages:" + ex.Message);
                throw ex;
            }
            return returnValue;
        }

        public Orders GetOrderWithTagsAndImages(Orders order, HttpRequest request, HttpResponse response)
        {
            Orders returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (order != null)
                    {
                        OrdersDB orderDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (order.Sequence > 0 || !string.IsNullOrEmpty(order.JobRef))
                        {
                            if (order.Sequence > 0)
                            {
                                order = orderDB.getOrderByJobSequence(order.Sequence ?? 0);
                            }
                            else if (!string.IsNullOrEmpty(order.JobRef))
                            {
                                order = orderDB.getOrderByJobRef(order.JobRef);
                            }
                            if (order != null)
                            {
                                order.OI_FireProtection_I = GetAllOrderTagsByOrder(order, request);
                            }
                        }
                        else
                        {
                            order.IsSucessfull = false;
                            order.Message = "Unable to retrieve details. Order Details are incorrect.";
                        }
                    }
                    else
                    {
                        order = new Orders();
                        order.IsSucessfull = false;
                        order.Message = "Unable to retrieve details. Order Details are null.";
                    }
                }

                //in any case return AutoCreateJobRef for client 
                if (order == null)
                {
                    order = new Orders { AutoCreateJobRef = settings.AutoCreateJobRef };
                }
                else
                {
                    order.AutoCreateJobRef = settings.AutoCreateJobRef;
                }
                returnValue = order;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public List<Cld_Ord_Labels> GetAllOrderTagsByOrder(Orders order, HttpRequest request)
        {
            List<Cld_Ord_Labels> orderTags = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (order != null)
                    {
                        if (order.Sequence > 0)
                        {
                            Cld_Ord_Labels_DB orderTagsDB = new Cld_Ord_Labels_DB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            orderTags = orderTagsDB.selectAllCld_Ord_LabelsByJobSequence(order.Sequence ?? 0);
                            if (orderTags != null)
                            {
                                foreach (Cld_Ord_Labels orderTag in orderTags)
                                {
                                    orderTag.OI_FireProtection_I_Images = new OrdersTagsImagesRepository().GetOrderTagImagesByJoinSequence(orderTag.Sequence ?? 0, request);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return orderTags;
        }

        public ResponseModel EmailOrdersWithTagsAndImages(EmailOrderTags emailOptions, HttpRequest request, HttpResponse response)
        {
            ResponseModel returnValue = new ResponseModel();

            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                List<string> fileAttachmentsPaths = new List<string>();
                if (emailOptions.Orders != null && emailOptions.Orders.Count > 0)
                {
                    foreach (Orders ord in emailOptions.Orders)
                    {
                        string filePathWithName = Path.Combine(settings.EmailAttachmentsPath, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + ord.Sequence + ".pdf");
                        if (Utilities.GenerateOrderPDFWithTagAndImages(settings, ord, filePathWithName))
                        {
                            fileAttachmentsPaths.Add(filePathWithName);
                        }
                        else
                        {
                            //log and report error
                        }
                    }
                }
                if (emailOptions.Attachments != null && emailOptions.Attachments.Count > 0)
                {
                    foreach (EmailAttachments attachment in emailOptions.Attachments)
                    {
                        string filePath = Path.Combine(settings.EmailAttachmentsPath, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + attachment.FileName);
                        if (attachment.IsBase64)
                        {
                            if (Utilities.SaveBase64File(attachment.Base64File, filePath))
                            {
                                fileAttachmentsPaths.Add(filePath);
                            }
                        }
                        else
                        {
                            if (Utilities.SaveFileFromRequest(request, attachment.FileName, filePath))
                            {
                                fileAttachmentsPaths.Add(filePath);
                            }
                        }
                    }
                }
                emailOptions.From.EmailAddress = settings.FromEmailAddress;
                if (Utilities.SendMail(emailOptions.From, emailOptions.To, emailOptions.Cc, emailOptions.Bcc, emailOptions.Subject, emailOptions.Body,
                                       fileAttachmentsPaths, "", ""))
                {
                    returnValue.IsSucessfull = true;
                    returnValue.Message = "Email has been successfully Sent.";
                }
                else
                {
                    returnValue.IsSucessfull = false;
                    returnValue.Message = "Unable to Send Email. " + Utilities.Message;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Error Occured while Sending Email. " + ex.Message;
            }
            return returnValue;
        }

        public List<Orders> SearchOrderWithTagsAndImages(SearchOrderTags searchOptions, HttpRequest request, HttpResponse response)
        {
            List<Orders> ordersResult = null;
            List<Orders> ordersFilterOut = new List<Orders>();

            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (searchOptions != null)
                    {
                        OrdersDB ordersDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                        Cld_Ord_Labels_DB orderTagsDB = new Cld_Ord_Labels_DB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Cld_Ord_Labels_FilesDB orderTagsImagesDB = new Cld_Ord_Labels_FilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if (searchOptions.IsJobRef)
                        {
                            ordersResult = ordersDB.getOrdersByJobRef(searchOptions.JobRef);
                        }
                        else
                        {
                            ordersResult = ordersDB.getAllOrders();
                        }
                        if (ordersResult != null)
                        {
                            foreach (Orders order in ordersResult)
                            {
                                if (searchOptions.IsTagNumber)
                                {
                                    order.OI_FireProtection_I = orderTagsDB.selectAllCld_Ord_LabelsByJobSequenceAndTagNumberSearch(order.Sequence ?? 0, searchOptions.TagNumber);
                                    if (order.OI_FireProtection_I == null)
                                    {
                                        ordersFilterOut.Add(order);
                                    }
                                }
                                else
                                {
                                    order.OI_FireProtection_I = orderTagsDB.selectAllCld_Ord_LabelsByJobSequence(order.Sequence ?? 0);
                                }
                                if (order.OI_FireProtection_I != null)
                                {
                                    List<Cld_Ord_Labels> list = new List<Cld_Ord_Labels>();
                                    foreach (Cld_Ord_Labels orderTag in order.OI_FireProtection_I)
                                    {
                                        orderTag.OI_FireProtection_I_Images = orderTagsImagesDB.selectCld_Ord_Labels_FilesByJoinSequenceUserAndDateSearch(orderTag.Sequence ?? 0, searchOptions.IsTagCreatedDate, searchOptions.TagCreatedDate, searchOptions.IsTagUser, searchOptions.TagUser);
                                        if (orderTag.OI_FireProtection_I_Images == null && (searchOptions.IsTagCreatedDate || searchOptions.IsTagUser))
                                        {
                                            //order.OI_FireProtection_I.Remove(orderTag);
                                            // If order does not have an order tag then remove order from results as well.
                                            list.Add(orderTag);
                                        }
                                    }
                                    foreach (var items in list)
                                    {
                                        order.OI_FireProtection_I.Remove(items);
                                    }
                                }

                                if ((searchOptions.IsTagCreatedDate || searchOptions.IsTagUser) && (order.OI_FireProtection_I == null || order.OI_FireProtection_I.Count == 0))
                                {
                                    ordersFilterOut.Add(order);
                                }
                            }
                            if (ordersFilterOut != null && ordersFilterOut.Count > 0)
                            {
                                foreach (Orders orderTobeFilter in ordersFilterOut)
                                {
                                    ordersResult.Remove(orderTobeFilter);
                                }
                            }
                        }
                        else
                        {
                            ordersResult = new List<Orders>();
                            Orders order = new Orders();
                            order.IsSucessfull = true;
                            order.Message = "No Record Found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ordersResult;
        }

        public List<Orders> SearchOrderWithTagsAndImages2(SearchOrderTags searchOptions, HttpRequest request, HttpResponse response)
        {
            List<Orders> ordersResult = null;
            List<Orders> ordersFilterOut = new List<Orders>();

            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (searchOptions != null)
                    {
                        OrdersDB ordersDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        ordersResult = ordersDB.selectAllOI_FireProtection_IByJobSequenceAndTagNumberSearch(searchOptions.IsJobRef, searchOptions.JobRef, searchOptions.IsTagNumber, searchOptions.TagNumber, searchOptions.IsTagCreatedDate, searchOptions.TagCreatedDate, searchOptions.IsTagUser, searchOptions.TagUser);
                        if (ordersResult == null)
                        {
                            ordersResult = new List<Orders>();
                            Orders order = new Orders();
                            order.IsSucessfull = true;
                            order.Message = "No Record Found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ordersResult;
        }

        public List<JobRefALL> GetJobRefListForTimeSheet(SearchOrderTags searchOptions, HttpRequest request, HttpResponse response)
        {
            List<JobRefALL> result = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (searchOptions != null)
                    {
                        OrdersDB ordersDB = new OrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = ordersDB.GetJobRefListForTimeSheet(searchOptions.IsJobRef, searchOptions.JobRef, searchOptions.IsTagNumber, searchOptions.TagNumber, searchOptions.IsTagCreatedDate, searchOptions.TagCreatedDate, searchOptions.IsTagUser, searchOptions.TagUser);
                        if (result == null)
                        {
                            result = new List<JobRefALL>();
                            Orders order = new Orders();
                            order.IsSucessfull = true;
                            order.Message = "No Record Found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public Cld_Ord_Labels UpdateTagNoBySequence(long sequence, string tagNo, HttpRequest Request, HttpResponse Response)
        {
            Cld_Ord_Labels returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[Request.Headers["ProjectId"]];
                if (settings != null)
                {
                    Cld_Ord_Labels_DB cldOrdLabelsDB = new Cld_Ord_Labels_DB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    //OI_FireProtection_I orderTags = cldOrdLabelsDB.selectOI_FireProtection_IByJobSequenceAndTag(sequence, tagNo);
                    Cld_Ord_Labels orderTags = null;
                    if (cldOrdLabelsDB.updateCld_Ord_Labels(sequence, tagNo, Convert.ToInt32(Request.Headers["UserId"]), DateTime.Now))
                    {
                        orderTags = new Cld_Ord_Labels();
                        orderTags.Sequence = sequence;
                        //orderTags.JobSequence = jobSequence;
                        orderTags.TagNo = tagNo;
                    }
                    //if (orderTags == null)
                    //{
                    //    long sequence = -1;
                    //    if (cldOrdLabelsDB.updateOI_FireProtection_I(sequence, tagNo, Convert.ToInt32(Request.Headers["UserId"]), DateTime.Now))
                    //    {
                    //        orderTags = new OI_FireProtection_I();
                    //        orderTags.Sequence = sequence;
                    //        orderTags.JobSequence = jobSequence;
                    //        orderTags.TagNo = tagNo;
                    //    }
                    //}
                    //else
                    //{
                    //    orderTags.OI_FireProtection_I_Images = new OrdersTagsImagesRepository().GetOrderTagImagesByJoinSequence(orderTags.Sequence, Request);
                    //}
                    returnValue = orderTags;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public string GetUrlOfArchiveSystem(long orderSequence, HttpRequest request, HttpResponse response)
        {
            string returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId("AFS");

                    if (applicationSettings != null && applicationSettings.Count > 0)
                    {
                        string url = applicationSettings[0].Setting1;
                        string coName = applicationSettings[0].Setting2;
                        PassthroughModel passthroughModel = new PassthroughModel()
                        {
                            PassthroughString = Utilities.strUFFAFU(Utilities.GetLocalIPAddress()),
                            JobSequence = orderSequence,
                            JobClientId = -1,
                            JobAddressId = -1,
                            EntityId = -1,
                            FlagAdminMode = false
                        };
                        passthroughModel = _passthroughRepository.Create(passthroughModel, request, response);
                        if (passthroughModel != null)
                        {
                            returnValue = String.Format("{0}connect?ID={1}{2}&CO={3}", (url.EndsWith("/") ? url : url + "/"), Utilities.StrHash(Utilities.StrCreateEnabler()), passthroughModel.Sequence, coName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return returnValue;
        }
    }
}
