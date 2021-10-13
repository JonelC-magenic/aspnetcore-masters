using Microsoft.IdentityModel.Tokens;

namespace ASPNetCoreMastersTodoList.Api
{
    public class JwtOptions
    {
        public SecurityKey SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}