using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefGenericLabelsQueries
    {

        public static string getSelectAll(string databaseType)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * FROM un_ref_generic_labels ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByGenericFieldId(string databaseType, long genericFieldId)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                              "  FROM un_ref_generic_labels " +
                              " WHERE generic_field_id = " + genericFieldId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByGeneticFieldName(string databaseType,string geneticFieldName)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                              "  FROM un_ref_generic_labels " +
                              " WHERE genetic_field_name = '" + geneticFieldName + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType,long genericFieldId, string geneticFieldName, string  customisedFieldName)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "INSERT INTO un_ref_generic_labels (generic_field_id, genetic_field_name, "+
                                      "       customised_field_name) " +
                                      "VALUES ("+ genericFieldId + ",'" + geneticFieldName + "', '" + customisedFieldName + "')";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string update(string databaseType,long genericFieldId, string geneticFieldName, string customisedFieldName)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "UPDATE un_ref_generic_labels " +
                                      "SET genetic_field_name =  '" + geneticFieldName + "', " +
                                      "customised_field_name =  '" + customisedFieldName + "' " +
                                      "WHERE generic_field_id = " + genericFieldId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string delete(string databaseType, long genericFieldId)
       {
           string returnValue = "";
           try
           {
               
                        returnValue = "DELETE FROM un_ref_generic_labels " +
                             " WHERE generic_field_id = " + genericFieldId;
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }

    }
}

