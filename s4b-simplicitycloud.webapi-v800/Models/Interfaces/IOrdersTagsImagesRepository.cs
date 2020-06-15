using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IOrdersTagsImagesRepository : IRepository
    {
        Cld_Ord_Labels_Files AddUpdateImage(Cld_Ord_Labels_Files oiFireProtectionIImages, HttpRequest Request);
        bool DeleteTagImageBySequence(long sequence, HttpRequest request);
    }
}
