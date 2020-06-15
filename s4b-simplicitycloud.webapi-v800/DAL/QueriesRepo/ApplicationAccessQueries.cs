using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class ApplicationAccessQueries
    { 

        public static string getSelectAllByProcessId(string databaseType, long processId)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                              "  FROM un_application_access" +
                              " WHERE process_id = " + processId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string insert(string databaseType, bool usrLevel01, bool usrLevel02, bool usrLevel03, bool usrLevel04, bool usrLevel05, bool usrLevel06,
                                   bool usrLevel07, bool usrLevel08, bool usrLevel09, bool usrLevel10)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "INSERT INTO un_application_access(usr_level01, usr_level02, usr_level03, usr_level04, usr_level05, usr_level06, usr_level07, usr_level08, usr_level09, usr_level10) " +
                              "VALUES (" + usrLevel01 + ", '" + usrLevel02 + ", " + usrLevel03 + ", " + usrLevel04 + ", " + usrLevel05 + ", " + usrLevel06 + ", " + usrLevel07 + ", " + usrLevel08 +
                                       ", " + usrLevel09 + ", " + usrLevel10 + "')";
                       
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string update(string databaseType, long processId, bool usrLevel01, bool usrLevel02, bool usrLevel03, bool usrLevel04, bool usrLevel05, bool usrLevel06,
                                  bool usrLevel07, bool usrLevel08, bool usrLevel09, bool usrLevel10)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "UPDATE un_application_access " +
                        "   SET usr_level01 =  " + usrLevel01 + ", " +
                        "       usr_level02 =  " + usrLevel02 + ", " +
                        "       usr_level03 =  " + usrLevel03 + ", " +
                        "       usr_level04 =  " + usrLevel04 + ", " +
                        "       usr_level05 =  " + usrLevel05 + ", " +
                        "       usr_level06 =  " + usrLevel06 + ", " +
                        "       usr_level07 =  " + usrLevel07 + ", " +
                        "       usr_level08 =  " + usrLevel08 + ", " +
                        "       usr_level09 =  " + usrLevel09 + ", " +
                        "       usr_level10 =  " + usrLevel10 + ", " +
                        "WHERE process_id = " + processId;
                        
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string delete(string databaseType, long processId)
           {
               string returnValue = "";
               try
               {
                
                        returnValue = "DELETE FROM un_application_access " +
                                 "WHERE process_id = " + processId;
                
               }
               catch (Exception ex)
               {
               }
               return returnValue;
           }
    
    }
}

