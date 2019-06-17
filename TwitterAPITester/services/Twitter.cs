using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace TwitterAPITester.services
{
    public class Twitter

    {
        public string OAuthConsumerSecret { get; set; }
        public string OAuthConsumerKey { get; set; }


        //get Token
        public async Task<string> GetAccessToken()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            var customerInfo = Convert.ToBase64String(new UTF8Encoding()
                                        .GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials",
                                                    Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = await httpClient.SendAsync(request);
            string json = await response.Content.ReadAsStringAsync();
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(json);
            return item["access_token"];
        }
        //Tweets by Location
        public List<models.SearchResponseModel.Status> getTweetsByGeo(string lat, string lng, int radius, int count, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = GetAccessToken().GetAwaiter().GetResult(); 
            }
            var requestGeoTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(@"
https://api.twitter.com/1.1/search/tweets.json?count={3}&geocode={0},{1},{2}mi&exclude_replies=1"
           , lat, lng, radius, count));
            requestGeoTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var http = new HttpClient();
            HttpResponseMessage responseUserTimeLine = http.SendAsync(requestGeoTimeline).GetAwaiter().GetResult();
            string json = responseUserTimeLine.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<models.SearchResponseModel.Rootobject>(json).statuses.ToList<models.SearchResponseModel.Status>();
        }
        //Tweets by User
        public List<models.SearchResponseModel.Status> getTweetsByUser(string userName, int count, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = GetAccessToken().GetAwaiter().GetResult();
            }
            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(@"
https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}&exclude_replies=1"
, count, userName));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = httpClient.SendAsync(requestUserTimeline).GetAwaiter().GetResult();
            string json = responseUserTimeLine.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<List<models.SearchResponseModel.Status>>(json);
        }
    }
}
