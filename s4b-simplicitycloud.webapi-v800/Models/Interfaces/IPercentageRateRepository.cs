﻿using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IPercentageRateRepository : IRepository
    {
        ResponseModel GetAdjustmentCodes(HttpRequest request);
    }
}
