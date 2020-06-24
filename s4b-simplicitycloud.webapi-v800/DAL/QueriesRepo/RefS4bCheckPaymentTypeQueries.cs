using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefS4bCheckPaymentTypesQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "",whereStr="";
			if (Sequence > 0)
				whereStr = " And sequence=" + Sequence;
            try
            {

				returnValue = @" SELECT * 
					FROM    un_ref_s4b_check_pymt_types
                    WHERE flg_deleted<>" + Utilities.GetBooleanForDML(databaseType, true)  + whereStr;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

	}
}

