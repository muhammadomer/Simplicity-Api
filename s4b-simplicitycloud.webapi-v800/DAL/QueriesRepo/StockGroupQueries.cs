using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class StockGroupQueries
    {

        public static string getSelectAllByentityId(string databaseType, long entityId)
        {
            string returnValue = "";
            try
            {
                returnValue = " SELECT  FROM    un_stock_group WHERE entity_id = " + entityId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string SelectAllByGroupCodeAndTreeviewLevel(string databaseType, string groupCode, int treeviewLevel)
        {
            return @"SELECT * 
                       FROM un_stock_group
                      WHERE group_code = '" + groupCode + "' " +
                    "   AND treeview_level = " + treeviewLevel;
        }

        public static string Insert(string databaseType, long entityId, bool flgHidden, string groupCode, int treeviewLevel, string parentCode, 
                                    string groupDesc, int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_stock_group(entity_id,  flg_hidden,  group_code,  treeview_level,  parent_code,  group_desc,  created_by,  date_created," +
                                      "                             last_amended_by,  date_last_amended)" +
                                      "VALUES ( " + entityId + ",   " + Utilities.GetBooleanForDML(databaseType, flgHidden) + ",   '" + groupCode + "',   '" + treeviewLevel + "',   '" + parentCode + "',   '" +
                                      groupDesc + "',  " + createdBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ",  " + lastAmendedBy + ",   " +
                                      Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, long entityId, bool flgHidden, string groupCode, int treeviewLevel, string parentCode, string groupDesc,
                                    int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                
                returnValue = " UPDATE   un_stock_group" +
                                "   SET  flg_hidden = " + Utilities.GetBooleanForDML(databaseType, flgHidden) + ",  " +
                                " group_code =  '" + groupCode + "',  " +
                                " treeview_level =  '" + treeviewLevel + "',  " +
                                " parent_code =  '" + parentCode + "',  " +
                                " group_desc =  '" + groupDesc + "',  " +
                                " created_by =  " + createdBy + ",  " +
                                " last_amended_by =  " + lastAmendedBy + ",  " +
                                " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                                " WHERE entity_id = " + entityId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long entityId)
        {
            string returnValue = "";
            try
            {
               
                returnValue = " DELETE FROM   un_stock_group WHERE entity_id = " + entityId;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long entityId)
        {
            string returnValue = "";
            try
            {
                bool flg = true;
                returnValue = " UPDATE   un_stock_group" +
                                      "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                                      " WHERE entity_id = " + entityId;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

