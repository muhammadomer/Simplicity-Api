using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;
using Newtonsoft.Json;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class SupplierInvoicesQueries
    {
        public static string getSelectAllRossumUnfinalizedInvoices(ClientRequest clientRequest)
        {
            string query = @"select * from( select case when trans_type='D' then 'supplier' else (case when trans_type ='C' then 
	                    (case when (select count(*) from 
	                    (SELECT sup.data FROM un_entity_details_supplementary sup WHERE sup.entity_id = inv.contact_id AND sup.data_type='021') as aa)>1 
	                    then 'sub-contractor' else 'contractor' end) else '' end) end trans_type, inv.invoice_no,inv.sequence, core.entity_pymt_type,core.name_short,core.name_long,core.name_sage,
                    core.entity_id, inv.itemised_date,inv.sum_amt_subtotal,sum_amt_vat, sum_amt_total,rosum.file_name_cab_id, inv.date_created,
                    (case when (select count(flg_checked) from un_invoice_itemised_items items where flg_checked=0 and items.invoice_sequence= inv.sequence)= 0 then 'Approved'
							else 
							case when (select count(flg_checked) from un_invoice_itemised_items items where flg_checked=1 and items.invoice_sequence= inv.sequence)<>0 then 'Partial' else 'UnApproved' end
							end
						)approved,
                        rossum_po_no,
					(SELECT MAX(ord.job_ref) FROM (un_purchase_orders AS po INNER JOIN un_purchase_order_items AS poi ON po.order_id = poi.order_id) 
						INNER JOIN un_orders AS ord ON poi.job_sequence = ord.sequence WHERE po.order_ref = inv.rossum_po_no GROUP BY ord.job_ref) AS ord_job_reference,
						(SELECT MAX(edc_f.name_short) FROM ((un_purchase_orders AS po INNER JOIN un_purchase_order_items AS poi ON po.order_id = poi.order_id) INNER JOIN un_orders AS ord ON poi.job_sequence = ord.sequence) INNER JOIN un_entity_details_core AS edc_f ON ord.job_manager = edc_f.entity_id
						WHERE po.order_ref = inv.rossum_po_no GROUP BY ord.job_ref, edc_f.name_short ) AS ord_job_manager_name,
                    inv.rossum_sequence
	                from un_invoice_itemised inv 
	                --left outer join  un_invoice_itemised_items invItem on inv.sequence= invItem.invoice_sequence
                    left outer join un_entity_details_core core on inv.contact_id=core.entity_id 
                    inner join un_rossum_files rosum on inv.rossum_sequence = rosum.sequence ) result
	                where 1=1 ";
            if (clientRequest.globalFilter != null && clientRequest.globalFilter != "")
            {
                string filterValue = clientRequest.globalFilter;
                string[] separators = new string[] { " " };
                string wordFilterQuery = "";
                //----find if word exist
                foreach (string word in filterValue.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    wordFilterQuery += (wordFilterQuery.Length > 0 ? "or" : "") + " invoice_no like '%" + word + "%' or  name_long like '%" + word + "%' or rossum_po_no like '%" + word + "%' or ord_job_reference like '%" + word + "%'";
                }
                wordFilterQuery += wordFilterQuery.Length > 0 ? " or " : "";
                string globalFilterQuery = " and ( " + wordFilterQuery + " invoice_no like '%" + filterValue + "%' or name_long like '%" + filterValue + "%' or rossum_po_no like '%" + filterValue + "%' or ord_job_reference like '%" + filterValue + "%') ";
                query += globalFilterQuery;
            }
            dynamic filter = JsonConvert.DeserializeObject<dynamic>(clientRequest.filters.ToString());
            if (filter.transType == "supplier" || filter.transType == "contractor"|| filter.transType == "sub-contractor")
            {
                query += " and trans_type='" + filter.transType+"'";
            }
            query += " order by date_created desc";
            return query;
        }      
        public static string getSelectAllUnfinalizedInvoices(ClientRequest clientRequest)
        {
            string query = @"select * from (select case when trans_type='D' then 'supplier' else (case when trans_type ='C' then 
	                    (case when (select count(*) from 
	                    (SELECT sup.data FROM un_entity_details_supplementary sup WHERE sup.entity_id = inv.contact_id AND sup.data_type='021') as aa)>1 
	                    then 'sub-contractor' else 'contractor' end) else '' end) end trans_type, inv.invoice_no,inv.sequence, core.entity_pymt_type,core.name_short,core.name_long,core.name_sage,
                        core.entity_id, inv.itemised_date,
                        inv.sum_amt_subtotal,
                        sum_amt_vat, 
                        sum_amt_total,inv.date_created,
                        (case when (select count(flg_checked) from un_invoice_itemised_items items where flg_checked=0 and items.invoice_sequence= inv.sequence)= 0 then 'Approved'
							else 
							case when (select count(flg_checked) from un_invoice_itemised_items items where flg_checked=1 and items.invoice_sequence= inv.sequence)<>0 then 'Partial' else 'UnApproved' end
							end
						)approved,
                    rossum_po_no,
					(SELECT MAX(ord.job_ref) FROM (un_purchase_orders AS po INNER JOIN un_purchase_order_items AS poi ON po.order_id = poi.order_id) 
						INNER JOIN un_orders AS ord ON poi.job_sequence = ord.sequence WHERE po.order_ref = inv.rossum_po_no GROUP BY ord.job_ref) AS ord_job_reference,
						(SELECT MAX(edc_f.name_short) FROM ((un_purchase_orders AS po INNER JOIN un_purchase_order_items AS poi ON po.order_id = poi.order_id) INNER JOIN un_orders AS ord ON poi.job_sequence = ord.sequence) INNER JOIN un_entity_details_core AS edc_f ON ord.job_manager = edc_f.entity_id
						WHERE po.order_ref = inv.rossum_po_no GROUP BY ord.job_ref, edc_f.name_short ) AS ord_job_manager_name,
                            inv.rossum_sequence
                            from un_invoice_itemised inv
                        --inner join 
                          --  un_invoice_itemised_items invItem on inv.sequence= invItem.invoice_sequence
                            inner join un_entity_details_core core on inv.contact_id=core.entity_id 
							) result where 1=1 and sequence not in(select inv.sequence from un_invoice_itemised inv
                        inner join un_rossum_files ross on inv.rossum_sequence = ross.sequence) ";
            if (clientRequest.globalFilter != null && clientRequest.globalFilter != "")
            {
                string filterValue = clientRequest.globalFilter;
                string[] separators = new string[] { " " };
                string wordFilterQuery = "";
                //----find if word exist
                foreach (string word in filterValue.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    wordFilterQuery += (wordFilterQuery.Length > 0 ? "or" : "") + " invoice_no like '%" + word + "%' or  name_long like '%" + word + "%' or rossum_po_no like '%"+word+ "%' or ord_job_reference like '%" + word + "%'";
                }
                wordFilterQuery += wordFilterQuery.Length > 0 ? " or " : "";
                string globalFilterQuery = " and ( " + wordFilterQuery + " invoice_no like '%" + filterValue + "%' or name_long like '%" + filterValue + "%' or rossum_po_no like '%" + filterValue + "%' or ord_job_reference like '%" + filterValue + "%') ";
                query += globalFilterQuery;
            }
            dynamic filter = JsonConvert.DeserializeObject<dynamic>(clientRequest.filters.ToString());
            if (filter.transType == "supplier" || filter.transType == "contractor"|| filter.transType == "sub-contractor")
            {
                query += " and trans_type='" + filter.transType+"'";
            }
            return query;
        }
        public static string insertInvoiceItemised(string databaseType, InvoiceItemised invoiceItemised)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "insert into un_invoice_itemised (trans_type,contact_id,invoice_no,itemised_date,sum_amt_main,sum_amt_vat,sum_amt_total) " +
                                      "VALUES (" + invoiceItemised.TransType + ", '" + invoiceItemised.ContactId+ "', " + invoiceItemised.InvoiceNo+ ", " +
                                      " " + Utilities.GetDateTimeForDML(databaseType, invoiceItemised.ItemisedDate,true,false)
                                      + ", '" + invoiceItemised.SumAmtMain + "'"
                                      + ", '" + invoiceItemised.SumAmtVAT
                                      + "', '" + invoiceItemised.SumAmtTotal + "')";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public static string insertInvoiceItemisedItems(string databaseType, InvoiceItemised invoiceItemised)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        {
                            foreach(var item in invoiceItemised.InvoiceLines)
                            {
                                returnValue += "insert into un_invoice_itemised_items(invoice_sequence, item_date, item_quantity, item_ref, stock_code, item_desc, item_unit,"+
                                                "item_amt, item_discount_percent,item_amt_discount,item_amt_subtotal,item_vat_percent,item_amt_vat,item_amt_total,"+
                                                "created_by,date_created,SSMA_TimeStamp)" +
                                              "VALUES (" + invoiceItemised.Sequence + ", '" + item.ItemDate + "', '" + item.ItemQuantity + "', '" +
                                              " '" + item.ItemRef + "', '" + item.StockCode + "', '" + item.ItemDesc + "','" + item.ItemUnit + "','" +
                                              item.ItemAmt +"','" +item.ItemDiscountPercent + "','" + item.ItemAmtDiscount +"','" + item.ItemAmtSubTotal + "','"+ item.ItemVATPercent + "','" + item.ItemAmtVAT+
                                              "','" + item.ItemAmtTotal + "','" + item.CreatedBy+ "','getdate()', 'SSMA_TimeStamp'";
                            }
                            
                        }
                        
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

    }
}

