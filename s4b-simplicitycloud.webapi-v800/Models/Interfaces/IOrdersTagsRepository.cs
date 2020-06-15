using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IOrdersTagsRepository : IRepository
    {
        Cld_Ord_Labels FindCreateTagByTagNoAndJobSequence(long jobSequence, string tagNo, HttpRequest Request, HttpResponse Response);
        Orders CreateUpdateOrderWithTagsAndImages(Orders order, HttpRequest request, HttpResponse response);
        Orders GetOrderWithTagsAndImages(Orders order, HttpRequest request, HttpResponse response);
        List<Orders> SearchOrderWithTagsAndImages(SearchOrderTags searchOptions, HttpRequest request, HttpResponse response);
        List<Orders> SearchOrderWithTagsAndImages2(SearchOrderTags searchOptions, HttpRequest request, HttpResponse response);
        Orders FindCreateOrderByJobRefWithTag(string jobRef, bool getTagsDetails, HttpRequest request, HttpResponse response);
        ResponseModel EmailOrdersWithTagsAndImages(EmailOrderTags emailOptions, HttpRequest request, HttpResponse response);
        Cld_Ord_Labels FindTagByOrderSequenceAndTagNo(long jobSequence, string tagNo, HttpRequest request, HttpResponse response);
        List<Cld_Ord_Labels> SearchTagByOrderSequenceAndTagNo(long jobSequence, string tagNo, HttpRequest request, HttpResponse response);
        List<Cld_Ord_Labels> FindOtherTagByOrderSequenceAndTagNo(long jobSequence,long sequence, string tagNo, HttpRequest request, HttpResponse response);

        Cld_Ord_Labels UpdateTagNoBySequence(long sequence, string tagNo, HttpRequest Request, HttpResponse Response);
        string GetUrlOfArchiveSystem(long orderSequence, HttpRequest request, HttpResponse response);


        List<JobRefALL> GetJobRefListForTimeSheet(SearchOrderTags searchOptions, HttpRequest request, HttpResponse response);
    }
}
