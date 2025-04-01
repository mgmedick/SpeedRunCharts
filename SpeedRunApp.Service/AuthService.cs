using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Specialized;
using SpeedRunApp.Model.JSON;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Linq;
using Google.Apis.Auth;
using System.Security.Cryptography;

namespace SpeedRunApp.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config = null;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<SocialTokenResponse> ValidateSocialToken(string accessToken, int socialAccountTypeID)
        {
            SocialTokenResponse result = null;
            if (socialAccountTypeID == (int)SocialAccountType.Google)
            {
                var response = await GoogleJsonWebSignature.ValidateAsync(accessToken);
                if (response != null)
                {
                    result = new SocialTokenResponse() { Email = response.Email };
                }
            }
            else
            {
                var response = await GetFacebookResponse(accessToken);
                if (response != null)
                {
                    result = new SocialTokenResponse() { Email = (string)response.GetValue("email") };
                }
            }

            return result;
        }
        
        private async Task<JObject> GetFacebookResponse(string accessToken)
        {
            JObject data = null;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestUrl = "https://graph.facebook.com/me";
                var parameters = new Dictionary<string, string> {
                    {"access_token", accessToken },
                    {"fields", "name,email"}            
                };                
                requestUrl = QueryHelpers.AddQueryString(requestUrl, parameters);
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                using (var response = await client.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var dataString = await response.Content.ReadAsStringAsync();
                        data = JObject.Parse(dataString);
                    }
                }
            }

            return data;
        }

        public async Task<bool> ValidateGoogleRecaptcha(string token)
        {
            bool result = false;
            var clientSecret = _config.GetSection("Auth").GetSection("Google").GetSection("RecaptchaSecret").Value;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var parameters = new Dictionary<string,string>{
                    {"secret", clientSecret},
                    {"response", token}
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://www.google.com/recaptcha/api/siteverify") { Content = new FormUrlEncodedContent(parameters) };

                using (var response = await client.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var dataString = await response.Content.ReadAsStringAsync();
                        var data = JObject.Parse(dataString);
                        
                        if (data != null)
                        {
                            result = (bool)data.GetValue("success");
                        }
                    }
                }
            }

            return result;
        }                
    }
}
