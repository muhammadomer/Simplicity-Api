using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class AddInfoRepository : IAddInfoRepository
    {
        //private readonly ILoggerFactory Logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public AddInfoRepository(ILoggerFactory loggerFactory)
        {
            //Logger = loggerFactory;
        }

        public Cld_Ord_Labels_Files_Add_Info CreaeUpdateAddInfo(Cld_Ord_Labels_Files_Add_Info addInfo, HttpRequest request, HttpResponse response)
        {
            Cld_Ord_Labels_Files_Add_Info returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (addInfo != null)
                    {
                        Cld_Ord_Labels_Files_Add_InfoDB orderDB = new Cld_Ord_Labels_Files_Add_InfoDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Cld_Ord_Labels_Files_Add_Info newOrder = null;
                        if (addInfo.Sequence > 0 )
                        {
                            if (addInfo.Sequence > 0)
                            {
                                newOrder = orderDB.selectAllCld_Ord_Labels_Files_Add_InfoSequence(addInfo.Sequence ?? 0);
                            }                            
                            if (newOrder == null )
                            {
                                long sequence = -1;
                                if (orderDB.insertCld_Ord_Labels_Files_Add_Info(out sequence,addInfo.JobSequence ?? 0, addInfo.OiSequence ?? 0, addInfo.HeaderSequence ?? 0, addInfo.JoinSequence ?? 0, addInfo.FlgDeleted,addInfo.AddInfo,Convert.ToInt32(request.Headers["UserId"]), DateTime.Now, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                                {
                                    newOrder = new Cld_Ord_Labels_Files_Add_Info();
                                    newOrder.Sequence = sequence;
                                    //newOrder.JobRef = order.JobRef;
                                }
                            }
                        }
                        if (newOrder != null)
                        {
                            //newOrder.OI_FireProtection_I = new List<OI_FireProtection_I>();
                            //if (order.OI_FireProtection_I != null)
                            //{
                            //    foreach (OI_FireProtection_I tag in order.OI_FireProtection_I)
                            //    {
                            //        OI_FireProtection_IDB oiFireProtectionIDB = new OI_FireProtection_IDB(settings.ConnectionString);
                            //        OI_FireProtection_I newOrderTag = oiFireProtectionIDB.selectOI_FireProtection_IByJobSequenceAndTag(newOrder.Sequence, tag.TagNo);
                            //        if (newOrderTag == null)
                            //        {
                            //            long tagSequence = -1;
                            //            if (oiFireProtectionIDB.insertOI_FireProtection_I(out tagSequence, newOrder.Sequence, tag.TagNo, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                            //            {
                            //                newOrderTag = new OI_FireProtection_I();
                            //                newOrderTag.Sequence = tagSequence;
                            //                newOrderTag.JobSequence = newOrder.Sequence;
                            //                newOrderTag.TagNo = tag.TagNo;
                            //            }
                            //        }
                            //        if (newOrderTag != null && tag.OI_FireProtection_I_Images != null)
                            //        {
                            //            newOrderTag.OI_FireProtection_I_Images = new List<OI_FireProtection_I_Images>();
                            //            foreach (OI_FireProtection_I_Images tagImages in tag.OI_FireProtection_I_Images)
                            //            {
                            //                tagImages.JobSequence = newOrderTag.JobSequence;
                            //                tagImages.JoinSequence = newOrderTag.Sequence;
                            //                OI_FireProtection_I_Images updatedImage = new OrdersTagsImagesRepository().AddUpdateImage(tagImages, request);
                            //                newOrderTag.OI_FireProtection_I_Images.Add(updatedImage);
                            //            }
                            //        }
                            //        newOrder.OI_FireProtection_I.Add(newOrderTag);
                            //    }
                            //}
                        }
                        returnValue = newOrder;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public Cld_Ord_Labels_Files_Add_Info GetAddInfoWithSequence(long sequenceId, HttpRequest request, HttpResponse response)
        {
            Cld_Ord_Labels_Files_Add_Info returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    Cld_Ord_Labels_Files_Add_InfoDB orderDB = new Cld_Ord_Labels_Files_Add_InfoDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    Cld_Ord_Labels_Files_Add_Info order = orderDB.selectAllCld_Ord_Labels_Files_Add_InfoSequence(sequenceId);
                    //if (order == null)
                    //{
                        //long sequence = -1;
                        //if (orderDB.insertOrders(out sequence, jobRef, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                        //{
                        //    order = new Orders();
                        //    order.Sequence = sequence;
                        //    order.JobRef = order.JobRef;
                        //}
                    //}
                    //else if (getTagsDetails)
                    //{
                    //    order = GetOrderWithTagsAndImages(order, request, response);
                    //}
                    returnValue = order;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public Cld_Ord_Labels_Files_Add_Info GetAddInfoWithDesc(long sequenceId,string addInfo, HttpRequest request, HttpResponse response)
        {
            Cld_Ord_Labels_Files_Add_Info returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    Cld_Ord_Labels_Files_Add_InfoDB orderDB = new Cld_Ord_Labels_Files_Add_InfoDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    Cld_Ord_Labels_Files_Add_Info order = orderDB.selectCld_Ord_Labels_Files_Add_InfoDesc(sequenceId, addInfo);
                    returnValue = order;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        //public DriveService AuthenticateOauth(string clientId, string clientSecret, string userName, HttpRequest request, HttpResponse response)
        //{
        //    //ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
        //    ProjectSettings settings = Configs.settings["KILNBRIDGE"];
        //    DriveService ds = null;
        //    if (settings != null)
        //    {
        //        OiFireProtectionIAddInfoDB orderDB = new OiFireProtectionIAddInfoDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //        ds = orderDB.AuthenticateOauth(clientId, clientSecret, userName);
        //    }
        //    return ds;
        //}


    }
}
