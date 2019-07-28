﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text.Encodings.Web;
using Newtonsoft.Json.Linq;

namespace SpeedrunComSharp
{
    internal static class JSON
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

            return JsonConvert.DeserializeObject<object>(value);
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
    }

    /*
    public sealed class DynamicJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray jArray = (JArray)JArray.ReadFrom(reader);

            return objectType == typeof(object) ? new DynamicJsonObject(jArray) : null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            ReadOnlyCollection<Type> supportedTypes = new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) }));
            return supportedTypes.Contains(objectType);
        }
    }

    public sealed class DynamicJsonObject : DynamicObject
    {
        private readonly JObject _jObj;

        //public IDictionary<string, object> Properties { get { return _dictionary; } }

        public DynamicJsonObject()
            : this(new JObject())
        { }

        public DynamicJsonObject(JObject jObj)
        {
            if (jObj == null)
                throw new ArgumentNullException("obj");

            _jObj = jObj;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("{\r\n");
            ToString(sb);
            return sb.ToString();
        }

        private void ToString(StringBuilder sb, int depth = 1)
        {
            var firstInDictionary = true;
            foreach (var pair in _jObj)
            {
                if (!firstInDictionary)
                    sb.Append(",\r\n");
                sb.Append('\t', depth);
                firstInDictionary = false;
                var value = pair.Value;
                var name = pair.Key;

                if (value.Type == JTokenType.String)
                {
                    sb.AppendFormat("\"{0}\": \"{1}\"", JavaScriptEncoder.Default.Encode(name), JavaScriptEncoder.Default.Encode((string)value));
                }
                else if (value.Type == JTokenType.Object && (value.ToObject<object>() is DynamicJsonObject))
                {                  
                    sb.Append("\"" + JavaScriptEncoder.Default.Encode(name) + "\": {\r\n");
                    (value.ToObject<DynamicJsonObject>()).ToString(sb, depth + 1);
                }
                else if (value.Type == JTokenType.Object && (value.ToObject<object>() is IDictionary<string, object>))
                {
                    sb.Append("\"" + JavaScriptEncoder.Default.Encode(name) + "\": {\r\n");
                    new DynamicJsonObject(value.ToObject<Dictionary<string, object>>()).ToString(sb, depth + 1);
                }
                else if (value is IEnumerable<object>)
                {
                    sb.Append("\"" + JavaScriptEncoder.Default.Encode(name) + "\": [\r\n");
                    var firstInArray = true;
                    foreach (var arrayValue in (IEnumerable<object>)value)
                    {
                        if (!firstInArray)
                            sb.Append(",\r\n");
                        sb.Append('\t', depth + 1);
                        firstInArray = false;
                        if (arrayValue is IDictionary<string, object>)
                            new DynamicJsonObject((IDictionary<string, object>)arrayValue).ToString(sb, depth + 2);
                        else if (arrayValue is DynamicJsonObject)
                        {
                            sb.Append("{\r\n");
                            ((DynamicJsonObject)arrayValue).ToString(sb, depth + 2);
                        }
                        else if (arrayValue is string)
                            sb.AppendFormat("\"{0}\"", JavaScriptEncoder.Default.Encode((string)arrayValue));
                        else if (arrayValue is bool)
                            sb.AppendFormat("{0}", JavaScriptEncoder.Default.Encode(((bool)arrayValue).ToString(CultureInfo.InvariantCulture).ToLowerInvariant()));
                        else if (arrayValue is int)
                            sb.AppendFormat("{0}", JavaScriptEncoder.Default.Encode(((int)arrayValue).ToString(CultureInfo.InvariantCulture)));
                        else if (arrayValue is double)
                            sb.AppendFormat("{0}", JavaScriptEncoder.Default.Encode(((double)arrayValue).ToString(CultureInfo.InvariantCulture)));
                        else if (arrayValue is decimal)
                            sb.AppendFormat("{0}", JavaScriptEncoder.Default.Encode(((decimal)arrayValue).ToString(CultureInfo.InvariantCulture)));
                        else
                            sb.AppendFormat("\"{0}\"", JavaScriptEncoder.Default.Encode((arrayValue ?? "").ToString()));

                    }
                    sb.Append("\r\n");
                    sb.Append('\t', depth);
                    sb.Append("]");
                }
                else if (value.Type == JTokenType.Boolean)
                    sb.AppendFormat("\"{0}\": {1}", JavaScriptEncoder.Default.Encode(name), JavaScriptEncoder.Default.Encode(((bool)value).ToString(CultureInfo.InvariantCulture).ToLowerInvariant()));
                else if (value.Type == JTokenType.Integer)
                    sb.AppendFormat("\"{0}\": {1}", JavaScriptEncoder.Default.Encode(name), JavaScriptEncoder.Default.Encode(((int)value).ToString(CultureInfo.InvariantCulture)));
                else if (value.Type == JTokenType.Float)
                    sb.AppendFormat("\"{0}\": {1}", JavaScriptEncoder.Default.Encode(name), JavaScriptEncoder.Default.Encode(((double)value).ToString(CultureInfo.InvariantCulture)));
                else
                {
                    sb.AppendFormat("\"{0}\": \"{1}\"", JavaScriptEncoder.Default.Encode(name), JavaScriptEncoder.Default.Encode((value ?? "").ToString()));
                }
            }
            sb.Append("\r\n");
            sb.Append('\t', depth - 1);
            sb.Append("}");
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                _dictionary[binder.Name] = value;
                return true;
            }
            else
            {
                _dictionary.Add(binder.Name, value);
                return true;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == "Properties")
            {
                result = _dictionary
                    .Select(x => new KeyValuePair<string, dynamic>(x.Key, WrapResultObject(x.Value)))
                    .ToDictionary(x => x.Key, x => x.Value);
                return true;
            }

            if (!_dictionary.TryGetValue(binder.Name, out result))
            {
                // return null to avoid exception.  caller can check for null this way...
                result = null;
                return true;
            }

            result = WrapResultObject(result);

            if (result is string)
                result = JavaScriptStringDecode(result as string);

            return true;
        }

        public static string JavaScriptStringDecode(string source)
        {
            // Replace some chars.
            var decoded = source.Replace(@"\'", "'")
                        .Replace(@"\""", @"""")
                        .Replace(@"\/", "/")
                        .Replace(@"\t", "\t")
                        .Replace(@"\n", "\n");

            // Replace unicode escaped text.
            var rx = new Regex(@"\\[uU]([0-9A-F]{4})");

            decoded = rx.Replace(decoded, match => ((char)int.Parse(match.Value.Substring(2), NumberStyles.HexNumber))
                                                    .ToString(CultureInfo.InvariantCulture));

            return decoded;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 1 && indexes[0] != null)
            {
                if (!_dictionary.TryGetValue(indexes[0].ToString(), out result))
                {
                    // return null to avoid exception.  caller can check for null this way...
                    result = null;
                    return true;
                }

                result = WrapResultObject(result);
                return true;
            }

            return base.TryGetIndex(binder, indexes, out result);
        }

        private static object WrapResultObject(object result)
        {
            var dictionary = result as IDictionary<string, object>;
            if (dictionary != null)
                return new DynamicJsonObject(dictionary);

            var arrayList = result as ArrayList;
            if (arrayList != null && arrayList.Count > 0)
            {
                return arrayList[0] is IDictionary<string, object>
                    ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)))
                    : new List<object>(arrayList.Cast<object>());
            }

            return result;
        }
    }
    */
}
