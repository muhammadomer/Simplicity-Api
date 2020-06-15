using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Globalization;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrderHireDB : MainDB
    {

        public OrderHireDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertOrderHire(out long sequence, OrderHire obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(OrderHireQueries.insert(this.DatabaseType, obj), conn))
                    {
                        Utilities.WriteLog("Insert Query:" + objCmd.CommandText);
                        objCmd.ExecuteNonQuery();
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

        public bool updateBySequence(OrderHire obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                   
                    using (OleDbCommand objCmd =
                        new OleDbCommand(OrderHireQueries.update(this.DatabaseType, obj), conn))
                    {
                        Utilities.WriteLog("Update Query:" + objCmd.CommandText);
                        objCmd.ExecuteNonQuery();
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
        public List<OrderHire> selectListOfOrdersHire(ClientRequest clientRequest,DateTime? fromDate, DateTime? toDate,int hireType, out int count, bool isCountRequired)
        {
            List<OrderHire> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderHireQueries.getSelectListOfOrdersHire(this.DatabaseType,clientRequest, fromDate,toDate,hireType), conn))
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
                            returnValue = new List<OrderHire>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OrderHire(row));
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

        public OrderHire selectOrdersHireBySequence( int sequence)
        {
            OrderHire returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderHireQueries.getSelectOrdersHireBySequence(this.DatabaseType, sequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new OrderHire();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue = Load_OrderHire(row);
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

        public List<OrderHire> selectAssetSelectedForDateRange(long assetSequence,DateTime? fromDate, DateTime? toDate)
        {
            List<OrderHire> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderHireQueries.getAssetSelectedForDateRange(this.DatabaseType, assetSequence, fromDate, toDate), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrderHire>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OrderHire(row));
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
        public List<OrderHire> selectOrdersHireForReportByDate (DateTime? fromDate, DateTime? toDate)
        {
            List<OrderHire> returnValue = new List<OrderHire>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand cmd = new OleDbCommand(OrderHireQueries.getSelectOrderHireForReportByDate(this.DatabaseType, fromDate,toDate), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if(dt.Rows!=null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows){
                                returnValue.Add( Load_OrderHire(row));
                            }
                        }

                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        private OrderHire Load_OrderHire(DataRow row)
        {
            OrderHire orderHire = null;
            if (row != null)
            {
                orderHire = new OrderHire();
                orderHire.Sequence = DBUtil.GetLongValue(row, "sequence");
                orderHire.JobSequence = DBUtil.GetLongValue(row, "job_sequence");
                orderHire.JobRef = DBUtil.GetStringValue(row, "job_ref");
                orderHire.JobAddress = DBUtil.GetStringValue(row, "job_address");
                orderHire.HireType = DBUtil.GetIntValue(row, "hire_type");
                orderHire.AssetSequence = DBUtil.GetLongValue(row, "asset_sequence");
                orderHire.POISequence = DBUtil.GetLongValue(row, "poi_sequence");
                orderHire.AssetId = DBUtil.GetLongValue(row, "asset_Id");
                orderHire.ItemModel = DBUtil.GetLongValue(row, "item_model");
                orderHire.AssetCategory = DBUtil.GetStringValue(row, "asset_category_desc");
                orderHire.SupplierName = DBUtil.GetStringValue(row, "supplier");
                orderHire.SupplierAddress = DBUtil.GetStringValue(row, "supplier_address");
                orderHire.SupplierPORef = DBUtil.GetStringValue(row, "supplier_po_ref");
                orderHire.Location = DBUtil.GetStringValue(row, "location");
                orderHire.ItemCode = DBUtil.GetStringValue(row, "item_code");
                orderHire.ItemDesc = DBUtil.GetStringValue(row, "item_desc");
                orderHire.ItemQuantity = DBUtil.GetDoubleValue(row, "item_quantity");
                orderHire.ContractRef = DBUtil.GetStringValue(row, "contract_ref");
                orderHire.FlgChargeable = DBUtil.GetBooleanValue(row, "flg_chargeable");
                orderHire.RateType = DBUtil.GetIntValue(row, "rate_type");
                orderHire.DateHireStart = DBUtil.GetDateValue(row, "date_hire_start");
                orderHire.DateHireEnd = DBUtil.GetDateValue(row, "date_hire_end");
                orderHire.FlgHalfDay = DBUtil.GetBooleanValue(row, "flg_half_day");
                orderHire.NumberOfDays = DBUtil.GetLongValue(row, "number_of_days");
                orderHire.NumberOfWeeks = DBUtil.GetLongValue(row, "number_of_weeks");
                orderHire.TotalDays = DBUtil.GetLongValue(row, "total_days");
                orderHire.FlgRretruned = DBUtil.GetBooleanValue(row, "flg_retruned");
                orderHire.DateReturned = DBUtil.GetDateValue(row, "date_retruned");
                orderHire.EndRreferenece = DBUtil.GetStringValue(row, "end_referenece");
                orderHire.FlgEextendHire = DBUtil.GetBooleanValue(row, "flg_extend_hire");
                orderHire.ExtendHireRef = DBUtil.GetStringValue(row, "extend_hire_ref");
                orderHire.FlgDamaged = DBUtil.GetBooleanValue(row, "flg_damaged");
                orderHire.DamageType = DBUtil.GetIntValue(row, "damage_type");
                orderHire.DateDamaged = DBUtil.GetDateValue(row, "date_damaged");
                orderHire.DamageTypeDesc = DBUtil.GetStringValue(row, "damage_type_desc");
                orderHire.HireDayRate = DBUtil.GetDoubleValue(row, "hire_day_rate");
                orderHire.HireDayRate = DBUtil.GetDoubleValue(row, "hire_day_rate");
                orderHire.HireTotal = DBUtil.GetDoubleValue(row, "hire_total");
                orderHire.HireNotes = DBUtil.GetStringValue(row, "hire_notes");
                orderHire.HireTfrCostsTotal = DBUtil.GetDoubleValue(row, "hire_tfr_costs_total");
                orderHire.JobCostCenter = DBUtil.GetStringValue(row, "job_cost_centre");
                orderHire.CreatedBy = DBUtil.GetIntValue(row,"created_by");
                orderHire.DateCreated = DBUtil.GetDateValue(row,"date_created");
                orderHire.LastAmendedBy = DBUtil.GetIntValue(row,"last_amended_by");
                orderHire.DateLastAmended = DBUtil.GetDateValue(row,"date_last_amended");
            }
            return orderHire;
        }
    }
}
