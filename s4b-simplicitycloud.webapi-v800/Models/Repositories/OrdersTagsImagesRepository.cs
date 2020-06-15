using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrdersTagsImagesRepository : IOrdersTagsImagesRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public Cld_Ord_Labels_Files AddUpdateImage(Cld_Ord_Labels_Files oiFireProtectionIImages, HttpRequest Request)
        {
            Cld_Ord_Labels_Files returnValue = null;
            try
            {
                if (oiFireProtectionIImages != null)
                {
                    ProjectSettings settings = Configs.settings[Request.Headers["ProjectId"]];
                    //if (oiFireProtectionIImages.AddInfoSequence < 1 && !string.IsNullOrEmpty(oiFireProtectionIImages.AddInfo))
                    if (oiFireProtectionIImages.JobSequence> 0 && !string.IsNullOrEmpty(oiFireProtectionIImages.AddInfo))
                    {
                        Cld_Ord_Labels_Files_Add_Info addInfo = new Cld_Ord_Labels_Files_Add_Info { AddInfo = oiFireProtectionIImages.AddInfo, JobSequence = oiFireProtectionIImages.JobSequence, JoinSequence = oiFireProtectionIImages.JoinSequence };
                        Cld_Ord_Labels_Files_Add_InfoDB addInfoDb = new Cld_Ord_Labels_Files_Add_InfoDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Cld_Ord_Labels_Files_Add_Info info = addInfoDb.selectCld_Ord_Labels_Files_Add_InfoDesc(oiFireProtectionIImages.JobSequence ?? 0, oiFireProtectionIImages.AddInfo);
                        if (info == null)
                        {
                            long addInfoSequence = -1;
                            bool addInfoSuccessfully = addInfoDb.insertCld_Ord_Labels_Files_Add_Info(out addInfoSequence, oiFireProtectionIImages.JobSequence ?? 0, oiFireProtectionIImages.OiSequence, oiFireProtectionIImages.HeaderSequence, oiFireProtectionIImages.JoinSequence ?? 0, oiFireProtectionIImages.FlgDeleted, oiFireProtectionIImages.AddInfo, oiFireProtectionIImages.CreatedBy, DateTime.Now, oiFireProtectionIImages.CreatedBy, DateTime.Now);
                            if (addInfoSuccessfully)
                            {
                                if (addInfo != null && addInfo.Sequence < 1)
                                {
                                    oiFireProtectionIImages.AddInfoSequence = addInfoSequence;
                                }
                            }
                        }
                        else
                        {
                            oiFireProtectionIImages.AddInfoSequence = info.Sequence;
                        }
                    }
                    
                    string imagePath = generateImagePath(settings, oiFireProtectionIImages.JobSequence ?? 0, oiFireProtectionIImages.JoinSequence ?? 0);
                    string imageFileName = generateImageFileName(oiFireProtectionIImages.ImageName);
                    string imagePathAndFileName = Path.Combine(imagePath, imageFileName);
                    string imageURL = generateImageURL(settings, oiFireProtectionIImages.JobSequence ?? 0, oiFireProtectionIImages.JoinSequence ?? 0, imageFileName);
                    string logoURL = Utilities.generateLogoURL(settings, imageURL, imageFileName);
                    if(String.IsNullOrEmpty(logoURL))
                    {
                        logoURL = imageURL;
                    }
                    long sequence = -1;
                    if(oiFireProtectionIImages.Sequence>0)
                    {
                        sequence = createUpdateTagImage(oiFireProtectionIImages.Sequence ?? 0, oiFireProtectionIImages.JobSequence ?? 0, 
                                                        oiFireProtectionIImages.OiSequence,
                                                        oiFireProtectionIImages.HeaderSequence, oiFireProtectionIImages.JoinSequence ?? 0, oiFireProtectionIImages.AddInfoSequence ?? 0, oiFireProtectionIImages.FileDate,
                                                        oiFireProtectionIImages.FileDesc, imagePathAndFileName, imageURL, logoURL, oiFireProtectionIImages.CreatedBy, oiFireProtectionIImages.ImageName, oiFireProtectionIImages.DriveFileId, Request);
                        oiFireProtectionIImages.ImageURL = imageURL;
                        oiFireProtectionIImages.LogoURL = logoURL;
                    }
                    else
                    {
                        sequence = createUpdateTagImage(sequence, oiFireProtectionIImages.JobSequence ?? 0, oiFireProtectionIImages.OiSequence,
                                                        oiFireProtectionIImages.HeaderSequence, oiFireProtectionIImages.JoinSequence ?? 0, oiFireProtectionIImages.AddInfoSequence ?? 0, 
                                                        oiFireProtectionIImages.FileDate, oiFireProtectionIImages.FileDesc, 
                                                        imagePathAndFileName, imageURL, logoURL, oiFireProtectionIImages.CreatedBy, oiFireProtectionIImages.ImageName, oiFireProtectionIImages.DriveFileId, Request);
                        oiFireProtectionIImages.Sequence = sequence;
                        oiFireProtectionIImages.LogoURL = logoURL;
                    }
                    if (string.IsNullOrEmpty( oiFireProtectionIImages.ImageName ))
                    {
                        //edit image case - do not upload any image
                    }
                    else
                    {
                        if (oiFireProtectionIImages.FlgIsBase64Img)
                        {
                            if (oiFireProtectionIImages.Base64Img != null && oiFireProtectionIImages.Base64Img != "")
                            {

                                byte[] imageBytes = Convert.FromBase64String(oiFireProtectionIImages.Base64Img);
                                //if image then resize
                                if (IsImageFile(imageFileName))
                                {
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        Image image = Image.FromStream(ms, true);
                                        bool isImageResize = ResizeImage(image, 150, 150, imagePathAndFileName);
                                    }
                                }
                                else
                                {
                                    File.WriteAllBytes(imagePathAndFileName, imageBytes);
                                }
                                oiFireProtectionIImages.Base64Img = null;

                            }
                        }
                        else
                        {
                            IFormFile image = Request.Form.Files.GetFile(oiFireProtectionIImages.ImageName);
                            if (image != null)
                            {
                                using (var fileStream = new FileStream(imagePathAndFileName, FileMode.Create)) // updated by Faheem
                                {
                                    image.CopyTo(fileStream);
                                }
                                
                            }
                        }
                    }
                   
                    if(oiFireProtectionIImages.Sequence>0)
                    {
                        var imageDb = new Cld_Ord_Labels_FilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        oiFireProtectionIImages = imageDb.selectCld_Ord_Labels_FilesBySequence(oiFireProtectionIImages.Sequence ?? 0);
                    }
                    returnValue = oiFireProtectionIImages;
                }
                else
                {
                    // Report and Log error 
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private bool IsImageFile(string fileName)
        {
            return (fileName.Contains(".jpg") 
                || fileName.Contains(".jpeg") 
                || fileName.Contains(".jfif") 
                || fileName.Contains(".bmp") 
                || fileName.Contains(".tif") 
                || fileName.Contains(".tiff") 
                || fileName.Contains(".gif")
                || fileName.Contains(".png")
                );
        }
        private bool ResizeImage(Image imgPhoto, int Width, int Height,string imagePathAndFileName)
        {
            bool result = true;
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            try
            {
                nPercentW = ((float)Width / (float)sourceWidth);
                nPercentH = ((float)Height / (float)sourceHeight);
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                    destX = Convert.ToInt16((Width - (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = System.Convert.ToInt16((Height - (sourceHeight * nPercent)) / 2);
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                 imgPhoto.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.Clear(Color.White);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.SmoothingMode = SmoothingMode.HighQuality;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                grPhoto.CompositingQuality = CompositingQuality.HighQuality;

                grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

                var imageEncoders = ImageCodecInfo.GetImageEncoders();
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                bmPhoto.Save(imagePathAndFileName, imageEncoders[1], encoderParameters);

                bmPhoto.Dispose();
                grPhoto.Dispose();
                imgPhoto.Dispose();
            }
            catch (Exception e)
            {
                result = false;
                
            }
            return result;
            
        }
        //public Image ResizeImage(Image image, float width, float height)
        //{
        //    // the colour for letter boxing, can be a parameter
        //    var brush = new SolidBrush(Color.Black);

        //    // target scaling factor
        //    float scale = Math.Min(width / image.Width, height / image.Height);

        //    // target image
        //    var bmp = new Bitmap((int)width, (int)height);
        //    var graph = Graphics.FromImage(bmp);
        //    var scaleWidth = (int)(image.Width * scale);
        //    var scaleHeight = (int)(image.Height * scale);

        //    // fill the background and then draw the image in the 'centre'
        //    graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
        //    graph.DrawImage(image, new Rectangle(((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight));

        //    return bmp;
        //}
        private long createUpdateTagImage(long sequence, long jobSequence, long oiSequence, long headerSequence, long joinSequence, 
                                          long addInfoSequence, DateTime? imageDate, string fileDesc, string imagePathAndFileName, 
                                          string imageURL, string logoURL, int imageUser,string imageName,string driveFileId , HttpRequest Request)
        {
            long returnValue = -1;
            try
            {
                ProjectSettings settings = Configs.settings[Request.Headers["ProjectId"]];
                Cld_Ord_Labels_FilesDB cldOrdLabelsFilesDB = new Cld_Ord_Labels_FilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                if (sequence > 0)
                {
                    if (cldOrdLabelsFilesDB.updateAll(sequence,
                        jobSequence, oiSequence, headerSequence,
                        joinSequence,addInfoSequence, false, imagePathAndFileName, imageURL, logoURL, imageDate, fileDesc,
                        (imageUser <= 0) ? Convert.ToInt32(Request.Headers["UserId"]) : imageUser, DateTime.Now,
                        (imageUser <= 0) ? Convert.ToInt32(Request.Headers["UserId"]) : imageUser, DateTime.Now,imageName,driveFileId))
                    {
                    }
                }
                else
                { 
                    if (cldOrdLabelsFilesDB.insertCld_Ord_Labels_Files(out sequence,
                        jobSequence, oiSequence, headerSequence,
                        joinSequence, addInfoSequence, false, imagePathAndFileName, imageURL, logoURL, imageDate, fileDesc,
                        (imageUser<=0) ? Convert.ToInt32(Request.Headers["UserId"]) : imageUser, DateTime.Now,
                        (imageUser <= 0) ? Convert.ToInt32(Request.Headers["UserId"]) : imageUser, DateTime.Now,driveFileId))
                    {
                    }
                }
                returnValue = sequence;
            }
            catch (Exception ex)
            {

            }
            return returnValue;
        }

        private string generateImageFileName(string imageName)
        {
            string formattedImageFileName = imageName;
            formattedImageFileName = formattedImageFileName.Replace(" ","_");
            string imageFileName = DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + formattedImageFileName;
            return imageFileName;
        }

        private string generateImagePath(ProjectSettings settings, long jobSequence, long joinSequence)
        {
            string imagePath = Path.Combine(settings.TagImagePath.ToString(), jobSequence.ToString(), joinSequence.ToString());
            Directory.CreateDirectory(imagePath);
            return imagePath;
        }

        private string generateImageURL(ProjectSettings settings, long jobSequence, long joinSequence, string imageFileName)
        {
            Uri baseUri = new Uri(settings.TagImageBaseURL.ToString());
            Uri myUri = new Uri(baseUri, jobSequence.ToString() + "/" + joinSequence.ToString() + "/" + imageFileName);
            return myUri.ToString();
        }

        internal List<Cld_Ord_Labels_Files> GetOrderTagImagesByJoinSequence(long joinSequence, HttpRequest request)
        {
            List<Cld_Ord_Labels_Files> orderTagImages = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (joinSequence > 0)
                    {
                        Cld_Ord_Labels_FilesDB cldOrdLabelsFilesDB = new Cld_Ord_Labels_FilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        orderTagImages = cldOrdLabelsFilesDB.selectCld_Ord_Labels_FilesByJoinSequence(joinSequence);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return orderTagImages;
        }

        public bool DeleteTagImageBySequence(long sequence, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (sequence > 0)
                    {
                        Cld_Ord_Labels_FilesDB cldOrdLabelsFilesDB = new Cld_Ord_Labels_FilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = cldOrdLabelsFilesDB.updateDeleteFlagBySequence(sequence, true, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now);
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
