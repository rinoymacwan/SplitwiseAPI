using Newtonsoft.Json;
using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SplitwiseAPI.Utility.Helpers
{
    public class Tokens
    {
        public static async Task<string> GenerateJwt(Users user, ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
                name = user.Name
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
