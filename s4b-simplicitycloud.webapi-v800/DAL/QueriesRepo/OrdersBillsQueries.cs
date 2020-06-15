using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrdersBillsQueries
    {

        public static string getSelectBySequence(string databaseType, long billSequence,long jobSequence)
        {
            string returnValue = "";
            string strWhere = "";
            try
            {   
                if (billSequence > 0)
                    strWhere = " Where un_orders_bills.sequence=" + billSequence;
                else if (jobSequence > 0)
                    strWhere = " WHERE un_orders_bills.job_sequence = " + jobSequence + " And un_orders_bills.flg_request_made=" + Utilities.GetBooleanForDML(databaseType, true) + "  AND un_orders_bills.flg_set_to_proforma=" + Utilities.GetBooleanForDML(databaseType, true) +" AND un_orders_bills.flg_set_to_invoice=" + Utilities.GetBooleanForDML(databaseType, false);
                returnValue = @"SELECT un_orders_bills.*, un_orders.job_ref
                        ,un_orders.job_client_name , un_entity_details_core.name_short as parent_name
                    FROM(un_orders_bills  INNER JOIN un_orders ON un_orders.sequence = un_orders_bills.job_sequence) 
                    LEFT JOIN un_entity_details_core ON un_orders_bills.entity_join_id = un_entity_details_core.entity_id "
                    + strWhere;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectAllByJobSequence(string databaseType, long jobSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT *  FROM un_orders_bills  WHERE job_sequence = " + jobSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllSaleInvoices(string databaseType, DateTime? fromDate,DateTime? toDate)
        {
            string returnValue = "";
            string whereClause = "";
            try
            {
                if (fromDate == null)
                    returnValue = "SELECT Top 100 ";
                else {
                    returnValue = @"SELECT ";
                    switch (databaseType)
                    {
                        case "MSACCESS":
                            whereClause = " and OB.invoice_date Between#" + ((DateTime)fromDate).ToString("MM/dd/yyyy") + " 00:00:00# " +
                            "   AND #" + ((DateTime)toDate).ToString("MM/dd/yyyy") + " 23:59:59#";
                            break;
                        case "SQLSERVER":
                        default:
                            whereClause = " and OB.invoice_date Between '" + ((DateTime)fromDate).ToString("yyyy-MM-dd") + " 00:00:00'" +
                           "   AND '" + ((DateTime)toDate).ToString("yyyy-MM-dd") + " 23:59:59'";
                            break;
                    }
                }
                returnValue += @" OB.sequence, Ord.job_client_name, Ord.job_client_ref, OB.invoice_no, OB.invoice_date
                    , OB.amount_total, invEntry.outstanding ,EDC.name_sage,Ord.job_ref,un_ref_entity_pymt_type.entity_pymt_desc
                FROM (((un_orders AS Ord INNER JOIN un_orders_bills AS OB ON Ord.sequence = OB.job_sequence) 
                    LEFT JOIN (
                            Select invoiceno_or_itemref,iif(flg_settled=" + Utilities.GetBooleanForDML(databaseType, true) + @",0,entry_amt_total - entry_amt_allocated) as outstanding
                            From un_invoice_entries_new 
                            Where trans_type ='B' and entry_type='SI'
                    )  AS invEntry ON OB.invoice_no = invEntry.invoiceno_or_itemref) 
                LEFT JOIN un_entity_details_core AS EDC ON OB.client_id = EDC.entity_id) 
                LEFT JOIN un_ref_entity_pymt_type ON EDC.entity_pymt_type = un_ref_entity_pymt_type.entity_pymt_id
                WHERE OB.flg_set_to_proforma = " + Utilities.GetBooleanForDML(databaseType, true)+" AND OB.flg_set_to_invoice =" + Utilities.GetBooleanForDML(databaseType, true) + whereClause
                + " Order by OB.invoice_date desc";
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectSaleInvoiceBySequence(string databaseType, long billSequence)
        {
            string returnValue = "";
            try
            {  
                returnValue = @"SELECT OB.sequence, Orders.job_ref, OB.invoice_no, OB.invoice_date, OB.mailling_address, OB.footnote , OB.set_to_invoice_date
                    , Orders.job_date_start,orders.job_date_finish,Orders.job_client_name, Orders.job_client_ref, Orders.job_trade_code,orders.job_desc,orders.job_cost_centre 
					, un_ref_trade_code_type.trade_desc
                    , Orders.job_address, un_entity_details_core.address_full
                    ,OB.amount_initial, OB.amount_discount, OB.pcent_retention, OB.amount_retention, OB.amount_sub_total, OB.amount_vat, OB.amount_cis, OB.amount_total
                    ,(SELECT eds.data FROM un_entity_details_supplementary AS eds WHERE eds.entity_id = 1 AND eds.data_type = '008') AS vat_reg
                FROM  ((un_orders_bills AS OB INNER JOIN   un_orders AS Orders  ON Orders.sequence = OB.job_sequence) 
                    Left Outer JOIN un_entity_details_core ON Orders.job_client_id = un_entity_details_core.entity_id)
                    Left Outer join un_ref_trade_code_type ON un_ref_trade_code_type.trade_id = Orders.job_trade_code
                WHERE OB.flg_set_to_invoice = " + Utilities.GetBooleanForDML(databaseType, true) +" And OB.sequence= " + billSequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectOrderItemsForInvoicingByJobSequence(string databaseType, long jobSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT oi.sequence AS item_sequence, oi.job_sequence, oi.row_index,oi.item_type, oi.flg_row_is_text, oi.item_code, oi.item_desc
                 ,oi.item_quantity,oi.amount_balance, oi.amount_total
                FROM un_order_items AS oi
                WHERE oi.job_sequence=" + jobSequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectBySequenceAndType(string databaseType, long jobSequence,string type)
        {
            string returnValue = "";
            string strWhere = "";
            try
            {
                if (type.ToUpper() == "REQUEST")
                    strWhere = " WHERE job_sequence = " + jobSequence + " And un_orders_bills.flg_request_made=" + Utilities.GetBooleanForDML(databaseType, true) + "AND un_orders_bills.flg_set_to_invoice=" + Utilities.GetBooleanForDML(databaseType,false) ;
                if (type.ToUpper() == "APPLICATION")
                    strWhere = " WHERE job_sequence = " + jobSequence +  " And un_orders_bills.flg_request_made=" + Utilities.GetBooleanForDML(databaseType, true) +" AND un_orders_bills.flg_set_to_proforma= " + Utilities.GetBooleanForDML(databaseType, true) + " AND un_orders_bills.flg_set_to_invoice= " + Utilities.GetBooleanForDML(databaseType, false);
                if (type.ToUpper() == "INVOICE")
                    strWhere = " WHERE job_sequence = " + jobSequence +  " And un_orders_bills.flg_request_made=" + Utilities.GetBooleanForDML(databaseType, true) +" AND un_orders_bills.flg_set_to_invoice=" + Utilities.GetBooleanForDML(databaseType, true);
                if (type.ToUpper() == "APPLICATIONANDINVOICE")
                    strWhere = " WHERE job_sequence = " + jobSequence + " And un_orders_bills.flg_request_made=" + Utilities.GetBooleanForDML(databaseType, true)+" AND (un_orders_bills.flg_set_to_proforma=" + Utilities.GetBooleanForDML(databaseType, true) +" OR un_orders_bills.flg_set_to_invoice=" + Utilities.GetBooleanForDML(databaseType, true) +")";
                returnValue = @"SELECT un_orders_bills.*, un_orders.job_ref
                        ,un_orders.job_client_name , un_entity_details_core.name_short as parent_name
                    FROM(un_orders_bills  INNER JOIN un_orders ON un_orders.sequence = un_orders_bills.job_sequence) 
                    LEFT JOIN un_entity_details_core ON un_orders_bills.entity_join_id = un_entity_details_core.entity_id "
                    + strWhere;
            }
            catch (Exception ex)
            {
                throw ex;  
            }
            return returnValue;
        }

        public static string getSelectInvoiceBySequence(string databaseType, long billSequence)
        {
            string returnValue = "";
            try
            {   
                returnValue = @"SELECT un_orders_bills.*, un_orders.job_ref
                        ,un_orders.job_client_name , un_entity_details_core.name_short as parent_name
                    FROM(un_orders_bills  INNER JOIN un_orders ON un_orders.sequence = un_orders_bills.job_sequence) 
                        LEFT JOIN un_entity_details_core ON un_orders_bills.entity_join_id = un_entity_details_core.entity_id 
                    WHERE un_orders_bills.sequence = " + billSequence + " And un_orders_bills.flg_request_made=" + Utilities.GetBooleanForDML(databaseType,true)
                    +" AND un_orders_bills.flg_set_to_invoice= " + Utilities.GetBooleanForDML(databaseType,true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectOrderAppForPaymentAndInvoices(string databaseType,long jobSequence)
        {
            string returnValue = "";
            try
            {   
                   returnValue = @"SELECT 'AFP' as EntryType, OB.sequence, OB.job_sequence, OB.bill_ref, OB.client_id, OB.invoice_no, OB.invoice_date, OB.amount_initial, OB.amount_discount
                    , OB.amount_retention, OB.amount_sub_total, OB.amount_vat, OB.amount_cis, OB.amount_total
                    ,Orders.job_ref, Orders.job_client_name, 0 as entry_amt_allocated_total, OrderItems.outstanding 
                    FROM (un_orders_bills AS OB INNER JOIN un_orders AS Orders ON OB.job_sequence = Orders.sequence) INNER JOIN 
                        (Select job_sequence,sum(amount_balance) as outstanding From  un_order_items group by job_sequence)  AS OrderItems ON Orders.sequence = OrderItems.job_sequence
                    WHERE (OB.flg_set_to_proforma=" + Utilities.GetBooleanForDML(databaseType, true)
                    +" AND OB.flg_set_to_invoice <> " + Utilities.GetBooleanForDML(databaseType, true)+")" 
                    + " And (orders.sequence=" + jobSequence +")";
                    returnValue += " Union ALL ";
                returnValue += @"SELECT 'SI' as EntryType,OB.sequence, OB.job_sequence, OB.bill_ref, OB.client_id, OB.invoice_no, OB.invoice_date, OB.amount_initial, OB.amount_discount
                        , OB.amount_retention, OB.amount_sub_total, OB.amount_vat, OB.amount_cis, OB.amount_total
                        , Orders.job_ref, Orders.job_client_name, (inv.entry_amt_allocated + inv.entry_amt_allocated_labour) AS entry_amt_allocated_total
                        , (inv.entry_amt_total -  (inv.entry_amt_allocated + inv.entry_amt_allocated_labour)) AS outstanding
                    FROM (un_orders_bills AS OB INNER JOIN un_orders AS Orders ON OB.job_sequence = Orders.sequence) 
                        INNER JOIN un_invoice_entries_new AS inv ON OB.invoice_no = inv.invoiceno_or_itemref
                    WHERE OB.flg_set_to_proforma = " + Utilities.GetBooleanForDML(databaseType, true) 
                    +" AND OB.flg_set_to_invoice=" + Utilities.GetBooleanForDML(databaseType, true) 
                    +" And OB.flg_is_vat_inv <> " + Utilities.GetBooleanForDML(databaseType, true)
                    +" AND inv.trans_type = 'B' AND inv.entry_type = 'SI' And Orders.Sequence=" + jobSequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectListOfAppForPayments(string databaseType, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate)
        {
            string returnValue = "";
            try
            {
                string whereClause = "";
                if (!string.IsNullOrEmpty(fromDate.ToString()))
                    whereClause = " and OB.invoice_date  >= '" + ((DateTime)fromDate).ToString("yyyy-MM-dd") + " 00:00:00'";
                if (!string.IsNullOrEmpty(toDate.ToString()))
                    whereClause += "  AND OB.invoice_date  <='" + ((DateTime)toDate).ToString("yyyy-MM-dd") + " 23:59:59'";
                returnValue = @"SELECT edc.name_sage ,OB.sequence, OB.job_sequence, OB.bill_ref,OB.client_id,EDC.entity_join_id, Ord.job_client_name, EDC.name_short AS parent_name, Ord.job_client_ref
                , OB.invoice_no, OB.invoice_date, Ord.job_ref, OB.amount_total, OB.amount_vat, OB.amount_sub_total,OB.flg_set_to_proforma,OB.flg_set_to_invoice
                FROM (un_orders AS Ord 
                    INNER JOIN un_orders_bills AS OB ON Ord.sequence = OB.job_sequence) 
                    LEFT JOIN un_entity_details_core AS EDC ON OB.entity_join_id = EDC.entity_id
                WHERE OB.flg_set_to_proforma=" + Utilities.GetBooleanForDML(databaseType, true) + " AND OB.flg_set_to_invoice <> " + Utilities.GetBooleanForDML(databaseType, true);
                if (!string.IsNullOrEmpty(whereClause))
                {
                    returnValue += whereClause;
                }
                if (clientRequest.globalFilter != "")
                {
                    string filterValue = clientRequest.globalFilter;
                    string globalFilterQuery = " ";
                    returnValue += globalFilterQuery;
                }
                returnValue += " Order by OB.invoice_date Desc";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getOrderValidForRequest(string databaseType, long jobSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT un_orders_bills.sequence
                FROM un_orders_bills
                WHERE un_orders_bills.flg_request_made= " + Utilities.GetBooleanForDML(databaseType, true)+" AND un_orders_bills.flg_set_to_invoice=" + Utilities.GetBooleanForDML(databaseType, false)
                +" AND un_orders_bills.job_sequence=" + jobSequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string insert(string databaseType,OrdersBills orderBill )
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "INSERT INTO un_orders_bills(job_sequence,  bill_ref,  client_id,  entity_join_id,  flg_parent_override, " +
                                      "       invoice_no,  invoice_date,  amount_initial,  amount_discount,  pcent_retention, " +
                                      "       amount_retention,  amount_sub_total,  amount_vat,  amount_cis,  amount_total, " +
                                      "       job_date,  flg_job_date_start,  job_date_start,  flg_job_date_finish,  job_date_finish, " +
                                      "       mailling_address,  footnote,  flg_request_made,  request_made_date,  flg_set_to_proforma, " +
                                      "       set_to_proforma_date,  sage_id,  flg_set_to_invoice,  set_to_invoice_date,  invoice_index, " +
                                      "       flg_has_a_vat_inv,  flg_is_vat_inv,  join_bill_sequence,  rci_id,  created_by,  date_created, " +
                                      "       last_amended_by,  date_last_amended,  flg_archive) " +
                                      "VALUES (" + orderBill.JobSequence + ", '" + orderBill.BillRef + "', " + orderBill.ClientId + ", " + orderBill.EntityJoinId + ", " +
                                      "      " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgParentOverride)
                                      + ", '" + orderBill.InvoiceNo + "'"
                                      + ", " + Utilities.GetDateTimeForDML(databaseType, orderBill.InvoiceDate, true, false)
                                      + ", " + orderBill.AmountInitial + ", " + orderBill.AmountDiscount + ", " + orderBill.PcentRetention
                                      + ", " + orderBill.AmountRetention + ", " + orderBill.AmountSubTotal + ", " + orderBill.AmountVat + ", " + orderBill.AmountCis + ", " + orderBill.AmountTotal
                                      + ", " + Utilities.GetDateTimeForDML(databaseType, orderBill.JobDate, true, false) + ", " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgJobDateStart)
                                      + ", " + Utilities.GetDateTimeForDML(databaseType, orderBill.JobDateStart, true, false)
                                      + ", " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgJobDateFinish)
                                      + ", " + Utilities.GetDateTimeForDML(databaseType, orderBill.JobDateFinish, true, false)
                                      + ",   '" + orderBill.MaillingAddress + "'"
                                      + ", '" + (String.IsNullOrEmpty(orderBill.Footnote) ? " " : orderBill.Footnote) +"'"
                                      + ", " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgRequestMade)
                                      + ", " + Utilities.GetDateTimeForDML(databaseType, orderBill.RequestMadeDate, true, false) + ", " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgSetToProforma)
                                      + ", " + Utilities.GetDateTimeForDML(databaseType, orderBill.SetToProformaDate, true, false) + ", " + orderBill.SageId + ", " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgSetToInvoice)
                                      + ", " + Utilities.GetDateTimeForDML(databaseType, orderBill.SetToInvoiceDate, true, false)
                                      + ", " + orderBill.InvoiceIndex + ",   " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgHasAVatInv) 
                                      + ", " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgIsVatInv) + ", " + orderBill.JoinBillSequence
                                      + ", " + orderBill.RciId + ",   " + orderBill.CreatedBy 
                                      + ", " + Utilities.GetDateTimeForDML(databaseType, orderBill.DateCreated, true, true)
                                      + ", " + orderBill.LastAmendedBy 
                                      + ", " + Utilities.GetDateTimeForDML(databaseType, orderBill.DateLastAmended, true, true) 
                                      + ", " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgArchive) + ")";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string update(string databaseType, OrdersBills orderBill)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_orders_bills" +
                                 "   SET  job_sequence =  " + orderBill.JobSequence + ",  " +
                                 "        bill_ref =  '" + orderBill.BillRef + "',  " +
                                 "        client_id =  " + orderBill.ClientId + ",  " +
                                 "        entity_join_id =  " + orderBill.EntityJoinId + ",  " +
                                 "        flg_parent_override = " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgParentOverride) + ",  " +
                                 "        invoice_no =  '" + orderBill.InvoiceNo + "',  " +
                                 "        invoice_date =  " + Utilities.GetDateTimeForDML(databaseType, orderBill.InvoiceDate, true, false) + ",  " +
                                 "        amount_initial =  " + orderBill.AmountInitial + ",  " +
                                 "        amount_discount =  " + orderBill.AmountDiscount + ",  " +
                                 "        pcent_retention =  " + orderBill.PcentRetention + ",  " +
                                 "        amount_retention =  " + orderBill.AmountRetention + ",  " +
                                 "        amount_sub_total =  " + orderBill.AmountSubTotal + ",  " +
                                 "        amount_vat =  " + orderBill.AmountVat + ",  " +
                                 "        amount_cis =  " + orderBill.AmountCis + ",  " +
                                 "        amount_total =  " + orderBill.AmountTotal + ",  " +
                                 "        job_date =  " + Utilities.GetDateTimeForDML(databaseType, orderBill.JobDate, true, false) + ",  " +
                                 "        flg_job_date_start = " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgJobDateStart) + ",  " +
                                 "        job_date_start =  " + Utilities.GetDateTimeForDML(databaseType, orderBill.JobDateStart, true, false) + ",  " +
                                 "        flg_job_date_finish = " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgJobDateFinish) + ",  " +
                                 "        job_date_finish = " + Utilities.GetDateTimeForDML(databaseType, orderBill.JobDateFinish, true, false) + ",  " +
                                 "        mailling_address =  '" + orderBill.MaillingAddress + "',  " +
                                 "        footnote =  '" + (String.IsNullOrEmpty(orderBill.Footnote) ? " " : orderBill.Footnote) + "',  " +
                                 "        flg_request_made = " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgRequestMade) + ",  " +
                                 "        request_made_date =  " + Utilities.GetDateTimeForDML(databaseType,orderBill.RequestMadeDate,true, false) + ",  " +
                                 "        flg_set_to_proforma = " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgSetToProforma) + ",  " +
                                 "        set_to_proforma_date =  " + Utilities.GetDateTimeForDML(databaseType, orderBill.SetToProformaDate, true, false) + ",  " +
                                 "        sage_id =  " + orderBill.SageId + ",  " +
                                 "        flg_set_to_invoice = " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgSetToInvoice) + ",  " +
                                 "        set_to_invoice_date =  " + Utilities.GetDateTimeForDML(databaseType, orderBill.SetToInvoiceDate, true, false) + ",  " +
                                 "        invoice_index =  " + orderBill.InvoiceIndex + ",  " +
                                 "        flg_has_a_vat_inv = " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgHasAVatInv) + ",  " +
                                 "        flg_is_vat_inv = " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgIsVatInv) + ",  " +
                                 "        join_bill_sequence =  " + orderBill.JoinBillSequence + ",  " +
                                 "        rci_id =  " + orderBill.RciId + ",  " +
                                 "        flg_archive = " + Utilities.GetBooleanForDML(databaseType, orderBill.FlgArchive) + ",  " +
                                 "        last_amended_by =  " + orderBill.LastAmendedBy + ",  " +
                                 "        date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, orderBill.DateLastAmended,true,true) + 
                                 "  WHERE sequence = " + orderBill.Sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string updateInvoiceNoAndSetToJTDateBySequence(string databaseType, long sequence, 
                                                                     string invoiceNo, DateTime? setToJTDate, 
                                                                     long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = "UPDATE un_orders_bills" +
                             "   SET  invoice_no = " + invoiceNo + ",  " +
                             "        date_set_to_jt = " + Utilities.getAccessDate(setToJTDate) + "',  " +
                             "        last_amended_by = " + lastAmendedBy + ", " +
                             "        date_last_amended = " + Utilities.getAccessDate(dateLastAmended) + " " +
                             "  WHERE sequence = " + sequence;
                        break;
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE un_orders_bills" +
                             "   SET  invoice_no = " + invoiceNo + ",  " +
                             "        date_set_to_jt = " + Utilities.getSQLDate(setToJTDate) + "',  " +
                             "        last_amended_by = " + lastAmendedBy + ", " +
                             "        date_last_amended = " + Utilities.getSQLDate(dateLastAmended) + " " +
                             "  WHERE sequence = " + sequence;
                        break;
                }
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
                returnValue = "DELETE FROM   un_orders_bills" +
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
                bool flg = true;
                returnValue = "UPDATE   un_orders_bills" +
                              "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) + ", " +
                              "WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

