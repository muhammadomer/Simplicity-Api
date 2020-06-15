using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class ScheduleItemsDB : MainDB
    {

        public ScheduleItemsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public List<ScheduleItemsHierarchy> selectScheduleItemHierarchy(int groupId,out int count, bool isCountRequired)
        {
            List<ScheduleItemsHierarchy> returnValue = null;
            count = 0;
            string parentCode = "",category="";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(ScheduleItemsQueries.selectScheduleItemHierarchy(this.DatabaseType, groupId), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        if (isCountRequired)
                        {
                            count = da.Fill(new DataSet("temp"));
                        }
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<ScheduleItemsHierarchy>();
                            ScheduleItemsHierarchy item= new ScheduleItemsHierarchy();
                            ScheduleItemsHierarchy categoryItems = new ScheduleItemsHierarchy();
                            foreach (DataRow row in dt.Rows)
                            {
                                if(row["ztop_product_code"].ToString() == parentCode)
                                {
                                    if(row["category_parent_code"].ToString() == row["ztop_product_code"].ToString() && row["category_product_code"].ToString() != category)
                                    {
                                        categoryItems = AddCategory(row,dt); 
                                        item.Child.Add(categoryItems);
                                        category = row["category_product_code"].ToString();
                                    }
                                }
                                else
                                {
                                    //add parent
                                    item = AddTopLevel(row);
                                    item.Child= new List<ScheduleItemsHierarchy>();
                                    if (row["category_parent_code"].ToString() != "")// add category
                                    {
                                        categoryItems = AddCategory(row,dt); ;
                                        item.Child.Add(categoryItems);
                                        category = row["category_product_code"].ToString();
                                    }
                                   
                                    returnValue.Add(item);
                                    parentCode = row["ztop_product_code"].ToString();
                                }
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

        public List<ScheduleItems> selectScheduleItemsByGroup(int groupId,string parentCode, out int count, bool isCountRequired)
        {
            List<ScheduleItems> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(ScheduleItemsQueries.selectScheduleItemsByGroup(this.DatabaseType, groupId, parentCode), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        if (isCountRequired)
                        {
                            count = da.Fill(new DataSet("temp"));
                        }
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<ScheduleItems>();
                            foreach(DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_ScheduleItems(row));
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

        public List<ProductGroupDesc> selectItemsGroupsDesc(out int count, bool isCountRequired)
        {
            List<ProductGroupDesc> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(ScheduleItemsQueries.selectItemsGroupsDesc(this.DatabaseType), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        if (isCountRequired)
                        {
                            count = da.Fill(new DataSet("temp"));
                        }
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<ProductGroupDesc>();
                            foreach (DataRow row in dt.Rows)
                            { 
                                returnValue.Add(Load_GroupDesc(row));
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
        private ProductGroupDesc Load_GroupDesc(DataRow row)
        {
            ProductGroupDesc group = null;
            try
            {
                if (row != null)
                {
                    group = new ProductGroupDesc();
                    group.GroupId = DBUtil.GetIntValue(row, "group_id");
                    group.TransType = DBUtil.GetStringValue(row, "trans_type");
                    group.GroupDesc = DBUtil.GetStringValue(row, "group_desc");
                    group.CurrencyCode = DBUtil.GetStringValue(row, "currency_code");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return group;
        }
        private ScheduleItems Load_ScheduleItems(DataRow row)
        {
            ScheduleItems item = null;
            try
            {
                if (row != null)
                {
                    item = new ScheduleItems();
                    item.GroupId = DBUtil.GetIntValue(row, "group_id");
                    item.ProductCode = DBUtil.GetStringValue(row, "product_code");
                    item.ParentCode = DBUtil.GetStringValue(row, "parent_code");
                    item.ProductDesc = DBUtil.GetStringValue(row, "product_desc");
                    item.ProductUnits = DBUtil.GetStringValue(row, "product_units");
                    item.AmountLabour = DBUtil.GetDoubleValue(row, "amount_labour");
                    item.AmountPlant = DBUtil.GetDoubleValue(row, "amount_plant");
                    item.AmountMaterials = DBUtil.GetDoubleValue(row, "amount_materials");
                    item.AmountTotal = DBUtil.GetDoubleValue(row, "amount_total");
                    item.ItemVam = DBUtil.GetDateTimeValue(row, "product_vam");
                    item.ProductCostCentre = DBUtil.GetStringValue(row, "product_cost_centre");
                    item.ProductWeight = DBUtil.GetDoubleValue(row, "product_weight");
                    item.ProductLength = DBUtil.GetDoubleValue(row, "product_length");
                    item.ProductWidth = DBUtil.GetDoubleValue(row, "product_width");
                    item.ProductHeight = DBUtil.GetDoubleValue(row, "product_height");
                 }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return item;
        }
        private ScheduleItemsHierarchy AddTopLevel(DataRow row)
        {
            ScheduleItemsHierarchy Item = null;
            try
            {
                if (row != null)
                {

                    Item = new ScheduleItemsHierarchy();
                    Item.GroupId = Convert.ToInt32(row["group_id"].ToString());
                    Item.ParentCode = row["ztop_parent_code"].ToString();
                    Item.ProductCode = row["ztop_product_code"].ToString();
                    Item.ProductDesc = row["ztop_product_desc"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Item;
        }

        private ScheduleItemsHierarchy AddCategory(DataRow row,DataTable dt)
        {
            ScheduleItemsHierarchy Items = null;
            ScheduleItemsHierarchy subCategoryItems = new ScheduleItemsHierarchy();
            try
            {
                if (row != null)
                {

                    Items = new ScheduleItemsHierarchy();
                    Items.GroupId = Convert.ToInt32(row["group_id"].ToString());
                    Items.ParentCode = row["category_parent_code"].ToString();
                    Items.ProductCode = row["category_product_code"].ToString();
                    Items.ProductDesc = row["category_product_desc"].ToString();
                    Items.Child = new List<ScheduleItemsHierarchy>();
                    //---Find all Sub-Category of this category
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "subCategory_parent_code='" + row["category_product_code"].ToString() +"'";
                    foreach (DataRow dr in dv.ToTable().Rows)
                    {
                        subCategoryItems = AddSubCategory(dr);
                        Items.Child.Add(subCategoryItems);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Items;
        }

        private ScheduleItemsHierarchy AddSubCategory(DataRow row)
        {
            ScheduleItemsHierarchy Items = null;
            try
            {
                if (row != null)
                {

                    Items = new ScheduleItemsHierarchy();
                    Items.GroupId = Convert.ToInt32(row["group_id"].ToString());
                    Items.ParentCode = row["subCategory_parent_code"].ToString();
                    Items.ProductCode = row["subCategory_product_code"].ToString();
                    Items.ProductDesc = row["subCategory_product_desc"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Items;
        }


    }
}
