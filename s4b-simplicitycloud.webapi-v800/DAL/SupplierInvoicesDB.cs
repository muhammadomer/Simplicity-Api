using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using SimplicityOnlineWebApi.Models;
using System.Reflection.Metadata;
using System.Transactions;
using SimplicityOnlineWebApi.Models.ViewModels;
using System.Linq;
using Universal.Common.Reflection.Emit.Extensions;

namespace SimplicityOnlineWebApi.DAL
{
    public class SupplierInvoicesDB : MainDB
    {
        protected readonly DatabaseInfo DbInfo;

        public SupplierInvoicesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
            this.DbInfo = dbInfo;
        }
        public List<SupplierInvoiceVM> selectRossumUnfinalizedInvoices(ClientRequest clientRequest, out int count, bool isCountRequired)
        {
            List<SupplierInvoiceVM> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(SupplierInvoicesQueries.getSelectAllRossumUnfinalizedInvoices(clientRequest), conn))
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
                            returnValue = new List<SupplierInvoiceVM>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_Invoices(row));
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
        public List<SupplierInvoiceVM> selectUnfinalizedInvoices(ClientRequest clientRequest, out int count, bool isCountRequired)
        {
            List<SupplierInvoiceVM> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(SupplierInvoicesQueries.getSelectAllUnfinalizedInvoices(clientRequest), conn))
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
                            returnValue = new List<SupplierInvoiceVM>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_Invoices(row));
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
        public SupplierInvoiceVM selectItemisedInvoice(string invoiceNo)
        {
            SupplierInvoiceVM returnValue = null;
            string qryInvoice = "";
            try
            {
                qryInvoice = @"select case when trans_type='D' then 'supplier' else (case when trans_type ='C' then 
	                (case when (select count(*) from 
	                (SELECT sup.data FROM un_entity_details_supplementary sup WHERE sup.entity_id = inv.contact_id AND sup.data_type='021') as aa)>1 
	                then 'sub-contractor' else 'contractor' end) else '' end) end suppliertype, 
                    * from un_invoice_itemised inv
                                left outer join un_rossum_files rosum on inv.rossum_sequence = rosum.sequence where invoice_no = '" + invoiceNo + "'";
                returnValue = Load_InvoiceItemised(qryInvoice,true);
                if (returnValue != null)
                {
                    qryInvoice = @"select cost.costcentre_desc,reg.vehicle_reg,items.* from un_invoice_itemised_items items 
                                    left outer join un_cost_centres cost on items.cost_centre_id = cost.costcentre_id  
                                    left join 
                                    (SELECT ar.sequence AS asset_sequence, arsv.vehicle_reg
                                      FROM un_asset_register_supp_vehicles AS arsv
                                     INNER JOIN un_asset_register AS ar
                                        ON arsv.join_sequence = ar.sequence)reg on items.asset_sequence=reg.asset_sequence 
                                    where items.invoice_sequence='" + returnValue.Sequence + "'";
                    returnValue.InvoiceLines = getInvoiceItemisedItems(qryInvoice,true);
                }  
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public bool SaveInvoice(InvoiceItemised invoice)
        {
            Boolean returnValue = false;
            long sequence = -1;
            string qryInvoice = "";
            if (invoice.Sequence == 0)
            {
                string qry = $"select * from un_invoice_itemised " +
                    $"where invoice_no='{invoice.InvoiceNo}' and contact_id = " + invoice.ContactId;

                bool invoiceNoExist = invoiceNoAlreadyExist(qry);
                if (invoiceNoExist)
                {
                    throw new InvalidExpressionException("InvoiceNo. already exist!");
                }
                qryInvoice = "insert into un_invoice_itemised " +
                    " (itemised_details,trans_type, contact_id, invoice_no, itemised_date, sum_amt_main, " +
                    "sum_amt_vat,sum_amt_labour, sum_amt_discount, sum_amt_subtotal, flg_inc_vat,  " +
                    "sum_amt_total,flg_checked, date_checked, created_by, rossum_po_no, rossum_dn_no, rossum_sequence ) " +
                    "VALUES ('" +
                    Utilities.GetDBString(invoice.ItemisedDetail)  + "', '" +
                    invoice.TransType + "', " +
                    invoice.ContactId + ", '" +
                    invoice.InvoiceNo + "', " +
                    Utilities.GetDateTimeForDML(DatabaseType, invoice.ItemisedDate, true, true) + "," +
                    invoice.SumAmtMain + ", " +
                    invoice.SumAmtVAT + ", " +
                    invoice.SumAmtLabour + ", " +
                    invoice.SumAmtDiscount + ", " +
                    invoice.SumAmtSubTotal + ", " +
                    Utilities.GetBooleanForDML(DatabaseType, invoice.FlgIncVAT) + "," +
                    invoice.SumAmtTotal + ", " +
                    Utilities.GetBooleanForDML(DatabaseType, invoice.FlgChecked) + "," +
                    Utilities.GetDateTimeForDML(DatabaseType, invoice.DateChecked, true, true) + "," +
                    invoice.CreatedBy + ",'" +
                    Utilities.GetDBString(invoice.RossumPurchaseOrderoNo) + "', '" +
                    Utilities.GetDBString(invoice.RossumDeliveryNotNo) + "', '" +
                    invoice.RossumFileSequence + "')";
            }
            else
            {
                //qryInvoice = $"select * from un_invoice_itemised where invoice_no='{invoice.InvoiceNo}' and sequence <> '{invoice.Sequence}'";
                //bool invoiceNoExist = invoiceNoAlreadyExist(qryInvoice);
                //if (invoiceNoExist)
                //{
                //    throw new InvalidExpressionException("InvoiceNo. already exist!");
                //}
                qryInvoice = $"update un_invoice_itemised " +
                      $"set itemised_details='{Utilities.GetDBString(invoice.ItemisedDetail)}', " +
                      $"trans_type='{Utilities.GetDBString(invoice.TransType)}', " +
                      $"contact_id={invoice.ContactId}, " +
                      $"invoice_no='{Utilities.GetDBString(invoice.InvoiceNo)}', " +
                      $"itemised_date={Utilities.GetDateValueForDML(DatabaseType, invoice.ItemisedDate)}, " +
                      $"sum_amt_main={invoice.SumAmtMain}, " +
                      $"sum_amt_labour={invoice.SumAmtLabour}, " +
                      $"sum_amt_discount='{invoice.SumAmtDiscount}', " +
                      $"sum_amt_subtotal='{invoice.SumAmtSubTotal}', " +
                      $"sum_amt_vat='{invoice.SumAmtVAT}', " +
                      $"sum_amt_total='{invoice.SumAmtTotal}' " +
                      $"last_amended_by='{invoice.LastAmendedBy}' " +
                      $"date_last_amended='{invoice.DateLastAmended}' " +
                      $"where sequence = '{invoice.Sequence}'";
            }
            try
            {
                using (OleDbConnection conn = getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(qryInvoice, conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            List<SupplierInvoiceItemsVM> itemisedItems = null;
                            if (invoice.Sequence == 0)
                            {
                                sequence = Utilities.GetDBAutoNumber(conn);
                                invoice.Sequence = sequence;
                            }
                            else
                            {
                                itemisedItems = new List<SupplierInvoiceItemsVM>();
                                qryInvoice = @"select * from un_invoice_itemised_items where invoice_sequence='" + invoice.Sequence + "'";
                                itemisedItems = getInvoiceItemisedItems(qryInvoice,false);
                                foreach (var existingChild in itemisedItems)
                                {
                                    if (!invoice.InvoiceLines.Any(c => c.Sequence == existingChild.Sequence))
                                    {
                                        qryInvoice = @"delete from un_invoice_itemised_items where sequence="+existingChild.Sequence;
                                        removeInvoiceItemisedItems(qryInvoice);
                                    }
                                }
                            }
                            qryInvoice = "";
                            foreach (var item in invoice.InvoiceLines)
                            {
                                if (item.Sequence == 0)
                                {
                                    qryInvoice = "insert into un_invoice_itemised_items(invoice_sequence,import_type,import_type_ref, item_date, " +
                                        "item_quantity, item_ref, stock_code, item_desc, item_unit," +
                                                  "item_amt,item_amt_labour, item_discount_percent,item_amt_discount,item_amt_subtotal,item_vat_percent,item_amt_vat,item_amt_total," +
                                                  "created_by,date_created,item_type,tel_sequence,flg_job_seq_exclude,cost_centre_id,flg_checked,sage_nominal_code,sage_tax_code,asset_sequence)" +
                                                "VALUES ("+ 
                                                invoice.Sequence + ", "+
                                                item.ImportType+",'"+
                                                Utilities.GetDBString(item.ImportTypeRef)+"'," + 
                                                Utilities.GetDateValueForDML(DatabaseType, item.ItemDate) + ", " + 
                                                item.ItemQuantity + ", '" +
                                                Utilities.GetDBString(item.ItemRef) + "', '" +
                                                Utilities.GetDBString(item.StockCode) + "', '" +
                                                Utilities.GetDBString(item.ItemDesc) + "','" +
                                                Utilities.GetDBString(item.ItemUnit) + "'," +
                                                item.ItemAmt + "," +
                                                item.ItemAmtLabour + "," +
                                                item.ItemDiscountPercent + "," + 
                                                item.ItemAmtDiscount + "," + 
                                                item.ItemAmtSubTotal + "," + 
                                                item.ItemVATPercent + "," + 
                                                item.ItemAmtVAT +"," + 
                                                item.ItemAmtTotal +","+
                                                item.CreatedBy + "," + 
                                                Utilities.GetDateTimeForDML(DatabaseType, DateTime.Now, true, true) + "," +
                                                item.ItemType+"," +
                                                item.TelSequence+",'" +
                                                Utilities.GetBooleanForDML(DatabaseType, item.FlgJobSeqExclude) +"','"+
                                                Utilities.GetDBString(item.CostCentreId) + "','" +
                                                Utilities.GetBooleanForDML(DatabaseType,item.FlgChecked) + "','"+
                                                Utilities.GetDBString(item.SageNominalCode) + "','" +
                                                Utilities.GetDBString(item.SageTaxCode) +"','"+
                                                item.AssetSequence+"')";
                                }
                                else
                                {
                                    qryInvoice = $"update un_invoice_itemised_items set " +
                                        $"item_date={Utilities.GetDateValueForDML(DatabaseType, item.ItemDate)}, " +
                                        $"item_quantity={item.ItemQuantity}, " +
                                        $"item_ref='{Utilities.GetDBString(item.ItemRef)}', " +
                                        $"stock_code='{Utilities.GetDBString(item.StockCode)}', " +
                                        $"item_desc='{Utilities.GetDBString(item.ItemDesc)}', " +
                                        $"item_unit='{Utilities.GetDBString(item.ItemUnit)}'," +
                                        $"item_amt={item.ItemAmt}, " +
                                        $"item_amt_labour={item.ItemAmtLabour}," +
                                        $"item_discount_percent={item.ItemDiscountPercent}," +
                                        $"item_amt_discount={item.ItemAmtDiscount }," +
                                        $"item_amt_subtotal={item.ItemAmtSubTotal}," +
                                        $"item_vat_percent={item.ItemVATPercent}," +
                                        $"item_amt_vat={item.ItemAmtVAT}," +
                                        $"item_amt_total={item.ItemAmtTotal}," +
                                        $"import_type='{item.ImportType}'," +
                                        $"import_type_ref='{Utilities.GetDBString(item.ImportTypeRef)}'," +
                                        $"item_type={item.ItemType}," +
                                        $"tel_sequence='{item.TelSequence}'," +
                                        $"flg_job_seq_exclude='{Utilities.GetBooleanForDML(DatabaseType,item.FlgJobSeqExclude)}'," +
                                        $"cost_centre_id='{Utilities.GetDBString(item.CostCentreId)}'," +
                                        $"flg_checked='{Utilities.GetBooleanForDML(DatabaseType,item.FlgChecked)}', " +
                                        $"sage_nominal_code='{Utilities.GetDBString(item.SageNominalCode)}'," +
                                        $"sage_tax_code='{Utilities.GetDBString(item.SageTaxCode)}', " +
                                        $"asset_sequence='{item.AssetSequence}' " +
                                        $"where sequence= {item.Sequence}";
                                }
                                objCmd.CommandText = qryInvoice; //NOTE: insert / update one by one. MS access doesn't support multiple insert statements in one go.
                                result = objCmd.ExecuteNonQuery();
                            }
                            returnValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
                {
                    returnValue = false;
                    throw ex;
                }
            return returnValue;
        }
        public bool UpdateInvoiceSupplier(InvoiceItemised invoice)
        {
            string qryInvoice = $"update un_invoice_itemised set contact_id={invoice.ContactId} where sequence = '{invoice.Sequence}'";
            return updateInvoiceSupplier(qryInvoice);
        }
        private bool invoiceNoAlreadyExist(string qry)
        {
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdSelect =new OleDbCommand(qry, conn))
                {
                    OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows != null && dt.Rows.Count > 0)
                        return true;
                }
            }
            return false;
        }  
        private bool updateInvoiceSupplier(string qry)
        {
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmd =
                    new OleDbCommand(qry, conn))
                {
                    int result = objCmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public List<VehicleViewModel> selectVehicle()
        {
            List<VehicleViewModel> returnValue = null;
            string qryVehicle = "";
            try
            {
                qryVehicle = @"SELECT ar.sequence AS asset_sequence, arsv.vehicle_reg,ar.flg_deleted
                                  FROM un_asset_register_supp_vehicles AS arsv
                                 INNER JOIN un_asset_register AS ar
                                    ON arsv.join_sequence = ar.sequence
                                 WHERE ar.flg_deleted <> 1";
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(qryVehicle, conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<VehicleViewModel>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_Vehicle(row));
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
        public List<ItemisedItemTelViewModel> selectItemsTel()
        {
            List<ItemisedItemTelViewModel> returnValue = null;
            string qryItemsTel = "";
            try
            {
                qryItemsTel = @"select * from un_invoice_itemised_item_tels";
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(qryItemsTel, conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<ItemisedItemTelViewModel>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_ItemisedItemTel(row));
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
        public List<CostCodeViewModel> selectCostCode()
        {
            List<CostCodeViewModel> returnValue = null;
            string qryCostCode = "";
            try
            {
                qryCostCode = @"select * from un_cost_centres";
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(qryCostCode, conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<CostCodeViewModel>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_ItemisedCostCode(row));
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
        private SupplierInvoiceVM Load_Invoices(DataRow row)
        {
            SupplierInvoiceVM invoiceItem = null;
            if (row != null)
            {
                invoiceItem = new SupplierInvoiceVM();
                invoiceItem.Sequence = DBUtil.GetLongValue(row, "sequence");
                invoiceItem.ItemisedDate = Convert.ToDateTime(row["itemised_date"]);
                invoiceItem.InvoiceNo = DBUtil.GetStringValue(row, "invoice_no");
                invoiceItem.TransType = DBUtil.GetStringValue(row, "trans_type");
                invoiceItem.LongName = DBUtil.GetStringValue(row, "name_long");
                invoiceItem.SageACRef = DBUtil.GetStringValue(row, "name_sage");
                invoiceItem.ShortName = DBUtil.GetStringValue(row, "name_short");
                invoiceItem.SubTotal = DBUtil.GetStringValue(row, "sum_amt_subtotal");
                invoiceItem.Total = DBUtil.GetStringValue(row, "sum_amt_total");
                invoiceItem.VAT = DBUtil.GetStringValue(row, "sum_amt_vat");
                invoiceItem.FileNameCabId = DBUtil.GetStringValue(row, "file_name_cab_id");
                invoiceItem.EntityId = DBUtil.GetLongValue(row, "entity_id");
                invoiceItem.DateCreated = Convert.ToDateTime(row["date_created"]);
                invoiceItem.Approved = DBUtil.GetStringValue(row, "approved");
                invoiceItem.RossumPONo = DBUtil.GetStringValue(row, "rossum_po_no");
                invoiceItem.JobReference = DBUtil.GetStringValue(row, "ord_job_reference");
                invoiceItem.JobManager = DBUtil.GetStringValue(row, "ord_job_manager_name");
            }
            return invoiceItem;
        }

        private SupplierInvoiceVM Load_InvoiceItemised(string qry,Boolean includeRossumFile)
        {
            SupplierInvoiceVM invoiceItem = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(qry, conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                dr.GetValue(0);
                                invoiceItem = new SupplierInvoiceVM();
                                invoiceItem.Sequence = DBUtil.GetLongValue(dr, "sequence");
                                invoiceItem.ItemisedDate = Convert.ToDateTime(dr["itemised_date"].ToString());
                                invoiceItem.InvoiceNo = DBUtil.GetStringValue(dr, "invoice_no");
                                invoiceItem.ItemisedDetail = DBUtil.GetStringValue(dr, "itemised_details");
                                invoiceItem.TransType = DBUtil.GetStringValue(dr, "trans_type");
                                invoiceItem.SumAmtMain = float.Parse(DBUtil.GetStringValue(dr, "sum_amt_main"));
                                invoiceItem.SumAmtTotal = float.Parse(DBUtil.GetStringValue(dr, "sum_amt_total"));
                                invoiceItem.SumAmtVAT = float.Parse(DBUtil.GetStringValue(dr, "sum_amt_vat"));
                                invoiceItem.DateCreated = Convert.ToDateTime(dr["date_created"].ToString());
                                invoiceItem.ContactId = DBUtil.GetIntValue(dr, "contact_id");
                                invoiceItem.RossumFileSequence = DBUtil.GetLongValue(dr, "rossum_sequence");
                                if (includeRossumFile)
                                {
                                    invoiceItem.FileNameCabId = DBUtil.GetStringValue(dr, "file_name_cab_id");
                                    invoiceItem.SupplierType = DBUtil.GetStringValue(dr, "suppliertype");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "Load_InvoiceItemised()");
                throw ex;
            }
            return invoiceItem;
        }


        private List<SupplierInvoiceItemsVM> getInvoiceItemisedItems(string qryInvoice,bool isforList)
        {
            List<SupplierInvoiceItemsVM> InvoiceLines = new List<SupplierInvoiceItemsVM>();
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdSelect =
                    new OleDbCommand(qryInvoice, conn))
                {
                    OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        InvoiceLines = new List<SupplierInvoiceItemsVM>();
                        foreach (DataRow row in dt.Rows)
                        {
                            InvoiceLines.Add(Load_InvoiceItemisedInvoice(row,isforList));
                        }
                    }
                }
            }
            return InvoiceLines;
        }

        private SupplierInvoiceItemsVM Load_InvoiceItemisedInvoice(DataRow row,bool isforList)
        {
            SupplierInvoiceItemsVM invoiceItem = null;
            if (row != null)
            {
                invoiceItem = new SupplierInvoiceItemsVM();
                invoiceItem.Sequence = DBUtil.GetLongValue(row, "sequence");
                invoiceItem.InvoiceSequence = DBUtil.GetLongValue(row, "invoice_sequence");
                invoiceItem.ItemDate = (DateTime)DBUtil.GetDateTimeValue(row, "item_date");
                invoiceItem.ItemQuantity = (float)DBUtil.GetDecimalValue(row, "item_quantity");
                invoiceItem.ItemRef = DBUtil.GetStringValue(row, "item_ref");
                invoiceItem.ImportType = DBUtil.GetIntValue(row, "import_type");
                invoiceItem.ImportTypeRef = DBUtil.GetStringValue(row, "import_type_ref");
                invoiceItem.StockCode = DBUtil.GetStringValue(row, "stock_code");
                invoiceItem.ItemDesc = DBUtil.GetStringValue(row, "item_desc");
                invoiceItem.ItemUnit = DBUtil.GetStringValue(row, "item_unit");
                invoiceItem.ItemAmt = (float)DBUtil.GetDoubleValue(row, "item_amt");
                invoiceItem.ItemDiscountPercent = (float)DBUtil.GetDoubleValue(row, "item_discount_percent");
                invoiceItem.ItemAmtDiscount = (float)DBUtil.GetDoubleValue(row, "item_amt_discount");
                invoiceItem.ItemAmtSubTotal = (float)DBUtil.GetDoubleValue(row, "item_amt_subtotal");
                invoiceItem.ItemVATPercent = (float)DBUtil.GetDoubleValue(row, "item_vat_percent");
                invoiceItem.ItemAmtVAT = (float)DBUtil.GetDoubleValue(row, "item_amt_vat");
                invoiceItem.ItemAmtTotal = (float)DBUtil.GetDoubleValue(row, "item_amt_total");
                invoiceItem.ItemAmtLabour = (float)DBUtil.GetDoubleValue(row, "item_amt_labour");
                invoiceItem.SageNominalCode = DBUtil.GetStringValue(row, "sage_nominal_code");
                invoiceItem.SageTaxCode = DBUtil.GetStringValue(row, "sage_tax_code");
                invoiceItem.FlgJobSeqExclude = DBUtil.GetBooleanValue(row, "flg_job_seq_exclude");
                invoiceItem.ItemType = DBUtil.GetIntValue(row, "item_type");
                invoiceItem.TelSequence = DBUtil.GetIntValue(row, "tel_sequence");
                invoiceItem.CostCentreId = DBUtil.GetStringValue(row, "cost_centre_id");
                invoiceItem.FlgChecked = DBUtil.GetBooleanValue(row, "flg_checked");
                invoiceItem.AssetSequence = DBUtil.GetIntValue(row, "asset_sequence");
                if (isforList)
                {
                    invoiceItem.CostCodeDesc = DBUtil.GetStringValue(row, "costcentre_desc");
                    invoiceItem.VehicleReg = DBUtil.GetStringValue(row, "vehicle_reg");
                    invoiceItem.TelRefDesc = DBUtil.GetStringValue(row, "tel_desc");
                }
            }
            return invoiceItem;
        }

        public SupplierInvoiceVM GetInvoiceByInvNo(string invoiceNo)
        {
            SupplierInvoiceVM returnValue = new SupplierInvoiceVM();
            string qry = @"select * from un_invoice_itemised where invoice_no = '" + invoiceNo + "'";
            returnValue = Load_InvoiceItemised(qry,false);
            return returnValue;
        }
        public SupplierInvoiceVM GetInvoiceBySequenceNo(long sequence)
        {
            SupplierInvoiceVM returnValue = new SupplierInvoiceVM();
            string qry = @"select * from un_invoice_itemised where sequence = " + sequence + "";
            returnValue = Load_InvoiceItemised(qry, false);
            return returnValue;
        }
        private bool removeInvoiceItemisedItems(string qryInvoice)
        {
            bool returnValue = false;
            using (OleDbConnection conn = this.getDbConnection())
            {
                using (OleDbCommand objCmdUpdate =
                    new OleDbCommand(qryInvoice, conn))
                {
                    objCmdUpdate.ExecuteNonQuery();
                    returnValue = true;
                }
            }
            return returnValue;
        }
        public SageViewModel GetSageDetail()
        {
            SageViewModel returnValue = new SageViewModel();
            string qry = @"select top(1) (select data from un_entity_details_supplementary where data_type='028' and entity_id=5928) tax_code,
                        (select data nominalcode from un_entity_details_supplementary where data_type='027' and entity_id=5928) sage_nominal_code 
                        from un_entity_details_supplementary ";
            returnValue = Load_SageDetail(qry);
            return returnValue;
        }
        private SageViewModel Load_SageDetail(string qry)
        {
            SageViewModel sage = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(qry, conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                dr.GetValue(0);
                                sage = new SageViewModel();
                                sage.SageNominalCode = DBUtil.GetStringValue(dr, "sage_nominal_code");
                                sage.SageTaxCode = DBUtil.GetStringValue(dr, "tax_code");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message, "Load_InvoiceItemised()");
                throw ex;
            }
            return sage;
        }
        private VehicleViewModel Load_Vehicle(DataRow dr)
        {
            VehicleViewModel vehicle = null;
            if (dr != null)
            {
                vehicle = new VehicleViewModel();
                vehicle.AssetSequence = DBUtil.GetLongValue(dr, "asset_sequence");
                vehicle.VehicleReg = DBUtil.GetStringValue(dr, "vehicle_reg");
            }
            return vehicle;
        } 
        private ItemisedItemTelViewModel Load_ItemisedItemTel(DataRow dr)
        {
            ItemisedItemTelViewModel itemTel = null;
            if (dr != null)
            {
                itemTel = new ItemisedItemTelViewModel();
                itemTel.Sequence = DBUtil.GetLongValue(dr, "sequence");
                itemTel.ItemIndex = DBUtil.GetStringValue(dr, "item_index");
                itemTel.TelType = DBUtil.GetStringValue(dr, "tel_type");
                itemTel.TelDesc = DBUtil.GetStringValue(dr, "tel_desc");
            }
            return itemTel;
        }
        private CostCodeViewModel Load_ItemisedCostCode(DataRow dr)
        {
            CostCodeViewModel costCode = null;
            if (dr != null)
            {
                costCode = new CostCodeViewModel();
                costCode.Sequence = DBUtil.GetLongValue(dr, "sequence");
                costCode.CostCentreId = DBUtil.GetStringValue(dr, "costcentre_id");
                costCode.CostCentreDesc = DBUtil.GetStringValue(dr, "costcentre_desc");
            }
            return costCode;
        }
    }
}
