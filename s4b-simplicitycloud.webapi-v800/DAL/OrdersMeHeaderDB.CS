using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrdersMeHeaderDB : MainDB
    {

        public OrdersMeHeaderDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertOrdersMeHeader(out long sequence, long jobSequence, string meUserText1, long meUserCombo1, long meScheduleCount,
                                         bool flgMeFinalised, string meNotes, bool flgMeEnquiry, DateTime? dateMeEnquiry, bool flgMeTenderReceived,
                                         DateTime? dateMeTenderReceived, bool flgMeTenderDue, DateTime? dateMeTenderDue, bool flgMeTenderSent,
                                         DateTime? dateMeTenderSent, bool flgMeApproved, DateTime? dateMeApproved, bool flgMeUserDate1,
                                         DateTime? dateMeUserDate1, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrdersMeHeaderQueries.insert(this.DatabaseType, jobSequence, meUserText1, meUserCombo1, meScheduleCount,
                                                                      flgMeFinalised, meNotes, flgMeEnquiry, dateMeEnquiry, flgMeTenderReceived,
                                                                      dateMeTenderReceived, flgMeTenderDue, dateMeTenderDue, flgMeTenderSent,
                                                                      dateMeTenderSent, flgMeApproved, dateMeApproved, flgMeUserDate1, dateMeUserDate1,
                                                                      createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(sql, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                sequence = long.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.'" + METHOD_NAME + "'\n";
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public OrdersMeHeader selectAllOrdersMeHeaderSequence(long sequence, bool incOrder)
        {
            OrdersMeHeader returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersMeHeaderQueries.getSelectAllBySequenceWithCategory(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new OrdersMeHeader();
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    returnValue = Load_OrdersMeHeader(dr);
                                    if(incOrder)
                                    {
                                        DatabaseInfo dbInfo = new DatabaseInfo();
                                        dbInfo.DatabaseType = this.DatabaseType;
                                        dbInfo.ConnectionString = this.connectionString;
                                        returnValue.Order = new OrdersDB(dbInfo).getOrderByJobSequence(returnValue.JobSequence ?? 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

		public OrdersMeHeader selectAllOrdersMeHeaderByJobSequence(long jobSequence)
		{
			OrdersMeHeader returnValue = null;
			try
			{
				using (OleDbConnection conn = this.getDbConnection())
				{
					using (OleDbCommand objCmdSelect =
						new OleDbCommand(OrdersMeHeaderQueries.getSelectAllByJobSequence(this.DatabaseType, jobSequence), conn))
					{
						using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
						{
							if (dr.HasRows)
							{
								returnValue = new OrdersMeHeader();
								if (dr.HasRows)
								{
									dr.Read();
									returnValue = Load_OrdersMeHeader(dr);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return returnValue;
		}
		public bool updateBySequence(long sequence, long jobSequence, string meUserText1, long meUserCombo1, long meScheduleCount, bool flgMeFinalised,
                                     string meNotes, bool flgMeEnquiry, DateTime? dateMeEnquiry, bool flgMeTenderReceived, DateTime? dateMeTenderReceived,
                                     bool flgMeTenderDue, DateTime? dateMeTenderDue, bool flgMeTenderSent, DateTime? dateMeTenderSent, bool flgMeApproved,
                                     DateTime? dateMeApproved, bool flgMeUserDate1, DateTime? dateMeUserDate1, long createdBy, DateTime? dateCreated,
                                     long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersMeHeaderQueries.update(this.DatabaseType, sequence, jobSequence, meUserText1, meUserCombo1, meScheduleCount,
                                                                      flgMeFinalised, meNotes, flgMeEnquiry, dateMeEnquiry, flgMeTenderReceived,
                                                                      dateMeTenderReceived, flgMeTenderDue, dateMeTenderDue, flgMeTenderSent,
                                                                      dateMeTenderSent, flgMeApproved, dateMeApproved, flgMeUserDate1, dateMeUserDate1,
                                                                      createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteBySequence(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersMeHeaderQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteByFlgDeleted(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersMeHeaderQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        private OrdersMeHeader Load_OrdersMeHeader(OleDbDataReader dr)
        {
            OrdersMeHeader ordersMeHeader = null;
            try
            {
                if (dr != null)
                {
                    ordersMeHeader = new OrdersMeHeader();
                    ordersMeHeader.Sequence = long.Parse(dr["sequence"].ToString());
                    ordersMeHeader.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    ordersMeHeader.MeUserText1 = Utilities.GetDBString(dr["me_user_text1"]);
                    ordersMeHeader.MeUserCombo1 = long.Parse(dr["me_user_combo1"].ToString());
                    ordersMeHeader.MeScheduleCount = long.Parse(dr["me_schedule_count"].ToString());
                    ordersMeHeader.FlgMeFinalised = bool.Parse(dr["flg_me_finalised"].ToString());
                    ordersMeHeader.MeNotes = Utilities.GetDBString(dr["me_notes"]);
                    ordersMeHeader.FlgMeEnquiry = bool.Parse(dr["flg_me_enquiry"].ToString());
                    ordersMeHeader.DateMeEnquiry = Utilities.getDBDate(dr["date_me_enquiry"]);
                    ordersMeHeader.FlgMeTenderReceived = bool.Parse(dr["flg_me_tender_received"].ToString());
                    ordersMeHeader.DateMeTenderReceived = Utilities.getDBDate(dr["date_me_tender_received"]);
                    ordersMeHeader.FlgMeTenderDue = bool.Parse(dr["flg_me_tender_due"].ToString());
                    ordersMeHeader.DateMeTenderDue = Utilities.getDBDate(dr["date_me_tender_due"]);
                    ordersMeHeader.FlgMeTenderSent = bool.Parse(dr["flg_me_tender_sent"].ToString());
                    ordersMeHeader.DateMeTenderSent = Utilities.getDBDate(dr["date_me_tender_sent"]);
                    ordersMeHeader.FlgMeApproved = bool.Parse(dr["flg_me_approved"].ToString());
                    ordersMeHeader.DateMeApproved = Utilities.getDBDate(dr["date_me_approved"]);
                    ordersMeHeader.FlgMeUserDate1 = bool.Parse(dr["flg_me_user_date1"].ToString());
                    ordersMeHeader.DateMeUserDate1 = Utilities.getDBDate(dr["date_me_user_date1"]);
                    ordersMeHeader.MeProjectTitle = this.ColumnExists(dr, "me_project_title") ? (dr["me_project_title"] == null || dr["me_project_title"] == DBNull.Value) ? "" : dr["me_project_title"].ToString() : string.Empty;
                    ordersMeHeader.FlgSpecShowClient = this.ColumnExists(dr, "flg_spec_show_client") ? bool.Parse(dr["flg_spec_show_client"].ToString()) : false;
                    ordersMeHeader.FlgSpecShowJobAddress = this.ColumnExists(dr, "flg_spec_show_job_address") ? bool.Parse(dr["flg_spec_show_job_address"].ToString()) : false;
                    ordersMeHeader.CategorySequence = this.ColumnExists(dr, "category_sequence") ? Int32.Parse(dr["category_sequence"].ToString()) : -1;
                    ordersMeHeader.SpecNotes = this.ColumnExists(dr, "spec_notes") ? (dr["spec_notes"] == null || dr["spec_notes"] == DBNull.Value) ? "" : dr["me_project_title"].ToString() : string.Empty;
                    ordersMeHeader.CreatedBy = long.Parse(dr["created_by"].ToString());
                    ordersMeHeader.DateCreated = Utilities.getDBDate(dr["date_created"]);
                    ordersMeHeader.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    ordersMeHeader.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);

                    //Ref Category
                    if(ordersMeHeader.CategorySequence>-1)
                    { 
                        RefOrdTenderCategories refOrdTenderCategories = new RefOrdTenderCategories();
                        refOrdTenderCategories.CategorySequence = ordersMeHeader.CategorySequence;
                        refOrdTenderCategories.CategoryDesc = this.ColumnExists(dr, "category_desc") ? (dr["category_desc"] == null || dr["category_desc"] == DBNull.Value) ? "" : dr["category_desc"].ToString() : string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return ordersMeHeader;
        }
    }
}
