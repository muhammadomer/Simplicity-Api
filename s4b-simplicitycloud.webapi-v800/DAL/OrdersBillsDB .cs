using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrdersBillsDB : MainDB
    {

        public OrdersBillsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertOrdersBills(out long sequence, OrdersBills obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrdersBillsQueries.insert(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool updateBySequence(OrdersBills obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersBillsQueries.update(this.DatabaseType, obj), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool updateInvoiceNoAndSetToJTDateBySequence(long sequence, string invoiceNo,
                                                            DateTime? setToJTDate, int lastAmendedBy, DateTime? lastAmendedDate)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrdersBillsQueries.updateInvoiceNoAndSetToJTDateBySequence(this.DatabaseType, sequence, 
                                        invoiceNo, setToJTDate, lastAmendedBy, lastAmendedDate), conn))
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
                        new OleDbCommand(OrdersBillsQueries.delete(this.DatabaseType, sequence), conn))
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
                        new OleDbCommand(OrdersBillsQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
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

        public bool getOrderValidForRequest(long jobSequence)
        {
            bool returnValue = true;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getOrderValidForRequest(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows) //---If Record exist then can not insert another request for this order
                            {   
                                returnValue = false;
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

        public OrdersBills selectOrdersBillsBySequence(long billSequence)
        {
            OrdersBills returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectBySequence(this.DatabaseType, billSequence,-1), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = Load_OrdersBills(dt.Rows[0]);
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

        public OrdersBills selectOrderBillForEditingBySequence(long billSequence,long jobSequence)
        {
            OrdersBills returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectBySequence(this.DatabaseType, billSequence,jobSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {  
                            returnValue = Load_OrdersBills(dt.Rows[0]);
                            DatabaseInfo dbInfo = new DatabaseInfo();
                            dbInfo.DatabaseType = this.DatabaseType;
                            dbInfo.ConnectionString = this.connectionString;
                            OrdersBillsItemsDB orderBillsItemsDB = new OrdersBillsItemsDB(dbInfo);
                            long jobSeq = Convert.ToInt64( dt.Rows[0][1].ToString());
                            long billSeq = Convert.ToInt64(dt.Rows[0][0].ToString());
                            returnValue.OrderBillItems = orderBillsItemsDB.selectOrderBillItemsForEditingByBillSequence(billSeq, jobSeq);
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

        public SaleInvoices selectSaleInvoiceBySequence(long billSequence)
        {
            SaleInvoices returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectSaleInvoiceBySequence(this.DatabaseType, billSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = Load_SaleInvoice(dt.Rows[0]);
                            DatabaseInfo dbInfo = new DatabaseInfo();
                            dbInfo.DatabaseType = this.DatabaseType;
                            dbInfo.ConnectionString = this.connectionString;
                            OrdersBillsItemsDB orderBillsItemsDB = new OrdersBillsItemsDB(dbInfo);
                            long billSeq = Convert.ToInt64(dt.Rows[0][0].ToString());
                            returnValue.OrderBillItems = orderBillsItemsDB.selectAllOrderBillItemsByBillSequence(billSeq);
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

        public List<OrdersBills> selectOrdersBillsByJobSequenceAndType(long sequence,string type)
        {
            List<OrdersBills> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectBySequenceAndType(this.DatabaseType, sequence,type), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrdersBills>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OrdersBills(row));
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


        public List<OrdersBills> selectOrdersBillInvoiceByBillSequence(long sequence)
        {
            List<OrdersBills> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectInvoiceBySequence(this.DatabaseType, sequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrdersBills>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OrdersBills(row));
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

        public List<OrdersBills> selectOrdersApplicationForPaymentAndInvoices(long jobSequence)
        {
            List<OrdersBills> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectOrderAppForPaymentAndInvoices(this.DatabaseType,jobSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrdersBills>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OrdersBills(row));
                            }
                        }
                        else
                        {
                            ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
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

        

        public List<OrdersBills> selectAllOrdersBillsByJobSequence(long jobSequence)
        {
            List<OrdersBills> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectAllByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrdersBills>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OrdersBills(row));
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

        public List<OrdersBills> selectListOfSaleInvoices(DateTime? fromDate,DateTime? toDate)
        {
            List<OrdersBills> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectAllSaleInvoices(this.DatabaseType, fromDate,toDate), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrdersBills>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OrdersBills(row));
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
        public List<OrdersBills> selectListOfAppForPayments(ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate, out int count, bool isCountRequired)
        {
            List<OrdersBills> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectListOfAppForPayments(this.DatabaseType, clientRequest, fromDate, toDate), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        if (isCountRequired)
                        {
                            count = da.Fill(new DataSet("temp"));
                        }
                        DataTable dt = new DataTable();
                        da.Fill(clientRequest.first, clientRequest.rows, dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrdersBills>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OrdersBills(row));
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

        private OrdersBills Load_OrdersBills(DataRow row)
        {
            OrdersBills ordersBills = null;
            if (row != null)
            {
                ordersBills = new OrdersBills();
                ordersBills.Sequence = DBUtil.GetLongValue(row, "sequence");
                ordersBills.JobSequence = DBUtil.GetLongValue(row, "job_sequence");
                ordersBills.JobRef = DBUtil.GetStringValue(row, "job_ref");
                ordersBills.BillRef = DBUtil.GetStringValue(row, "bill_ref");
                ordersBills.ClientRef = DBUtil.GetStringValue(row, "client_ref");
                ordersBills.EntryType = DBUtil.GetStringValue(row, "EntryType");
                ordersBills.ClientId = DBUtil.GetLongValue(row, "client_id");
                ordersBills.ClientName = DBUtil.GetStringValue(row, "job_client_name");
                ordersBills.ClientParent = DBUtil.GetStringValue(row, "parent_name");
                ordersBills.EntityJoinId = DBUtil.GetLongValue(row, "entity_join_id");
                ordersBills.FlgParentOverride = DBUtil.GetBooleanValue(row, "flg_parent_override");
                ordersBills.InvoiceNo = DBUtil.GetStringValue(row, "invoice_no");
                ordersBills.InvoiceDate = DBUtil.GetDateValue(row, "invoice_date");
                ordersBills.PcentRetention = DBUtil.GetDoubleValue(row, "pcent_retention");
                ordersBills.JobDate = DBUtil.GetDateValue(row, "job_date");
                ordersBills.FlgJobDateStart = DBUtil.GetBooleanValue(row, "flg_job_date_start");
                ordersBills.FlgJobDateFinish = DBUtil.GetBooleanValue(row, "flg_job_date_finish");
                ordersBills.JobDateFinish = DBUtil.GetDateValue(row,"job_date_finish");
                ordersBills.MaillingAddress = DBUtil.GetStringValue(row,"mailling_address");
                ordersBills.EmailAddress = DBUtil.GetStringValue(row,"email_address");
                ordersBills.Footnote = DBUtil.GetStringValue(row,"footnote");
                ordersBills.FlgRequestMade = DBUtil.GetBooleanValue(row,"flg_request_made");
                ordersBills.RequestMadeDate = DBUtil.GetDateValue(row,"request_made_date");
                ordersBills.FlgSetToProforma = DBUtil.GetBooleanValue(row,"flg_set_to_proforma");
                ordersBills.SetToProformaDate = DBUtil.GetDateValue(row,"set_to_proforma_date");
                ordersBills.SageId = DBUtil.GetLongValue(row,"sage_id");
                ordersBills.FlgSetToInvoice = DBUtil.GetBooleanValue(row,"flg_set_to_invoice");
                ordersBills.SetToInvoiceDate = DBUtil.GetDateValue(row,"set_to_invoice_date");
                ordersBills.InvoiceIndex = DBUtil.GetLongValue(row,"invoice_index");
                ordersBills.FlgHasAVatInv = DBUtil.GetBooleanValue(row,"flg_has_a_vat_inv");
                ordersBills.FlgIsVatInv = DBUtil.GetBooleanValue(row,"flg_is_vat_inv");
                ordersBills.JoinBillSequence = DBUtil.GetLongValue(row,"join_bill_sequence");
                ordersBills.RciId = DBUtil.GetLongValue(row,"rci_id");
                ordersBills.AmountInitial = DBUtil.GetDoubleValue(row,"amount_initial");
                ordersBills.AmountDiscount = DBUtil.GetDoubleValue(row,"amount_discount");
                ordersBills.PcentRetention = DBUtil.GetDoubleValue(row,"pcent_retention");
                ordersBills.AmountRetention = DBUtil.GetDoubleValue(row,"amount_retention");
                ordersBills.AmountSubTotal = DBUtil.GetDoubleValue(row,"amount_sub_total");
                ordersBills.AmountVat = DBUtil.GetDoubleValue(row,"amount_vat");
                ordersBills.AmountCis = DBUtil.GetDoubleValue(row,"amount_cis");
                ordersBills.AmountTotal = DBUtil.GetDoubleValue(row,"amount_total");
                ordersBills.OutstandingAmt = DBUtil.GetDoubleValue(row, "outstanding");
                ordersBills.AllocatedAmt = DBUtil.GetDoubleValue(row, "entry_amt_allocated_total");
                ordersBills.CreatedBy = DBUtil.GetIntValue(row,"created_by");
                ordersBills.DateCreated = DBUtil.GetDateValue(row,"date_created");
                ordersBills.LastAmendedBy = DBUtil.GetIntValue(row,"last_amended_by");
                ordersBills.DateLastAmended = DBUtil.GetDateValue(row,"date_last_amended");
                ordersBills.FlgArchive = DBUtil.GetBooleanValue(row,"flg_archive");
                ordersBills.Prospect = "Not Set";
                ordersBills.NameSage = DBUtil.GetStringValue(row, "name_sage");
                ordersBills.PaymentType = DBUtil.GetStringValue(row, "entity_pymt_desc");
            }
            return ordersBills;
        }

        private SaleInvoices Load_SaleInvoice(DataRow row)
        {
            SaleInvoices inv = null;
            if (row != null)
            {
                inv = new SaleInvoices();
                inv.Sequence = DBUtil.GetLongValue(row, "sequence");
                inv.JobReference = DBUtil.GetStringValue(row, "job_ref");
                inv.ClientRef = DBUtil.GetStringValue(row, "job_client_ref");
                inv.ClientName = DBUtil.GetStringValue(row, "job_client_name");
                inv.FromAddress = DBUtil.GetStringValue(row, "address_full");
                inv.ToAddress = DBUtil.GetStringValue(row, "mailling_address");
                inv.InvoiceNo = DBUtil.GetStringValue(row, "invoice_no");
                inv.DateRaised = DBUtil.GetDateValue(row, "set_to_invoice_date");
                inv.StartDate = DBUtil.GetDateValue(row, "job_date_start");
                inv.FinishDate = DBUtil.GetDateValue(row, "job_date_finish");
                inv.JobAddress = DBUtil.GetStringValue(row, "job_address");
                inv.JobDescription = DBUtil.GetStringValue(row, "job_desc");
                inv.JobCostCentre = DBUtil.GetStringValue(row, "job_cost_centre");
                inv.VatReg = DBUtil.GetStringValue(row, "vat_reg");
                inv.TradeCode = DBUtil.GetStringValue(row, "trade_desc");
                inv.Footnote = DBUtil.GetStringValue(row, "footnote");
                inv.WorkCenter = DBUtil.GetStringValue(row, "work_center");
                inv.AmountInitial = DBUtil.GetDoubleValue(row, "amount_initial");
                inv.AmountDiscount = DBUtil.GetDoubleValue(row, "amount_discount");
                inv.PcentRetention = DBUtil.GetDoubleValue(row, "pcent_retention");
                inv.AmountRetention = DBUtil.GetDoubleValue(row, "amount_retention");
                inv.AmountSubTotal = DBUtil.GetDoubleValue(row, "amount_sub_total");
                inv.AmountVat = DBUtil.GetDoubleValue(row, "amount_vat");
                inv.AmountCis = DBUtil.GetDoubleValue(row, "amount_cis");
                inv.AmountTotal = DBUtil.GetDoubleValue(row, "amount_total");
            }
            return inv;
        }

    }
}
