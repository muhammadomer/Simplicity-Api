using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAssetTestHRepository : IRepository
    {
        List<AssetTestH> GetAllBySequences(long sequence, long assetSequence, long typeSequence, HttpRequest request);
        ResponseModel Insert(AssetTestH assetTestH, HttpRequest request);
        bool UpdateBySequence(AssetTestH assetTestH, HttpRequest request);
        bool UpdateLocked(long assetSequence, HttpRequest request);
        
    }
}
