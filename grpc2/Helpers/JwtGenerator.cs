using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace grpc2.Helpers;

public static class JwtGenerator
{
    public static Task<string> GenerateJwtToken()
    {
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: "AuthServer",
            audience: "AuthClient",
            notBefore: now,
            expires: now.Add(TimeSpan.FromMinutes(1440)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                    "BEAVER_ZOMBIES_ON_HAUNTED_MOUND_TILL_WE_ARE_IN_THE_GROUND"u8.ToArray()),
                SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Task.FromResult(encodedJwt);
    }
}