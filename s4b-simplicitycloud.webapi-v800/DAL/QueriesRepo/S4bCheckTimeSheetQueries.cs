using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class S4bCheckTimeSheetQueries
    {

        public static string insert(string databaseType,S4bCheckTimeSheet obj )
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_s4b_check_timesheet(job_sequence,  pymt_type,  flg_deleted,  de_sequence 
                , date_start_date,date_start_time, start_time_location,date_finish_time,finish_time_location
				, row_time_total,row_notes,flg_imported
				, created_by,  date_created 
                , last_amended_by,  date_last_amended) 
                VALUES (" + obj.JobSequence +  ", " + obj.PaymentType + ", " + Utilities.GetBooleanForDML(databaseType, false)
                + ", " + obj.DeSequence +"," + Utilities.GetDateTimeForDML(databaseType,obj.DateStartDate,true,true)
				+","+ Utilities.GetDateTimeForDML(databaseType,obj.DateStartTime,true,true)
				+",'"+ obj.StartTimeLocation +"'"
				+"," + Utilities.GetDateTimeForDML(databaseType,obj.DateFinishTime,true,true)
				+",'" + obj.FinishTimeLocation +"'"
				+"," + obj.RowTimeTotal +",'" + obj.RowNotes + "',"  + Utilities.GetBooleanForDML(databaseType, obj.FlgImported)
				+ ", " + obj.CreatedBy 
                + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true)
                + ", " + obj.LastAmendedBy 
                + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) + ")";
                       
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

		public static string deleteBySequence(string databaseType, S4bCheckTimeSheet obj)
		{
			string returnValue = "";
			try
			{
				returnValue = @"Update un_s4b_check_timesheet
				Set flg_deleted=" +  Utilities.GetBooleanForDML(databaseType, true)
				+ ", last_amended_by=" + obj.LastAmendedBy
				+ ", date_last_amended=" + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) 
				+ " WHERE Sequence=" + obj.Sequence;

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return returnValue;
		}


	}
}

