using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrdersMeHeaderQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                                   "  FROM    un_orders_me_header" +
                                   " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

		public static string getSelectAllByJobSequence(string databaseType, long jobSequence)
		{
			string returnValue = "";
			try
			{
				switch (databaseType)
				{
					case "MSACCESS":
					case "SQLSERVER":
					default:
						returnValue = @"SELECT un_orders_me_header.*
									   FROM un_orders_me_header 
									  WHERE job_sequence = " + jobSequence;
						break;
				}
			}
			catch (Exception ex)
			{
			}
			return returnValue;
		}
		public static string getSelectAllBySequenceWithCategory(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT un_orders_me_header.*, un_ref_ord_tender_categories.category_desc " +
                                      "  FROM un_orders_me_header " +
                                      "  LEFT JOIN un_ref_ord_tender_categories " +
                                      "    ON un_orders_me_header.category_sequence = un_ref_ord_tender_categories.category_sequence " +
                                      " WHERE sequence = " + Sequence;
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string insert(string databaseType,  long jobSequence,  string meUserText1,  long meUserCombo1,  long meScheduleCount,  bool flgMeFinalised,
                                    string meNotes,  bool flgMeEnquiry, DateTime? dateMeEnquiry, bool flgMeTenderReceived, DateTime? dateMeTenderReceived, 
                                    bool flgMeTenderDue, DateTime? dateMeTenderDue, bool flgMeTenderSent, DateTime? dateMeTenderSent, bool flgMeApproved, 
                                    DateTime? dateMeApproved, bool flgMeUserDate1, DateTime? dateMeUserDate1, long createdBy,  DateTime? dateCreated,
                                    long lastAmendedBy,  DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {    
                returnValue = "INSERT INTO   un_orders_me_header(job_sequence,  me_user_text1,  me_user_combo1,  me_schedule_count,  flg_me_finalised,"+
                "                                  me_notes,  flg_me_enquiry,  date_me_enquiry,  flg_me_tender_received,"+
                "                                  date_me_tender_received,  flg_me_tender_due,  date_me_tender_due,  flg_me_tender_sent,"+
                "                                  date_me_tender_sent,  flg_me_approved,  date_me_approved,  flg_me_user_date1,  date_me_user_date1,  created_by,"+
                "                                  date_created,  last_amended_by,  date_last_amended)" +
                "VALUES (" +  jobSequence + ",   '" +  meUserText1 + "',  " +  meUserCombo1 + ",  " +  meScheduleCount + ",   " +  Utilities.GetBooleanForDML(databaseType, flgMeFinalised) 
                + ",  '" +meNotes + "',   " + Utilities.GetBooleanForDML(databaseType, flgMeEnquiry) + ",   " + Utilities.GetDateTimeForDML(databaseType, dateMeEnquiry,true,true) 
                + ",  " + Utilities.GetBooleanForDML(databaseType, flgMeTenderReceived) 
                + ",  " + Utilities.GetDateTimeForDML(databaseType, dateMeTenderReceived,true,true) 
                + ",   " +  Utilities.GetBooleanForDML(databaseType, flgMeTenderDue) 
                + ",   " + Utilities.GetDateTimeForDML(databaseType, dateMeTenderDue,true,true) 
                + ",   " + Utilities.GetBooleanForDML(databaseType, flgMeTenderSent) + ",   " + Utilities.GetDateTimeForDML(databaseType, dateMeTenderSent,true,true) 
                + ",   " + Utilities.GetBooleanForDML(databaseType, flgMeApproved) + ",   " + Utilities.GetDateTimeForDML(databaseType, dateMeApproved,true,true)
                + ",   " + Utilities.GetBooleanForDML(databaseType, flgMeUserDate1) + ",   " + Utilities.GetDateTimeForDML(databaseType, dateMeUserDate1,true,true) 
                + ",  " +  createdBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true)
                                     + ",  " + lastAmendedBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
                   
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, long sequence,  long jobSequence,  string meUserText1,  long meUserCombo1,  long meScheduleCount,  
                                    bool flgMeFinalised, string meNotes,  bool flgMeEnquiry, DateTime? dateMeEnquiry, bool flgMeTenderReceived, 
                                    DateTime? dateMeTenderReceived, bool flgMeTenderDue, DateTime? dateMeTenderDue, bool flgMeTenderSent, 
                                    DateTime? dateMeTenderSent, bool flgMeApproved, DateTime? dateMeApproved, bool flgMeUserDate1, 
                                    DateTime? dateMeUserDate1, long createdBy,  DateTime? dateCreated, long lastAmendedBy,  DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                    returnValue = "UPDATE   un_orders_me_header" +
                                "   SET   sequence =  " +  sequence + ",  " + 
		                        " job_sequence =  " +  jobSequence + ",  " + 
		                        " me_user_text1 =  '" +  meUserText1 + "',  " + 
		                        " me_user_combo1 =  " +  meUserCombo1 + ",  " + 
		                        " me_schedule_count =  " +  meScheduleCount + ",  " + 
		                        " flg_me_finalised = " +  Utilities.GetBooleanForDML(databaseType, flgMeFinalised) + ",  " + 
		                        " me_notes =  '" +  meNotes + "',  " + 
		                        " flg_me_enquiry = " + Utilities.GetBooleanForDML(databaseType, flgMeEnquiry) + ",  " + 
		                        " date_me_enquiry =  " +  Utilities.GetDateTimeForDML(databaseType, dateMeEnquiry,true,true) + ", " + 
		                        " flg_me_tender_received = " + Utilities.GetBooleanForDML(databaseType, flgMeTenderReceived) + ",  " + 
		                        " date_me_tender_received =  " +  Utilities.GetDateTimeForDML(databaseType, dateMeTenderReceived,true,true) + ", " + 
		                        " flg_me_tender_due = " + Utilities.GetBooleanForDML(databaseType, flgMeTenderDue) + ",  " + 
		                        " date_me_tender_due =  " + Utilities.GetDateTimeForDML(databaseType, dateMeTenderDue,true,true) + ", " + 
		                        " flg_me_tender_sent = " + Utilities.GetBooleanForDML(databaseType, flgMeTenderSent) + ",  " + 
		                        " date_me_tender_sent =  " + Utilities.GetDateTimeForDML(databaseType, dateMeTenderSent,true,true) + ", " + 
		                        " flg_me_approved = " + Utilities.GetBooleanForDML(databaseType, flgMeApproved) + ",  " + 
		                        " date_me_approved =  " + Utilities.GetDateTimeForDML(databaseType, dateMeApproved,true,true) + ", " + 
		                        " flg_me_user_date1 = " + Utilities.GetBooleanForDML(databaseType, flgMeUserDate1) + ",  " + 
		                        " date_me_user_date1 =  " + Utilities.GetDateTimeForDML(databaseType, dateMeUserDate1,true,true) + ", " + 
		                        " created_by =  " +  createdBy + ",  " + 
		                        " date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " + 
		                        " last_amended_by =  " +  lastAmendedBy + ",  " + 
		                        " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " + 
		                        "  WHERE sequence = " + sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
              
                returnValue = "DELETE FROM   un_orders_me_header" +
                                     "WHERE sequence = " + sequence;
            } 
       
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_orders_me_header" +
                                  "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, true)
                                  +" WHERE sequence = " + sequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

