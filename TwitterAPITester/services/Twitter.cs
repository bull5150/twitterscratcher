using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft;
using Newtonsoft.Json;

namespace TwitterAPITester.services
{
    public class Twitter

    {
        public string OAuthConsumerSecret { get; set; }
        public string OAuthConsumerKey { get; set; }


        //get token
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

        //get by user name
        public async Task<IEnumerable<string>> GetTweets(string userName, int count, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();
            }

            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(@"
https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}&trim_user=1&exclude_replies=1"
,count, userName));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage responseUserTimeLine = await httpClient.SendAsync(requestUserTimeline);
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(await responseUserTimeLine.Content.ReadAsStringAsync());
            var enumerableTweets = (json as IEnumerable<dynamic>);
            if (enumerableTweets == null)
            {
                return null;
            }
            return enumerableTweets.Select(t => (string)(t["text"].ToString()));
        }

        //get by geolocation
        public async Task<IEnumerable<string>> GetTweetsGeo(string lat, string lng, int radius, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();
            }
            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(@"
https://api.twitter.com/1.1/search/tweets.json?count=50&geocode={0},{1},{2}mi&exclude_replies=1"
, lat, lng, radius));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var http = new HttpClient();
            HttpResponseMessage responseUserTimeLine = await http.SendAsync(requestUserTimeline);
            string json = await responseUserTimeLine.Content.ReadAsStringAsync();
            List<models.SearchResponseModel.Status> jsonList = JsonConvert.DeserializeObject<models.SearchResponseModel.Rootobject>(json).statuses.ToList<models.SearchResponseModel.Status>();
            return null;
         }
    }
}
