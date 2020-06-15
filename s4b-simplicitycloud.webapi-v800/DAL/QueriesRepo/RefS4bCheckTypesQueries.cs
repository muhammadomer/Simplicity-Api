using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefS4bCheckTypesQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "",whereStr="";
			if (Sequence > 0)
				whereStr = " And sequence=" + Sequence;
            try
            {

				returnValue = @" SELECT * 
					FROM    un_ref_s4b_check_types
                    WHERE flg_deleted<>" + Utilities.GetBooleanForDML(databaseType, true)  + whereStr;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

		public static string getSelectAllByCheckType(string databaseType, int CheckType)
		{
			string returnValue = "";
			try
			{
				returnValue = @" SELECT * FROM    un_ref_s4b_check_types
                    WHERE flg_deleted<>"+ Utilities.GetBooleanForDML(databaseType,true) + " And check_type =" + CheckType;
			}
			catch (Exception ex)
			{
			}
			return returnValue;
		}

		public static string getSelectAll(string databaseType)
		{
			string returnValue = "";
			try
			{
				returnValue = @" SELECT * FROM    un_ref_s4b_check_types
                    WHERE flg_deleted<>" + Utilities.GetBooleanForDML(databaseType, true) ;
			}
			catch (Exception ex)
			{
			}
			return returnValue;
		}
	}
}

