using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class ProductListQueries
    {
        public static string getSelectAllBygroupId(string databaseType, long groupId)
        {
            string returnValue = "";
            try
            {
                           
                      returnValue = "SELECT * " +
                                    "  FROM    un_product_list" +
                                    " WHERE group_id = " + groupId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByProductCode(string databaseType, string productCode)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                                      "  FROM un_product_list " +
                                      " WHERE UCASE(product_code) = " + productCode.ToUpper();
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType,  long groupId,  string transType,  string productCode,  string productUnits,  double amountLabour,  double amountPlant,  
                                    double amountMaterials,  double amountTotal,  string productVam,  string productCostCentre,  long sageId,  string productSageNominalCode,  
                                    string productSageTaxCode,  string productOutsDia,  string productDOrW,  string productWeight,  string productLength,  string productWidth, 
                                    string productHeight,  string productAreaMin,  string productAreaMax,  string productTypeId,  long createdBy,  DateTime dateCreated,
                                    long lastAmendedBy,  DateTime dateLastAmended)
        {
            string returnValue = "";
            try
            {
                                      
                       returnValue = "INSERT INTO   un_product_list(group_id,  trans_type,  product_code,  product_units,  amount_labour,  amount_plant,  amount_materials,  amount_total,"+
                                     "                              product_vam,  product_cost_centre,  sage_id,  product_sage_nominal_code,  product_sage_tax_code,  product_outs_dia,"+
                                     "                              product_d_or_w,  product_weight,  product_length,  product_width,  product_height,  product_area_min,  product_area_max,"+
                                     "                              product_type_id,  created_by,  date_created,  last_amended_by,  date_last_amended)" +
                                     "VALUES ( " +  groupId + ",   '" +  transType + "',   '" +  productCode + "',   '" +  productUnits + "',  " +  amountLabour + ",  " +  amountPlant + ",  " + 
                                     amountMaterials + ",  " +  amountTotal + ",   '" +  productVam + "',   '" +  productCostCentre + "',  " +  sageId + ",   '" +  productSageNominalCode + "',   '" + 
                                     productSageTaxCode + "',   '" +  productOutsDia + "',   '" +  productDOrW + "',   '" +  productWeight + "',   '" +  productLength + "',   '" + 
                                     productWidth + "',   '" +  productHeight + "',   '" +  productAreaMin + "',   '" +  productAreaMax + "',   '" +  productTypeId + "',  " +  
                                     createdBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ",  " +  lastAmendedBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true)+ ")";
                    
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, long groupId, string transType, string productCode, string productUnits, double amountLabour, double amountPlant,
                                    double amountMaterials, double amountTotal, string productVam, string productCostCentre, long sageId, string productSageNominalCode,
                                    string productSageTaxCode, string productOutsDia, string productDOrW, string productWeight, string productLength, string productWidth,
                                    string productHeight, string productAreaMin, string productAreaMax, string productTypeId, long createdBy, DateTime dateCreated,
                                    long lastAmendedBy, DateTime dateLastAmended)
        {
            string returnValue = "";
            try
            {
               
                        
                    returnValue = "UPDATE   un_product_list" +
                                 "   SET   trans_type =  '" +  transType + "',  " + 
		                         " product_code =  '" +  productCode + "',  " + 
		                         " product_units =  '" +  productUnits + "',  " + 
		                         " amount_labour =  " +  amountLabour + ",  " + 
		                         " amount_plant =  " +  amountPlant + ",  " + 
		                         " amount_materials =  " +  amountMaterials + ",  " + 
		                         " amount_total =  " +  amountTotal + ",  " + 
		                         " product_vam =  '" +  productVam + "',  " + 
		                         " product_cost_centre =  '" +  productCostCentre + "',  " + 
		                         " sage_id =  " +  sageId + ",  " + 
		                         " product_sage_nominal_code =  '" +  productSageNominalCode + "',  " + 
		                         " product_sage_tax_code =  '" +  productSageTaxCode + "',  " + 
		                         " product_outs_dia =  '" +  productOutsDia + "',  " + 
		                         " product_d_or_w =  '" +  productDOrW + "',  " + 
		                         " product_weight =  '" +  productWeight + "',  " + 
		                         " product_length =  '" +  productLength + "',  " + 
		                         " product_width =  '" +  productWidth + "',  " + 
		                         " product_height =  '" +  productHeight + "',  " + 
		                         " product_area_min =  '" +  productAreaMin + "',  " + 
		                         " product_area_max =  '" +  productAreaMax + "',  " + 
		                         " product_type_id =  '" +  productTypeId + "',  " + 
		                         " created_by =  " +  createdBy + ",  " + 
		                         " date_created =  " +  Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " + 
		                         " last_amended_by =  " +  lastAmendedBy + ",  " + 
		                         " date_last_amended =  " +  Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                                 "  WHERE group_id = " + groupId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string delete(string databaseType, long groupId)
        {
            string returnValue = "";
            try
            {
             
                       returnValue = "DELETE FROM   un_product_list" +
                                     "WHERE group_id = " + groupId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string deleteFlagDeleted(string databaseType, long groupId)
        {
            string returnValue = "";
            try
            {
               
                    bool flg = true;
                    returnValue = "UPDATE   un_product_list" +
                                  "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                                  "WHERE group_id = " + groupId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

