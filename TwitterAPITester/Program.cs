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
                OAuthConsumerKey = "",
                OAuthConsumerSecret = ""
            };

            //get tweets by geolocation
            IEnumerable<string> twitts = twitter.GetTweetsGeo("41.266867", "-95.931400", 10).Result;
            foreach (var t in twitts)
            {
                Console.WriteLine(t + "\n");
            }
            Console.ReadKey();

            //get tweets by user
            //IEnumerable<string> twitts = twitter.GetTweets("", 10).Result;
            //foreach (var t in twitts)
            //{
            //    Console.WriteLine(t + "\n");
            //}
            //Console.ReadKey();
        }
    }
}
