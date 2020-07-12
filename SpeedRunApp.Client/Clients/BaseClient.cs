using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using SpeedRunApp.Model.Data;
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
            Cache = new Dictionary<Uri, dynamic>();
            Client = client;
        }

        public string AccessToken { internal get; set; }
        public string UserAgent { get; private set; }
        public int MaxCacheElements { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public ClientContainer Client { get; private set; }
        private Dictionary<Uri, dynamic> Cache { get; set; }

        public static Uri GetSiteUri(string subUri)
        {
            return new Uri(BaseUri, subUri);
        }

        public static Uri GetAPIUri(string subUri)
        {
            return new Uri(APIUri, subUri);
        }

        //public ReadOnlyCollection<T> ParseCollection<T>(dynamic collection, Func<dynamic, T> parser)
        //{
        //    return Client.Common.ParseCollection<T>(collection, parser);
        //}

        //public ReadOnlyCollection<T> ParseCollection<T>(dynamic collection)
        //{
        //    return Client.Common.ParseCollection<T>(collection);
        //}

        public IEnumerable<T> ParseCollection<T>(dynamic collection, Func<dynamic, T> parser)
        {
            var enumerable = collection as IEnumerable<dynamic>;
            if (enumerable == null)
                return new List<T>(new T[0]);

            return enumerable.Select(parser).ToList();
        }

        public IEnumerable<T> ParseCollection<T>(dynamic collection)
        {
            var enumerable = collection as IEnumerable<dynamic>;
            if (enumerable == null)
                return new List<T>(new T[0]);

            return enumerable.OfType<T>().ToList();
        }

        //public APIException ParseAPIException(Stream stream)
        //{
        //    return Client.Common.ParseAPIException(stream);
        //}

        public APIException ParseAPIException(Stream stream)
        {
            var json = JSONHelper.FromStream(stream);
            var properties = json.Properties as IDictionary<string, dynamic>;
            if (properties.ContainsKey("errors"))
            {
                var errors = json.errors as IList<dynamic>;
                return new APIException(json.message as string, errors.Select(x => x as string));
            }
            else
                return new APIException(json.message as string);
        }

        public dynamic DoRequest(Uri uri)
        {
            lock (this)
            {
                dynamic result = null;

                if (Cache.ContainsKey(uri))
                {
                    result = Cache[uri];
                    Cache.Remove(uri);
                }
                else
                {
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
                }

                Cache.Add(uri, result);

                while (Cache.Count > MaxCacheElements)
                {
                    Cache.Remove(Cache.Keys.First());
                }

                return result;
            }
        }

//        internal dynamic DoRequest(Uri uri)
//        {
//            lock (this)
//            {
//                dynamic result = null;

//                if (Cache.ContainsKey(uri))
//                {
//#if DEBUG_WITH_API_CALLS
//                Console.WriteLine("Cached API Call: {0}", uri.AbsoluteUri);
//#endif
//                    result = Cache[uri];
//                    Cache.Remove(uri);
//                }
//                else
//                {
//#if DEBUG_WITH_API_CALLS
//                Console.WriteLine("Uncached API Call: {0}", uri.AbsoluteUri);
//#endif
//                    try
//                    {
//                        result = JSONHelper.FromUri(uri, UserAgent, AccessToken, Timeout);
//                    }
//                    catch (WebException ex)
//                    {
//                        try
//                        {
//                            using (var stream = ex.Response.GetResponseStream())
//                            {
//                                var apiException = ParseAPIException(stream);
//                                if (!new Regex(@"User \w+ could not be found").IsMatch(apiException.Message))
//                                {
//                                    throw apiException;
//                                }
//                            }
//                        }
//                        catch (APIException ex2)
//                        {
//                            throw ex2;
//                        }
//                        catch
//                        {
//                            throw ex;
//                        }
//                    }
//                }

//                Cache.Add(uri, result);

//                while (Cache.Count > MaxCacheElements)
//                    Cache.Remove(Cache.Keys.First());

//                return result;
//            }
//        }

        public IEnumerable<T> DoRequest<T>(Uri uri, Func<dynamic, T> parser)
        {
            var result = DoRequest(uri);

            var elements = result.data as IEnumerable<dynamic>;

            if (elements != null)
            {
                foreach (var element in elements)
                {
                    yield return parser(element);
                }
            }

            //if (elements != null)
            //{
            //    foreach (var element in elements)
            //    {
            //        yield return parser(element);
            //    }
            //} else
            //{
            //    yield return default(T);
            //}
        }
    }
}
