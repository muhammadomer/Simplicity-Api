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
    public class SendUserNotification
    {
        private string FireBase_URL = "https://fcm.googleapis.com/fcm/send";
        private string key_server;
        public SendUserNotification()
        { 

        }
        public SendUserNotification(string projectId,bool isSecondaryDatabase, string secondaryDatabaseId)
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
        //--- send Firebase Notification
        public async Task<bool>  FirebasePushNotifyAsync(UserNotifications obj)
        {
            bool returnValue=false;
            try
            {
                var arr = obj.registration_ids.Split(',');
                var data = new
                {

                    notification = new { obj.title, obj.body, obj.click_action,  obj.sound },
                    registration_ids = arr
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
                returnValue = false;
            }
            return returnValue;
           
        }
        //--- Send SMS Notification
        public async Task<bool> SMSSendNotifyAsync(string message, string destinationNumber)
        {
            bool returnValue = false;
            try
            {

                var data = new
                {

                    message = message
                };

                // Using Newtonsoft.Json
                var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                string url = "https://messagingapi.sinch.com/v1/sms/" + destinationNumber ;
                string appKey = "5ddb8cbf-a5fb-42cf-8d1e-b9e425df5e11";
                string appSecret = "ZCK3Q8lQVUKAPMlgtRZG9g==";
                string usernameAndPassword = "application\\" + appKey + ":" + appSecret;
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(usernameAndPassword);
                string base64Str = System.Convert.ToBase64String(plainTextBytes);
                var request = WebRequest.CreateHttp(url);
                request.Method = "POST";
                request.Headers.Add("Authorization", "basic" + " " + base64Str) ;
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(jsonBody);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = request.GetResponse();
                var json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;

        }
    }
}
