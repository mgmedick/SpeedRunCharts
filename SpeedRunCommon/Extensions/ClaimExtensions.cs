using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Security.Claims;

namespace SpeedRunCommon.Extensions
{
    public static class ClaimExtensions
    {
        public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
        {
            var identity = (ClaimsIdentity)currentPrincipal.Identity;
            if (identity != null) {
                var existingClaim = identity.FindFirst(key);
                if (existingClaim != null) {
                    identity.RemoveClaim(existingClaim);
                }

                identity.AddClaim(new Claim(key, value));
            }
        }
    }
}
