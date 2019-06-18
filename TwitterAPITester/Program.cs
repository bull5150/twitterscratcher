using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TwitterAPITester
{
    class Program
    {
        static void Main(string[] args)
        {
            var twitter = new services.Twitter
            {
                OAuthConsumerKey = "[Your Stuff Here]",
                OAuthConsumerSecret = "[Your Stuff Here]"
            };

            //set type or username
            string type = "term";
            string term = "Cornhuskers";
            string lookBack = "2019-06-10";
            string username = "realDonaldTrump";
            //used for week of CWS omaha ne
            string lat = "41.266867";
            string lng = "-95.931400";
            int radiusMiles = 1;
            //number of tweets returned
            int tweetCount = 50;

            List<models.SearchResponseModel.Status> tweetList = new List<models.SearchResponseModel.Status>();

            switch (type){
                case "user":
                    tweetList = twitter.getTweetsByUser(username, lookBack, tweetCount);
                    foreach (models.SearchResponseModel.Status tweet in tweetList)
                    {
                        Console.WriteLine("-------- @" + tweet.user.screen_name + " " + tweet.user.location + "-----" + tweet.created_at);
                        Console.WriteLine(tweet.text + "\n");
                    }
                    Console.ReadLine();
                    break;
                case "geo":
                    tweetList = twitter.getTweetsByGeo(lat, lng, radiusMiles, tweetCount);
                    foreach (models.SearchResponseModel.Status tweet in tweetList)
                    {
                        Console.WriteLine("-------- @" + tweet.user.screen_name + " " + tweet.user.location + "-----" + tweet.created_at);
                        Console.WriteLine(tweet.text + "\n");
                    }
                    Console.ReadLine();
                    break;
                case "term":
                    tweetList = twitter.getTweetsByTerm(term, tweetCount);
                    foreach (models.SearchResponseModel.Status tweet in tweetList)
                    {
                        Console.WriteLine("-------- @" + tweet.user.screen_name + " " + tweet.user.location + "-----" + tweet.created_at);
                        Console.WriteLine(tweet.text + "\n");
                    }
                    Console.ReadLine();
                    break;
            }
        }
    }
}
