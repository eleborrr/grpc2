using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using grpc2.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace grpc2.Services;

public class PrivateService: Private.PrivateBase
{
    [Authorize]
    public override Task<SecretData> GetSecret(Empty request,  ServerCallContext serverCallContext)
    {
        return Task.FromResult(new SecretData() { Secret = "BEAVER TINDER TILL WE DIE" });
    }

}