using System;
using SimplicityOnlineWebApi.Commons;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetRegisterLocationQueries
    { 

        public static string getSelectAllBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_asset_register_location " +
                        " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string insert(string databaseType, long entityId, bool flgDeleted, bool flgUseBuilding, string  assetLocationBuilding, bool flgUseFloor, 
                                   string assetLocationFloor, bool flgUseRoom, string assetLocationRoom, long assetLocationRoomType, string assetLocation)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "INSERT INTO un_asset_register_location (entity_id, flg_deleted, flg_use_building, asset_location_building, flg_use_floor, asset_location_floor, flg_use_room" +
                                                                ",asset_location_room, asset_location_room_type, asset_location)" +
                        "VALUES (" + entityId + ", " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " + Utilities.GetBooleanForDML(databaseType, flgUseBuilding) + ", '" + assetLocationBuilding + "', " + Utilities.GetBooleanForDML(databaseType, flgUseFloor) + ", '" + assetLocationFloor + "', " + Utilities.GetBooleanForDML(databaseType, flgUseRoom) +
                        ", '" + assetLocationRoom + "', " + assetLocationRoomType + ", '" + assetLocation + "')";
                       
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string update(string databaseType, long sequence, long entityId, bool flgDeleted, bool flgUseBuilding, string assetLocationBuilding, bool flgUseFloor,
                                   string assetLocationFloor, bool flgUseRoom, string assetLocationRoom, long assetLocationRoomType, string assetLocation)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_asset_register_location " +
                        "   SET entity_id =  " + entityId + ", " +
                        "   flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " +
                        "   flg_use_building =  " + Utilities.GetBooleanForDML(databaseType, flgUseBuilding) + ", " +
                        "   asset_location_building =  '" + assetLocationBuilding + "', " +
                        "   flg_use_floor=  " + Utilities.GetBooleanForDML(databaseType, flgUseFloor) + ", " +
                        "   asset_location_floor =  '" + assetLocationFloor + "', " +
                        "   flg_use_room =  " + Utilities.GetBooleanForDML(databaseType, flgUseRoom) + ", " +
                        "   asset_location_room =  '" + assetLocationRoom + "', " +
                        "   asset_location_room_type =  " + assetLocationRoomType + ", " +
                        "   asset_location =  '" + assetLocation + "', " +
                        "WHERE sequence = " + sequence;
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string delete(string databaseType, long sequence)
       {
           string returnValue = "";
           try
           {
               
                returnValue = "DELETE FROM un_asset_register_location " +
                    " WHERE sequence = " + sequence;
               
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }

       public static string deleteFlagDeleted(string databaseType, long sequence)
       {
           string returnValue = "";
           try
           {
               bool flg = true;
                returnValue = "UPDATE un_asset_register_location" +
                        "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                        " WHERE sequence = " + sequence;
                
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }       


    }
}

