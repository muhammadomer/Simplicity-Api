using System.Collections.Generic;
using SimplicityOnlineBLL.Entities;
namespace  SimplicityOnlineBLL.Entities
{
	public class AppSetting
	{   
        public bool IsSucessfull { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public ProjectSetting ProjectSetting { get; set; }
        public User User { get; set; }
        public GoogleDriveAPIKeys GoogleDriveAPIKeyNew { get; set; }
        public GoogleDriveAPIKeys GoogleDriveAPIKey { get; set; }
        public GoogleDriveAPIKeys GoogleDriveAPIKeyOld { get; set; }
        public FirebaseAPIKeys FirebaseAPIKey { get; set; }
        public List<RefGenericLabels> GenericLabels { get; set; }
    }

    public class User
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string userEmail { get; set; }
    }

    public class ProjectSetting
    {
        public string FilingCabinetRootFolder { get; set; }
    }

    public class GoogleDriveAPIKeys
    {
        public string client_Id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string sub { get; set; }

    }

    public class FirebaseAPIKeys
    {
        
        public string apiKey { get; set; }
        public string authDomain { get; set; }
        public string databaseURL { get; set; }
        public string projectId { get; set; }
        public string storageBucket { get; set; }
        public string messagingSenderId { get; set; }
        public string serverKey { get; set; }
    }
}