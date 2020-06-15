using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class ProductListDB : MainDB
    {

        public ProductListDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertProductList(out long groupId, string transType, string productCode, string productUnits, double amountLabour, double amountPlant,
                                     double amountMaterials, double amountTotal, string productVam, string productCostCentre, long sageId, string productSageNominalCode,
                                     string productSageTaxCode, string productOutsDia, string productDOrW, string productWeight, string productLength, string productWidth,
                                     string productHeight, string productAreaMin, string productAreaMax, string productTypeId, long createdBy, DateTime dateCreated,
                                     long lastAmendedBy, DateTime dateLastAmended)
        {
            bool returnValue = false;
            groupId = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(ProductListQueries.insert(this.DatabaseType, groupId, transType, productCode, productUnits, amountLabour, amountPlant,
                                                                    amountMaterials, amountTotal, productVam, productCostCentre, sageId, productSageNominalCode,
                                                                    productSageTaxCode, productOutsDia, productDOrW, productWeight, productLength, productWidth,
                                                                    productHeight, productAreaMin, productAreaMax, productTypeId, createdBy, dateCreated, lastAmendedBy,
                                                                    dateLastAmended), conn))
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
                                groupId = long.Parse(dr[0].ToString());
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

        public List<ProductList> selectAllProductListGroupId(long groupId)
        {
            List<ProductList> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(ProductListQueries.getSelectAllBygroupId(this.DatabaseType, groupId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<ProductList>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_ProductList(dr));
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

        public ProductList selectProductByProductCode(string productCode)
        {
            ProductList returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(ProductListQueries.getSelectAllByProductCode(this.DatabaseType, productCode), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new ProductList();
                                if(dr.HasRows)
                                {
                                    dr.Read();
                                    returnValue = Load_ProductList(dr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                //Requires Logging
            }
            return returnValue;
        }

        public bool updateBygroupId(long groupId, string transType, string productCode, string productUnits, double amountLabour, double amountPlant,
                                    double amountMaterials, double amountTotal, string productVam, string productCostCentre, long sageId, string productSageNominalCode,
                                    string productSageTaxCode, string productOutsDia, string productDOrW, string productWeight, string productLength, string productWidth,
                                    string productHeight, string productAreaMin, string productAreaMax, string productTypeId, long createdBy, DateTime dateCreated,
                                    long lastAmendedBy, DateTime dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(ProductListQueries.update(this.DatabaseType, groupId, transType, productCode, productUnits, amountLabour, amountPlant,
                                                                    amountMaterials, amountTotal, productVam, productCostCentre, sageId, productSageNominalCode,
                                                                    productSageTaxCode, productOutsDia, productDOrW, productWeight, productLength, productWidth,
                                                                    productHeight, productAreaMin, productAreaMax, productTypeId, createdBy, dateCreated, lastAmendedBy,
                                                                    dateLastAmended), conn))
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

        public bool deleteBygroupId(long groupId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(ProductListQueries.delete(this.DatabaseType, groupId), conn))
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

        public bool deleteByFlgDeleted(long groupId)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(ProductListQueries.deleteFlagDeleted(this.DatabaseType, groupId), conn))
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
        private ProductList Load_ProductList(OleDbDataReader dr)

        {
            ProductList productList = null;
            try
            {
                if (dr != null)
                {

                    productList = new ProductList();
                    productList.GroupId = long.Parse(dr["group_id"].ToString());
                    productList.TransType = Utilities.GetDBString(dr["trans_type"]);
                    productList.ProductCode = Utilities.GetDBString(dr["product_code"]);
                    productList.ProductUnits = Utilities.GetDBString(dr["product_units"]);
                    productList.ProductVam = Utilities.GetDBString(dr["product_vam"]);
                    productList.ProductCostCentre = Utilities.GetDBString(dr["product_cost_centre"]);
                    productList.SageId = long.Parse(dr["sage_id"].ToString());
                    productList.ProductSageNominalCode = Utilities.GetDBString(dr["product_sage_nominal_code"]);
                    productList.ProductSageTaxCode = Utilities.GetDBString(dr["product_sage_tax_code"]);
                    productList.ProductOutsDia = Utilities.GetDBString(dr["product_outs_dia"]);
                    productList.ProductDOrW = Utilities.GetDBString(dr["product_d_or_w"]);
                    productList.ProductWeight = Utilities.GetDBString(dr["product_weight"]);
                    productList.ProductLength = Utilities.GetDBString(dr["product_length"]);
                    productList.ProductWidth = Utilities.GetDBString(dr["product_width"]);
                    productList.ProductHeight = Utilities.GetDBString(dr["product_height"]);
                    productList.ProductAreaMin = Utilities.GetDBString(dr["product_area_min"]);
                    productList.ProductAreaMax = Utilities.GetDBString(dr["product_area_max"]);
                    productList.ProductTypeId = Utilities.GetDBString(dr["product_type_id"]);
                    productList.CreatedBy = long.Parse(dr["created_by"].ToString());
                    productList.DateCreated = Utilities.getSQLDate(DateTime.Parse(dr["date_created"].ToString()));
                    productList.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    productList.DateLastAmended = Utilities.getSQLDate(DateTime.Parse(dr["date_last_amended"].ToString()));

                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return productList;
        }

    }
}
