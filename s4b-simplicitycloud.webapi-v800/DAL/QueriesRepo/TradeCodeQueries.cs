using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public class TradeCodeQueries
    {
        internal static string SelectTradeIdAndCodeFields(string datebaseType)
        {
            string returnValue = "";
            returnValue = "Select trade_id, Trade_desc from un_ref_trade_code_type";

            return returnValue;
        }
    }
}
