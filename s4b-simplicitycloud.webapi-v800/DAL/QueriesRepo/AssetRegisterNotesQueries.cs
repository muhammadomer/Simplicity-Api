using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetRegisterNotesQueries
    { 

        public static string getSelectAllBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_asset_register_notes" +
                        " WHERE sequence = " + sequence;
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long joinSequence, long assetSequence, long assetNote, long createdBy, DateTime? dateCreated,
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "INSERT INTO un_asset_register_notes (join_sequence, asset_sequence, asset_note, created_by, date_created, last_amended_by, date_last_amended)" +
                        "VALUES (" + joinSequence + ", " + assetSequence + ", " + assetNote + ", " + createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " + lastAmendedBy + ", "
                                    + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, long sequence, long joinSequence, long assetSequence, string assetNote, long createdBy, DateTime? dateCreated,
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_asset_register_notes " +
                    "   SET join_sequence =  " + joinSequence + "," +
                    "  asset_sequence =  " + assetSequence + "," +
                    "  asset_note =  " + assetNote + "," +
                    "  created_by =  " + createdBy + "," +
                    "  date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + "," +
                    "  last_amended_by =  " + lastAmendedBy + "," +
                    "  date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true)  +
                    " WHERE sequence = " + sequence;
                
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
                returnValue = "DELETE FROM un_asset_register_notes " +
                             " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

    }
}

