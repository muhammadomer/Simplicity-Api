using System;
using SimplicityOnlineWebApi.Commons;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetRegisterDeptsQueries
    { 

        public static string getSelectAllByDeptId(string databaseType,long assetDeptId)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_asset_register_depts" +
                        " WHERE asset_dept_id = " + assetDeptId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string insert(string databaseType, bool flgDeleted, string  assetDeptDesc)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_asset_register_depts (flg_deleted, asset_dept_desc)" +
                        "VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", ' " + assetDeptDesc + "')";
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string update(string databaseType, long assetDeptId, bool flgDeleted, string assetDeptDesc)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "UPDATE un_asset_register_Depts " +
                        "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flgDeleted)  +
                        "   asset_Dept_desc =  '" + assetDeptDesc + "'" +
                        "  WHERE asset_dept_id = " + assetDeptId;
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string delete(string databaseType, long assetDeptId)
       {
           string returnValue = "";
           try
           {
                returnValue = "DELETE FROM un_asset_register_Depts " +
                        " WHERE asset_dept_id = " + assetDeptId;
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }

       public static string deleteFlagDeleted(string databaseType, long assetDeptId)
       {
           string returnValue = "";
           try
           {
                bool flg = true;
                returnValue = "UPDATE un_asset_register_Depts" +
                        "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                        " WHERE asset_dept_id = " + assetDeptId;
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }       


    }
}

