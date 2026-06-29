using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public static class TokenBuilder
    {
        internal static TokenValidationParameters tokenValidationParams;
        private static SigningCredentials signingCredentials;
        //Construct our JWT authentication paramaters then inject the parameters into the current TokenBuilder instance
        // If injecting an RSA key for signing use this method
        // Be weary of common jwt trips: https://trustfoundry.net/jwt-hacking-101/ and https://www.sjoerdlangkemper.nl/2016/09/28/attacking-jwt-authentication/
        //public static void ConfigureJwtAuthentication(this IServiceCollection services, RSAParameters rsaParams)
        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = GetJwtSettings(configuration);
            tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateLifetime = true,
                ValidAudience = jwtSettings.Audience,
                ValidateAudience = true,
                RequireSignedTokens = true,
                // Use our signing credentials key here
                // optionally we can inject an RSA key as
                //IssuerSigningKey = new RsaSecurityKey(rsaParams),
                IssuerSigningKey = signingCredentials.Key,
                ClockSkew = TimeSpan.FromMinutes(0)
            };
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParams;
                #if PROD || UAT
                    options.IncludeErrorDetails = false;
                #elif DEBUG
                    options.RequireHttpsMetadata = false;
                #endif
            });
        }
        
        public static string CreateJsonWebToken(
               string username,
               IEnumerable<string> roles,
               string audienceUri,
               string issuerUri,
               Guid applicationId,
               DateTime expires,
               string deviceId = null,
               bool isReAuthToken = false)
        {
            var claims = new List<Claim>();
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            var head = new JwtHeader();
            var payload = new JwtPayload(claims.ToArray());
            var jwt = new JwtSecurityToken(issuerUri, audienceUri, claims, DateTime.UtcNow, expires, signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static (string Key, string Issuer, string Audience) GetJwtSettings(IConfiguration configuration)
        {
            var key = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("Jwt:Key nao configurado. Defina a chave JWT via appsettings, Secret Manager ou variavel de ambiente.");
            }

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new InvalidOperationException("Jwt:Issuer nao configurado.");
            }

            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException("Jwt:Audience nao configurado.");
            }

            var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            return (key, issuer, audience);
        }
    }
}
