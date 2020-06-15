
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Models.Repositories;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrdersTagsImagesController : Controller
    {
        private readonly IOrdersTagsImagesRepository OrdersTagsImagesRepository;
        private readonly IAttachmentFilesFolderRepository AttachmentFilesFolderRepository;
        private readonly IAddInfoRepository AddInfoRepository;

        public OrdersTagsImagesController(
            IOrdersTagsImagesRepository ordersTagsImagesRepository,
            IAttachmentFilesFolderRepository attachmentFilesFolderRepository,
            IAddInfoRepository addInfoRepository)
        {
            this.OrdersTagsImagesRepository = ordersTagsImagesRepository;
            this.AttachmentFilesFolderRepository = attachmentFilesFolderRepository;
            this.AddInfoRepository = addInfoRepository;
        }

        [HttpPost]
        [ActionName("CreateUpdateTagImages")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult CreateTagImagesByTagAndOrder()
        {
            Cld_Ord_Labels_Files orderTagImages = JsonConvert.DeserializeObject<Cld_Ord_Labels_Files> (Request.Form["OI_FireProtection_I_Images"]);
            orderTagImages = OrdersTagsImagesRepository.AddUpdateImage(orderTagImages, Request);
            if (orderTagImages == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(orderTagImages);
        }

        [HttpPost]
        [ActionName("CreateUpdateTagImagesBase64")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult CreateUpdateTagImagesBase64([FromBody]Cld_Ord_Labels_Files orderTagImages)
        {
            //var parentFolderNames = orderTagImages.FolderNames;
            //var fileName = orderTagImages.ImageName;
            //string base64Img = orderTagImages.Base64Img;
            Cld_Ord_Labels_Files attachmentImage = new Cld_Ord_Labels_Files { FolderNames = orderTagImages.FolderNames, ImageName = orderTagImages.ImageName, Base64Img = orderTagImages.Base64Img, FlgIsBase64Img = true };
            DriveRequest request = new DriveRequest { Name = orderTagImages.ImageName, ParentFolderNames = orderTagImages.FolderNames, FireProtectionImages = attachmentImage };
            AttachmentFiles file = AttachmentFilesFolderRepository.AddFileInSpecificFolder(request, Request, HttpContext.Response);
            //save drive file Id
            if (file != null)
                orderTagImages.DriveFileId = file.Id;
            orderTagImages = OrdersTagsImagesRepository.AddUpdateImage(orderTagImages, Request);

            if (orderTagImages == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(orderTagImages);
        }

        [HttpPost]
        [ActionName("DeleteTagImageBySequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult DeleteTagImageBySequence([FromBody]long sequence)
        {
            bool success = OrdersTagsImagesRepository.DeleteTagImageBySequence(sequence, Request);
            return new ObjectResult(success);
        }
    }
}
