using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using SpeedrunComSharp.Model;
using System.Text.RegularExpressions;

namespace SpeedRunApp.Client 
{
    public abstract class BaseClient
    {

        public static readonly Uri BaseUri = new Uri("https://www.speedrun.com/");
        public static readonly Uri APIUri = new Uri(BaseUri, "api/v1/");
        public const string APIHttpHeaderRelation = "alternate https://www.speedrun.com/api";

        public BaseClient(ClientContainer client)
        {
            Timeout = client.Timeout ?? TimeSpan.FromSeconds(30);

            UserAgent = client.UserAgent;
            MaxCacheElements = client.MaxCacheElements;
            AccessToken = client.AccessToken;
            Client = client;
        }

        public string AccessToken { internal get; set; }
        public string UserAgent { get; private set; }
        public int MaxCacheElements { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public ClientContainer Client { get; private set; }

        public static Uri GetSiteUri(string subUri)
        {
            return new Uri(BaseUri, subUri);
        }

        public static Uri GetAPIUri(string subUri)
        {
            return new Uri(APIUri, subUri);
        }

        public ReadOnlyCollection<T> ParseCollection<T>(dynamic collection, Func<dynamic, T> parser)
        {
            return Client.Common.ParseCollection<T>(collection, parser);
        }

        public ReadOnlyCollection<T> ParseCollection<T>(dynamic collection)
        {
            return Client.Common.ParseCollection<T>(collection);
        }

        public APIException ParseAPIException(Stream stream)
        {
            return Client.Common.ParseAPIException(stream);
        }

        public dynamic DoRequest(Uri uri)
        {
            dynamic result = null;

            try
            {
                result = JSONHelper.FromUri(uri, UserAgent, AccessToken, Timeout);
            }
            catch (WebException ex)
            {
                try
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        var apiException = ParseAPIException(stream);
                        if (!new Regex(@"User \w+ could not be found").IsMatch(apiException.Message))
                        {
                            throw apiException;
                        }
                    }
                }
                catch (APIException ex2)
                {
                    throw ex2;
                }
                catch
                {
                    throw ex;
                }
            }

            return result;
        }

        public IEnumerable<T> DoRequest<T>(Uri uri, Func<dynamic, T> parser)
        {
            var result = DoRequest(uri);

            var elements = result.data as IEnumerable<dynamic>;

            foreach (var element in elements)
            {
                yield return parser(element);
            }
        }
    }
}
