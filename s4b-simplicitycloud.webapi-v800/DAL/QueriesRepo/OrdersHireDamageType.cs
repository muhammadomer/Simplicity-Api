using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefOrdersHireDamageTypeQueries
    {
        internal static string SelectAll(string databaseType)
        {
            string returnValue = @"SELECT damage_type_sequence, row_index, damage_type_desc, flg_deleted
            FROM un_ref_ord_hire_damage_types
            ORDER BY row_index";
            return returnValue;
        }
    }
}
