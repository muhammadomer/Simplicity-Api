using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrderMeSchHeaderQueries
    {

       
      public static string insert(string databaseType, OrdersMeSchHeader oi)
        {
            return @"INSERT INTO un_orders_me_sch_header(join_sequence,job_sequence,flg_deleted ,flg_finalised
           ,me_version_no,date_me_version_no,me_version_option,me_version_notes,cee_item_count,pack_sequence
		   ,flg_order_tender , date_order_tender
           ,created_by,date_created ,last_amended_by,date_last_amended)
           VALUES(" +  oi.joinSequence +"," + oi.JobSequence 
			+ "," + Utilities.GetBooleanForDML(databaseType, oi.FlgDeleted)+ "," + Utilities.GetBooleanForDML(databaseType, oi.FlgFinalised) 
			+ ",'" + oi.MeVersionNo  + "'," + Utilities.GetDateTimeForDML(databaseType, oi.DateMeVersionNo,true,false) 
			+ ",'" + oi.MeVersionOption + "','" + oi.MeVersionNotes + "'," + oi.CeeItemCount + "," + oi.PackSequence 
			+ "," + Utilities.GetBooleanForDML(databaseType, oi.FlgOrdTender) + ","+ Utilities.GetDateTimeForDML(databaseType, oi.DateOrderTender,true,false) 
			+ ","  +  oi.CreatedBy + "," + Utilities.GetDateTimeForDML(databaseType, oi.DateCreated, true, true)
			+ "," + oi.LastAmendedBy + "," + Utilities.GetDateTimeForDML(databaseType, oi.DateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, OrdersMeSchHeader OrderItem)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE   un_orders_me_sch_header" +
                            "   SET  job_sequence =  " + OrderItem.JobSequence + ",  " +
							"        join_sequence = " + OrderItem.joinSequence +", " +
							"        flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgDeleted) + ",  " +
							"        flg_finalised =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgFinalised) + ",  " +
							"        me_version_no =  '" + OrderItem.MeVersionNo + "',  " +
							"        date_me_version_no =  " + Utilities.GetDateTimeForDML(databaseType, OrderItem.DateMeVersionNo,true,false) + ",  " +
							"        me_version_option =  '" + OrderItem.MeVersionOption + "',  " +
							"        me_version_notes =  '" + OrderItem.MeVersionNotes + "',  " +
							"        cee_item_count =  " + OrderItem.CeeItemCount + ",  " +
							"        pack_sequence  =  " + OrderItem.PackSequence + ",  " +
							"        flg_order_tender =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgOrdTender) + "',  " +
							"        date_order_tender =  " + Utilities.GetDateTimeForDML(databaseType,  OrderItem.DateOrderTender,true,false) + ",  " +
                            "        last_amended_by =  " + OrderItem.LastAmendedBy + ", " +
                            "        date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, OrderItem.DateLastAmended,true,true) + 
                            "  WHERE sequence = " + OrderItem.Sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


       
    }
}

