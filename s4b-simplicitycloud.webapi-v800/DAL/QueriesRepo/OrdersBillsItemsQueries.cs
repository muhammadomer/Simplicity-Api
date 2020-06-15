using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrdersBillsItemsQueries
    {
        
        public static string insertBillItem(string databaseType, OrderBillItems row)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_orders_bill_items(bill_sequence,item_sequence,job_sequence, flg_text_line, pcent_payment,amount_payment
                        , flg_discounted, pcent_discount, amount_discount, flg_retention, pcent_retention, amount_retention, amount_sub_total
                        ,pcent_vat, amount_vat, pcent_cis, amount_cis, sage_nominal_code, sage_tax_code,sage_bank_code, cost_centre_id)
                     VALUES(" + row.BillSequence
                     +"," + row.ItemSequence 
                     + "," + row.JobSequence
                     + "," + Utilities.GetBooleanForDML(databaseType, row.FlgTextLine)
                     + "," + row.PcentPayment
                     + "," + row.AmountPayment
                     + "," + Utilities.GetBooleanForDML(databaseType, row.FlgDiscounted)
                     + "," + row.PcentDiscount
                     + "," + row.AmountDiscount
                     + "," + Utilities.GetBooleanForDML(databaseType,row.FlgRetention)
                     + "," + row.PcentRetention
                     + "," +row.AmountRetention
                     + "," + row.AmountSubTotal
                     + "," + row.PcentVat
                    + "," + row.AmountVat
                    + "," + row.PcentCis
                    + "," + row.AmountCis
                    + ", '" + row.SageNominalCode + "'"
                    + ", '" + row.SageTaxCode + "'"
                    + ", '" + row.SageBankCode +"'"
                    + ", '" + row.CostCentreId +"'"
                    + ")";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string updateBillItem(string databaseType, OrderBillItems row)
        {
            string returnValue = "";
            try
            {
                returnValue = @"Update un_orders_bill_items Set 
                flg_text_line = " + Utilities.GetBooleanForDML(databaseType, row.FlgTextLine)
                + ",pcent_payment = " + row.PcentPayment
                + ",amount_payment = " + row.AmountPayment
                + " , flg_discounted = " + Utilities.GetBooleanForDML(databaseType, row.FlgDiscounted)
                + ", pcent_discount = " + row.PcentDiscount
                + ", amount_discount = " + row.AmountDiscount
                + ", flg_retention = " + +Utilities.GetBooleanForDML(databaseType, row.FlgRetention)
                + ", pcent_retention = " + row.PcentRetention
                + ", amount_retention = " + row.AmountRetention
                + ", amount_sub_total = " + row.AmountSubTotal
                + " ,pcent_vat = " + row.PcentVat
                + ", amount_vat =" + row.AmountVat
                + ", pcent_cis = " + row.PcentCis
                + ", amount_cis = " + row.AmountCis
                + ", sage_nominal_code = '" + row.SageNominalCode + "'"
                + ", sage_tax_code = '" + row.SageTaxCode + "'"
                + ",sage_bank_code = '" + row.SageBankCode + "'"
                + " , cost_centre_id = '" + row.CostCentreId + "'"
                + " Where sequence= " + row.Sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectOrderBillItemsForEditingByBillSequence(string databaseType, long billSequence,long jobsequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT BItems.sequence, BItems.item_sequence, BItems.job_sequence, oi.item_type, BItems.flg_text_line
                    , oi.item_code, oi.item_desc, oi.item_quantity, oi.amount_balance, oi.amount_total
                    , BItems.pcent_payment, BItems.amount_payment, BItems.flg_discounted, BItems.pcent_discount, BItems.amount_discount
                    , BItems.flg_retention, BItems.pcent_retention, BItems.amount_retention,BItems.amount_sub_total, BItems.pcent_vat, BItems.amount_vat
                    , BItems.pcent_cis, BItems.amount_cis,BItems.sage_nominal_code, BItems.sage_tax_code, BItems.sage_bank_code,BItems.cost_centre_id
                FROM un_order_items AS oi INNER JOIN un_orders_bill_items AS BItems ON oi.sequence = BItems.item_sequence
                WHERE BItems.bill_sequence= " + billSequence + @"
                Union All
                SELECT -1 as sequence,oi.sequence AS item_sequence, oi.job_sequence, oi.item_type, oi.flg_row_is_text
                    , oi.item_code, oi.item_desc,oi.item_quantity,oi.amount_balance, oi.amount_total
                    ,0 as pcent_payment, 0 as amount_payment, 0 as flg_discounted, 0 as pcent_discount, 0 as amount_discount
                    , 0 as flg_retention, 0 as pcent_retention,0 as amount_retention,0 as amount_sub_total, 0 as pcent_vat, 0 as amount_vat
                    , 0 as pcent_cis, 0 as amount_cis ,null as sage_nominal_code, null as sage_tax_code,null as  sage_bank_code, null as cost_centre_id
                FROM un_order_items AS oi
                WHERE oi.job_sequence= " + jobsequence +@"
                And oi.sequence in (Select item_sequence from un_orders_bill_items WHERE bill_sequence <>  " + billSequence + @")
                And oi.sequence not in (Select item_sequence from un_orders_bill_items WHERE bill_sequence = " + billSequence + @")
                Union All
                SELECT -1 as sequence,oi.sequence AS item_sequence, oi.job_sequence, oi.item_type, oi.flg_row_is_text
                    , oi.item_code, oi.item_desc,oi.item_quantity,oi.amount_balance, oi.amount_total
                    ,0 as pcent_payment, 0 as amount_payment, 0 as flg_discounted, 0 as pcent_discount, 0 as amount_discount
                    , 0 as flg_retention, 0 as pcent_retention,0 as amount_retention,0 as amount_sub_total, 0 as pcent_vat, 0 as amount_vat
                    , 0 as pcent_cis, 0 as amount_cis ,null as sage_nominal_code, null as sage_tax_code,null as  sage_bank_code, null as cost_centre_id
                FROM un_order_items AS oi
                WHERE oi.job_sequence= " + jobsequence + @"
                And oi.sequence not in (Select item_sequence from un_orders_bill_items)";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectOrderBillItemsBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT BItems.sequence, BItems.item_sequence, BItems.job_sequence, BItems.flg_text_line
                    , BItems.pcent_payment, BItems.amount_payment, BItems.flg_discounted, BItems.pcent_discount, BItems.amount_discount
                    , BItems.flg_retention, BItems.pcent_retention, BItems.amount_retention,BItems.amount_sub_total, BItems.pcent_vat, BItems.amount_vat
                    , BItems.pcent_cis, BItems.amount_cis,BItems.sage_nominal_code, BItems.sage_tax_code, BItems.sage_bank_code,BItems.cost_centre_id
                FROM un_orders_bill_items AS BItems 
                WHERE BItems.sequence= " + sequence ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectAllOrderBillItemsByBillSequence(string databaseType, long billSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT BItems.sequence , BItems.bill_sequence, BItems.item_sequence, BItems.job_sequence, BItems.flg_text_line
                    , OItems.item_code, OItems.item_desc,OItems.item_units, OItems.item_quantity , OItems.amount_total
                    , BItems.pcent_payment, BItems.amount_payment, BItems.flg_discounted, BItems.pcent_discount, BItems.amount_discount
                    , BItems.flg_retention, BItems.pcent_retention, BItems.amount_retention, BItems.amount_sub_total, BItems.pcent_vat, BItems.amount_vat
                    , BItems.pcent_cis, BItems.amount_cis, BItems.sage_nominal_code, BItems.sage_tax_code, BItems.sage_bank_code, BItems.cost_centre_id
                FROM un_orders_bill_items AS BItems
                    INNER JOIN un_order_items AS OItems ON BItems.item_sequence = OItems.sequence
                Where BItems.bill_sequence = " + billSequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
    }
}

