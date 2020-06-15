using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrdersBillsItemsDB : MainDB
    {

        public OrdersBillsItemsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public OrderBillItems InsertOrderBillItem(OrderBillItems Object)
        {
            OrderBillItems returnValue = new OrderBillItems();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(OrdersBillsItemsQueries.insertBillItem(this.DatabaseType, Object), conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            long sequence = Utilities.GetDBAutoNumber(conn);
                            Object.Sequence = sequence;
                            returnValue = Object;
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

        public OrderBillItems UpdateOrderBillItem(OrderBillItems Object)
        {
            OrderBillItems returnValue = new OrderBillItems();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(OrdersBillsItemsQueries.updateBillItem(this.DatabaseType, Object), conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnValue = Object;
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
        public List<OrderBillItems> selectOrdersItemsForInvoicingByJobSequence(long jobSequence)
        {
            List<OrderBillItems> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsQueries.getSelectOrderItemsForInvoicingByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrderBillItems>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_Order_Bill_Items(row));
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

        public List<OrderBillItems> selectOrderBillItemsForEditingByBillSequence(long billSequence,long jobSequence)
        {
            List<OrderBillItems> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsItemsQueries.getSelectOrderBillItemsForEditingByBillSequence(this.DatabaseType, billSequence,jobSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrderBillItems>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_Order_Bill_Items(row));
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

        public OrderBillItems selectOrderBillItemsBySequence(long sequence)
        {
            OrderBillItems returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsItemsQueries.getSelectOrderBillItemsBySequence(this.DatabaseType,sequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new OrderBillItems();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue = Load_Order_Bill_Items(row);
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

        public List<OrderBillItems> selectAllOrderBillItemsByBillSequence(long billSequence)
        {
            List<OrderBillItems> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrdersBillsItemsQueries.getSelectAllOrderBillItemsByBillSequence(this.DatabaseType, billSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrderBillItems>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_Order_Bill_Items(row));
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

        private OrderBillItems Load_Order_Bill_Items(DataRow row)
        {
            OrderBillItems items = null;
            try
            {
                if (row != null)
                {

                    items = new OrderBillItems();
                    items.Sequence = DBUtil.GetLongValue(row, "sequence");
                    items.JobSequence = DBUtil.GetLongValue(row, "job_sequence");
                    items.ItemSequence = DBUtil.GetLongValue(row, "item_sequence");
                    items.BillSequence = DBUtil.GetLongValue(row, "bill_sequence");
                    items.ItemType = DBUtil.GetIntValue(row,"item_type");
                    items.FlgTextLine = DBUtil.GetBooleanValue(row,"flg_row_is_text");
                    items.ItemCode = DBUtil.GetStringValue(row,"item_code");
                    items.ItemDesc = DBUtil.GetStringValue(row,"item_desc");
                    items.ItemUnits = DBUtil.GetStringValue(row, "item_units");
                    items.ItemQty = DBUtil.GetDoubleValue(row,"item_quantity");
                    items.ItemAmountBalance = DBUtil.GetDoubleValue(row,"amount_balance");
                    items.ItemAmountTotal = DBUtil.GetDoubleValue(row,"amount_total");
                    items.PcentPayment = DBUtil.GetDoubleValue(row, "pcent_payment");
                    items.AmountPayment = DBUtil.GetDoubleValue(row, "amount_payment");
                    items.FlgDiscounted = DBUtil.GetBooleanValue(row, "flg_discounted");
                    items.PcentDiscount = DBUtil.GetDoubleValue(row, "pcent_discount");
                    items.AmountDiscount = DBUtil.GetDoubleValue(row, "amount_discount");
                    items.FlgRetention = DBUtil.GetBooleanValue(row, "flg_retention");
                    items.PcentRetention = DBUtil.GetDoubleValue(row, "pcent_retention");
                    items.AmountRetention = DBUtil.GetDoubleValue(row, "amount_retention");
                    items.AmountSubTotal = DBUtil.GetDoubleValue(row, "amount_sub_total");
                    items.PcentVat = DBUtil.GetDoubleValue(row, "pcent_vat");
                    items.AmountVat = DBUtil.GetDoubleValue(row, "amount_vat");
                    items.PcentCis = DBUtil.GetDoubleValue(row, "pcent_cis");
                    items.AmountCis = DBUtil.GetDoubleValue(row, "amount_cis");
                    items.SageNominalCode = DBUtil.GetStringValue(row, "sage_nominal_code");
                    items.SageTaxCode = DBUtil.GetStringValue(row, "sage_tax_code");
                    items.SageBankCode = DBUtil.GetStringValue(row, "sage_bank_code");
                    items.CostCentreId = DBUtil.GetStringValue(row, "cost_centre_id");
                  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return items;
        }
    }
}
