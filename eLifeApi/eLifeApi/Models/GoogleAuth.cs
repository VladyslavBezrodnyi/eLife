using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace eLifeApi.Models
{
    public class GoogleAuth
    {
        protected string googleplus_client_id = "735792416221-qvheh8i5ogk9o8qbdvabjrljgidkrdhb.apps.googleusercontent.com";    // Replace this with your Client ID
        protected string googleplus_client_secret = "iv7DA6xwd_zA5oLt6EOzhtBW";                                                // Replace this with your Client Secret
        protected string googleplus_redirect_url = "http://localhost:49433/Home/Index";                                         // Replace this with your Redirect URL; Your Redirect URL from your developer.google application should match this URL.
        protected string Parameters;

        public void CreateGoooleClient()
        {
          
            //get the access token 
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            webRequest.Method = "POST";
            Parameters = "client_id=" + googleplus_client_id + "&client_secret=" + googleplus_client_secret + "&redirect_uri=" + googleplus_redirect_url + "&grant_type=authorization_code";
            byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream postStream = webRequest.GetRequestStream();
            // Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            WebResponse response = webRequest.GetResponse();
            postStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(postStream);
            string responseFromServer = reader.ReadToEnd();

            GooglePlusAccessToken serStatus = JsonConvert.DeserializeObject<GooglePlusAccessToken>(responseFromServer);

            if (serStatus != null)
            {
                string accessToken = string.Empty;
                accessToken = serStatus.access_token;

                if (!string.IsNullOrEmpty(accessToken))
                {
                    getgoogleplususerdataSer(accessToken);
                }
            }
        }
        private async void getgoogleplususerdataSer(string access_token)
        {
           
                HttpClient client = new HttpClient();
                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + access_token;

                client.CancelPendingRequests();
                HttpResponseMessage output = await client.GetAsync(urlProfile);

                if (output.IsSuccessStatusCode)
                {
                    string outputData = await output.Content.ReadAsStringAsync();
                    GoogleUserOutputData serStatus = JsonConvert.DeserializeObject<GoogleUserOutputData>(outputData);

                    if (serStatus != null)
                    {
                    }
                }
        }
    }
}