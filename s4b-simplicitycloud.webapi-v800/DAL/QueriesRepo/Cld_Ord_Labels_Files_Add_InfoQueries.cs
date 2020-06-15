using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class Cld_Ord_Labels_Files_Add_InfoQueries
    {

        public static string getSelectAllBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                              "  FROM un_cld_ord_labels_files_add_info " +
                              " WHERE sequence = " + sequence;
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByDesc(string databaseType, long jobSequence, string addInfo)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                             "  FROM un_cld_ord_labels_files_add_info " +
                             " WHERE job_sequence = " + jobSequence + " and add_info='" + addInfo + "'";
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long jobSequence, long oiSequence, long headerSequence, long joinSequence, bool flgDeleted, string addInfo, long createdBy, DateTime? dateCreated,
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "INSERT INTO un_cld_ord_labels_files_add_info (job_sequence, oi_sequence, header_sequence, join_sequence, flg_deleted, add_info, created_by, date_created, last_amended_by, date_last_amended) " +
                              "VALUES (" + jobSequence + ", " + oiSequence + ", " + headerSequence + ", " + joinSequence + ", " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", '" + addInfo + "', " + createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " + lastAmendedBy + ", "
                                         + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
                       
                

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, long sequence, long jobSequence, long oiSequence, long headerSequence, long joinSequence, bool flgDeleted, string addInfo, long createdBy, DateTime? dateCreated,
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "UPDATE un_cld_ord_labels_files_add_info " +
                             "   SET job_sequence =  " + jobSequence + ", " +
                              "  oi_sequence =  " + oiSequence + ", " +
                              "  header_sequence =  " + headerSequence + ", " +
                              "  join_sequence =  " + joinSequence + ", " +
                              "  flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " +
                              "  add_info =  '" + addInfo + "', " +
                              "  created_by =  " + createdBy + ", " +
                              "  date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                              "  last_amended_by =  " + lastAmendedBy + ", " +
                              "  date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
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
                
                returnValue = "DELETE FROM un_cld_ord_labels_files_add_info " +
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
                
                                returnValue = "UPDATE un_cld_ord_labels_files_add_info " +
                         "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                         " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        //public static DriveService AuthenticateOauth(string clientId, string clientSecret, string userName)
        //{

        //    //Google Drive scopes Documentation:   https://developers.google.com/drive/web/scopes
        //    string[] scopes = new string[] { DriveService.Scope.Drive,  // view and manage your files and documents
        //                                     DriveService.Scope.DriveAppdata,  // view and manage its own configuration data
        //                                     //DriveService.Scope.DriveAppsReadonly,   // view your drive apps
        //                                     DriveService.Scope.DriveFile,   // view and manage files created by this app
        //                                     DriveService.Scope.DriveMetadataReadonly,   // view metadata for files
        //                                     DriveService.Scope.DriveReadonly,   // view files and documents on your drive
        //                                     DriveService.Scope.DriveScripts };  // modify your app scripts


        //    try
        //    {
        //        // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
        //        UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
        //                                                                                     , scopes
        //                                                                                     , userName
        //                                                                                     , CancellationToken.None
        //                                                                                     , new FileDataStore("SimplicityOnline.Drive.Auth.Store")).Result;

        //        //using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
        //        //{
        //        //    string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //        //    credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart");

        //        //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //        //        GoogleClientSecrets.Load(stream).Secrets,
        //        //        scopes,
        //        //        userName,
        //        //        CancellationToken.None,
        //        //        new FileDataStore(credPath, true)).Result;
        //        //    Console.WriteLine("Credential file saved to: " + credPath);
        //        //}


        //        DriveService service = new DriveService(new BaseClientService.Initializer()
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = "Simplicity Online",
        //        });

        //        if(service==null)
        //        {
        //            Console.WriteLine("Authentication error");
        //            return null;
        //        }
        //        //shouldn't be part of this method;
        //        IList<Google.Apis.Drive.v3.Data.File> files = GetFiles(service, "name='Test Folder1'");
        //        IList<Google.Apis.Drive.v3.Data.File> filesInSpecific = null;

        //        if (files!=null && files.Count>0)
        //        {
        //            filesInSpecific = GetFiles(service, string.Format("'{0}' in parents",files[0].Id));
        //        }

        //        return service;
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex.InnerException);
        //        return null;

        //    }

        //}

        //public static IList<Google.Apis.Drive.v3.Data.File> GetFiles(DriveService service, string search)
        //{

        //    IList<Google.Apis.Drive.v3.Data.File> files = new List< Google.Apis.Drive.v3.Data.File >();

        //    try
        //    {
        //        //List all of the files and directories for the current user.  
        //        // Documentation: https://developers.google.com/drive/v2/reference/files/list
        //        FilesResource.ListRequest list = service.Files.List();

        //        //FilesResource.ListRequest list = service.Files.List()
        //        list.PageSize = 100;

        //        //list.MaxResults = 1000;
        //        if (search != null)
        //        {
        //            list.Q = search; //string.Format("'{0}' in parents", "0B7tji1MGQFO3Y3oxcnc2WUpuQXc"); //"mimeType = 'application/vnd.google-apps.folder'";
        //        }

        //        //FileList filesFeed = list.Execute();
        //        files = list.Execute().Files;

        //        ////// Loop through until we arrive at an empty page
        //        //while (filesFeed.Items != null)
        //        //{
        //        //    // Adding each item  to the list.
        //        //    foreach (File item in filesFeed.Items)
        //        //    {
        //        //        Files.Add(item);
        //        //    }

        //        //    // We will know we are on the last page when the next page token is
        //        //    // null.
        //        //    // If this is the case, break.
        //        //    if (filesFeed.NextPageToken == null)
        //        //    {
        //        //        break;
        //        //    }

        //        //    // Prepare the next page of results
        //        //    list.PageToken = filesFeed.NextPageToken;

        //        //    // Execute and process the next page request
        //        //    filesFeed = list.Execute();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        // In the event there is an error with the request.
        //        Console.WriteLine(ex.Message);
        //    }
        //    return files;
        //}
    }
}

