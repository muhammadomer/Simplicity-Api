using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Commons
{
    public class FirebasePush
    {
        private string FireBase_URL = "https://fcm.googleapis.com/fcm/send";
        private string key_server;
        public FirebasePush()
        { 

        }
        public FirebasePush(string projectId,bool isSecondaryDatabase, string secondaryDatabaseId)
        {
            //---Get Firebase settings
            ProjectSettings settings = Configs.settings[projectId];
            CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, isSecondaryDatabase, secondaryDatabaseId));
            CldSettings firebaseAPI = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingFirebaseAPI);
            if (firebaseAPI != null)
            {
                FirebaseAPIKeys api_list = JsonConvert.DeserializeObject<FirebaseAPIKeys>(firebaseAPI.SettingValue);
                this.key_server = api_list.serverKey;
            }
            
        }
        public async Task<bool> NotifyAsync(UserNotifications obj)
        {
            bool returnValue=false;
            try
            {

                var data = new
                {

                    notification = new { obj.title, obj.body, obj.click_action },
                    registration_ids = new[]
                    {
                        obj.registration_ids
                    }
                };

                // Using Newtonsoft.Json
                var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = WebRequest.CreateHttp(this.FireBase_URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "key=" + this.key_server);
                var buffer = Encoding.UTF8.GetBytes(jsonBody);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                var json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
           
        }
    }
}
