using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetRegisterCatsQueries
    { 

        public static string getSelectAllByAssetCategoryId(string databaseType, long assetCategoryId)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_asset_register_cats" +
                        " WHERE asset_category_id = " + assetCategoryId;
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string insert(string databaseType, bool flgDeleted, string  assetCategoryDesc)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_asset_register_cats (flg_deleted, asset_category_desc)" +
                        "VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", '" + assetCategoryDesc + "')";
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string update(string databaseType, long assetCategoryId, bool flgDeleted, string assetCategoryDesc)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_asset_register_cats " +
                        "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " +
                        "   asset_category_desc =  '" + assetCategoryDesc + "', " +
                        " WHERE asset_category_id = " + assetCategoryId;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string delete(string databaseType, long assetCategoryId)
       {
           string returnValue = "";
           try
           {
                returnValue = "DELETE FROM un_asset_register_cats " +
                        " WHERE asset_category_id = " + assetCategoryId;
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }

       public static string deleteFlagDeleted(string databaseType, long assetCategoryId)
       {
           string returnValue = "";
           try
           {
                bool flg = true;
                returnValue = "UPDATE un_asset_register_cats" +
                    "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                    " WHERE asset_category_id = " + assetCategoryId;
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }       

    }
}

