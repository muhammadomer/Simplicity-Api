using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAssetTestIRepository : IRepository
    {
        List<AssetTestI> GetAllBySequences(long sequence, HttpRequest request);
        ResponseModel Insert(AssetTestI assetTestI, long testListSequence, HttpRequest request);
        bool UpdateBySequence(AssetTestI assetTestI, HttpRequest request);
        
    }
}
