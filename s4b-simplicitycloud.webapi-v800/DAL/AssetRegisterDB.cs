using Microsoft.VisualBasic;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;

namespace SimplicityOnlineWebApi.DAL
{

	public class AssetRegisterDB: MainDB
		{
			 
        public AssetRegisterDB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

		public bool insertAssetRegister(out long sequence, bool flgDeleted, string transType, long entityId, long itemJoinDept, long itemJoinCategory, long itemJoinSupplementary,
										string itemManufacturer, string itemModel, string itemSerialRef, string itemExtraInfo, string itemUserField1,
										string itemUserField2, string itemUserField3, double itemQuantity, DateTime? dateInstalled, DateTime? dateAcquired,
										DateTime? dateDisposed, double itemValueBook, double itemValueDepreciation, double itemValueDisposal, string itemDesc,
										string itemAddress, bool flgUseAddressId, long itemAddressId, long itemLocationJoinId, string itemLocation,
										bool flgItemChargeable, double itemCostMaterialRate, double itemCostLabourRate, double itemCostAssetRateWeek, double itemCostLabourRateWeek,
										bool flgService, long serviceStartDay,
										long serviceStartMonth, long serviceRenewal, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
		{
			bool returnValue = false;
			sequence = -1;
			try
			{
				using (OleDbConnection conn = this.getDbConnection())
				{

					using (OleDbCommand objCmdInsert =
						new OleDbCommand(AssetRegisterQueries.insert(this.DatabaseType, flgDeleted, transType, entityId, itemJoinDept, itemJoinCategory, itemJoinSupplementary,
																	 Strings.Left(itemManufacturer, 32), Strings.Left(itemModel, 32), Strings.Left(itemSerialRef, 32),
																	 Strings.Left(itemExtraInfo, 32), Strings.Left(itemUserField1, 32),
																	 Strings.Left(itemUserField2, 32), Strings.Left(itemUserField3, 32), itemQuantity, dateInstalled, dateAcquired,
																	 dateDisposed, itemValueBook, itemValueDepreciation, itemValueDisposal, itemDesc,
																	 itemAddress, flgUseAddressId, itemAddressId, itemLocationJoinId, Strings.Left(itemLocation, 32),
																	 flgItemChargeable, itemCostMaterialRate, itemCostLabourRate, itemCostAssetRateWeek, itemCostLabourRateWeek, flgService, serviceStartDay,
																	 serviceStartMonth, serviceRenewal, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
					{
						objCmdInsert.ExecuteNonQuery();
						sequence = Utilities.GetDBAutoNumber(conn);
					}
				}
				returnValue = true;
			}
			catch (Exception ex)
			{
				//errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                   ex.InnerException;
				// Requires Logging
			}
			return returnValue;
		}

		public List<AssetRegister> selectAllAssetRegisterBySequence(long sequence)
            {
                List<AssetRegister> returnValue = null;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdSelect =
                            new OleDbCommand(AssetRegisterQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                        {
                            using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    returnValue = new List<AssetRegister>();
                                    while (dr.Read())
                                    {
                                        returnValue.Add(Load_AssetRegister(dr));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                             ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }

		public bool updateBySequence(long sequence, bool flgDeleted, string transType, long entityId, long itemJoinDept, long itemJoinCategory, long itemJoinSupplementary,
									string itemManufacturer, string itemModel, string itemSerialRef, string itemExtraInfo, string itemUserField1,
									string itemUserField2, string itemUserField3, string itemQuantity, DateTime? dateInstalled, DateTime? dateAcquired,
									DateTime? dateDisposed, double itemValueBook, double itemValueDepreciation, double itemValueDisposal, string itemDesc,
									string itemAddress, bool flgUseAddressId, long itemAddressId, long itemLocationJoinId, string itemLocation,
									bool flgItemChargeable, double itemCostMaterialRate, double itemCostLabourRate, double itemCostAssetRateWeek, double itemCostLabourRateWeek,
									bool flgService, long serviceStartDay,
									long serviceStartMonth, long serviceRenewal, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
		{
			bool returnValue = false;
			try
			{
				using (OleDbConnection conn = this.getDbConnection())
				{
					using (OleDbCommand objCmdUpdate =
						new OleDbCommand(AssetRegisterQueries.update(this.DatabaseType, sequence, flgDeleted, transType, entityId, itemJoinDept, itemJoinCategory, itemJoinSupplementary,
																   itemManufacturer, itemModel, itemSerialRef, itemExtraInfo, itemUserField1,
																   itemUserField2, itemUserField3, itemQuantity, dateInstalled, dateAcquired,
																   dateDisposed, itemValueBook, itemValueDepreciation, itemValueDisposal, itemDesc,
																   itemAddress, flgUseAddressId, itemAddressId, itemLocationJoinId, itemLocation,
																   flgItemChargeable, itemCostMaterialRate, itemCostLabourRate, itemCostAssetRateWeek, itemCostLabourRateWeek, flgService, serviceStartDay,
																   serviceStartMonth, serviceRenewal, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
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
		public List<AssetRegister> getAllAssetsList()
        {
            List<AssetRegister> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AssetRegisterQueries.getSelectAllAssets(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AssetRegister>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AssetRegister(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                             ex.InnerException;
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
                            new OleDbCommand(AssetRegisterQueries.deleteBySequence(this.DatabaseType, sequence), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                                ex.InnerException;
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
                            new OleDbCommand(AssetRegisterQueries.deleteBySequence(this.DatabaseType, sequence), conn))
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

        private AssetRegister Load_AssetRegister(OleDbDataReader dr)
        {
            AssetRegister assetRegister = null;
            try
            { 
                if(dr!=null)
                {
                    assetRegister = new AssetRegister();
                    assetRegister.ARC = new AssetRegisterCats();
                    assetRegister.ARD = new AssetRegisterDepts();
                    assetRegister.EDC = new EntityDetailsCore();
                    assetRegister.Arst = new RefAssetRegisterSupplementaryTables();
                    assetRegister.Sequence = long.Parse(dr["sequence"].ToString());
                    assetRegister.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    assetRegister.TransType = Utilities.GetDBString(dr["trans_type"]);
                    assetRegister.EntityId = long.Parse(dr["entity_id"].ToString());
                    assetRegister.ItemJoinDept = long.Parse(dr["item_join_dept"].ToString());
                    assetRegister.ItemJoinCategory = long.Parse(dr["item_join_category"].ToString());
                    assetRegister.ItemJoinSupplementary = long.Parse(dr["item_join_supplementary"].ToString());
                    assetRegister.ItemManufacturer = Utilities.GetDBString(dr["item_manufacturer"]);
                    assetRegister.ItemModel = Utilities.GetDBString(dr["item_model"]);
                    assetRegister.ItemSerialRef = Utilities.GetDBString(dr["item_serial_ref"]);
                    assetRegister.ItemExtraInfo = Utilities.GetDBString(dr["item_extra_info"]);
                    assetRegister.ItemUserField1 = Utilities.GetDBString(dr["item_user_field1"]);
                    assetRegister.ItemUserField2 = Utilities.GetDBString(dr["item_user_field2"]);
                    assetRegister.ItemUserField3 = Utilities.GetDBString(dr["item_user_field3"]);
                    assetRegister.ItemQuantity =  double.Parse(dr["item_quantity"].ToString());
                    assetRegister.DateInstalled = Utilities.getDBDate(dr["date_installed"]);
                    assetRegister.DateAcquired = Utilities.getDBDate(dr["date_acquired"]);
                    assetRegister.DateDisposed = Utilities.getDBDate(dr["date_disposed"]);
                    assetRegister.ItemValueBook = double.Parse(dr["item_value_book"].ToString());
                    assetRegister.ItemValueDepreciation = double.Parse(dr["item_value_depreciation"].ToString());
                    assetRegister.ItemValueDisposal = double.Parse(dr["item_value_disposal"].ToString());
                    assetRegister.ItemDesc = Utilities.GetDBString(dr["item_desc"]);
                    assetRegister.ItemAddress = Utilities.GetDBString(dr["item_address"]);
                    assetRegister.FlgUseAddressId = bool.Parse(dr["flg_use_address_id"].ToString());
                    assetRegister.ItemAddressId = long.Parse(dr["item_address_id"].ToString());
                    assetRegister.ItemLocationJoinId = long.Parse(dr["item_location_join_id"].ToString());
                    assetRegister.ItemLocation = Utilities.GetDBString(dr["item_location"]);
                    assetRegister.FlgItemChargeable = bool.Parse(dr["flg_item_chargeable"].ToString());
                    assetRegister.ItemCostMaterialRate = double.Parse(dr["item_cost_material_rate"].ToString());
                    assetRegister.ItemCostLabourRate = double.Parse(dr["item_cost_labour_rate"].ToString());
                    assetRegister.FlgService = bool.Parse(dr["flg_service"].ToString());
                    assetRegister.ServiceStartDay = Utilities.GetDBLong(dr["service_start_day"]);
                    assetRegister.ServiceStartMonth = Utilities.GetDBLong(dr["service_start_month"]);
                    assetRegister.ServiceRenewal = Utilities.GetDBLong(dr["service_renewal"].ToString());
                    assetRegister.CreatedBy = long.Parse(dr["created_by"].ToString());
                    assetRegister.DateCreated = Utilities.getDBDate(dr["date_created"]);
                    assetRegister.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    assetRegister.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
                    assetRegister.EDC.NameShort = dr["name_short"].ToString();
                    assetRegister.EDC.AddressFull = dr["address_full"].ToString();
                    assetRegister.ARC.AssetCategoryDesc = dr["asset_category_desc"].ToString();
                    assetRegister.ARD.AssetDeptDesc = dr["asset_dept_desc"].ToString();
                    assetRegister.Arst.SuppTableDesc = dr["supp_table_desc"].ToString();    
                }
            }
            catch(Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return assetRegister;
        }

        private AssetDetail Load_AssetDetail(OleDbDataReader dr)
        {
            AssetDetail assetDetail= null;
            try
            {
                if (dr != null)
                {
                    assetDetail = new AssetDetail();
                    //assetRegister.Sequence = long.Parse(dr["sequence"].ToString());
                    //assetRegister.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    //assetRegister.TransType = Utilities.getDBString(dr["trans_type"]);
                    //assetRegister.EntityId = long.Parse(dr["entity_id"].ToString());

                    assetDetail.DeSequence = long.Parse(dr["de_sequence"].ToString());
                    assetDetail.DateAppStart = Utilities.getDBDate(dr["date_app_start"]);
                    assetDetail.ResourceName = Utilities.GetDBString(dr["resource_name"]);
                    assetDetail.JobRef = dr["job_ref"].ToString();
                    assetDetail.JobAddress = Utilities.GetDBString(dr["job_address"].ToString());
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return assetDetail;
        }
        public List<AssetRegister> selectAllAssetsList(ClientRequest clientRequest, out int count, bool isCountRequired)
        {
            List<AssetRegister> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AssetRegisterQueries.getSelectAllAssetsList(this.DatabaseType, clientRequest), conn))
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
                            returnValue = new List<AssetRegister>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_AssetRegister(row));
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

        public List<AssetRegister> Search(FilterOption filter)
        {
            List<AssetRegister> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(AssetRegisterQueries.search(this.DatabaseType, filter), conn))
                    {
                        OleDbDataReader dr = objCmdUpdate.ExecuteReader();
                        if (dr.HasRows)
                        {
                            returnValue = new List<AssetRegister>();
                            while (dr.Read())
                            {
                                returnValue.Add(Load_AssetRegister(dr));
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

        public List<AssetDetail> getAssetsDetail(long sequence)
        {
            List<AssetDetail> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AssetRegisterQueries.getAssetDetails(this.DatabaseType,sequence ), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AssetDetail>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AssetDetail(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                             ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public AssetRegister getAssetRegisterByLocationMakeModelTypeSearialNo(string location, string make, string model, string type, string serialNo)
        {
            AssetRegister returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AssetRegisterQueries.getAssetRegisterByLocationMakeModelTypeSearialNo(this.DatabaseType, location, make, model, type, serialNo), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_AssetRegister(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                             ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        private AssetRegister Load_AssetRegister(DataRow row)
        {
            AssetRegister assetRegister = null;
            try
            {
                if (row != null)
                {
                    assetRegister = new AssetRegister();
                    assetRegister.ARC = new AssetRegisterCats();
                    assetRegister.ARD = new AssetRegisterDepts();
                    assetRegister.EDC = new EntityDetailsCore();
                    assetRegister.Arst = new RefAssetRegisterSupplementaryTables();
                    assetRegister.Sequence = DBUtil.GetLongValue(row, "sequence");
                    assetRegister.FlgDeleted = DBUtil.GetBooleanValue(row,"flg_deleted");
                    assetRegister.TransType = DBUtil.GetStringValue(row,"trans_type");
                    assetRegister.EntityId = DBUtil.GetLongValue(row,"entity_id");
                    assetRegister.ItemJoinDept = DBUtil.GetLongValue(row,"item_join_dept");
                    assetRegister.ItemJoinCategory = DBUtil.GetLongValue(row,"item_join_category");
                    assetRegister.ItemJoinSupplementary = DBUtil.GetLongValue(row,"item_join_supplementary");
                    assetRegister.ItemManufacturer = DBUtil.GetStringValue(row,"item_manufacturer");
                    assetRegister.AssetId = DBUtil.GetStringValue(row, "item_user_field1");
                    assetRegister.ItemModel = DBUtil.GetStringValue(row,"item_model");
                    assetRegister.ItemSerialRef = DBUtil.GetStringValue(row,"item_serial_ref");
                    assetRegister.ItemExtraInfo = DBUtil.GetStringValue(row,"item_extra_info");
                    assetRegister.ItemUserField1 = DBUtil.GetStringValue(row,"item_user_field1");
                    assetRegister.ItemUserField2 = DBUtil.GetStringValue(row,"item_user_field2");
                    assetRegister.ItemUserField3 = DBUtil.GetStringValue(row,"item_user_field3");
                    assetRegister.ItemQuantity = DBUtil.GetDoubleValue(row,"item_quantity");
                    assetRegister.DateInstalled = DBUtil.GetDateValue(row,"date_installed");
                    assetRegister.DateAcquired = DBUtil.GetDateValue(row,"date_acquired");
                    assetRegister.DateDisposed = DBUtil.GetDateValue(row,"date_disposed");
                    assetRegister.ItemValueBook = DBUtil.GetDoubleValue(row,"item_value_book");
                    assetRegister.ItemValueDepreciation = DBUtil.GetDoubleValue(row,"item_value_depreciation");
                    assetRegister.ItemValueDisposal = DBUtil.GetDoubleValue(row,"item_value_disposal");
                    assetRegister.ItemDesc = DBUtil.GetStringValue(row,"item_desc");
                    assetRegister.ItemAddress = DBUtil.GetStringValue(row,"item_address");
                    assetRegister.FlgUseAddressId = DBUtil.GetBooleanValue(row,"flg_use_address_id");
                    assetRegister.ItemAddressId = DBUtil.GetLongValue(row,"item_address_id");
                    assetRegister.ItemLocationJoinId = DBUtil.GetLongValue(row,"item_location_join_id");
                    assetRegister.ItemLocation = DBUtil.GetStringValue(row,"item_location");
                    assetRegister.FlgItemChargeable = DBUtil.GetBooleanValue(row,"flg_item_chargeable");
                    assetRegister.ItemCostMaterialRate = DBUtil.GetDoubleValue(row,"item_cost_material_rate");
                    assetRegister.ItemCostLabourRate = DBUtil.GetDoubleValue(row,"item_cost_labour_rate");
                    assetRegister.FlgService = DBUtil.GetBooleanValue(row,"flg_service");
                    assetRegister.ServiceStartDay = DBUtil.GetLongValue(row,"service_start_day");
                    assetRegister.ServiceStartMonth = DBUtil.GetLongValue(row,"service_start_month");
                    assetRegister.ServiceRenewal = DBUtil.GetLongValue(row,"service_renewal");
                    //assetRegister.CreatedBy = DBUtil.GetLongValue(row,"created_by");
                    //assetRegister.DateCreated = DBUtil.GetDateTimeValue(row,"date_created");
                    //assetRegister.LastAmendedBy = DBUtil.GetLongValue(row,"last_amended_by");
                    //assetRegister.DateLastAmended = DBUtil.GetDateTimeValue(row,"date_last_amended");
                    assetRegister.EDC.NameShort = DBUtil.GetStringValue( row,"name_short");
                    assetRegister.EDC.AddressFull = DBUtil.GetStringValue(row,"address_full");
                    assetRegister.ARC.AssetCategoryDesc = DBUtil.GetStringValue(row, "asset_category_desc");
                    assetRegister.ARD.AssetDeptDesc = DBUtil.GetStringValue(row,"asset_dept_desc");
                    assetRegister.Arst.SuppTableDesc = DBUtil.GetStringValue(row,"supp_table_desc");
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return assetRegister;
        }


    }
}