using Newtonsoft.Json.Linq;
using SpeedRunApp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;

namespace SpeedRunApp.Client
{
    public static class JSONHelper
    {
        public static dynamic FromResponse(WebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                return FromStream(stream);
            }
        }

        public static dynamic FromStream(Stream stream)
        {
            var reader = new StreamReader(stream);
            var json = "";
            try
            {
                json = reader.ReadToEnd();
            }
            catch { }
            return FromString(json);
        }

        public static dynamic FromString(string value)
        {
            //JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            //{
            //    Converters = new List<JsonConverter> { new DynamicJsonConverter() }
            //};
            JObject obj = JObject.Parse(value);
            IDictionary<string, object> objDictionary = obj.ToDictionary();

            //return JsonConvert.DeserializeObject<object>(value);
            return new DynamicJsonObject(objDictionary);
        }

        public static dynamic FromUri(Uri uri, string userAgent, string accessToken, TimeSpan timeout)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = (int)timeout.TotalMilliseconds;
            request.UserAgent = userAgent;
            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Add("X-API-Key", accessToken.ToString());
            var response = request.GetResponse();
            return FromResponse(response);
        }

        public static string Escape(string value)
        {
            return JavaScriptEncoder.Default.Encode(value);
        }

        public static dynamic FromUriPost(Uri uri, string userAgent, string accessToken, TimeSpan timeout, string postBody)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = (int)timeout.TotalMilliseconds;
            request.Method = "POST";
            request.UserAgent = userAgent;
            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Add("X-API-Key", accessToken.ToString());
            request.ContentType = "application/json";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postBody);
            }

            var response = request.GetResponse();

            return FromResponse(response);
        }

        public static Dictionary<string, object> ToDictionary(this JObject json)
        {
            var propertyValuePairs = json.ToObject<Dictionary<string, object>>();
            ProcessJObjectProperties(propertyValuePairs);
            ProcessJArrayProperties(propertyValuePairs);
            return propertyValuePairs;
        }

        private static void ProcessJObjectProperties(IDictionary<string, object> propertyValuePairs)
        {
            var objectPropertyNames = (from property in propertyValuePairs
                                       let propertyName = property.Key
                                       let value = property.Value
                                       where value is JObject
                                       select propertyName).ToList();

            objectPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToDictionary((JObject)propertyValuePairs[propertyName]));
        }

        private static void ProcessJArrayProperties(IDictionary<string, object> propertyValuePairs)
        {
            var arrayPropertyNames = (from property in propertyValuePairs
                                      let propertyName = property.Key
                                      let value = property.Value
                                      where value is JArray
                                      select propertyName).ToList();

            arrayPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToList((JArray)propertyValuePairs[propertyName]));
        }

        public static ArrayList ToList(this JArray array)
        {
            return new ArrayList(array.ToObject<object[]>().Select(ProcessArrayEntry).ToList());
        }

        private static object ProcessArrayEntry(object value)
        {
            if (value is JObject)
            {
                return ToDictionary((JObject)value);
            }
            if (value is JArray)
            {
                return ToList((JArray)value);
            }
            return value;
        }
    }
}
