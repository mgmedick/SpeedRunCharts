using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using SpeedrunComSharp.Model;

namespace SpeedrunComSharp.Client 
{
    public abstract class BaseClient
    {

        public static readonly Uri BaseUri = new Uri("https://www.speedrun.com/");
        public static readonly Uri APIUri = new Uri(BaseUri, "api/v1/");
        public const string APIHttpHeaderRelation = "alternate https://www.speedrun.com/api";

        public string AccessToken { internal get; set; }

        public bool IsAccessTokenValid
        {
            get
            {
                if (AccessToken == null)
                    return false;

                try
                {
                    var profile = Profile;
                    return true;
                }
                catch { }

                return false;
            }
        }

        public string UserAgent { get;  private set; }
        public int MaxCacheElements { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public ClientContainer Client { get; private set; }
        private Dictionary<Uri, dynamic> Cache { get; set; }

        public User Profile
        {
            get
            {
                var uri = GetProfileUri(string.Empty);
                var result = DoRequest(uri);
                return Client.Users.Parse(result.data);
            }
        }

        public BaseClient(ClientContainer client)
        {
            Timeout = client.Timeout ?? TimeSpan.FromSeconds(30);

            UserAgent = client.UserAgent;
            MaxCacheElements = client.MaxCacheElements;
            AccessToken = client.AccessToken;
            Cache = new Dictionary<Uri, dynamic>();
            Client = client;
        }

        public static Uri GetSiteUri(string subUri)
        {
            return new Uri(BaseUri, subUri);
        }

        public static Uri GetAPIUri(string subUri)
        {
            return new Uri(APIUri, subUri);
        }

        public static Uri GetProfileUri(string subUri)
        {
            return GetAPIUri(string.Format("profile{0}", subUri));
        }

        public ElementDescription GetElementDescriptionFromSiteUri(string siteUri)
        {
            try
            {
                var request = WebRequest.Create(siteUri);
                request.Timeout = (int)Timeout.TotalMilliseconds;
                var response = request.GetResponse();
                var linksString = response.Headers["Link"];
                var links = Client.Common.ParseLinks(linksString);
                var link = links.FirstOrDefault(x => x.Relation == APIHttpHeaderRelation);

                if (link == null)
                    return null;

                var uri = link.Uri;
                var elementDescription = Client.Common.ParseElementDescription(uri);

                return elementDescription;
            }
            catch
            {
                return null;
            }
        }

        internal ReadOnlyCollection<T> ParseCollection<T>(dynamic collection, Func<dynamic, T> parser)
        {
            return Client.Common.ParseCollection<T>(collection, parser);
        }

        internal ReadOnlyCollection<T> ParseCollection<T>(dynamic collection)
        {
            return Client.Common.ParseCollection<T>(collection);
        }

        internal APIException ParseAPIException(Stream stream)
        {
            return Client.Common.ParseAPIException(stream);
        }

        internal dynamic DoPostRequest(Uri uri, string postBody)
        {
            try
            {
                return JSONHelper.FromUriPost(uri, UserAgent, AccessToken, Timeout, postBody);
            }
            catch (WebException ex)
            {
                try
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        throw ParseAPIException(stream);
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

        internal dynamic DoRequest(Uri uri)
        {
            lock (this)
            {
                dynamic result;

                if (Cache.ContainsKey(uri))
                {
#if DEBUG_WITH_API_CALLS
                Console.WriteLine("Cached API Call: {0}", uri.AbsoluteUri);
#endif
                    result = Cache[uri];
                    Cache.Remove(uri);
                }
                else
                {
#if DEBUG_WITH_API_CALLS
                Console.WriteLine("Uncached API Call: {0}", uri.AbsoluteUri);
#endif
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
                                throw ParseAPIException(stream);
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
                    Cache.Remove(Cache.Keys.First());

                return result;
            }
        }

        internal ReadOnlyCollection<T> DoDataCollectionRequest<T>(Uri uri, Func<dynamic, T> parser)
        {
            var result = DoRequest(uri);
            var elements = result.data as IEnumerable<dynamic>;
            if (elements == null)
                return new ReadOnlyCollection<T>(new T[0]);

            return elements.Select(parser).ToList().AsReadOnly();
        }

        private IEnumerable<T> doPaginatedRequest<T>(Uri uri, Func<dynamic, T> parser)
        {
            do
            {
                var result = DoRequest(uri);

                if (result.pagination.size == 0)
                {
                    yield break;
                }

                var elements = result.data as IEnumerable<dynamic>;

                foreach (var element in elements)
                {
                    yield return parser(element);
                }

                yield break;

                //var links = result.pagination.links as IEnumerable<dynamic>;
                //if (links == null)
                //{
                //    yield break;
                //}

                //var paginationLink = links.FirstOrDefault(x => x.rel == "next");
                //if (paginationLink == null)
                //{
                //    yield break;
                //}

                //uri = new Uri(paginationLink.uri as string);
            } while (true);
        }

        internal IEnumerable<T> DoPaginatedRequest<T>(Uri uri, Func<dynamic, T> parser)
        {
            return doPaginatedRequest(uri, parser).Cache();
        }
    }
}
