using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using grpc2.Helpers;

namespace grpc2.Services;

public class AuthService: Auth.AuthBase
{
    public override async Task<AuthData> GetJwt(Empty request, ServerCallContext context)
    {
        return new AuthData(){ Secret = await JwtGenerator.GenerateJwtToken()};
    }
}