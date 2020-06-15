using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAddInfoRepository : IRepository
    {
        Cld_Ord_Labels_Files_Add_Info CreaeUpdateAddInfo(Cld_Ord_Labels_Files_Add_Info addInfo, HttpRequest request, HttpResponse response);

        Cld_Ord_Labels_Files_Add_Info GetAddInfoWithSequence(long sequence, HttpRequest request, HttpResponse response);
        Cld_Ord_Labels_Files_Add_Info GetAddInfoWithDesc(long sequence,string addInfo, HttpRequest request, HttpResponse response);

        //DriveService AuthenticateOauth(string clientId, string clientSecret, string userName, HttpRequest request, HttpResponse response);

        //public bool updateBySequence(long sequence, long jobSequence, long oiSequence, long headerSequence, long joinSequence, bool flgDeleted, string addInfo, long createdBy, DateTime? dateCreated,
        //                            long lastAmendedBy, DateTime? dateLastAmended);
    }
}
